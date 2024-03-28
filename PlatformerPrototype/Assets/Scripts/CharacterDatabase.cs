using System.Collections;
using System.Collections.Generic;
using UnityEngine;

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

}
