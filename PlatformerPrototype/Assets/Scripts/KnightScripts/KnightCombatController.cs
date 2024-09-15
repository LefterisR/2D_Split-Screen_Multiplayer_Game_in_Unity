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
    private float baseDmgValue;

    private PlayerInput playerInput;
    private InputActionAsset inputAsset;
    private InputActionMap player;

    public string activeActionMap;

    private bool _fire1Ready = true;

    private float dmgBuffTimeCounter;
    private bool isDmgBuffActive = false;

    [Header("Combat Sfx")]
    public AudioSource audioSource;
    public AudioClip knAttackSFX;
    public AudioClip dmgBuffSfx;
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

        baseDmgValue = meleeDmg;

        Debug.Log("Enemy layer mask code int " +enemyLayerCode);
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
            knightController.IsRunning = false;
            audioSource.PlayOneShot(knAttackSFX, 0.8f);
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

        if (dmgBuffTimeCounter > 0)
        {
            dmgBuffTimeCounter -= Time.deltaTime;
        }

        if (isDmgBuffActive && dmgBuffTimeCounter <= 0) 
        {
            meleeDmg = baseDmgValue;
            isDmgBuffActive = false;
            Debug.Log(meleeDmg);
        }



    }


    //Activate damage buff
    private void OnTriggerEnter2D(Collider2D collision)
    {
        if (collision.CompareTag(TagHandler.DamageBuff)) 
        {
            audioSource.PlayOneShot(dmgBuffSfx, 0.8f);
            dmgBuffTimeCounter = BuffData.damageBuffTime;
            isDmgBuffActive = true;
            meleeDmg += BuffData.damageBuff;

            collision.GetComponent<DamageBuffBehaviour>().DestroyDamagePotion();

            Debug.Log(meleeDmg);

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
        Collider2D[] enemy = Physics2D.OverlapCircleAll(meleeAttackPoint.position, scanRadius);
        
        foreach (Collider2D enemyEntity in enemy)
        {
            if (enemyEntity != null)
            {

                if (enemyEntity.isTrigger) continue;

                if (enemyEntity.gameObject.layer == enemyLayerCode) 
                {
                    enemyEntity.GetComponent<PlayerHealth>().TakeDamage(meleeDmg);
                    Debug.Log(enemyEntity.name);
                }
                if(enemyEntity.gameObject.layer == LayersHandler.Mobs) 
                {
                    Debug.Log(enemyEntity.name);
                    enemyEntity.GetComponent<SlimeBehaviour>().TakeDamage(meleeDmg);
                }
                if (enemyEntity.gameObject.layer == LayersHandler.Crate) 
                {
                    Debug.Log(enemyEntity.name);
                    enemyEntity.GetComponent<BreakableCrateLogic>().BrakeCrate(meleeDmg);
                }
                if (enemyEntity.gameObject.layer == LayersHandler.Archer) 
                {
                    enemyEntity.GetComponent<ArcherBehaviour>().TakeDamage(meleeDmg);
                }
            }
        }

    }

    private void OnDrawGizmos()
    {
        Gizmos.DrawWireSphere(meleeAttackPoint.position, scanRadius);
    }

}
