using System.Collections;
using System.Collections.Generic;
using Unity.PlasticSCM.Editor.WebApi;
using UnityEngine;

[RequireComponent(typeof(Rigidbody2D))]
public class MageController : MonoBehaviour
{
    private Vector2 input = new(0, 0);

    [SerializeField]
    private float runningSpeed = 7.5f;
    [SerializeField]
    private float baseSpeed = 7.5f;
    [SerializeField]
    private float maxSpeed = 16f;
    [SerializeField]
    private float accelerationPace = 0.25f;
    [SerializeField]
    private float airSpeed = 7.5f;

    [Header("Jump Data")]
    [SerializeField]
    private float jumpVelocity = 10;
    [SerializeField]
    private float descMultiplier = 2.55f;
    [SerializeField]
    private float ascendMultiplier = 2f;
   


    //Components 
    Rigidbody2D rbMage;
    PhysicsMaterial2D physicsMaterial;
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
        physicsMaterial = GetComponent<Collider2D>().sharedMaterial;   
        animator = GetComponent<Animator>();
        contact = GetComponent<EnvironmentData>();

    }
    // Start is called before the first frame update
    void Start()
    {

    }


    private void IncreaseSpeed(float pace)
    {
        runningSpeed += pace;
    }

    private void FixedUpdate()
    {
        if (CanMove)
        {   //Get Input Direction
            input = new(Input.GetAxisRaw(InputFields.HorizontalAxis), 0);
            Vector2 inputNormalized = input.normalized;

            //Ground
            if (inputNormalized.x != 0 && contact.TouchGround)
            {
                if (runningSpeed < maxSpeed) IncreaseSpeed(accelerationPace);
            }
            else
            {
                runningSpeed = baseSpeed;
            }

            Debug.Log("Current speed: " + runningSpeed);
            rbMage.velocity = new(runningSpeed * inputNormalized.x, rbMage.velocity.y);

            //Air 
            if (!contact.TouchGround)
            {
                runningSpeed = airSpeed;
                rbMage.velocity = new(runningSpeed * inputNormalized.x, rbMage.velocity.y);
            }

            if (Input.GetButton(InputFields.Jump) && contact.TouchGround)
            {
                //Create momentum illusion
                Debug.Log("Space pressed");
                //rbMage.velocity = Vector2.up * jumpVelocity;
                rbMage.velocity = new(0, jumpVelocity);
            }

            

            animator.SetFloat(MageAnimStrings.yVelocity, rbMage.velocity.y);

            if (rbMage.velocity.x != 0)
            {
                IsRunning = true;
            }
            else
            {
                IsRunning = false;
            }

            //Prevent player glued on the wall

           

        }

        if (rbMage.velocity.y < 0 && !contact.TouchGround)
        {
            rbMage.velocity += (descMultiplier - 1) * Physics2D.gravity.y * Vector2.up;
        }
        else if (rbMage.velocity.y > 0 && !contact.TouchGround)
        {
            rbMage.velocity += (ascendMultiplier - 1) * Physics2D.gravity.y * Vector2.up;
        }

    }

    // Update is called once per frame
    void Update()
    {
        if (input.x > 0f)
        {
            transform.localScale = Vector3.one;
        }
        else if (input.x < 0f)
        {

            transform.localScale = new Vector3(-1f, 1f, 1f);
        }

        
            if (contact.HitWall && !contact.TouchGround)
            {
                rbMage.velocity = new(0, rbMage.velocity.y);
                physicsMaterial.friction=0;
                
                Debug.Log("On wall, friction set to +"+physicsMaterial.friction);
            }
            else 
            {
                physicsMaterial.friction=0.4f;
                Debug.Log("On ground, friction set to +" + physicsMaterial.friction);
               
            }

        


    }



}
