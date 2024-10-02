using Cinemachine;
using System;
using System.Collections;
using System.Collections.Generic;
using UnityEngine;

//Author Rizos Eleftherios
public class GameManager : MonoBehaviour
{
    // Start is called before the first frame update
    private int confirmedPlayerChoice1;
    private int confirmedPlayerChoice2;
    private readonly string player1Map = "PlayerKeyboard";
    private readonly string player2Map = "PlayerController";
    private readonly string player1MapAlt = "SplitKeyboard1";
    private readonly string player2MapAlt = "SplitKeyboard2";
    private readonly int p1Code = 1;
    private readonly int p2Code = 2;
    private readonly int schemeMKCcode = 0;
    private readonly int schemeSKcode = 1;
    private string selectedInputLayoutP1 = "PlayerKeyboard";
    private string selectedInputLayoutP2 = "PlayerController";

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

    public int targetFrameRate = 60;

    void LoadCharacterToScene(int index,int playerCode) 
    {
        GameObject selectedCharacter = characterDatabase.GetCharacterPrefab(index);

        //Retrieve approprite component
        
        
        if (playerCode == 1)
        {
            
            if (index == 0)     //Knight
            {
                selectedCharacter.GetComponent<KnightController>().activeActionMap = selectedInputLayoutP1; 
                selectedCharacter.GetComponent<KnightCombatController>().activeActionMap = selectedInputLayoutP1;

                //Set enemy layer
                selectedCharacter.GetComponent<KnightCombatController>().enemyLayerCode = LayersHandler.Player2;
            }
            else if (index == 1) //Mage 
            {
                selectedCharacter.GetComponent<MageController>().activeActionMap = selectedInputLayoutP1;
                
                selectedCharacter.GetComponent<MageCombatController>().activeActionMap = selectedInputLayoutP1;

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
                selectedCharacter.GetComponent<KnightController>().activeActionMap = selectedInputLayoutP2;
                selectedCharacter.GetComponent<KnightCombatController>().activeActionMap = selectedInputLayoutP2;

                //Set enemy layer
                selectedCharacter.GetComponent<KnightCombatController>().enemyLayerCode = LayersHandler.Player1;
            }
            else if (index == 1) //Mage 
            {
                selectedCharacter.GetComponent<MageController>().activeActionMap = selectedInputLayoutP2;
                selectedCharacter.GetComponent<MageCombatController>().activeActionMap = selectedInputLayoutP2;

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
        QualitySettings.vSyncCount = 0;
        Application.targetFrameRate = targetFrameRate;
    }
    void Start()
    {

        int layoutCode = PlayerPrefs.GetInt("scheme");

        if (layoutCode == schemeMKCcode)
        {
            selectedInputLayoutP1 = player1Map;
            selectedInputLayoutP2 = player2Map;
        }
        else if (layoutCode == schemeSKcode)
        {

            selectedInputLayoutP1 = player1MapAlt;
            selectedInputLayoutP2 = player2MapAlt;

        }

        confirmedPlayerChoice1 = PlayerPrefs.GetInt("selectedOptionP1");
        LoadCharacterToScene(confirmedPlayerChoice1,p1Code);

        confirmedPlayerChoice2 = PlayerPrefs.GetInt("selectedOptionP2");
        LoadCharacterToScene(confirmedPlayerChoice2,p2Code);

        virtualCameraP1.Follow = _activePlayer1.transform;

        virtualCameraP2.Follow = _activePlayer2.transform;

    }

  
    
}
