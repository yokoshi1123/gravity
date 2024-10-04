using System.Collections;
using System.Collections.Generic;
using UnityEngine;
using UnityEngine.SceneManagement;

public class LoadScene : MonoBehaviour
{
    public void Load(string scenename)
    {
        Time.timeScale = 1f;
        SceneManager.LoadScene(scenename);
    }
}