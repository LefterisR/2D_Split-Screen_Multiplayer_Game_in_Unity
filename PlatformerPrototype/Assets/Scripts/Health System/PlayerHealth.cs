using System.Collections;
using System.Collections.Generic;
using UnityEngine;

public class PlayerHealth : MonoBehaviour
{
    // Start is called before the first frame update
    public float shield;
    public float maxShield = 40;

    public float health;
    public float maxHealth = 100;

    [SerializeField]
    private bool isAlive = true;


    void Start()
    {
        shield = maxShield;
        health = maxHealth;
    }

    public void TakeDamage( float damageAmount ) 
    {

        if (shield >= 0)
        {
            if (shield - damageAmount < 0) shield = 0;
            else shield -= damageAmount;
            
        }
        else 
        {   if(health-damageAmount < 0) health = 0;
            else health -= damageAmount;
        }

        if(health <=0) isAlive = false;
    
    }

    //Healh pick up



}
