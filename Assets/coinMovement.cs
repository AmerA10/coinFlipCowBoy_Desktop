using System.Collections;
using System.Collections.Generic;
using UnityEngine;
using UnityEngine.Rendering;

public class coinMovement : MonoBehaviour
{
    // Start is called before the first frame update
    public float coinSpeed = 5f;
    public float coinFallSmoothing = 0.5f;
    public float coinFallMultiplier = 2f;
    private Vector2 refVelocity;
    public bool inAir {get; private set;}

    [SerializeField] private LayerMask whatIsGround;
    [SerializeField] private Transform groundCheck;
    private bool isGrounded;
    private Vector2 targetDirection;
    private Rigidbody2D rb;
    public Transform playerPos;


    private void Awake()
    {
        rb = GetComponent<Rigidbody2D>();
    }
    void Start()
    {
        targetDirection = (PlayerShooting.mousePos - this.transform.position);
      
        CoinLaunch(coinSpeed);
    }

    private void FixedUpdate()
    {

        if (inAir && rb.velocity.y <= 0)
        {
         
            CoinFall();        
        }
  
    }
    /*
     *      
         rb.AddForce(new Vector2(0f, Jumpspeed), ForceMode2D.Impulse);
     */

    // Update is called once per frame
    void Update()
    {
        
    }

    private void OnCollisionEnter2D(Collision2D collision)
    {
        Vector3 currentVel = rb.velocity;
        Vector3 targetDeflectVel = Vector2.Reflect(this.transform.position, collision.transform.right);
        rb.velocity = targetDeflectVel + currentVel;
        if (collision.transform.tag.Equals("Ground"))
        {
            Destroy(this.gameObject);
        }
    }
    public void CoinLaunch (float coinSpeed)
    {
        Vector2 targetDir = targetDirection * coinSpeed;
        rb.AddRelativeForce(targetDir, ForceMode2D.Impulse);
        inAir = true;
    }

    private void CoinFall()
    {
        Vector2 targetVel = new Vector2(rb.velocity.x, coinFallMultiplier * Physics2D.gravity.y);
        rb.velocity = Vector2.SmoothDamp(rb.velocity, targetVel, ref refVelocity, coinFallSmoothing);
    }

    private void OnDrawGizmos()
    {
        Gizmos.DrawLine(playerPos.position,targetDirection);
    }
}
