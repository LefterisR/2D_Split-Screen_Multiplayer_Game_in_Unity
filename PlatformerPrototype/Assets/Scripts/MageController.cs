using System.Collections;
using System.Collections.Generic;
using UnityEngine;

public class MageController : MonoBehaviour
{
    private Vector2 input = new(0, 0);

    [SerializeField]
    private float runningSpeed = 8f;
    [SerializeField]
    private float airSpeed = 7.5f;
    [SerializeField]
    private float jumpForce;

    //Components 
    Rigidbody2D rbMage;
    Animator animator;

   

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


        speed = runningSpeed * inputNormalized.x;
        rbMage.velocity = new(speed, rbMage.velocity.y);
        SetSpriteOrientation(rbMage.velocity.x);

        if (rbMage.velocity != Vector2.zero)
        {
            IsRunning = true;
        }
        else
        {
            IsRunning = false;
        }
        
    }
}
