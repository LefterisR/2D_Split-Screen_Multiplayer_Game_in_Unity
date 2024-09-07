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
    //MageController mageController;

    //Enemy Tag & Layer Code
    public string enemyTag;
    public int enemyLayerCode;
    private LayerMask enemyLayerMask;

    //Melee attack 
    [Header("Melee Attack")]
    public Transform meleeAttackPoint;
    [SerializeField]
    private float scanRadius = 0.8f;
    [SerializeField]
    private float meleeDmg = 5f;
    private float timeBetweenMelee=0;
    public float meleeAttackTime = 1f;
    private bool _fire1Ready = true;
    private float baseDmgValueMelee;

    //Range attack 
    [Header("Range Attack")]
    private float timeBetweenShoots=0;
    [SerializeField]
    private float shootAttackTime = 1f;
    [SerializeField]
    private float baseDmgValueRange;
    public Transform firePoint;
    public MagicProjectileController magicProjectile;
    private bool _fire2Ready = true;
   

    [Header("Combat SFX")]
    public AudioSource audioSource;
    public AudioClip mgStaffSfx;
    public AudioClip mgSpellSfx;
    public AudioClip dmgBuffSfx;

    private bool isDmgBuffActive = false;
    private float dmgBuffTimeCounter;

    //Input Handle
    private PlayerInput playerInput;
    private InputActionAsset inputAsset;
    private InputActionMap player;

    // private float damageTimeCounter;

    private MageController mageController;
    
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

    private void Start()
    {
        enemyLayerMask = (1<<enemyLayerCode);

        baseDmgValueMelee = meleeDmg;
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
            mageController.IsRunning = false;
            audioSource.PlayOneShot(mgStaffSfx, 0.8f);
            timeBetweenMelee = meleeAttackTime;
            animator.SetTrigger(MageAnimStrings.fire2Trigger);
        }
    }

    private void Fire1(InputAction.CallbackContext context)
    {
        if (animator.GetBool(MageAnimStrings.canFire1) && IsGrounded() && _fire1Ready) 
        {
            mageController.IsRunning = false;
            audioSource.PlayOneShot(mgSpellSfx, 0.8f);
            timeBetweenShoots = shootAttackTime;
            animator.SetTrigger(MageAnimStrings.fire1Trigger);
        }
       // Debug.Log("Clicked Pressed");
    }

    // Update is called once per frame
    void Update()
    {

        if (timeBetweenShoots <= 0)
        {
            _fire1Ready = true;
            
        }
        else 
        {
           // Debug.Log(rb.velocity);
           // Debug.Log("Time: "+timeBetweenShoots);
            rb.velocity = Vector2.zero;
            

            _fire1Ready = false;
            timeBetweenShoots -= Time.deltaTime;
        }

        if (timeBetweenMelee <= 0)
        {
            _fire2Ready = true;
            
        }
        else 
        {
            rb.velocity = Vector2.zero;
            

            _fire2Ready = false;
            timeBetweenMelee -= Time.deltaTime;
        }


        if (dmgBuffTimeCounter > 0)
        {
            dmgBuffTimeCounter -= Time.deltaTime;
        }

        if (isDmgBuffActive && dmgBuffTimeCounter <= 0)
        {
            meleeDmg = baseDmgValueMelee;
            isDmgBuffActive = false;
            Debug.Log(meleeDmg);
        }

    }



    private void OnTriggerEnter2D(Collider2D collision)
    {
        if (collision.CompareTag(TagHandler.DamageBuff))
        {
            audioSource.PlayOneShot(dmgBuffSfx, 0.8f);
            dmgBuffTimeCounter = BuffData.damageBuffTime;
            isDmgBuffActive = true;
            meleeDmg += BuffData.damageBuff;

            collision.GetComponent<DamageBuffBehaviour>().DestroyDamagePotion();

            Debug.Log("Melee damage buffed " + meleeDmg);
            Debug.Log("Projectile damage buffed " + meleeDmg);
        }
    }



    private bool IsGrounded() 
    {
        return contact.TouchGround;
    }

  
    //Spawn projectile at set animation frame
    public void OnFire1SpawnProjectile()
    {
        
        //MagicProjectileController mC = Instantiate(magicProjectile, firePoint.position, firePoint.rotation);
       
        
        GameObject projectile = ObjectPool.instance.GetPooledObject();
        MagicProjectileController mC = projectile.GetComponent<MagicProjectileController>();
        mC.projectileDirection = new Vector2(transform.localScale.x, 0f);
        mC.enemyTagPC = enemyTag;

        if (isDmgBuffActive)
        {
            mC.SetProjectileDmg(baseDmgValueRange, BuffData.damageBuff, isDmgBuffActive);
        }
        else mC.SetProjectileDmg(baseDmgValueRange);

        projectile.transform.position = firePoint.position;
        projectile.SetActive(true);

    }

    //Attack Scan
    //A function used as an animator event, it is called upon a specific SINGLE fire1 frame

    public void AttackScanMage() 
    {
        //Detect overlapping enemies 
        Collider2D[] enemy = Physics2D.OverlapCircleAll(meleeAttackPoint.position, scanRadius);

        foreach (Collider2D enemyEntity in enemy) 
        {
            if (enemyEntity.gameObject.layer == enemyLayerCode)
            {
                enemyEntity.GetComponent<PlayerHealth>().TakeDamage(meleeDmg);
                Debug.Log(enemyEntity.name);
            }
            if (enemyEntity.gameObject.layer == LayersHandler.Mobs)
            {
                Debug.Log(enemyEntity.name);
                enemyEntity.GetComponent<SlimeBehaviour>().TakeDamage(meleeDmg);
            }
            if (enemyEntity.gameObject.layer == LayersHandler.Crate)
            {
                Debug.Log(enemyEntity.name);
                enemyEntity.GetComponent<BreakableCrateLogic>().BrakeCrate(meleeDmg);
            }
        }
 
    }

    private void OnDrawGizmos()
    {
        Gizmos.DrawWireSphere(meleeAttackPoint.position, scanRadius);
    }


}
