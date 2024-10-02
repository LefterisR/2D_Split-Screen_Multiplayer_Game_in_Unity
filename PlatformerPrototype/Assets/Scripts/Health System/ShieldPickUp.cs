using System.Collections;
using System.Collections.Generic;
using UnityEngine;

//Author Rizos Eleftherios
public class ShieldPickUp : MonoBehaviour
{
    [SerializeField]
    private float shieldBonus = 5f;

    public GameObject shieldPickUpEffect;
    public AudioClip shieldPickUpSound;
    public AudioSource audioSource;
    private void OnTriggerEnter2D(Collider2D collision)
    {
        if (collision.gameObject.CompareTag(TagHandler.Player1) || collision.gameObject.CompareTag(TagHandler.Player2))
        {
            float playerShield = collision.gameObject.GetComponent<PlayerHealth>().shield;
            float playerMaxShield = collision.gameObject.GetComponent<PlayerHealth>().maxShield;

            if (playerShield != playerMaxShield)
            {

                //Instantiate(shieldPickUpEffect, transform.position, Quaternion.identity);

                if (shieldPickUpEffect != null)
                {
                    // Instantiate the impact effect
                    GameObject effectInstance = Instantiate(shieldPickUpEffect, transform.position, Quaternion.identity);

                    // Get the Particle System component from the impact effect
                    ParticleSystem particleSystem = effectInstance.GetComponent<ParticleSystem>();

                    // Calculate the duration of the particle system's lifetime
                    float particleSystemDuration = particleSystem.main.duration + particleSystem.main.startLifetime.constant;

                    // Destroy the impact effect GameObject after the particle system's lifetime
                    Destroy(effectInstance, particleSystemDuration);
                }


                audioSource.clip = shieldPickUpSound;
                float sfxTime = shieldPickUpSound.length;
                audioSource.Play();
                GetComponent<SpriteRenderer>().enabled = false;
                GetComponent<Collider2D>().enabled = false;

                if ((playerShield + shieldBonus) > playerMaxShield)
                {
                    collision.gameObject.GetComponent<PlayerHealth>().shield = playerMaxShield;
                }
                else collision.gameObject.GetComponent<PlayerHealth>().shield += shieldBonus;

                Destroy(gameObject, sfxTime);
            }
        }
    }
}
