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

    public GameObject damageShieldEffect;
    public GameObject damageHealthEffect;


    void Start()
    {
        shield = maxShield;
        health = maxHealth;
    }

    public void TakeDamage( float damageAmount ) 
    {
        Debug.Log("Damage Taken");
        if (shield > 0)
        {
            if (shield - damageAmount < 0) shield = 0;
            else shield -= damageAmount;

            if (damageShieldEffect != null)
            {
                // Instantiate the impact effect
                GameObject effectInstance = Instantiate(damageShieldEffect, transform.position, Quaternion.identity);

                // Get the Particle System component from the impact effect
                ParticleSystem particleSystem = effectInstance.GetComponent<ParticleSystem>();

                // Calculate the duration of the particle system's lifetime
                float particleSystemDuration = particleSystem.main.duration + particleSystem.main.startLifetime.constant;

                // Destroy the impact effect GameObject after the particle system's lifetime
                Destroy(effectInstance, particleSystemDuration);
            }


        }
        else if(shield<=0)
        {   if(health-damageAmount < 0) health = 0;
            else health -= damageAmount;

            if (damageHealthEffect != null)
            {
                // Instantiate the impact effect
                GameObject effectInstance = Instantiate(damageHealthEffect, transform.position, Quaternion.identity);

                // Get the Particle System component from the impact effect
                ParticleSystem particleSystem = effectInstance.GetComponent<ParticleSystem>();

                // Calculate the duration of the particle system's lifetime
                float particleSystemDuration = particleSystem.main.duration + particleSystem.main.startLifetime.constant;

                // Destroy the impact effect GameObject after the particle system's lifetime
                Destroy(effectInstance, particleSystemDuration);
            }

        }

        if(health <=0) isAlive = false;
    
    }

    //Healh pick up



}
