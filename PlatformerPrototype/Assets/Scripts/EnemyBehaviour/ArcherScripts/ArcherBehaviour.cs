using System.Collections;
using System.Collections.Generic;
using UnityEngine;

public class ArcherBehaviour : MonoBehaviour
{
    public Collider2D targetArea;
    public Transform firePoint;

    private Animator animator;
    private AudioSource audioSource;
    private bool isLockedOn = false;
    private readonly float reloadTime = 1.5f;
    private float timer = 0;

    private GameObject focusedPlayer = null;
    private Transform lastKnownEnemyTransform;


    // Start is called before the first frame update
    void Start()
    {
        animator = GetComponent<Animator>();
        audioSource = GetComponent<AudioSource>();
    }

    // Update is called once per frame
    void Update()
    {
        if (isLockedOn) 
        {

            timer += Time.deltaTime;
            
            if (timer >= reloadTime)
            {
                timer = 0;
                animator.SetTrigger("fire");
                audioSource.Play();
            }

        }
        else 
        {
            focusedPlayer = null;
        }
        
        if (focusedPlayer != null) 
        {
            if (transform.position.x - focusedPlayer.transform.position.x > 0)
            {
                transform.localScale = new(-1, 1, 1);
            }
            else
            {
                transform.localScale = new(1, 1, 1);
            }
        }
       

    }

    private void OnTriggerEnter2D(Collider2D collision)
    {
        if (collision.CompareTag(TagHandler.Player1) || collision.CompareTag(TagHandler.Player2)) 
        {
            focusedPlayer = collision.gameObject;

            if(transform.position.x - collision.transform.position.x > 0)
            {
                transform.localScale = new(-1, 1, 1);
            }
            else 
            {
                transform.localScale = new(1, 1, 1);
            }

            Vector3 direction = collision.transform.position - targetArea.transform.position;
            float rot = Mathf.Atan2(-direction.y, -direction.x);
        
            transform.rotation = Quaternion.Euler(0,0,rot+3.5f);
            isLockedOn = true;
        }
    }

    private void OnTriggerExit2D(Collider2D collision)
    {
        if (collision.CompareTag(TagHandler.Player1) || collision.CompareTag(TagHandler.Player2))
        {
            lastKnownEnemyTransform = collision.transform;
            transform.rotation = Quaternion.Euler(0, 0, 0);
            isLockedOn = false;
            timer = 0;
        }
    }

    public void FireArrow() 
    {
        if (focusedPlayer != null) 
        {
            GameObject arrow = ArcherObjectPool.instance.GetPooledObject();
            arrow.transform.position = firePoint.position;
            arrow.GetComponent<Arrow>().target = focusedPlayer.transform;
            Debug.Log(focusedPlayer.name);
            arrow.SetActive(true);
        }
        else 
        {
            GameObject arrow = ArcherObjectPool.instance.GetPooledObject();
            arrow.transform.position = firePoint.position;
            arrow.GetComponent<Arrow>().target = lastKnownEnemyTransform;
            Debug.Log("Exited radius upon targeting");
            arrow.SetActive(true);
        }
        
    }
}
