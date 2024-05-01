using System;
using System.Collections;
using System.Collections.Generic;
using UnityEngine;
using UnityEngine.InputSystem;

public class MageCombatControllerNew : MonoBehaviour
{
    Animator animator;
    EnvironmentData contact;

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

    MageCombatControllerNew mageController;

    //Input Handle
    private PlayerInput playerInput;
    private InputActionAsset inputAsset;
    private InputActionMap player;

    [SerializeField]
    private string activeActionMap;
    private void Awake()
    {
        animator = GetComponent<Animator>();
        contact = GetComponent<EnvironmentData>();

        playerInput = GetComponent<PlayerInput>();
        inputAsset = playerInput.actions;
        player = inputAsset.FindActionMap(activeActionMap);

        mageController = GetComponent<MageCombatControllerNew>();

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

    // Start is called before the first frame update
    void Start()
    {
        
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
            mageController.GetComponent<Rigidbody2D>().velocity = Vector3.zero;
           
            _fire1Ready = false;
            timeBetweenShoots -= Time.deltaTime;
        }

        if (timeBetweenMelee <= 0)
        {
            _fire2Ready = true;

        }
        else 
        {
            mageController.GetComponent<Rigidbody2D>().velocity = Vector3.zero;
           
            _fire2Ready = false;
            timeBetweenMelee -= Time.deltaTime;
        }
    }

    private bool IsGrounded() 
    {
        return contact.TouchGround;
    }

    public void SetActiveActionMap(string map) 
    {
        activeActionMap = map;
    }

    public void OnFire1SpawnProjectile()
    {
        Instantiate(magicProjectile, firePoint.position, firePoint.rotation).projectileDirection = new Vector2(transform.localScale.x, 0f);
    }
}
