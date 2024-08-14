using System.Collections;
using System.Collections.Generic;
using UnityEngine;
using UnityEngine.UI;

public class PauseWindowController : MonoBehaviour
{
    [SerializeField] private GameObject pauseButton;
    [SerializeField] private GameObject pauseWindow;

    //private bool isPaused = false;
    
    public void Pause()
    {
        pauseButton.SetActive(false);
        pauseWindow.SetActive(true);
        Time.timeScale = 0;
    }

    public void Resume()
    {
        Time.timeScale = 1;
        pauseButton.SetActive(true);
        pauseWindow.SetActive(false);
    }
    void Update()
    {
        if (Input.GetKeyDown(KeyCode.Escape))
        { 
            Pause();
        }
    }
}
