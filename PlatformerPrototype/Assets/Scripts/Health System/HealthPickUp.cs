using System.Collections;
using System.Collections.Generic;
using UnityEngine;

public class HealthPickUp : MonoBehaviour
{
    [SerializeField]
    private float healthBonus = 10;

    public GameObject healthPickUpEffect;

    private void OnTriggerEnter2D(Collider2D collision)
    {
        if (collision.gameObject.CompareTag(TagHandler.Player1) || collision.gameObject.CompareTag(TagHandler.Player2)) 
        {
            float playerHealth = collision.gameObject.GetComponent<PlayerHealth>().health;
            float playerMaxHealth = collision.gameObject.GetComponent<PlayerHealth>().maxHealth;
            
            if (playerHealth!=playerMaxHealth)
            {

                // Instantiate(healthPickUpEffect, transform.position, Quaternion.identity);

                if (healthPickUpEffect != null)
                {
                    // Instantiate the impact effect
                    GameObject effectInstance = Instantiate(healthPickUpEffect, transform.position, Quaternion.identity);

                    // Get the Particle System component from the impact effect
                    ParticleSystem particleSystem = effectInstance.GetComponent<ParticleSystem>();

                    // Calculate the duration of the particle system's lifetime
                    float particleSystemDuration = particleSystem.main.duration + particleSystem.main.startLifetime.constant;

                    // Destroy the impact effect GameObject after the particle system's lifetime
                    Destroy(effectInstance, particleSystemDuration);
                }

                if ((playerHealth + healthBonus) > playerMaxHealth)
                {
                    collision.gameObject.GetComponent<PlayerHealth>().health = playerMaxHealth;
                }
                else collision.gameObject.GetComponent<PlayerHealth>().health += healthBonus;

                Destroy(gameObject);
            }
        }
    }
}
