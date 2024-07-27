using System.Collections;
using System.Collections.Generic;
using UnityEngine;

public class LeverTrapLogic : MonoBehaviour
{
    // Start is called before the first frame update

    [SerializeField]
    private Rigidbody2D spikeBallRb;

    [SerializeField]
    private Sprite trapUsedSprite;

    private SpriteRenderer trapRenderer;

    private void Start()
    {
        trapRenderer = GetComponent<SpriteRenderer>();  
    }

    private void OnTriggerEnter2D(Collider2D collision)
    {
        if(collision.CompareTag(TagHandler.Player1) || collision.CompareTag(TagHandler.Player2)) 
        {
            spikeBallRb.GetComponent<Collider2D>().enabled = true;
            //Debug.Log(collision.gameObject.name);
            spikeBallRb.gravityScale = 2.0f;
            trapRenderer.sprite = trapUsedSprite;
        }
    }
}
