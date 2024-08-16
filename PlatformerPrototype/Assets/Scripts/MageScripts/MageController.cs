using System;
using System.Collections;
using System.Collections.Generic;
using UnityEngine;
using UnityEngine.InputSystem;


[RequireComponent(typeof(Rigidbody2D),typeof(Animator))]
public class MageController : MonoBehaviour
{
    private Vector2 userInput = new(0, 0);

    [Header("Movement Values")]
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

    [Header("Jump Data")]
    public AudioSource audioSourceGeneric;
    public AudioSource audioSourceRunning;
    public AudioClip mgRunSFX;
    public AudioClip mgJumpSFX;

    //Components 
    Rigidbody2D rbMage;
    //PhysicsMaterial2D physicsMaterial;
    Animator animator;
    EnvironmentData contact;

    //Input System
    private PlayerInput playerInput;
    private InputActionAsset inputAsset;
    private InputActionMap player;

    public string activeActionMap;

    //Actions
    private InputAction move;

    private bool _isRunning = false;
    private bool _canMove = true;

    //Bool Property Fields
    public bool IsRunning
    {
        get { return _isRunning; }
        set
        {
            _isRunning = value;
            animator.SetBool(MageAnimStrings.isRunning, value);
        }

    }
 
    public bool CanMove
    {
        get { return animator.GetBool(KnightAnimStrings.canMove);  }
        set
        {
           // _canMove = value;
            animator.SetBool(KnightAnimStrings.canMove, value);
            _canMove = animator.GetBool(KnightAnimStrings.canMove);

        }

    }
    private void Awake()
    {
        playerInput = GetComponent<PlayerInput>();
        inputAsset = playerInput.actions;
        player = inputAsset.FindActionMap(activeActionMap);

        rbMage = GetComponent<Rigidbody2D>();
        animator = GetComponent<Animator>();
        contact = GetComponent<EnvironmentData>();
    }
    

    private void OnEnable()
    {
        move = player.FindAction("Move");
        move.Enable();

        player.FindAction("Jump").started += DoJump; //Subscribe Function
        player.FindAction("Jump").Enable();

    }
    private void OnDisable()
    {
        move.Disable();
        player.FindAction("Jump").Disable();
    }

    private void DoJump(InputAction.CallbackContext context)
    {
        if (IsGrounded() && CanMove)
        {
            audioSourceGeneric.PlayOneShot(mgJumpSFX, 0.6f);
            rbMage.velocity = new(0, jumpVelocity);
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
       //if(!CanMove) Debug.Log(CanMove);
        if (CanMove)
        {
            userInput = move.ReadValue<Vector2>();

            FlipSprite(userInput.x);

            rbMage.velocity = new(runningSpeed * userInput.x, rbMage.velocity.y);

            //Mage is on Air

            if (!IsGrounded())
            {
                rbMage.velocity = new(airSpeed * userInput.x, rbMage.velocity.y);
            }

            //Debug.Log("Current Mage Speed: " + runningSpeed);


            animator.SetFloat(MageAnimStrings.yVelocity, rbMage.velocity.y);

            if (rbMage.velocity.x != 0)
            {
                IsRunning = true;
            }
            else
            {
                IsRunning = false;
            }

        }


        if (contact.HitWall && !contact.TouchGround)
        {
            rbMage.velocity = new(0, rbMage.velocity.y);
     
        }
    }

    public void SetActiveActionMap(string map) 
    {
        activeActionMap = map;
    }
    private bool IsGrounded()
    {
        return contact.TouchGround;
    }

    private void ImplementArcadeJumpPh()
    {
        if (rbMage.velocity.y < 0 && !contact.TouchGround)
        {
            rbMage.velocity += (descMultiplier - 1) * Physics2D.gravity.y * Vector2.up;
        }
        else if (rbMage.velocity.y > 0 && !contact.TouchGround)
        {
            rbMage.velocity += (ascendMultiplier - 1) * Physics2D.gravity.y * Vector2.up;
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
