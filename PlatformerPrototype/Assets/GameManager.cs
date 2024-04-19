using Cinemachine;
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
    [SerializeField]
    CinemachineVirtualCamera virtualCamera;

    private GameObject _activePlayer;

    void LoadCharacterToScene(int index) 
    {
        GameObject selectedCharacter = characterDatabase.GetCharacterPrefab(index);
        
        _activePlayer = Instantiate(selectedCharacter,spawnPoint.position,spawnPoint.rotation);
    }

    private void Awake()
    {
        Cursor.visible = false;
    }
    void Start()
    {
        confirmedPlayerChoice = PlayerPrefs.GetInt("selectedOption");
        LoadCharacterToScene(confirmedPlayerChoice);

        virtualCamera.Follow = _activePlayer.transform;

        

    }

    // Update is called once per frame
    void Update()
    {
        
    }
}
