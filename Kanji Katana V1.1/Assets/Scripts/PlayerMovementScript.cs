using System.Collections;
using System.Collections.Generic;
using UnityEngine;

public class PlayerMovementScript : MonoBehaviour
{
    [Header("Movement")]
    private float moveSpeed;
    public float walkSpeed;
    public float sprintSpeed;

    public float groundDrag;
    private float velocity;

    [Header("Jumping")]
    [SerializeField] private float jumpButtonGracePeriod = 0.2f;
    public float jumpForce;
    public float jumpCooldown;
    public float airMultiplier;
    bool readyToJump;

    [Header("Crouching")]
    public float crouchSpeed;
    public float crouchYScale;
    private float startYScale;

    [Header("Keybinds")]
    public KeyCode jumpKey = KeyCode.Space;
    public KeyCode sprintKey = KeyCode.LeftShift;
    public KeyCode crouchKey = KeyCode.LeftControl;

    [Header("Ground Check")]
    public float playerHeight;
    public LayerMask whatIsGround;
    bool grounded;

    [Header("Slope Handling")]
    public float maxSlopeAngle;
    private RaycastHit slopeHit;

    public Transform orientation;

    ///[Header("Animation")]
    
    float horizontalInput;
    float verticalInput;

    Vector3 moveDirection;

    Rigidbody rb;

    private MovementState state;

    [Header("Animator Controller")]
    private Animator animator;
    public bool isWalking;
    public bool isSprinting;
    private bool isJumping;
    private bool isGrounded;
    private bool isFalling;
    private float? lastGroundedTime;
    private float? jumpButtonPressedTime;
    private int isFallingHash;
    private int isJumpingHash;
    private int isGroundedHash;
    public enum MovementState
    {
        idle,
        walking,
        sprinting,
        crouching,
        air
    }

    private void Start()
    {
        rb = GetComponent<Rigidbody>();
        rb.freezeRotation = true;
        readyToJump = true;

        startYScale = transform.localScale.y;
        animator = GameObject.Find("Y Bot@Idle").GetComponent<Animator>();
        isFallingHash = Animator.StringToHash("isFalling");
        isJumpingHash = Animator.StringToHash("isJumping");
        isGroundedHash = Animator.StringToHash("isGrounded");

    }
    private void Update()
    {
        //ground check
        
        //Physics.Raycast()
        Debug.Log(grounded);
        MyInput();
        SpeedControl();
        StateHandler();

        
    }

    private void FixedUpdate()
    {
        grounded = Physics.Raycast(transform.position, Vector3.down, .3f, whatIsGround); //
        //handle drag
        if (grounded)
        {
            isGrounded = true;
            rb.drag = groundDrag;
            lastGroundedTime = Time.time;
        }

        else
            rb.drag = 0;
        MovePlayer();
    }

    private void MyInput()
    {
        horizontalInput = Input.GetAxisRaw("Horizontal");
        verticalInput = Input.GetAxisRaw("Vertical");

        //when to jump
        if (Input.GetKeyDown(jumpKey) && readyToJump && grounded)
        {
            jumpButtonPressedTime = Time.time;
            animator.SetBool(isJumpingHash, true);
            isJumping = true;
            animator.SetBool(isGroundedHash, false);
            isGrounded = false;
            //Debug.Log("jump");
            readyToJump = false;
            Jump();

            Invoke(nameof(ResetJump), jumpCooldown);

        }
        else if (Time.time-lastGroundedTime >= 0.1)
        {
            animator.SetBool(isFallingHash, true);
            isFalling = true;
            animator.SetBool(isJumpingHash, false);
            isJumping = false;
            animator.SetBool("isGrounded", false);
            isGrounded = false;
            //if (Time.time - jumpButtonPressedTime <= jumpButtonGracePeriod)
            //{
            //    animator.SetBool("isJumping", true);
            //    isJumping = true;
            //    jumpButtonPressedTime = null;
            //    lastGroundedTime = null;
            //}
        }
        else if(grounded && isFalling)
        {
            animator.SetBool("isFalling", false);
            isFalling = false;
            animator.SetBool("isGrounded", true);
            isGrounded = true;
            animator.SetBool(isJumpingHash, false);
            isJumping = false;
        }
        //start crouch
        if (Input.GetKeyDown(crouchKey))
        {
            transform.localScale = new Vector3(transform.localScale.x, crouchYScale, transform.localScale.y);
            rb.AddForce(Vector3.down * 5f, ForceMode.Impulse);


        }

        //stop crouch
        if (Input.GetKeyUp(crouchKey))
        {
            transform.localScale = new Vector3(transform.localScale.x, startYScale, transform.localScale.z);
        }


    }

    private void StateHandler()
    {
        bool keyPressed = (horizontalInput !=0 || verticalInput !=0);

        //Mode - Crouching
        if (Input.GetKey(crouchKey))
        {
            state = MovementState.crouching;
            moveSpeed = crouchSpeed;
        }
        //Mode - sprinting
        else if (grounded && Input.GetKey(sprintKey))
        {
            state = MovementState.sprinting;
            moveSpeed = sprintSpeed;
            if(velocity >= 5 && keyPressed)
            {
                isSprinting = true;
            }
            else
            {
                isWalking = false;
                isSprinting = false;
            }
        }

        //Mode - Walking
        else if (grounded)
        {
            moveSpeed = walkSpeed;
            state = MovementState.walking;
            if (keyPressed)
            {
                isWalking = true;
                isSprinting = false;
            }
            else 
            {
                isWalking = false;
                isSprinting = false;
            }

            
        }

        

        //Mode - Air
        else
        {
            state = MovementState.air;
        }
        


    }

    private void MovePlayer()
    {
        moveDirection = orientation.forward * verticalInput + orientation.right * horizontalInput;
        Debug.Log("Slope " + OnSlope());

        /*
         REEADDD
        Next time, I want to get rid of .addForce, and just change the rb.velocity
        Use orientation and that new video tutorial in the playlist. Change orientation to point in the correct direction, or maybe make another empty object.
        Disable gravity when on a slope
         */
        if (OnSlope())
        {
            rb.useGravity = false;
            //rb.AddForce(GetSlopeMoveDirection() * moveSpeed * 20f, ForceMode.Force);
            if(!isJumping)
                rb.velocity = GetSlopeMoveDirection() * moveSpeed;
            //if (rb.velocity.y > 0)
            //    rb.AddForce(Vector3.down * 80f, ForceMode.Force);

        }

        else if (grounded) { 
            rb.useGravity = true; 
            //rb.AddForce(moveDirection.normalized * moveSpeed * 10f, ForceMode.Force);
            if(!isJumping)
                rb.velocity = moveDirection.normalized * moveSpeed;
        }

        else if (!grounded)
        {
            rb.useGravity = true;
            rb.AddForce(moveDirection.normalized * moveSpeed * 10f * airMultiplier, ForceMode.Force);
            //rb.velocity = (moveDirection.normalized * moveSpeed * 10f * airMultiplier);
        }

        //rb.useGravity = !OnSlope();
    }

    private void SpeedControl()
    {
        Vector3 flatVel = new Vector3(rb.velocity.x, 0f, rb.velocity.z);

        //limit velocity
        velocity = flatVel.magnitude;
        if (velocity > moveSpeed)
        {
            Vector3 limitedVel = flatVel.normalized * moveSpeed; //calculates what the velocity should be capped
            rb.velocity = new Vector3(limitedVel.x, rb.velocity.y, limitedVel.z); //actually applies the new limited velocity
        }

    }
    private void Jump()
    {
        //reset y velocity
        rb.velocity = new Vector3(rb.velocity.x, 0f, rb.velocity.z);
        rb.AddForce(transform.up * jumpForce, ForceMode.Impulse);
    }
    private void ResetJump()
    {
        readyToJump = true;
    }

    private bool OnSlope()
    {
        //if(Physics.Raycast(transform.position, Vector3.down, out slopeHit, playerHeight * .5f + .3f))
        if (Physics.Raycast(transform.position, Vector3.down, out slopeHit, .3f))
        {
            float angle = Vector3.Angle(Vector3.up, slopeHit.normal);
            return angle < maxSlopeAngle && angle != 0;
        }
        return false;
    }

    private Vector3 GetSlopeMoveDirection()
    {
        return Vector3.ProjectOnPlane(moveDirection, slopeHit.normal).normalized;
    }
    private void OnDrawGizmosSelected()
    {
        Gizmos.color = Color.red;
        Gizmos.DrawWireSphere(transform.position, 1f);

    }
}
