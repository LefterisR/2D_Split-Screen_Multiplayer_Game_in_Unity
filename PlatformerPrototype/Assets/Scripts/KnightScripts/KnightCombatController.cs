using System;
using System.Collections;
using System.Collections.Generic;
using UnityEngine;
using UnityEngine.InputSystem;

public class KnightCombatController : MonoBehaviour
{
    Animator animator;
    EnvironmentData contact;
    KnightController knightController;
    Rigidbody2D rb;

    [Header("Melee Attack")]
    [SerializeField]
    private float attackTime = 0.82f;
    private float timeBetweenMelee = 0f;

    private PlayerInput playerInput;
    private InputActionAsset inputAsset;
    private InputActionMap player;

    public string activeActionMap;

    private bool _fire1Ready = true;

    private void Awake()
    {
        animator = GetComponent<Animator>();
        contact = GetComponent<EnvironmentData>();
        knightController = GetComponent<KnightController>();
        rb = GetComponent<Rigidbody2D>();

        playerInput = GetComponent<PlayerInput>();
        inputAsset = playerInput.actions;
       player = inputAsset.FindActionMap(activeActionMap);

    }
    
    private void OnEnable()
    {
        player.FindAction("Fire1").performed += Fire1;
        player.FindAction("Fire1").Enable();
    }

    private void OnDisable()
    {
        player.FindAction("Fire1").Disable();
    }
    private void Fire1(InputAction.CallbackContext context)
    {
        if (_fire1Ready && IsGrounded()) 
        {
            animator.SetTrigger(KnightAnimStrings.meleeAttackTrigger);
            timeBetweenMelee = attackTime;
        }
    }

    void Update()
    {
        if (timeBetweenMelee <= 0)
        {
            _fire1Ready = true;
            knightController.CanMove = true;
        }
        else 
        {
            rb.velocity = Vector2.zero;
            knightController.CanMove = false;

            timeBetweenMelee -= Time.deltaTime;
            _fire1Ready = false;
            
            
        }
    }


    private bool IsGrounded() 
    {
        return contact.TouchGround;
    }

 
}
