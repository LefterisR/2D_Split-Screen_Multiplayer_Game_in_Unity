using System;
using System.Collections;
using System.Collections.Generic;
using UnityEngine;
using UnityEngine.InputSystem;
using UnityEngine.SocialPlatforms;
using UnityEngine.UI;

//Author Rizos Eleftherios
public class KnightController : MonoBehaviour
{
    private Vector2 userInput = new(0, 0);

    [Header("Movement Values")]
    [SerializeField]
    private float runningSpeed = 9f;
    [SerializeField]
    private float baseSpeed = 9f;
    [SerializeField]
    private float maxSpeed = 18f;
    [SerializeField]
    private float accelerationPace = 0.3f;
    [SerializeField]
    private float airSpeed = 10f;

    [Header("Jump Data")]
    [SerializeField]
    private float jumpVelocity = 10;
    [SerializeField]
    private float descMultiplier = 1.1f;
    [SerializeField]
    private float ascendMultiplier = 1.4f;

    [Header("Dash")]
    [SerializeField]
    private float dashSpeed = 25f;
    [SerializeField]
    private float dashTime = 0.5f;
    [SerializeField]
    private float distanceBetweenImages = 0.2f;
    [SerializeField]
    private float dashCooldown = 2f;

    [Header("Dash After Image Effect")]
    [SerializeField]
    private float imageLifeTime = 1.4f;
    public SpriteRenderer afterImage;

    //SFX
    [Header("Knight SFX")]
    public AudioSource audioSourceGeneric;
    public AudioSource audioSourceRunning;
    public AudioClip knRunSFX;
    public AudioClip knJumpSFX;
    public AudioClip knDashSFX;


    private float dashRechargeCounter = 0f; //Dash ability cooldown character
    private float dashTimeLeft=0;             //Dashing frames counter
    private float lastAfterImagePos;        //Location of the last image of after 
    private bool _fire2Ready = true;

    //Components 
    Rigidbody2D rbKnight;
    //PhysicsMaterial2D physicsMaterial;
    Animator animator;
    EnvironmentData contact;
    SpriteRenderer knightSpriteRenderer;

    //Input System
    private PlayerInput playerInput;
    private InputActionAsset inputAsset;
    private InputActionMap player;

    public string activeActionMap;
    //Actions
    private InputAction move;

    private bool _isRunning = false;
    private bool _canMove = true;

//   private KnightController knightController;
//    private bool _isDashing = false;

    //Bool Property Fields
    public bool IsRunning
    {
        get { return _isRunning; }
        set
        {
            _isRunning = value;
            animator.SetBool(KnightAnimStrings.isRunning, value);
        }

    }

    public bool CanMove
    {
        get { return _canMove; }
        set
        {
            _canMove = value;
            animator.SetBool(KnightAnimStrings.canMove, value) ;

        }

    }
    /*
    public bool IsDashing 
    {
        get { return _isDashing; }
        set 
        {
            _isDashing = value;
        }
    }
    */
    private void Awake()
    {
        playerInput = GetComponent<PlayerInput>();
        inputAsset = playerInput.actions;
        player = inputAsset.FindActionMap(activeActionMap);

        rbKnight = GetComponent<Rigidbody2D>();
        animator = GetComponent<Animator>();
        contact = GetComponent<EnvironmentData>();
    
        knightSpriteRenderer = GetComponent<SpriteRenderer>();  
    }

    private void OnEnable()
    {
        move = player.FindAction("Move");
        move.Enable();

        player.FindAction("Jump").started += DoJump; //Subscribe Function
        player.FindAction("Jump").Enable();

        player.FindAction("Fire2").started += Fire2;
        player.FindAction("Fire2").Enable();


    }
    private void OnDisable()
    {
        move.Disable();
        player.FindAction("Jump").Disable();
        player.FindAction("Fire2").Disable();
    }

    private void Fire2(InputAction.CallbackContext context)
    {
        if (_fire2Ready) 
        {
            //rbKnight.velocity = new Vector2(dashSpeed * transform.localScale.x, rbKnight.velocity.y);
            audioSourceGeneric.PlayOneShot(knDashSFX, 0.5f);
            dashRechargeCounter = dashCooldown;
            dashTimeLeft = dashTime;
            AfterImageEffect();
            lastAfterImagePos = transform.position.x;
            
        }
    }

 

    private void DoJump(InputAction.CallbackContext context)
    {
        if (IsGrounded())
        {
            audioSourceGeneric.PlayOneShot(knJumpSFX,0.5f);
            rbKnight.velocity = new(0, jumpVelocity);
        }
    }

    private void FixedUpdate()
    {
        Accelerate(userInput.x);
        ImplementArcadeJumpPh();
    }
    // Update is called once per frame
    void Update()
    {
        if (IsRunning && IsGrounded())
        {
            audioSourceRunning.enabled = true;
        }
        else 
        {
            audioSourceRunning.Pause();
            audioSourceRunning.enabled = false;
        }
        if (CanMove)
        {
            userInput = move.ReadValue<Vector2>();

            rbKnight.velocity = new(runningSpeed * userInput.x, rbKnight.velocity.y);

            FlipSprite(userInput.x);

            if (!IsGrounded())  //On Air
            {
                rbKnight.velocity = new(airSpeed * userInput.x, rbKnight.velocity.y);
            }

            //Debug.Log("Current Knight Speed: " + runningSpeed);

            animator.SetFloat(KnightAnimStrings.yVelocity, rbKnight.velocity.y);

            if (rbKnight.velocity.x != 0)
            {
                IsRunning = true;
            }
            else
            {
                IsRunning = false;
            }

            if (dashRechargeCounter <= 0)
            {
                _fire2Ready = true;
            }
            else
            {
                _fire2Ready = false;
                dashRechargeCounter -= Time.deltaTime;
            }

        }

        if (contact.HitWall && !contact.TouchGround)
        {
            rbKnight.velocity = new(0, rbKnight.velocity.y);
        }


        if (dashTimeLeft>0)
        {
            //Debug.Log(transform.localScale.x);
            rbKnight.velocity = new Vector2(dashSpeed * transform.localScale.x, rbKnight.velocity.y);
            dashTimeLeft -= Time.deltaTime;

            if (Mathf.Abs(transform.position.x - lastAfterImagePos) > distanceBetweenImages)
            {
                AfterImageEffect();
                lastAfterImagePos = transform.position.x;
            }
            CanMove = false;
        }
        else CanMove = true;

        
    }

    private bool IsGrounded()
    {
        return contact.TouchGround;
    }

    private void ImplementArcadeJumpPh()
    {
        if (rbKnight.velocity.y < 0 && !contact.TouchGround)
        {
            rbKnight.velocity += (descMultiplier - 1) * Physics2D.gravity.y * Vector2.up;
        }
        else if (rbKnight.velocity.y > 0 && !contact.TouchGround)
        {
            rbKnight.velocity += (ascendMultiplier - 1) * Physics2D.gravity.y * Vector2.up;
        }
    }
    private void Accelerate(float inputX)
    {
        if (inputX != 0 && IsGrounded())
        {
            if (runningSpeed < maxSpeed) IncreaseSpeed();
        }
        else
        {
            runningSpeed = baseSpeed;
        }
    }
    private void IncreaseSpeed()
    {
        runningSpeed += accelerationPace;
    }

    //Spawn and Despawn After Image
    private void AfterImageEffect()
    {
        SpriteRenderer image = Instantiate(afterImage, transform.position, transform.rotation);
        image.sprite = knightSpriteRenderer.sprite;
        image.transform.localScale = knightSpriteRenderer.transform.localScale;
        image.color = new Color(1f, 1f, 1f, 0.75f); //RBGA

        Destroy(image.gameObject, imageLifeTime);
    }

    private void FlipSprite(float inputX) 
    {
        if (inputX > 0f)
        {
            transform.localScale = Vector3.one;
        }
        else if (inputX < 0f)
        {

            transform.localScale = new Vector3(-1f, 1f, 1f);
        }
    }
}

