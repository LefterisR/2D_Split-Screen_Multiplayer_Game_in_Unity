using System.Collections;
using System.Collections.Generic;
using UnityEngine;
using UnityEngine.SceneManagement;

public class StartMenuLogic : MonoBehaviour
{
    [SerializeField]
    private AudioSource startButtonAudio;
    [SerializeField]
    private AudioClip startGameClip;
    [SerializeField] 
    private AudioClip buttonClip;
    [SerializeField] 
    private float startSoundTime;

    public GameObject creditsPannel;

    private int currentSceneIndex = 0; //Start Scene Index
    // Start is called before the first frame update
    void Start()
    {
        currentSceneIndex = SceneManager.GetActiveScene().buildIndex;
    }

    public void StartGame() 
    {
        startButtonAudio.clip = startGameClip;
        startButtonAudio.Play();
        StartCoroutine(LoadNextSceneDelayed());
    
    }

    public void QuitGame()
    {
        Application.Quit();
    }

    public void OnOpenCredits() 
    {

        startButtonAudio.clip = buttonClip;
        startButtonAudio.Play();
        creditsPannel.SetActive(true);
    
    }

    public void OnCloseCredits() 
    {
        startButtonAudio.clip = buttonClip;
        startButtonAudio.Play();
        creditsPannel.SetActive(false);
    }
    IEnumerator LoadNextSceneDelayed() 
    {
        
        yield return new WaitForSeconds(startSoundTime);
        SceneManager.LoadScene(currentSceneIndex + 1); //Character Selection index 1
    }
}
