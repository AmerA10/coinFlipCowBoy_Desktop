using System.Collections;
using System.Collections.Generic;
using UnityEngine;
using UnityEngine.Events;
public class playerMovement_New : MonoBehaviour
{
    // Start is called before the first frame update
    
    private Vector3 mousePos;
    public static bool isMouseRight;

    [SerializeField] private LayerMask whatIsGround;
    [SerializeField] private Transform groundCheck;
    
    [Header("Movement Stuff")]
    [Space]
    public Rigidbody2D rb;
    private float moveX;
    public bool jump {get; private set;}
    public bool jumpFall { get; private set; }

    private Vector2 move_ref= Vector2.zero;
    [SerializeField] private float speed = 5f;
    [SerializeField] private float speedSmooth = 0.05f;
    [SerializeField] private float airSpeed = 2.5f;
    [SerializeField] private float Jumpspeed = 5f;
    [SerializeField] private float fallMultiplier = 2f;
    [SerializeField] private float fallSmooth = 0.25f;


    private bool isGrounded;
    private bool isJumping;
    private bool isFalling;
    private bool isInAir;
    const float groundedRadius = 0.25f;
    private Vector2 refVelocity;

    private void Awake()
    {
        jump = false;
        rb = GetComponent<Rigidbody2D>();

    }

    private void FixedUpdate()
    {

        bool wasGrounded = isGrounded;

        isGrounded = false;

        Collider2D[] colliders = Physics2D.OverlapCircleAll(groundCheck.position, groundedRadius, whatIsGround);
    
        for (int i = 0; i < colliders.Length; i++)
        {
            if (colliders[i].gameObject != gameObject)
            {
                isGrounded = true;
                isFalling = false;
                isInAir = false;
            }
            else
            {
                isGrounded = false;
                isFalling = true;
                isInAir = true;
  
            }
        }
        MovePlayer(moveX);
        checkFall();


    }


    // Update is called once per frame
    void Update()
    {
        moveX = Input.GetAxisRaw("Horizontal");
        jump = Input.GetButtonDown("Jump");
        jumpFall = Input.GetButtonUp("Jump");
        mousePos = Camera.main.ScreenToWorldPoint(Input.mousePosition);
 
     
        if (Input.GetButton("Jump") && !isGrounded)
        {
            isJumping = true;
        }

        CheckMousePos();
        FlipPlayer();

        if (isGrounded && jump && !jumpFall)
        {
            playerJump();
        }
    }

    public void playerJump ()
    {
        if (!isInAir)
        {
            Debug.Log("velocity:" + rb.velocity.y);
            rb.AddForce(new Vector2(0f, Jumpspeed * Time.deltaTime), ForceMode2D.Impulse);
            isInAir = true;
        }
  
    }

    void MovePlayer(float moveX)
    {

        if (isGrounded)
        {
          
        
            Vector2 movePlayer = new Vector2(moveX * speed, 0f);
            rb.velocity = Vector2.SmoothDamp(rb.velocity, movePlayer, ref move_ref, speedSmooth);
            isJumping = false;
            isFalling = false;
        }
        
        else if (!isGrounded)
        {

            Vector2 movePlayer = new Vector2(moveX * airSpeed, rb.velocity.y);
            rb.velocity = Vector2.SmoothDamp(rb.velocity, movePlayer, ref move_ref, speedSmooth);
            isInAir = true;
        }
            
      
       
    }

    private void checkFall ()
    {
        if (!isGrounded)
        {
            if (jumpFall || (!jumpFall && rb.velocity.y <= 0))
            {
        
                isFalling = true;
                PlayerFall();


            }
        }
    }

    private void PlayerFall ()
    {
 
        if (!isGrounded && isFalling && isInAir)
        {
            Vector2 targetVel = new Vector2(rb.velocity.x, fallMultiplier * Physics2D.gravity.y);
            rb.velocity = Vector2.SmoothDamp(rb.velocity, targetVel, ref refVelocity, fallSmooth);
            
        
        }
     
    }


    private void CheckMousePos()
    {

        if (this.transform.position.x - mousePos.x <= 0)
        {
            isMouseRight = true;
        }
        else
        {
            isMouseRight = false;
        }

    }
    public void OnDrawGizmos()
    {
        if (isGrounded)
        {
            Gizmos.color = Color.green;
        }
        else if (!isGrounded)
        {
            Gizmos.color = Color.red;
        }
        Gizmos.DrawWireSphere(groundCheck.position, groundedRadius);
    }

    private void FlipPlayer()
    {
        if (isMouseRight)
        {
            rb.transform.localScale = new Vector3(1, 1, 1);
        }
        else if (!isMouseRight)
        {
            rb.transform.localScale = new Vector3(-1, 1, 1);
           

        }
    }


}
