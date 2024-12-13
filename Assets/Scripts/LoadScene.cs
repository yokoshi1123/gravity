using System.Collections;
using System.Collections.Generic;
using UnityEngine;
using UnityEngine.SceneManagement;

public class LoadScene : MonoBehaviour
{
    [SerializeField] private SaveDataManager saveDataManager;

    void Start()
    {
        saveDataManager = GameObject.Find("SaveDataManager").GetComponent<SaveDataManager>();
    }

    public void Load(string scenename)
    {
        Time.timeScale = 1f;
        SceneManager.LoadScene(scenename);
    }

    public void NewGame()
    {
        saveDataManager.NewGame();
    }

    public void LoadGame()
    {
        saveDataManager.LoadGameData();
    }
}