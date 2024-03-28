using System.Collections;
using System.Collections.Generic;
using UnityEngine;

public class MagicProjectileController : MonoBehaviour
{
    [SerializeField] 
    private float projectileSpeed = 15;

    public Vector2 projectileDirection;

    // public play on Impact
    public GameObject impactEffect;
    
    private Rigidbody2D rb;
    private new Collider2D collider2D;
    private void Awake()
    {
        rb = GetComponent<Rigidbody2D>();
        collider2D = GetComponent<Collider2D>();
    }
    // Start is called before the first frame update

    private void OnTriggerEnter2D(Collider2D collision)
    {
        if (collision.CompareTag(TagHandler.SolidObject)) 
        {
            //Play destruction anim
            Debug.Log("Projectile Collided with wall");
            
        }


        if(impactEffect != null)
        {
            // Instantiate the impact effect
            GameObject effectInstance = Instantiate(impactEffect, transform.position, Quaternion.identity);

            // Get the Particle System component from the impact effect
            ParticleSystem particleSystem = effectInstance.GetComponent<ParticleSystem>();

            // Calculate the duration of the particle system's lifetime
            float particleSystemDuration = particleSystem.main.duration + particleSystem.main.startLifetime.constant;

            // Destroy the impact effect GameObject after the particle system's lifetime
            Destroy(effectInstance, particleSystemDuration);
        }


        Destroy(gameObject);

    }

    void Update()
    {
        rb.velocity = projectileDirection * projectileSpeed;
    }

    
    private void OnBecameInvisible()
    {
        Destroy(gameObject);
    }

}
