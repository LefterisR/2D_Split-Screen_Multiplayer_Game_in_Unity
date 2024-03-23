using System;
using System.Collections;
using System.Collections.Generic;
using Unity.VisualScripting;
using UnityEditor.Experimental.GraphView;
using UnityEngine;

public class MageCombatController : MonoBehaviour
{
    // Start is called before the first frame update
    Animator animator;
    EnvironmentData contact;

    //Melee attack 
    [Header("Melee Attack")]
    private float timeBetweenAttackCounter;
    public float meleeAttackTime = 1f;
   
    //Range attack 
    [Header("Range Attack")]
    //[SerializeField] private float fire1AnimTime = 0.44f;
    private float timeBetweenShoots;
    [SerializeField]
    private float shootAttackTime = 1f;
    public Transform firePoint;
    public MagicProjectileController magicProjectile;

    MageController mageController;


    private void Awake()
    {
        animator = GetComponent<Animator>();
        contact = GetComponent<EnvironmentData>();
        mageController = GetComponent<MageController>();
        
    }
    
    void OnFire1SpawnOrb() 
    {
        Instantiate(magicProjectile, firePoint.position, firePoint.rotation).projectileDirection = new Vector2(transform.localScale.x, 0f);
    }

    // Update is called once per frame
    void Update()
    {
        if (timeBetweenAttackCounter <= 0)
        {
            mageController.CanMove = true;

            if ( animator.GetBool(MageAnimStrings.canFire2) && Input.GetButton(InputFields.Fire2) && contact.TouchGround)
            {   
               // Debug.Log("Fire2!");
                animator.SetTrigger(MageAnimStrings.fire2Trigger);
                timeBetweenAttackCounter = meleeAttackTime;
                
               // Debug.Log("Can move " + mageController.CanMove);
                
                
            }
           
            
             
        }
        else
        {
           // Debug.Log(timeBetweenAttackCounter);
            timeBetweenAttackCounter -= Time.deltaTime;
            mageController.CanMove = false;
            mageController.GetComponent<Rigidbody2D>().velocity= Vector3.zero;


        }
        //Must check if mage did not perform attack one => attackTime == 1
        if (timeBetweenShoots <= 0) 
        {
            mageController.CanMove = true;

            if(animator.GetBool(MageAnimStrings.canFire1) && Input.GetButton(InputFields.Fire1) && contact.TouchGround) 
            {
                //Debug.Log("Fire1!");
                animator.SetTrigger(MageAnimStrings.fire1Trigger);
                timeBetweenShoots = shootAttackTime;

            }

            

        }
        else
        {
           // Debug.Log(timeBetweenShoots);
            timeBetweenShoots -= Time.deltaTime;
            mageController.CanMove = false;
            mageController.GetComponent<Rigidbody2D>().velocity = Vector3.zero;

        }



    }

    /*
     IEnumerator SpawnProjAtAnimExit()
    {
        yield return new WaitForSeconds(fire1AnimTime);
        Instantiate(magicProjectile, firePoint.position, firePoint.rotation).projectileDirection = new Vector2(transform.localScale.x,0f);
    }
     */

}

