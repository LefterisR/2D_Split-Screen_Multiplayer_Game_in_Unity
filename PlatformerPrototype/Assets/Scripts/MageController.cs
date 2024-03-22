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
    [SerializeField]
    private bool projectileFired = false;
    [SerializeField]
    private bool attack2Performed = false;
    //Components 
    Rigidbody2D rbMage;
    Animator animator;

    EnvironmentData contact;


    private bool _isRunning = false;
    private bool _isFacingRight = true;
    private bool _canMove = true;

    //Projectile
    public MagicProjectileController magicProjectile;
    public Transform firePoint;
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
        get { return _canMove; }
        set 
        {
            _canMove = value;
        
        }
    
    }

    public void OnFire1AnimationExit() 
    {
        CanMove = true;
        projectileFired = false;
        Debug.Log("On Exit called fire1");
    }
    public void OnFire2AnimationExit()
    {
        CanMove = true;
        attack2Performed = false;
        Debug.Log("On Exit called fire2");
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

        if (CanMove) 
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
                rbMage.gravityScale = 1.4f; //jump start 
            }
            else if (rbMage.velocity.y < 0)
            {
                rbMage.gravityScale = 6f; //force rapid returnal speed
            }
            else rbMage.gravityScale = 2f; //reset default value

            if (contact.HitWall && !contact.TouchGround)
            {
                rbMage.velocity = new(0, rbMage.velocity.y);

            }
            //To do, sync animation with projectile instaniate
            if (Input.GetButton(InputFields.Fire1) && contact.TouchGround &&!projectileFired) 
            {
                //Instantiate Magic Projectile
                Instantiate(magicProjectile, firePoint.position, firePoint.rotation).projectileDirection = new Vector2(transform.localScale.x, 0f);
                //Dectivate movement during fire anim
                CanMove = false;
                projectileFired = true;
                //Play Fire1 Animation
                animator.SetTrigger(MageAnimStrings.fire1Trigger);
            }
            /* Contains bugs toDo, lock move 
             * if (Input.GetButton(InputFields.Fire2) && contact.TouchGround && !attack2Performed) 
            { 
                CanMove = false;
                attack2Performed = true;

                animator.SetTrigger(MageAnimStrings.fire2Trigger);
            
            }*/


        }

        SetSpriteOrientation(rbMage.velocity.x);












    }
}
