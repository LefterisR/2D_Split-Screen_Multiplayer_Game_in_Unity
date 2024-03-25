using System.Collections;
using System.Collections.Generic;
using UnityEngine;

public class KnightCombatController : MonoBehaviour
{
    Animator animator;
    EnvironmentData contact;

    KnightController knightController;
    Rigidbody2D rbKnight;

    [Header("Melee Attack")]
    [SerializeField]
    private float attackTime = 0.82f;
    private float timeBetweenMelee = 0f;

    [Header("Dash")]
    [SerializeField]
    private float dashCooldown = 4f; //4 seconds
    private float dashCooldownCounter = 0f; //Counter of the Dash Ability
    [SerializeField]
    private float dashingTime = 0.25f;  //Dash frames duration 
    [SerializeField]
    private float dashSpeed = 22f;      //Dashing speed
    private float timeBetweenDash = 0f; //Counter to keep track of the dashing life time frames
    
    


    private void Awake()
    {
        animator = GetComponent<Animator>();
        contact = GetComponent<EnvironmentData>();
        knightController = GetComponent<KnightController>();
        rbKnight = GetComponent<Rigidbody2D>();
    }

   
    // Update is called once per frame
    void Update()
    {
        if (timeBetweenMelee <= 0)
        {
            if (Input.GetButton(InputFields.Fire1) && contact.TouchGround)
            {
                animator.SetTrigger(KnightAnimStrings.meleeAttackTrigger);
                timeBetweenMelee = attackTime;
            }

        }
        else 
        {
            timeBetweenMelee-= Time.deltaTime;
            Debug.Log("Time between melee:" + timeBetweenMelee);
            rbKnight.velocity = Vector3.zero;
        }

        /*does not work yet
          if (dashCooldownCounter <= 0)
        {
            if (timeBetweenDash <= 0) //Dash ability frames
            {
                if (Input.GetButton(InputFields.Fire2))
                {
                    //Do Dash
                    rbKnight.velocity = new(dashSpeed*transform.localScale.x,rbKnight.velocity.y);
                    dashCooldownCounter = dashCooldown; //Player burnt the ability, start the clock
                    timeBetweenDash = dashingTime;
                    
                }
            }
            else 
            {
                timeBetweenDash -= Time.deltaTime; // Start the clock for the next dash
                Debug.Log("Dash Happens");
            }
           

        }
        else 
        {
            Debug.Log("Dush is under Cooldown " + dashCooldownCounter);
            dashCooldownCounter-=Time.deltaTime;
        
        }

         */




    }
}
