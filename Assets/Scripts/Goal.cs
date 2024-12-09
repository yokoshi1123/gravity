using System.Collections;
using System.Collections.Generic;
using UnityEngine;
using UnityEngine.SceneManagement;

public class Goal : MonoBehaviour
{
    GravityManager gravityManager;
    SaveDataManager saveDataManager;

    [SerializeField] private string nextStage;
    private bool build = false;

    [Header("ÉSÅ[Éã")][SerializeField] private AudioClip warpSE;

    // Start is called before the first frame update
    void Awake()
    {
        gravityManager = GameObject.Find("GravityManager").GetComponent<GravityManager>();
        build = gravityManager.GetBuild();
        if (build) saveDataManager = GameObject.Find("SaveDataManager").GetComponent<SaveDataManager>();
    }

    private void OnCollisionEnter2D(Collision2D collision)
    {
        //Debug.Log(collision.gameObject.name); 
        if (collision.gameObject.CompareTag("Player") || collision.transform.parent.gameObject.CompareTag("Player"))
        {
            if (build)
            {
                GetComponent<AudioSource>().PlayOneShot(warpSE, 0.5f);
                saveDataManager.SaveGameData(nextStage, 0, gravityManager.GetIsAvailable());
                //saveDataManager.LoadGameData();
            }
            //else
            //{
            SceneManager.LoadScene(nextStage);
            //}
        }
}
}
