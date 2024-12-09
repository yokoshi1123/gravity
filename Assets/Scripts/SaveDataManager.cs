using JetBrains.Annotations;
using System.Collections;
using System.Collections.Generic;
using System.IO;
using UnityEditor;
using UnityEngine;
using UnityEngine.SceneManagement;
using static System.Net.WebRequestMethods;

public class SaveDataManager : MonoBehaviour
{
    private Animator loadError;

    [HideInInspector] public SaveData data;

    private string filePath = "C:/Users/koshi/source/repos/gravity/";
    private string fileName = "SaveData.json";

    [SerializeField] private bool build = false;

    //[SerializeField] private string sName = "Stage1-1";
    //[SerializeField] private int resId = 2;
    //[SerializeField] private int prog = 0;

    ////[SerializeField] private 
    //public GameData gameData;

    void Awake()
    {
        if (build) filePath = Application.persistentDataPath;
        loadError = GameObject.Find("/Canvas/LoadError").GetComponent<Animator>();
        //gameData = new GameData();
        data = new SaveData();
        DontDestroyOnLoad(this.gameObject);
    }

    //void Update()
    //{
    //    if (Input.GetKey(KeyCode.S))
    //    {
    //        SaveGameData(resId);
    //    }
    //    if (Input.GetKey(KeyCode.L))
    //    {
    //        LoadGameData();
    //    }
    //}

    public void SaveGameData(string stageName, int respawnIndex, bool isAvailable)
    {
        data.stageName = stageName; // (SceneManager.GetActiveScene().name == "TitleScene") ? "Stage1-1" : SceneManager.GetActiveScene().name; // sName;
        data.respawnIndex = respawnIndex; //resId;
        if (stageName.Contains("Stage"))
        {
            data.totalProgress = int.Parse(data.stageName.Replace("Stage", "").Replace("-", "")) / 10;
        }
         //prog; // StageO-X‚ÌO‚Ì•”•ª
        data.isAvailable = isAvailable;
        string jsonStr = JsonUtility.ToJson(data);

        StreamWriter writer = new (filePath + fileName);
        writer.Write(jsonStr);
        writer.Flush();
        writer.Close();
    }

    public void LoadGameData()
    {
        try
        {
            StreamReader reader = new (filePath + fileName);

            string dataStr = reader.ReadToEnd();
            reader.Close();

            data = JsonUtility.FromJson<SaveData>(dataStr);
            //Debug.Log(data.stageName + ":" + data.respawnIndex + ":" + data.totalProgress + ":" + data.isAvailable);
            //sName = gameData.stageName;
            //resId = gameData.respawnIndex;
            //prog = gameData.totalProgress;

            SceneManager.LoadScene(data.stageName);
            //return JsonUtility.FromJson<GameData>(dataStr);
        }
        catch
        {            
            StartCoroutine(ShowLoadError());
        }        
    }

    public void NewGame()
    {
        //SaveGameData("Stage1-1", 0, false);
        data.totalProgress = 0;
        SaveGameData("NewsScene", 0, false);
        LoadGameData();
    }

    public bool GetBuild()
    {
        return build;
    }

    private IEnumerator ShowLoadError()
    {
        loadError.SetBool("Show", true);
        yield return new WaitForSeconds(2.5f);
        loadError.SetBool("Show", false);
    }
}
