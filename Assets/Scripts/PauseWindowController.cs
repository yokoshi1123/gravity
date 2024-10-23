using System.Collections;
using System.Collections.Generic;
using UnityEngine;
using UnityEngine.UI;

public class PauseWindowController : MonoBehaviour
{
    [SerializeField] private GameObject pauseButton;
    [SerializeField] private GameObject pauseWindow;

    private PlayerController playerController;

    //private bool isPaused = false;
    void Start()
    {
        //pauseButton = GameObject.Find("/Canvas/PauseButton");
        //pauseWindow = GameObject.Find("/Canvas/PauseWindow");
        playerController = GameObject.FindWithTag("Player").GetComponent<PlayerController>();
    }

    void Update()
    {
        if (Input.GetKeyDown(KeyCode.Escape))
        { 
            Pause();
        }
    }
    
    public void Pause()
    {
        playerController.SetCanMove(false);
        pauseButton.SetActive(false);
        pauseWindow.SetActive(true);
        Time.timeScale = 0f;
    }

    public void Resume()
    {
        Time.timeScale = 1f;
        playerController.SetCanMove(true);
        pauseButton.SetActive(true);
        pauseWindow.SetActive(false);
    }

    public void Restart()
    {
        Time.timeScale = 1f;
        pauseButton.SetActive(true);
        pauseWindow.SetActive(false);
        playerController.SetIsDead(true);
        playerController.SetCanMove(true);
    }

}
