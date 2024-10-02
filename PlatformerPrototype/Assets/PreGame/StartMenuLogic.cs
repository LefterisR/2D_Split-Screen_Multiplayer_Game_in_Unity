using System.Collections;
using System.Collections.Generic;
using UnityEngine;
using UnityEngine.SceneManagement;

//Author Rizos Eleftherios
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
    public GameObject helpPannel;

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

    public void OnOpenHelp()
    {

        startButtonAudio.clip = buttonClip;
        startButtonAudio.Play();
        helpPannel.SetActive(true);

    }

    public void OnCloseHelp()
    {
        startButtonAudio.clip = buttonClip;
        startButtonAudio.Play();
        helpPannel.SetActive(false);
    }

    IEnumerator LoadNextSceneDelayed() 
    {
        
        yield return new WaitForSeconds(startSoundTime);
        SceneManager.LoadScene(currentSceneIndex + 1); //Character Selection index 1
    }
}
