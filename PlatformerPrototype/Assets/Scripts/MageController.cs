using System.Collections;
using System.Collections.Generic;
using Unity.PlasticSCM.Editor.WebApi;
using UnityEngine;

[RequireComponent(typeof(Rigidbody2D))]
public class MageController : MonoBehaviour
{
    private Vector2 input = new(0, 0);

    [SerializeField]
    private float runningSpeed = 8f;
    [SerializeField]
    private float airSpeed = 7.5f;
    [SerializeField]
    private float jumpForce = 10.8f;
    
    
    //Components 
    Rigidbody2D rbMage;
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
            animator.SetBool(MageAnimStrings.isRunning, value);
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
        get { return animator.GetBool(MageAnimStrings.canMove); }
        set 
        {
            _canMove = animator.GetBool(MageAnimStrings.canMove);
        
        }
    
    }

  

    private void Awake()
    {
        //Retrive Components
        rbMage = GetComponent<Rigidbody2D>();
        animator = GetComponent<Animator>();
        contact = GetComponent<EnvironmentData>();

    }
    // Start is called before the first frame update
    void Start()
    {
        rbMage.gravityScale = 2f;
    }

    //Function to flip the gameObject
    /*
    private void SetSpriteOrientation(float velocityX)
    {
        if (velocityX > 0 && !IsFacingRight)
        {
            IsFacingRight = true;
        }
        else if (velocityX < 0 && IsFacingRight)
        {
            IsFacingRight = false;
        }

    }
    */
    private void ConfigureGravityScale() 
    {
        //Set Jump style
        if (rbMage.velocity.y > 0)
        {
            rbMage.gravityScale = 1.4f; //jump start 
        }
        else if (rbMage.velocity.y < 0)
        {
            rbMage.gravityScale = 6f; //force rapid returnal speed
        }
        else rbMage.gravityScale = 2f; //reset default value


    }

    private void Jump() 
    {
        rbMage.velocity = new(rbMage.velocity.x, jumpForce);
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
                transform.localScale = new Vector3(-1f,1f,1f);
            }

            //  Debug.Log("Current speed: " + speed);
            rbMage.velocity = new(speed, rbMage.velocity.y);
            animator.SetFloat(MageAnimStrings.yVelocity, rbMage.velocity.y);

            if (rbMage.velocity != Vector2.zero)
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
                rbMage.velocity = new(0, rbMage.velocity.y);
            }
            if (Input.GetButton(InputFields.Jump) && contact.TouchGround)
            {
                Jump();
            }
            ConfigureGravityScale();

        }

       





    }
}
