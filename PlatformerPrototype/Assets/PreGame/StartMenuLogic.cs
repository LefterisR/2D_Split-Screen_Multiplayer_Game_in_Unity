using System.Collections;
using System.Collections.Generic;
using UnityEngine;
using UnityEngine.SceneManagement;

public class StartMenuLogic : MonoBehaviour
{
    [SerializeField]
    private AudioSource startButtonAudio;
    [SerializeField] 
    private float startSoundTime;

    private int currentSceneIndex = 0; //Start Scene Index
    // Start is called before the first frame update
    void Start()
    {
        currentSceneIndex = SceneManager.GetActiveScene().buildIndex;
    }

    public void StartGame() 
    {
        startButtonAudio.Play();
        StartCoroutine(LoadNextSceneDelayed());
    
    }

    public void QuitGame()
    {
        Application.Quit();
    }
    IEnumerator LoadNextSceneDelayed() 
    {
        
        yield return new WaitForSeconds(startSoundTime);
        SceneManager.LoadScene(currentSceneIndex + 1); //Character Selection index 1
    }
}
