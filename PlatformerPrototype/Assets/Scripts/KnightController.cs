using System.Collections;
using System.Collections.Generic;
using UnityEngine;

public class KnightController : MonoBehaviour
{
    // Start is called before the first frame update
    private Vector2 input = new(0, 0);

    [SerializeField]
    private float runningSpeed = 8.5f;
    [SerializeField]
    private float airSpeed = 8f;
    [SerializeField]
    private float jumpForce = 12f;


    //Components 
    Rigidbody2D rbKnight;
    Animator animator;

    //Check enviroment contact directions
    EnvironmentData contact;


    private bool _isRunning = false;
    private bool _isFacingRight = true;


    private bool _canMove = true;

    public bool IsRunning
    {
        get { return _isRunning; }
        set
        {
            _isRunning = value;
            animator.SetBool(KnightAnimStrings.isRunning, value);
        }

    }

    public bool IsFacingRight
    {
        get { return _isFacingRight; }
        set
        {
            if (_isFacingRight != value)
            {
                //flip
                transform.localScale *= new Vector2(-1, 1);
            }
            _isFacingRight = value;

        }

    }


    public bool CanMove
    {
        get { return animator.GetBool(KnightAnimStrings.canMove); }
        set
        {
            _canMove = animator.GetBool(KnightAnimStrings.canMove);

        }

    }



    private void Awake()
    {
        //Retrive Components
        rbKnight = GetComponent<Rigidbody2D>();
        animator = GetComponent<Animator>();
        contact = GetComponent<EnvironmentData>();

    }
    // Start is called before the first frame update
    void Start()
    {
        rbKnight.gravityScale = 2f;
    }

    private void ConfigureGravityScale()
    {
        //Set Jump style
        if (rbKnight.velocity.y > 0)
        {
            rbKnight.gravityScale = 1.4f; //jump start 
           // Debug.Log("Current Gravity: " + rbKnight.gravityScale);
        }
        else if (rbKnight.velocity.y < 0)
        {
           // Debug.Log("Current Gravity: "+rbKnight.gravityScale);
            rbKnight.gravityScale = 6f; //force rapid returnal speed
        }
        else rbKnight.gravityScale = 2f; //reset default value
        

    }

    private void Jump()
    {
        rbKnight.velocity = new(rbKnight.velocity.x, jumpForce);
    }
    // Update is called once per frame
    void Update()
    {


        if (CanMove)
        {
            float speed;
            input = new(Input.GetAxisRaw(InputFields.HorizontalAxis), 0);
            Vector2 inputNormalized = input.normalized;

            if (contact.TouchGround) speed = runningSpeed * inputNormalized.x;
            else speed = airSpeed * inputNormalized.x;

            if (inputNormalized.x > 0f)
            {
                transform.localScale = Vector3.one;
            }
            else if (inputNormalized.x < 0f)
            {
                transform.localScale = new(-1f, 1f, 1f);
            }

            //  Debug.Log("Current speed: " + speed);
            rbKnight.velocity = new(speed, rbKnight.velocity.y);
            animator.SetFloat(KnightAnimStrings.yVelocity, rbKnight.velocity.y);

            if (rbKnight.velocity != Vector2.zero)
            {
                IsRunning = true;
            }
            else
            {
                IsRunning = false;
            }

            //Prevent player glued on the wall

            if (contact.HitWall && !contact.TouchGround)
            {
                rbKnight.velocity = new(0, rbKnight.velocity.y);
            }
            if (Input.GetButton(InputFields.Jump) && contact.TouchGround)
            {
                Jump();
            }
            ConfigureGravityScale();

        }







    }
}
