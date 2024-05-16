using System.Collections;
using System.Collections.Generic;
using UnityEngine;
using UnityEngine.UI;
using TMPro;

public class HealthUIHandler : MonoBehaviour
{
    public Sprite[] shields;

    [Header("Player 1 UI")]
    public Image player1ShieldIm;
    public TMP_Text p1ShieldValue;
    [Header("Player 2 UI")]
    public Image player2ShieldIm;
    //private int p1ShieldFractions;


    private PlayerHealth player1HealthScript;
    private float p1MaxShield;
    
    private PlayerHealth player2HealthScript;
    private float p2MaxShield;



    private void Start()
    {
        GameObject player1 = GameObject.FindGameObjectWithTag(TagHandler.Player1);
        GameObject player2 = GameObject.FindGameObjectWithTag(TagHandler.Player2);

        player1HealthScript = player1.GetComponent<PlayerHealth>();
        player2HealthScript = player2.GetComponent<PlayerHealth>();

        p1MaxShield = player1HealthScript.maxShield;
        p2MaxShield = player2HealthScript.maxShield;

        Debug.Log(p1MaxShield);
        Debug.Log(p2MaxShield);

    }

    private void UpdateP1ShieldIcon() 
    {
        if (player1HealthScript.shield == p1MaxShield)
        {
            player1ShieldIm.sprite = shields[0];
        }
        else if (player1HealthScript.shield > p1MaxShield / 2)
        {
            player1ShieldIm.sprite = shields[1];
        }
        else if (player1HealthScript.shield > 0f && player1HealthScript.shield <= p1MaxShield / 2)
        {
            player1ShieldIm.sprite = shields[2];
        }
        else if (player1HealthScript.shield<=0f) 
        {
            player1ShieldIm.sprite = shields[3];
        }
    
    }

    private void UpdateP2ShieldIcon()
    {
        if (player2HealthScript.shield == p2MaxShield)
        {
            player2ShieldIm.sprite = shields[0];
        }
        else if (player2HealthScript.shield > p2MaxShield / 2)
        {
            player2ShieldIm.sprite = shields[1];
        }
        else if (player2HealthScript.shield > 0f && player2HealthScript.shield <= p2MaxShield / 2)
        {
            player2ShieldIm.sprite = shields[2];
        }
        else if (player2HealthScript.shield <= 0f)
        {
            player2ShieldIm.sprite = shields[3];
        }

    }

    // Update is called once per frame
    void Update()
    {
        p1ShieldValue.text = player1HealthScript.shield.ToString();
        UpdateP1ShieldIcon();
        UpdateP2ShieldIcon();
    }
}
