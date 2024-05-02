using System;
using System.Collections;
using System.Collections.Generic;
using UnityEngine;
using UnityEngine.InputSystem;

public class MageCombatController : MonoBehaviour
{
    Animator animator;
    EnvironmentData contact;
    Rigidbody2D rb;
    MageController mageController;

    //Melee attack 
    [Header("Melee Attack")]
    private float timeBetweenMelee=0;
    public float meleeAttackTime = 1f;
    private bool _fire1Ready = true;

    //Range attack 
    [Header("Range Attack")]
    private float timeBetweenShoots=0;
    [SerializeField]
    private float shootAttackTime = 1f;
    public Transform firePoint;
    public MagicProjectileController magicProjectile;
    private bool _fire2Ready = true;


    //Input Handle
    private PlayerInput playerInput;
    private InputActionAsset inputAsset;
    private InputActionMap player;

    public string activeActionMap;
    private void Awake()
    {
        animator = GetComponent<Animator>();
        contact = GetComponent<EnvironmentData>();

        playerInput = GetComponent<PlayerInput>();
        inputAsset = playerInput.actions;
        player = inputAsset.FindActionMap(activeActionMap);

        rb = GetComponent<Rigidbody2D>();
        mageController = GetComponent<MageController>();

    }
   

    private void OnEnable()
    {   
        player.FindAction("Fire1").started += Fire1;
        player.FindAction("Fire1").Enable();

        player.FindAction("Fire2").started += Fire2;
        player.FindAction("Fire2").Enable();
    }

    private void OnDisable()
    {
        player.FindAction("Fire1").Disable();
        player.FindAction("Fire2").Disable();
    }

    private void Fire2(InputAction.CallbackContext context)
    {
        if (animator.GetBool(MageAnimStrings.canFire2) && IsGrounded() && _fire2Ready)
        {
            timeBetweenMelee = meleeAttackTime;
            animator.SetTrigger(MageAnimStrings.fire2Trigger);
        }
    }

    private void Fire1(InputAction.CallbackContext context)
    {
        if (animator.GetBool(MageAnimStrings.canFire1) && IsGrounded() && _fire1Ready) 
        {
            timeBetweenShoots = shootAttackTime;
            animator.SetTrigger(MageAnimStrings.fire1Trigger);
        }
        Debug.Log("Clicked Pressed");
    }

    // Update is called once per frame
    void Update()
    {

        if (timeBetweenShoots <= 0)
        {
            _fire1Ready = true;
            mageController.CanMove = true;
        }
        else 
        {
            rb.velocity = Vector2.zero;
            mageController.CanMove = false;

            _fire1Ready = false;
            timeBetweenShoots -= Time.deltaTime;
        }

        if (timeBetweenMelee <= 0)
        {
            _fire2Ready = true;
            mageController.CanMove = true;
        }
        else 
        {
            rb.velocity = Vector2.zero;
            mageController.CanMove = false;

            _fire2Ready = false;
            timeBetweenMelee -= Time.deltaTime;
        }
    }

    private bool IsGrounded() 
    {
        return contact.TouchGround;
    }

  

    public void OnFire1SpawnProjectile()
    {
        Instantiate(magicProjectile, firePoint.position, firePoint.rotation).projectileDirection = new Vector2(transform.localScale.x, 0f);
    }
}
