using System.Collections;
using System.Collections.Generic;
using UnityEngine;

//Author Rizos Eleftherios
[CreateAssetMenu]
public class CharacterDatabase :ScriptableObject
{
    public CharacterData[] character;

    public int characterCount 
    {
        get { return character.Length; }
    }

    public CharacterData GetCharacter(int index) 
    {
        return character[index];
    }

    public GameObject GetCharacterPrefab(int index) 
    {
        return character[index].characterEntity;
    }

}
