using System.Collections;
using System.Collections.Generic;
using TMPro;
using UnityEngine;
using UnityEngine.UI;
using UnityEngine.SceneManagement;

public class CharacterManager : MonoBehaviour
{
    [Header("Player 1 UI")]
    public Button nextButtonP1;
    public Button previousButtonP1;
    public Button selectP1;
    public TMP_Text nameTxtP1;
    public SpriteRenderer spriteArtP1;

    [Header("Player 2 UI")]
    public Button nextButtonP2;
    public Button previousButtonP2;
    public Button selectP2;
    public TMP_Text nameTxtP2;
    public SpriteRenderer spriteArtP2;

    [Header("General")]
    public AudioSource genericButtonPressSound;
    public TMP_Text currentActivePtxt;
    public Button startGameButton;

    public CharacterDatabase characterDatabase;

    private bool hasP1Chose = false;
    private bool hasP2Chose = false;

    private readonly string p1Name = "Player 1";
    private readonly string p2Name = "Player 2";

    private int _selectedOption = 0; //ALWAYS INITIALIZED

    private int currentSceneIndex;
    void Start()
    {
        currentSceneIndex = SceneManager.GetActiveScene().buildIndex;
        //UpdateCharacter(_selectedOption);
        InitializeCharacters();

        HandlePlayer2UI(false);

        startGameButton.enabled = false;

        currentActivePtxt.text = p1Name;
    }

    public void Next() 
    {
        genericButtonPressSound.Play();
        _selectedOption++;

        if (_selectedOption >= characterDatabase.characterCount) 
        {
            _selectedOption = 0;
        }
        
        

        UpdateCharacter(_selectedOption);
    }

    public void Previous() 
    {
        genericButtonPressSound.Play();
        _selectedOption--;

        if (_selectedOption < 0)
        {
            _selectedOption = characterDatabase.characterCount - 1;
        }
         

        UpdateCharacter(_selectedOption);

    }

    private void InitializeCharacters() 
    {
        CharacterData character = characterDatabase.GetCharacter(0);
        //Player 1
        spriteArtP1.sprite = character.characterSprite;
        nameTxtP1.text = character.Name;
        //Player 2
        spriteArtP2.sprite = character.characterSprite;
        nameTxtP2.text = character.Name;
    }
    private void UpdateCharacter(int selectedIndex) 
    {
        if (!hasP1Chose && !hasP2Chose)
        {
            CharacterData character = characterDatabase.GetCharacter(selectedIndex);
            spriteArtP1.sprite = character.characterSprite;
            nameTxtP1.text = character.Name;
        }
        else if (hasP1Chose && !hasP2Chose) 
        {
            CharacterData character = characterDatabase.GetCharacter(selectedIndex);
            spriteArtP2.sprite = character.characterSprite;
            nameTxtP2.text = character.Name;
        }
        
    }
    
    public void SelectCharacter() 
    {
        genericButtonPressSound.Play();
        if (!hasP1Chose && !hasP2Chose)
        {
            PlayerPrefs.SetInt("selectedOptionP1", _selectedOption);
            HandlePlayer2UI(true);
            HandlePlayer1UI(false);
            currentActivePtxt.text = p2Name;
            hasP1Chose = true;
            _selectedOption = 0;
        }
        else if (hasP1Chose && !hasP2Chose) 
        {
            
            PlayerPrefs.SetInt("selectedOptionP2", _selectedOption);
            HandlePlayer2UI(false);
            startGameButton.enabled = true;
            hasP2Chose = true;
        }
        
    }

    public void StartGame() 
    {
        StartCoroutine(LoadNextSceneDelayed());
    }
    void HandlePlayer1UI(bool value)
    {
        nextButtonP1.enabled = value;
        previousButtonP1.enabled = value;
        selectP1.enabled = value;

    }
    void HandlePlayer2UI(bool value) 
    {
        nextButtonP2.enabled = value;
        previousButtonP2.enabled = value;
        selectP2.enabled = value;
        
    }

    IEnumerator LoadNextSceneDelayed()
    {
        yield return new WaitForSeconds(0.4f);
        SceneManager.LoadScene(currentSceneIndex + 1); //Character Selection index 1
    }
   
}
