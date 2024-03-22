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
    private float timeBetweenAttackCounter;
    public float attackTime = 1f;

    //Range attack 
    [SerializeField] private float mageFire1AnimTime = 0.44f;
    private float spawnProjectileCounter;
    private float timeBetweenShoots;
    private float rateOfFire = 1.4f;
    public Transform firePoint;
    public MagicProjectileController magicProjectile;

    MageController mageController;
    
    private void Awake()
    {
        animator = GetComponent<Animator>();
        contact = GetComponent<EnvironmentData>();
        mageController = GetComponent<MageController>();
        
    }
    
    // Update is called once per frame
    void Update()
    {
        if (timeBetweenAttackCounter <= 0)
        {
            mageController.CanMove = true;

            if (Input.GetButton(InputFields.Fire2) && contact.TouchGround)
            {
                Debug.Log("Fire2!");
                animator.SetTrigger(MageAnimStrings.fire2Trigger);
                timeBetweenAttackCounter = attackTime;
                
                Debug.Log("Can move " + mageController.CanMove);
                timeBetweenAttackCounter = attackTime;
            }
            
        }
        else
        {
            Debug.Log(timeBetweenAttackCounter);
            timeBetweenAttackCounter -= Time.deltaTime;
            mageController.CanMove = false;
            mageController.GetComponent<Rigidbody2D>().velocity= Vector3.zero;


        }
        //Must check if mage did not perform attack one => attackTime == 1
        
        

        
    }
}
