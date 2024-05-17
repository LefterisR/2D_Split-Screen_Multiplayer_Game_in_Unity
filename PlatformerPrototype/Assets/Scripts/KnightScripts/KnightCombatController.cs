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

    public int enemyLayerCode;
    private LayerMask enemyLayerMask;

    [Header("Melee Attack")]
    [SerializeField]
    private float attackTime = 0.82f;
    private float timeBetweenMelee = 0f;
    public Transform meleeAttackPoint;
    [SerializeField]
    private float scanRadius = 0.8f;
    [SerializeField]
    private float meleeDmg = 17f;

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
    private void Start()
    {
        enemyLayerMask = (1 << enemyLayerCode);
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

    //Attack Scan
    //A function used as an animator event, it is called upon a specific SINGLE fire1 frame
    public void AttackScanKnight()
    {
        //Detect overlapping enemies 
        Collider2D[] enemy = Physics2D.OverlapCircleAll(meleeAttackPoint.position, scanRadius, enemyLayerMask);

        foreach (Collider2D enemyEntity in enemy)
        {
            if (enemyEntity != null)
            {
                Debug.Log(enemyEntity.name);
                enemyEntity.GetComponent<PlayerHealth>().TakeDamage(meleeDmg);
            }
        }

    }

    private void OnDrawGizmos()
    {
        Gizmos.DrawWireSphere(meleeAttackPoint.position, scanRadius);
    }

}
