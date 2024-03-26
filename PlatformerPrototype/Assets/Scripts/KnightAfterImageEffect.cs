using System.Collections;
using System.Collections.Generic;
using UnityEngine;

public class KnightAfterImageEffect : MonoBehaviour
{
    private Transform knightTransform;

    private SpriteRenderer spriteRender; //Altered Sprite for Effect
    private SpriteRenderer knightSprite; //Real time knight sprite feed.

    private Color color; //After image color effect

    //Color Effects
    private float alpha; //RGB_Alpha
    [SerializeField]
    private float alphaSet = 0.8f;
    private float alphaMultiplier = 0.85f;

    private float imageLifeTime = 1.5f;
    private float timeActivated; //After image life time counter

    private void OnEnable()
    {
        spriteRender = GetComponent<SpriteRenderer>();
        knightTransform = GameObject.FindGameObjectWithTag("Player").transform;
        knightSprite = knightTransform.GetComponent<SpriteRenderer>(); //Feed the Sprite Renderer

        alpha = alphaSet;
        spriteRender.sprite = knightSprite.sprite;
        transform.position = knightTransform.position;
        transform.rotation = knightTransform.rotation;

        timeActivated = Time.time; //Feed the time

    }


    private void Update()
    {
        alpha *= alphaMultiplier;

        color = new Color(1f, 1f, 1f, alpha); //RGBA

        spriteRender.color = color; //Set color to the after image

    }


}
