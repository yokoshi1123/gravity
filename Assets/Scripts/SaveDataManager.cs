using JetBrains.Annotations;
using System.Collections;
using System.Collections.Generic;
using System.IO;
using UnityEditor;
using UnityEngine;
using UnityEngine.SceneManagement;

public class SaveDataManager : MonoBehaviour
{
    private Animator loadError;

    private const string gamePath = "C:/Users/koshi/source/repos/gravity";

    [SerializeField] private string sName = "Stage1-1";
    [SerializeField] private int resId = 2;
    [SerializeField] private int prog = 0;

    [System.Serializable]
    public class GameData
    {
        public string stageName;
        public int respawnIndex;
        public int totalProgress;

        //public string StageName
        //{
        //    get => stageName;
        //    set => stageName = value;
        //}

        //public int RespawnIndex
        //{
        //    get => respawnIndex;
        //    set => respawnIndex = value;
        //}

        //public int TotalProgress
        //{
        //    get => totalProgress;
        //    set => totalProgress = value;
        //}
    }

    //[SerializeField] private 
    public GameData gameData;
    
    private void Start()
    {
        loadError = GameObject.Find("/Canvas/LoadError").GetComponent<Animator>();
        gameData = new GameData();
    }

    private void Update()
    {      
        if (Input.GetKey(KeyCode.S))
        {
            SaveGameData();
        }
        if (Input.GetKey(KeyCode.L))
        {
            LoadGameData();
        }        
    }

    public void SaveGameData()
    {
        gameData.stageName = (SceneManager.GetActiveScene().name == "TitleScene") ? "Stage1-1" : SceneManager.GetActiveScene().name; // sName;
        gameData.respawnIndex = resId;
        gameData.totalProgress = prog;
        string jsonStr = JsonUtility.ToJson(gameData);

        StreamWriter writer = new (gamePath + "/savedata.json");
        writer.Write(jsonStr);
        writer.Flush();
        writer.Close();
    }

    public void LoadGameData()
    {
        try
        {
            StreamReader reader = new (gamePath + "/savedata.json");

            string dataStr = reader.ReadToEnd();
            reader.Close();

            GameData gameData = JsonUtility.FromJson<GameData>(dataStr);
            sName = gameData.stageName;
            resId = gameData.respawnIndex;
            prog = gameData.totalProgress;

            SceneManager.LoadScene(sName);
            //return JsonUtility.FromJson<GameData>(dataStr);
        }
        catch
        {            
            StartCoroutine(ShowLoadError());
        }        
    }

    public void NewGame()
    {
        SaveGameData();
        LoadGameData();
    }

    private IEnumerator ShowLoadError()
    {
        loadError.SetBool("Show", true);
        yield return new WaitForSeconds(2.5f);
        loadError.SetBool("Show", false);
    }
}
