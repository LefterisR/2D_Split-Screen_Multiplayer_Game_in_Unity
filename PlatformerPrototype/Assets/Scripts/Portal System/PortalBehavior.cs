using System.Collections;
using System.Collections.Generic;
using Unity.VisualScripting;
using Unity.VisualScripting.Antlr3.Runtime;
using Unity.VisualScripting.FullSerializer;
using UnityEngine;

[RequireComponent(typeof(SpriteRenderer))]
public class PortalBehavior : MonoBehaviour
{

    private HashSet<GameObject> portalAstonauts = new HashSet<GameObject>();  //Stores the Objects traversing through the portal

    [SerializeField]
    private Transform dest;

    private SpriteRenderer spriteRenderer;

    public Sprite[] portalSprites;

    private float animationTime = 0.2f;

    private float animationTimerCnd = 0;

    private int i = 0;

    private AudioSource audioSource;
    private void Start()
    {
        spriteRenderer = GetComponent<SpriteRenderer>();
        audioSource = GetComponent<AudioSource>();
        animationTimerCnd = animationTime;
    }


    private void Update()
    {
        if (spriteRenderer != null) 
        {

            if (animationTimerCnd <= 0)
            {

                
                if ((portalSprites.Length-1) == i)
                {
                    i = 0;
                    spriteRenderer.sprite = portalSprites[i];
                }
                else
                {
                    i++;
                    spriteRenderer.sprite = portalSprites[i];
                }

                Debug.Log(spriteRenderer.sprite.name);

                animationTimerCnd = animationTime;
            }
            else 
            {
                animationTimerCnd-=Time.deltaTime;
            }
        
        }
    }

    //An object triggers the portal
    private void OnTriggerEnter2D(Collider2D collision)
    {
        //if an object was already in portal (in portal hashset) do not traverse it again
        if (portalAstonauts.Contains(collision.gameObject)) 
        {
            return;
        }

        if (dest.TryGetComponent(out PortalBehavior endOfJourney)) 
        {   
            audioSource.Play();
            endOfJourney.portalAstonauts.Add(collision.gameObject); //Add the the hash set of the destination portal the traveller game object 
        }

        collision.transform.position = dest.position;
    }

    private void OnTriggerExit2D(Collider2D collision)
    {
        portalAstonauts.Remove(collision.gameObject); //Astonaut has left the starting portal
    }



}
