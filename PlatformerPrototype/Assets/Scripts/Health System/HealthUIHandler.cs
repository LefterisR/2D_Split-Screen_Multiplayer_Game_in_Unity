using System.Collections;
using System.Collections.Generic;
using UnityEngine;
using UnityEngine.UI;
using TMPro;
using System.Runtime.InteropServices.WindowsRuntime;
using UnityEngine.SceneManagement;

public class HealthUIHandler : MonoBehaviour
{
    public Sprite[] shields;
    public Sprite[] hearts;

    [Header("Player 1 UI")]
    public Image player1ShieldIm;
    public TMP_Text p1ShieldValue;
    public TMP_Text p1HpValue;
    public Image p1HealthBar;
    public Image p1HeartIcon;
    [Header("Player 2 UI")]
    public Image player2ShieldIm;
    public TMP_Text p2ShieldValue;
    public TMP_Text p2HpValue;
    public Image p2HealthBar;
    public Image p2HeartIcon;

    [Header("Game Over Screen")]
    public AudioSource audioSource;
    public GameObject endGameWindow;
    public TMP_Text endGameMessage;
    public float timeToPause;

    private PlayerHealth player1HealthScript;
    private float p1MaxShield;
    private float p1MaxHealth;

    private PlayerHealth player2HealthScript;
    private float p2MaxShield;
    private float p2MaxHealth;
   
    static int callCounter = 0;
    private bool callFillHeart = true;


    public void OnExitGame() 
    {

        Application.Quit();

    }

    public void OnPlayAgain() 
    {
        Debug.Log("Button Clicked!");
        SceneManager.LoadScene(0);

    }

    private void Start()
    {

        GameObject player1 = GameObject.FindGameObjectWithTag(TagHandler.Player1);
        GameObject player2 = GameObject.FindGameObjectWithTag(TagHandler.Player2);

        player1HealthScript = player1.GetComponent<PlayerHealth>();
        player2HealthScript = player2.GetComponent<PlayerHealth>();

        p1MaxShield = player1HealthScript.maxShield;
        p1MaxHealth = player1HealthScript.maxHealth;

        p2MaxShield = player2HealthScript.maxShield;
        p2MaxHealth = player2HealthScript.maxHealth;

        p1HeartIcon.sprite = hearts[0];
        p2HeartIcon.sprite = hearts[0];

        Debug.Log(p1MaxShield);
        Debug.Log(p2MaxShield);

    }

    private void UpdatePlayer1HpBar() 
    {
        float hp = player1HealthScript.health;

        if (hp <= 0) 
        {
            EndGame("Player 2");
        }

        p1HealthBar.fillAmount = Mathf.Clamp(hp/p1MaxHealth, 0, 1);

        if (p1HealthBar.fillAmount == 0) p1HeartIcon.sprite = hearts[1];
        

    }

    private void UpdatePlayer2HpBar()
    {
        float hp = player2HealthScript.health;

        if (hp <= 0)
        {
            EndGame("Player 1");
        }

        p2HealthBar.fillAmount = Mathf.Clamp(hp / p2MaxHealth, 0, 1);

        if (p2HealthBar.fillAmount == 0) p2HeartIcon.sprite = hearts[1];
       

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
        if (callFillHeart)
        {
            Debug.Log("Called times " + callCounter);
            callFillHeart = SetHeartFull();
        }

        p1HpValue.text = player1HealthScript.health.ToString();
        
        p1ShieldValue.text = player1HealthScript.shield.ToString();

        p2HpValue.text = player2HealthScript.health.ToString();
        p2ShieldValue.text = player2HealthScript.shield.ToString();

        UpdatePlayer1HpBar();
        UpdatePlayer2HpBar();

        UpdateP1ShieldIcon();
        UpdateP2ShieldIcon();
    }

    private bool SetHeartFull() 
    {
        callCounter++;
        p1HeartIcon.sprite = hearts[0];
        p2HeartIcon.sprite = hearts[0];

        if (callCounter < 10) { return true; }
        else return false;
    }

    private void EndGame(string winner) 
    {
        Cursor.visible = true;

        audioSource.enabled = true;

        endGameWindow.SetActive(true);
        endGameMessage.text = winner + " is victorious!";

        //StartCoroutine(PauseGame());

    }

    IEnumerator PauseGame() 
    {

        yield return new WaitForSeconds(timeToPause);

        Time.timeScale = 0;
    
    }

}
