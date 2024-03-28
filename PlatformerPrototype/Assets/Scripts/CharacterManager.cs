using System.Collections;
using System.Collections.Generic;
using TMPro;
using UnityEngine;
using UnityEngine.UI;

public class CharacterManager : MonoBehaviour
{
    public CharacterDatabase characterDatabase;

    public TMP_Text nameTxt;
    public SpriteRenderer spriteArt;

    private int _selectedOption = 0; //ALWAYS INITIALIZED

    void Start()
    {
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
    

}
