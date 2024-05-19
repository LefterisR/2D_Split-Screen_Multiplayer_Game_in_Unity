using Cinemachine;
using System;
using System.Collections;
using System.Collections.Generic;
using UnityEngine;
public class GameManager : MonoBehaviour
{
    // Start is called before the first frame update
    private int confirmedPlayerChoice1;
    private int confirmedPlayerChoice2;
    private readonly string player1Map = "PlayerKeyboard";
    private readonly string player2Map = "PlayerController";
    private readonly int p1Code = 1;
    private readonly int p2Code = 2;

    [SerializeField]
    private CharacterDatabase characterDatabase;
    [SerializeField]
    private Transform spawnPoint1;
    [SerializeField]
    private Transform spawnPoint2;

    [SerializeField]
    CinemachineVirtualCamera virtualCameraP1;
    [SerializeField]
    CinemachineVirtualCamera virtualCameraP2;

    private GameObject _activePlayer1;
    private GameObject _activePlayer2;

    void LoadCharacterToScene(int index,int playerCode) 
    {
        GameObject selectedCharacter = characterDatabase.GetCharacterPrefab(index);
        
        //Retrieve approprite component
        
        if (playerCode == 1)
        {
            
            if (index == 0)     //Knight
            {
                selectedCharacter.GetComponent<KnightController>().activeActionMap = player1Map; 
                selectedCharacter.GetComponent<KnightCombatController>().activeActionMap = player1Map;

                //Set enemy layer
                selectedCharacter.GetComponent<KnightCombatController>().enemyLayerCode = LayersHandler.Player2;
            }
            else if (index == 1) //Mage 
            {
                selectedCharacter.GetComponent<MageController>().activeActionMap = player1Map;
                
                selectedCharacter.GetComponent<MageCombatController>().activeActionMap = player1Map;

                //Set enemy layer + tag
                selectedCharacter.GetComponent<MageCombatController>().enemyLayerCode = LayersHandler.Player2;
                selectedCharacter.GetComponent<MageCombatController>().enemyTag = TagHandler.Player2;

            }

            selectedCharacter.tag = TagHandler.Player1;
            selectedCharacter.layer = LayersHandler.Player1;
            _activePlayer1 = Instantiate(selectedCharacter, spawnPoint1.position, spawnPoint1.rotation);
        }
        else if(playerCode == 2)
        {
            
            if (index == 0)     //Knight
            {
                selectedCharacter.GetComponent<KnightController>().activeActionMap = player2Map;
                selectedCharacter.GetComponent<KnightCombatController>().activeActionMap = player2Map;

                //Set enemy layer
                selectedCharacter.GetComponent<KnightCombatController>().enemyLayerCode = LayersHandler.Player1;
            }
            else if (index == 1) //Mage 
            {
                selectedCharacter.GetComponent<MageController>().activeActionMap = player2Map;
                selectedCharacter.GetComponent<MageCombatController>().activeActionMap = player2Map;

                //Set enemy layer + tag
                selectedCharacter.GetComponent<MageCombatController>().enemyLayerCode = LayersHandler.Player1;
                selectedCharacter.GetComponent<MageCombatController>().enemyTag = TagHandler.Player1;
            }

            selectedCharacter.tag = TagHandler.Player2;
            selectedCharacter.layer = LayersHandler.Player2;

            _activePlayer2 = Instantiate(selectedCharacter, spawnPoint2.position, spawnPoint2.rotation);
        }
    }

    private void Awake()
    {
        Cursor.visible = false;
    }
    void Start()
    {
        confirmedPlayerChoice1 = PlayerPrefs.GetInt("selectedOptionP1");
        LoadCharacterToScene(confirmedPlayerChoice1,p1Code);

        confirmedPlayerChoice2 = PlayerPrefs.GetInt("selectedOptionP2");
        LoadCharacterToScene(confirmedPlayerChoice2,p2Code);

        virtualCameraP1.Follow = _activePlayer1.transform;

        virtualCameraP2.Follow = _activePlayer2.transform;

    }

  
    
}
