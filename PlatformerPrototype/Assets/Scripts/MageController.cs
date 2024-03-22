using System.Collections;
using System.Collections.Generic;
using Unity.PlasticSCM.Editor.WebApi;
using UnityEngine;

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

    EnvironmentData contact;


    private bool _isRunning = false;
    private bool _isFacingRight = true;
    

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

    }
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


   
    // Update is called once per frame
    void Update()
    {
        float speed;
        input = new(Input.GetAxisRaw(InputFields.HorizontalAxis), 0);
        Vector2 inputNormalized = input.normalized;

        if (contact.TouchGround) speed = runningSpeed * inputNormalized.x;
        else speed = airSpeed * inputNormalized.x;



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
       
        if (Input.GetButton(InputFields.Jump) && contact.TouchGround)
        {
            
            rbMage.velocity = new(rbMage.velocity.x, jumpForce);
           
        }

        //Set Jump style
        if (rbMage.velocity.y > 0)
        {
            rbMage.gravityScale = 0.95f; //jump start 
        }
        else if (rbMage.velocity.y < 0) 
        {
            rbMage.gravityScale = 5f; //force rapid returnal speed
        }
        else rbMage.gravityScale = 1f; //reset default value

        if (contact.HitWall && !contact.TouchGround)
        {
            rbMage.velocity = new(0, rbMage.velocity.y);

        }

       
        SetSpriteOrientation(rbMage.velocity.x);












    }
}
