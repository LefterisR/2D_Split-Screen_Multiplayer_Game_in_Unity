using System.Collections;
using System.Collections.Generic;
using UnityEngine;

public class GameManager : MonoBehaviour
{
    // Start is called before the first frame update
    private int confirmedPlayerChoice;

    [SerializeField]
    private CharacterDatabase characterDatabase;
    [SerializeField]
    private Transform spawnPoint;

    private void LoadCharacterToScene(int index) 
    {
        GameObject selectedCharacter = characterDatabase.GetCharacterPrefab(index);
        
        Instantiate(selectedCharacter,spawnPoint.position,spawnPoint.rotation);
    }
    
    void Start()
    {
        confirmedPlayerChoice = PlayerPrefs.GetInt("selectedOption");
        LoadCharacterToScene(confirmedPlayerChoice);
    }

    // Update is called once per frame
    void Update()
    {
        
    }
}
