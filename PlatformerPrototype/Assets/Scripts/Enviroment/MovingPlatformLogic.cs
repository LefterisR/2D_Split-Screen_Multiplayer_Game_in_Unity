using System.Collections;
using System.Collections.Generic;
using Unity.VisualScripting;
using UnityEngine;

//Author Rizos Eleftherios
public class MovingPlatformLogic : MonoBehaviour
{
    // Start is called before the first frame update

    [SerializeField]
    private float movementSpeed = 10f;
    [SerializeField]
    private int startPoint;

    public Transform[] movePoints;
    private int i;
    private void Start()
    {
        transform.position = movePoints[startPoint].position;
    }
    // Update is called once per frame
    void Update()
    {
        float step = movementSpeed * Time.deltaTime;
        if (Vector2.Distance(transform.position, movePoints[i].position) < 0.02f)
        {
            i++;
            if (i == movePoints.Length)
            {
                i = 0;
            }
        }

        transform.position = Vector2.MoveTowards(transform.position, movePoints[i].position, step);
    
    }

    /*
     *   if(collision.transform.position.y > transform.position.y)
    {
        collision.transform.SetParent(transform);
    }
}
     */
    private void OnTriggerEnter2D(Collider2D collision)
    {
        if (collision.CompareTag(TagHandler.Player1) || collision.CompareTag(TagHandler.Player2))
        {
            if (collision.transform.position.y > transform.position.y)
            {
                collision.transform.SetParent(transform, true);
            }
        }   
    }

    private void OnTriggerExit2D(Collider2D collision)
    {
        if (collision.CompareTag(TagHandler.Player1) || collision.CompareTag(TagHandler.Player2))
        {

            collision.transform.SetParent(null);

        }
    }

}
