using System.Collections;
using System.Collections.Generic;
using Unity.VisualScripting;
using UnityEngine;
using UnityEngine.UI;

public class InGameUIHandler : MonoBehaviour
{

    public GameObject pausePannel;

    private void Update()
    {
        if (Input.GetKeyDown(KeyCode.Escape)) 
        {
            Time.timeScale = 0.0f;
            pausePannel.SetActive(true);
            Cursor.visible = true;
        }
    }

    public void OnResumeButtonPressed()
    {
        Cursor.visible = false;
        Time.timeScale=1.0f;
        pausePannel.SetActive(false);
    }

    public void OnExitButtonPressed() 
    {
        Application.Quit();
    }

}
