using System.Collections;
using System.Collections.Generic;
using Unity.VisualScripting;
using UnityEngine;

public class SlimeBehaviour : MonoBehaviour
{

    [SerializeField]
    private float speed = 2.5f;
    [SerializeField]
    private float health = 16;
    
    public float slimeDmg = 4f;

    public GameObject potionDrop;

    [SerializeField]
    private int patrolPoint1;

    public Transform[] patrolPointsArray;
    private int i;

    private bool isStunned = false;
    public float stunDuration = 0.35f;
    private float stunCounter = 0;
    
    private SpriteRenderer spriteRenderer;
    private Animator anim;
    public float deathAnimTime;
    private bool isAlive = true;

    public Color getDmgColor;

    private void Awake()
    {
        foreach (Transform point in patrolPointsArray)
        {
            point.SetParent(null);
        }
    }
    private void Start()
    {
        transform.position = patrolPointsArray[patrolPoint1].position;
        spriteRenderer = GetComponent<SpriteRenderer>();
        anim = GetComponent<Animator>();
    }

    void Update()
    {
        if (!isStunned && isAlive)
        {
            float step = speed * Time.deltaTime;

            if (Vector2.Distance(transform.position, patrolPointsArray[i].position) < 0.02f)
            {
                i++;
                if (i == patrolPointsArray.Length)
                {
                    i = 0;
                }
            }

            transform.position = Vector2.MoveTowards(transform.position, patrolPointsArray[i].position, step);

            //The current position has a greater x value than the one the slime wants to go (patrolPoint[i]). Hence the interest point is at the left relative to slimes current location
            //in x axis

            if (patrolPointsArray[i].position.x - transform.position.x < 0)
            {
                //Flip to face the left side
                transform.localScale = new(-0.5f, 0.5f, 1);
            }
            else
            {
                //POI is to slime's right
                transform.localScale = new(0.5f, 0.5f, 1);
            }

        }
        else if (stunCounter > 0 && isStunned)
        {
            stunCounter-=Time.deltaTime;
        }

        if (stunCounter <= 0 && isStunned) 
        {
            isStunned = false;
            spriteRenderer.color = new Color(1, 1, 1, 1);
        }

        if (health <= 0) 
        {
            
            isAlive = false;
            StartCoroutine(WaitToDestory());
        }

    }

    public void TakeDamage(float dmgAmount)
    {
        if (isAlive) 
        {
            isStunned = true;
            stunCounter = stunDuration;
            spriteRenderer.color = getDmgColor;
            health -= dmgAmount;

        }
    }

    private void OnTriggerEnter2D(Collider2D collision)
    {
        if (collision.CompareTag(TagHandler.Player1) || collision.CompareTag(TagHandler.Player2)) 
        {
            collision.GetComponent<PlayerHealth>().TakeDamage(slimeDmg);
        }
    }
    IEnumerator WaitToDestory()
    {
        anim.SetTrigger("death");

        yield return new WaitForSeconds(deathAnimTime);

        if(potionDrop != null)
        Instantiate(potionDrop, transform.position, Quaternion.identity);

        Destroy(gameObject);

    }

}
