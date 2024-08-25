using System.Collections;
using System.Collections.Generic;
using Unity.VisualScripting;
using UnityEditor;
using UnityEngine;
using UnityEngine.Experimental.Rendering.Universal;
using UnityEngine.Rendering.Universal;

public class MagicProjectileController : MonoBehaviour
{
    [SerializeField] 
    private float projectileSpeed = 25;
    private float projectileDmg = 1f;

    public Vector2 projectileDirection;
    public string enemyTagPC;

    // public play on Impact
    public GameObject impactEffect;
    
    private Rigidbody2D rb;
    private Light2D projectileLight;
    
    private const float lightPulseClock = 1.6f;
    private float _pulseCounter = lightPulseClock;

    private readonly float _highIntensity = 2f;

    private readonly float _lowIntensity = 0.7f;

    private void Awake()
    {
        gameObject.layer = 9;
        rb = GetComponent<Rigidbody2D>();
        projectileLight = GetComponent<Light2D>();
    }
    // Start is called before the first frame update

    private void OnTriggerEnter2D(Collider2D collision)
    {
        if (collision.CompareTag(TagHandler.SolidObject)) 
        {
            //Play destruction anim
            Debug.Log("Projectile Collided with wall");
            
        }

        if (collision.CompareTag(enemyTagPC))
        {
            collision.gameObject.GetComponent<PlayerHealth>().TakeDamage(projectileDmg);
            Debug.Log("Hit Enemy");
        }

        if (collision.CompareTag(TagHandler.Slime))
        {
            collision.GetComponent<SlimeBehaviour>().TakeDamage(projectileDmg);
        }

        if (collision.CompareTag(TagHandler.Crate)) 
        {
            collision.GetComponent<BreakableCrateLogic>().BrakeCrate(projectileDmg);
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


        //Destroy(gameObject);
        gameObject.SetActive(false);
    }

    void Update()
    {
        rb.velocity = projectileSpeed * projectileDirection;
        rb.transform.Rotate(0f, 0f, 15f);

        if (_pulseCounter >= 0.8)
        {
            projectileLight.intensity = _highIntensity;
            _pulseCounter -= Time.deltaTime;
        }
        else if (_pulseCounter > 0 && _pulseCounter < 0.8)
        {
            projectileLight.intensity = _lowIntensity;
            _pulseCounter -= Time.deltaTime;
        }
        else if (_pulseCounter <= 0) 
        {
            projectileLight.intensity = _highIntensity;
            _pulseCounter = lightPulseClock;
        }

       // Debug.Log(projectileLight.intensity);
    
    }

    
   // private void OnBecameInvisible()
   // {
   //     Destroy(gameObject);
    //}

    public void SetProjectileDmg(float baseDmg, float buff = 0, bool isBuffed = false) 
    {
        if (isBuffed)
        {
            projectileDmg = baseDmg + buff;
        }
        else 
        {
            projectileDmg = baseDmg;
        }
        
    }

}
