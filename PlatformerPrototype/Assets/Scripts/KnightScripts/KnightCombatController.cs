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


    }
}
