using System.Collections;
using System.Collections.Generic;
using TMPro;
using UnityEngine;
using UnityEngine.UI;
using UnityEngine.SceneManagement;

public class CharacterManager : MonoBehaviour
{
    public CharacterDatabase characterDatabase;

    public TMP_Text nameTxt;
    public SpriteRenderer spriteArt;

    private int _selectedOption = 0; //ALWAYS INITIALIZED

    private int currentSceneIndex;
    void Start()
    {
        currentSceneIndex = SceneManager.GetActiveScene().buildIndex;
        UpdateCharacter(_selectedOption);
    }

    public void Next() 
    {
        _selectedOption++;

        if (_selectedOption >= characterDatabase.characterCount) 
        {
            _selectedOption = 0;
        }
        
        

        UpdateCharacter(_selectedOption);
    }

    public void Previous() 
    {
        _selectedOption--;

        if (_selectedOption < 0)
        {
            _selectedOption = characterDatabase.characterCount - 1;
        }
         

        UpdateCharacter(_selectedOption);

    }
    private void UpdateCharacter(int selectedIndex) 
    {
        CharacterData character = characterDatabase.GetCharacter(selectedIndex);
        spriteArt.sprite = character.characterSprite;
        nameTxt.text = character.Name;
    }
    
    public void SelectCharacter() 
    {
        PlayerPrefs.SetInt("selectedOption",_selectedOption);
        StartCoroutine(LoadNextSceneDelayed());
    }

    IEnumerator LoadNextSceneDelayed()
    {
        yield return new WaitForSeconds(0.4f);
        SceneManager.LoadScene(currentSceneIndex + 1); //Character Selection index 1
    }
   
}
