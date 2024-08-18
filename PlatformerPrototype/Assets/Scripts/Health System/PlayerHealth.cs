using System.Collections;
using System.Collections.Generic;
using Unity.VisualScripting;
using UnityEngine;

public class PlayerHealth : MonoBehaviour
{
    // Start is called before the first frame update
    public float shield;
    public float maxShield = 40;

    public float health;
    public float maxHealth = 100;

    //[SerializeField]
    //private bool isAlive = true;
    [SerializeField]
    private float spikeDmg = 2f;
    [SerializeField]
    private float spikeBallDmg = 8.5f;

    [Header("invicibility time")]
    [SerializeField]
    private float invincibilityTime = 1;
    private float invincCounter=0;
    [SerializeField]
    private float flashTime = 0.2f;
    private float flashCounter = 0;
    public Color flashColor = new Color(1, 0.22f, 0.22f, 1);
    private Color normalColor = new Color(1, 1, 1, 1);

    //Spikes
    private bool playerInSpikes = false;

    public GameObject damageShieldEffect;
    public GameObject damageHealthEffect;

    private SpriteRenderer playerSprite;

    public GameObject deathRayEffect;

    private void Awake()
    {
        playerSprite = GetComponent<SpriteRenderer>();
    }
    void Start()
    {
        shield = maxShield;
        health = maxHealth;
    }

    public void TakeDamage( float damageAmount ) 
    {

        if (invincCounter <= 0)
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
            else if (shield <= 0)
            {
                if (health - damageAmount < 0) health = 0;
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
            invincCounter = invincibilityTime;
        }
        else 
        {
            Debug.Log("i frames");
        }



        if (health <= 0)
        {
            //isAlive = false;
            Vector3 disp = new(0, 1, 0);
            Instantiate(deathRayEffect, transform.position + disp, Quaternion.identity);
            Destroy(gameObject);
            
        }
    }

    //Healh pick up

    private void OnCollisionEnter2D(Collision2D collision)
    {
        if (collision.gameObject.CompareTag(TagHandler.Spikes)) 
        {
            Debug.Log("Player in Spikes");
            TakeDamage(spikeDmg);
            playerInSpikes = true;
        }

        if (collision.gameObject.CompareTag(TagHandler.SpikeBall))
        {
            Debug.Log("Player on Spike Ball");
            TakeDamage(spikeBallDmg);
        }

    }

    private void OnCollisionExit2D(Collision2D collision)
    {

        if (collision.gameObject.CompareTag(TagHandler.Spikes))
        {
           
            playerInSpikes = false;
        }
    }
    private void FixedUpdate()
    {
        if (playerInSpikes) TakeDamage(spikeDmg);

       
    }

    private void Update()
    {
        if (invincCounter > 0)
        {
            invincCounter -= Time.deltaTime;
            flashCounter -=Time.deltaTime;
           // Debug.Log(flashCounter);

            if (playerSprite.sprite != null)
            {
                if (flashCounter <= 0)
                {
                    //Debug.Log(flashCounter);

                    playerSprite.color = flashColor;
                    flashCounter = flashTime;
                }
                else 
                {
                    playerSprite.color = normalColor;
                }

            }

            
        }

        if (invincCounter <= 0) 
        {
            playerSprite.color = normalColor;
            flashCounter = 0;
        }
    }

}
