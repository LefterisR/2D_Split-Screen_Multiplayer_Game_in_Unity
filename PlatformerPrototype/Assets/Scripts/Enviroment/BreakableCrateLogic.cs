using System.Collections;
using System.Collections.Generic;
using Unity.VisualScripting;
using UnityEngine;

public class BreakableCrateLogic : MonoBehaviour
{
    public Sprite brokenCrateSprite;
    public AudioSource source;

    private SpriteRenderer sr;
    private Collider2D col;

    private bool isDestoyed = false;

    private float hp = 1f;

    //float disableScriptCountdown = 2.5f;



    // Start is called before the first frame update
    void Start()
    {
        sr = GetComponent<SpriteRenderer>();
        col = GetComponent<Collider2D>();
    }


    public void BrakeCrate(float dmg) 
    {
        if (!isDestoyed)
        {
            hp -= dmg;
            isDestoyed=true;
            source.Play();

            sr.sprite = brokenCrateSprite;

            col.enabled = false;

        }

    }
    /*
    private void Update()
    {
        if (isDestoyed) 
        {
            disableScriptCountdown -= Time.deltaTime;
        }

        if (disableScriptCountdown <= 0) 
        {
            gameObject.GetComponent<BreakableCrateLogic>().enabled = false;
        }
    }
    */

}
