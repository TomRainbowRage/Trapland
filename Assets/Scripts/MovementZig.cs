using System.Collections;
using System.Collections.Generic;
using UnityEngine;
using UnityEngine.Animations;

public class MovementZig : MonoBehaviour
{
    public float jumpForce;
    public float moveSpeed;
    [Range(0, .3f)] [SerializeField] private float movementSmoothing = .05f;
    public LayerMask whatIsGround;
    public Transform feetPos;
    public Transform headPos;
    public BoxCollider2D crouchDisableCollider;
    public Animator animator;
    public float jumpStartTime;

    private SpriteRenderer sr;
    private Rigidbody2D rb;
    private Vector3 PlayerVelocity;
    private float moveInput;
    
    const float checkRadius = 0.2f;
    private float jumpTime;
    private bool isJumping;
    private bool facingRight = true;
    private int notCrouching = 1;

    [Header("Sprites")] //Sprites used for the player
    [Space]
    public Sprite fallSprite;
    public Sprite standSprite;
    public Sprite crouchSprite;

    [Header("KeyCodes")] // Keycodes that are used to control the player
    [Space]
    public KeyCode CrouchKey = KeyCode.S;
    public KeyCode JumpKey = KeyCode.W;
    public KeyCode AltCrouch = KeyCode.DownArrow;
    public KeyCode AltJump = KeyCode.UpArrow;
    public bool isGrounded;
    public bool isCrouching;
    
    
    
 
    
    

    
    
 
 
 
    // Start is called before the first frame update
    void Start()
    {
        rb = GetComponent<Rigidbody2D>();
        sr = GetComponent<SpriteRenderer>();
    }
 
    // Update is called once per frame
    void Update()
    {
        moveInput = Input.GetAxisRaw("Horizontal") * moveSpeed;
        animator.SetFloat("Speed", Mathf.Abs(moveInput) * notCrouching);

        if(Input.GetKeyDown(CrouchKey) || Input.GetKeyDown(AltCrouch))
        {
            //crouch = true;
            notCrouching = 0;
            isCrouching = true;

            animator.SetBool("Crouch", true);

            if (crouchDisableCollider != null)
					crouchDisableCollider.enabled = false;
        }
        else if(Input.GetKeyUp(CrouchKey) || Input.GetKeyUp(AltCrouch))
        {
            //crouch = false;
            notCrouching = 1;
            isCrouching = false;

            animator.SetBool("Crouch", false);

            if (crouchDisableCollider != null)
					crouchDisableCollider.enabled = true;
        }

        // If crouching, check to see if the character can stand up
		if (!isCrouching)
		{
			// If the character has a ceiling preventing them from standing up, keep them crouching
			if (Physics2D.OverlapCircle(headPos.position, checkRadius, whatIsGround))
			{
				notCrouching = 0;
                isCrouching = true;

                animator.SetBool("Crouch", true);

                if (crouchDisableCollider != null)
					crouchDisableCollider.enabled = false;
			}
		}

        //FaceMoveDirection();
        Jump();
    }
 
    void FixedUpdate()
    {
        //rb.velocity = new Vector2(moveInput * moveSpeed, rb.velocity.y);

        // Move the character by finding the target velocity
		Vector3 targetVelocity = new Vector2((moveInput * Time.fixedDeltaTime) * 10f, rb.velocity.y);
		// And then smoothing it out and applying it to the character
		rb.velocity = Vector3.SmoothDamp(rb.velocity, targetVelocity, ref PlayerVelocity, movementSmoothing) * notCrouching;

        // If the input is moving the player right and the player is facing left...
		if (moveInput > 0 && !facingRight)
		{
			// ... flip the player.
			Flip();
		}
		// Otherwise if the input is moving the player left and the player is facing right...
		else if (moveInput < 0 && facingRight)
		{
			// ... flip the player.
			Flip();
		}
    }
 
    void Jump()
    {
        //isGrounded = Physics2D.OverlapCircle(feetPos.position, checkRadius, whatIsGround);
        bool wasGrounded = isGrounded;
        isGrounded = false;

        Collider2D[] colliders = Physics2D.OverlapCircleAll(feetPos.position, checkRadius, whatIsGround);
		for (int i = 0; i < colliders.Length; i++)
		{
			if (colliders[i].gameObject != gameObject)
			{
				isGrounded = true;
				if (!wasGrounded)
					OnLand();//OnLandEvent.Invoke();
			}
		}

        if(!isCrouching)
        {
            if (isGrounded == true && (Input.GetKeyDown(JumpKey) || Input.GetKeyDown(AltJump)))
            {
                animator.SetBool("Jumping", true);
                //animator.enabled = false;
                //this.GetComponent<SpriteRenderer>().sprite = fallSprite;

                isJumping = true;
                jumpTime = jumpStartTime;
                //rb.velocity = Vector2.up * jumpForce;
                rb.velocity = new Vector2(rb.velocity.x, jumpForce);
            }
    
            if ((Input.GetKey(JumpKey) || Input.GetKey(AltJump)) && isJumping == true)
            {
                if (jumpTime > 0)
                {
                    //rb.velocity = Vector2.up * jumpForce;
                    rb.velocity = new Vector2(rb.velocity.x, jumpForce);
                    jumpTime -= Time.deltaTime;
                }
                else
                {
                    isJumping = false;
                }
            }
    
            if (Input.GetKeyUp(JumpKey) || Input.GetKeyUp(AltJump))
            {
                isJumping = false;
            }
        }

        
        /*
        if(animator.enabled == false && isGrounded)
        {
            animator.enabled = true;
            animator.SetBool("Jumping", false);
        }
        */
    }

    void OnLand()
    {
        //animator.enabled = true;
        animator.SetBool("Jumping", false);
    }
 
    private void Flip()
	{
		// Switch the way the player is labelled as facing.
		facingRight = !facingRight;

		// Multiply the player's x local scale by -1.
		Vector3 theScale = transform.localScale;
		theScale.x *= -1;
		transform.localScale = theScale;
	}
 
    void OnDrawGizmos()
    {
        Gizmos.DrawWireSphere(feetPos.position, checkRadius);
    } 
    
}
