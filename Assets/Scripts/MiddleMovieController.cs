using System.Collections;
using System.Collections.Generic;
using UnityEngine;
using UnityEngine.SceneManagement;


public class MiddleMovieController : MonoBehaviour
{
    //private Animator animator;
    //private FadeManager fadeManager;
    [SerializeField] private bool build = false;
    private SaveDataManager saveDataManager;


    // Start is called before the first frame update
    void Start()
    {
        //animator = GameObject.Find("MiddleMovie").GetComponent<Animator>();
        //fadeManager = GameObject.FindWithTag("Fade").GetComponent<FadeManager>();
        //fadeManager.fadeIn();

        if (build)
        {
            saveDataManager = GameObject.Find("SaveDataManager").GetComponent<SaveDataManager>();
            saveDataManager.SaveGameData("MiddleScene", 0, false);
        }
    }

    // Update is called once per frame
    void Update()
    {
        
    }

    private void EndAnimation()
    {
        SceneManager.LoadScene("NewsScene");
    }
}
