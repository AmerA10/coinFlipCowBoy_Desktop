using System.Collections;
using System.Collections.Generic;
using UnityEngine;

public class PlayerShooting : MonoBehaviour
{
    // Start is called before the first frame update
    public GameObject gun;
    public Transform gunShootPoint;
    public static Vector3 mousePos;
    private Camera mainCam;
    public static Vector3 targetPos;
    public LayerMask whatIsHit;
    private const int  gunAngleRestrain = 90;
    [SerializeField] private float distance;
    [SerializeField] private float rotationSpeed;
    
    public GameObject spotHit;
    private float angle;

    [SerializeField] private float camerShakeTime;
    [SerializeField] private float cameraShakeStrength;
    [SerializeField] private float gunShootTime;
    [SerializeField] private float gunCoolDownTime;

    public LineRenderer lineRenderer;
    public Camera main;
    void Start()
    {

        rotationSpeed = 25f;
        mainCam = Camera.main;
        distance = Mathf.Infinity;
        gunCoolDownTime = .5f;
        lineRenderer = GetComponent<LineRenderer>();

    }

    private void gunRotation()
    {
        Vector3 vectorToTarget = mousePos - gun.transform.position;
        //essentially this line creates a float that returns the angle between the starting point to the ending point
        if (playerMovement_New.isMouseRight)
        {
             angle = Mathf.Atan2(vectorToTarget.y, vectorToTarget.x) * Mathf.Rad2Deg;
        }
        else if (!playerMovement_New.isMouseRight)
        {
             angle = Mathf.Atan2(vectorToTarget.y, vectorToTarget.x *-1) * Mathf.Rad2Deg;
        }
        //this creates a rotation that is based on the forward axis,the x axis with the angle float clamped to -90 and 90
        Quaternion q = Quaternion.AngleAxis(Mathf.Clamp(angle, gunAngleRestrain * -1, gunAngleRestrain), transform.forward);
        
        //rotates the gun towards the q angle created by time and speed
        gun.transform.localRotation= Quaternion.Slerp(gun.transform.localRotation, q, Time.deltaTime * rotationSpeed);
        

    }

    // Update is called once per frame
    void Update()
    {

        mousePos = mainCam.ScreenToWorldPoint(Input.mousePosition);
        gunRotation();
        StartCoroutine(playerShoot());

    }
    private IEnumerator playerShoot()
    {
        targetPos = mousePos - gunShootPoint.transform.position;

        if (Time.time > gunShootTime)
        {
            if (Input.GetButtonDown("Fire1"))
            {
                RaycastHit2D hitInfo = Physics2D.Raycast(gunShootPoint.transform.position, targetPos, distance, whatIsHit);
                lineRenderer.SetPosition(0, gunShootPoint.transform.position);
                StartCoroutine(CameraShake(camerShakeTime, cameraShakeStrength));
                if (hitInfo)
                {
                    lineRenderer.positionCount = 4;
                    Debug.Log("HitInfo:" + hitInfo.transform.name);
                    reflectBullet(hitInfo);
                 

                    lineRenderer.SetPosition(1, hitInfo.point);
                }
                else
                {

                    lineRenderer.positionCount = 2;
                    lineRenderer.SetPosition(0, gunShootPoint.transform.position);
                    if (playerMovement_New.isMouseRight)
                    {
                        lineRenderer.SetPosition(1, gunShootPoint.transform.right * 100);
                    }
                    else
                    {
                        lineRenderer.SetPosition(1, gunShootPoint.transform.right * -100);
                    }
                    
                }
              
            
                gunShootTime = Time.time + gunCoolDownTime;

                lineRenderer.enabled = true;

                yield return new WaitForSeconds(0.2f);

                lineRenderer.enabled = false;

            }

        }

 
        Debug.DrawRay(gunShootPoint.transform.position, targetPos, Color.red);

    }

    private void reflectBullet(RaycastHit2D shotInfo)
    {

        Vector3 newTargetPos = Vector3.Reflect(targetPos, shotInfo.normal);
        RaycastHit2D reflectShot = Physics2D.Raycast(shotInfo.point, newTargetPos, distance, whatIsHit);
        Debug.DrawRay(shotInfo.point, newTargetPos, Color.yellow);

        if (reflectShot)
        {
            lineRenderer.SetPosition(2, shotInfo.point);
            lineRenderer.SetPosition(3, reflectShot.point);
            CheckHit(reflectShot);
            Debug.Log("SHot enemy:" + reflectShot.transform.tag);

        }
    
        else
        {

            Vector2 missShot = new Vector2(newTargetPos.x, newTargetPos.y ) * 1000;
            lineRenderer.SetPosition(2, shotInfo.point);
            lineRenderer.SetPosition(3, missShot);
        }





    }

    private void CheckHit(RaycastHit2D reflectShotInfo)
    {
        if (reflectShotInfo.transform.tag.Equals("Enemy"))
        {
            Destroy(reflectShotInfo.transform.gameObject);
        }
    }

    private IEnumerator CameraShake(float shakeDuration, float shakeStrength)
    {
        float timeElapsed = 0f;
        Vector3 startinCameraPos = main.transform.position;

        while (timeElapsed < shakeDuration)
        {
            float x = Random.Range(-1, 1) * shakeStrength;
            float y = Random.Range(-1, 1) * shakeStrength;

            main.transform.localPosition = new Vector3(x, y, startinCameraPos.z);

            timeElapsed += Time.deltaTime;

            yield return null;
        }

        main.transform.localPosition = startinCameraPos;



    }
}

    
