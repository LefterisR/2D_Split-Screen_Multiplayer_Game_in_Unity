using System.Collections;
using System.Collections.Generic;
using UnityEngine;

//Author Rizos Eleftherios
public class DamageBuffBehaviour : MonoBehaviour
{
    public GameObject dmgBuffPickUpEffect;
    public void DestroyDamagePotion() 
    {
        if (dmgBuffPickUpEffect != null)
        {
            // Instantiate the impact effect
            GameObject effectInstance = Instantiate(dmgBuffPickUpEffect, transform.position, Quaternion.identity);

            // Get the Particle System component from the impact effect
            ParticleSystem particleSystem = effectInstance.GetComponent<ParticleSystem>();

            // Calculate the duration of the particle system's lifetime
            float particleSystemDuration = particleSystem.main.duration + particleSystem.main.startLifetime.constant;

            Debug.Log(particleSystemDuration);

            // Destroy the impact effect GameObject after the particle system's lifetime
            Destroy(effectInstance, particleSystemDuration);
        }

        Destroy(gameObject);
    }
}
