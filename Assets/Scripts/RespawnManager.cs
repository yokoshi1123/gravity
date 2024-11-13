using System.Collections;
using System.Collections.Generic;
using System.Runtime.CompilerServices;
using UnityEngine;

public class RespawnManager : MonoBehaviour
{
    //以下はPlayerに渡すbool
    public bool respawn = false;
    public bool canMove = true;
    //public bool canActive = false;
    

    //以下はAnimationのつなぎのbool
    public bool resAnimation = false;


    //以下はAnimationから渡されるbool
    public bool respawning1 = false;
    public bool respawning2 = false;

    //以下は初期位置かどうかを判定するint
    public int changePosi = 0;

    //以下はrespawnの位置が変更されたときにtrueを返す
    //public bool respawnchanged = false;

    private GravityManager gravityManager;

    ///*[SerializeField]*/ private GameObject playerAvatar;
    private SpriteRenderer playerAvatar;
    private PlayerController playerController;

    private int respawn_index_current = 0;
    private int respawn_index_length = 0;
    private GameObject[] RespawnPointsList;


    // Start is called before the first frame update
    void Start()
    {
        gravityManager = GameObject.Find("GravityManager").GetComponent<GravityManager>();
        playerAvatar = GameObject.FindWithTag("Player").transform.GetChild(0).gameObject.GetComponent<SpriteRenderer>();
        playerController = GameObject.FindWithTag("Player").GetComponent<PlayerController>();


        //respawnpointの配列を作成
        GameObject[] Respawnpoints = GameObject.FindGameObjectsWithTag("Respawn");
        foreach (GameObject obj in Respawnpoints)
        {
            respawn_index_length++;
        }
        Debug.Log(respawn_index_length);

        RespawnPointsList = new GameObject[respawn_index_length + 1];
        int i = 1;
        foreach (GameObject obj in Respawnpoints)
        {
            RespawnPointsList[i] = obj;
            RespawnUpdater respawnUpdater = obj.transform.GetChild(0).gameObject.GetComponent<RespawnUpdater>();
            respawnUpdater.SetRespawnIndex(i);
            i++;
        }
    }

    // Update is called once per frame
    void Update()
    {

        /*if (respawning1)
        {
            Debug.Log("respawning1=true");
        }*/
        //if (respawn_index_current == 0) Debug.Log("index=0");
        
        if (respawn)
        {
            
            gravityManager.DestroyGF();
            //Debug.Log("respawn");
            //playerAvatar.enabled = false; // SetActive(false);
            //playerController.SetCanMove(false); //canMove = false;
            //Debug.Log("消える");
            respawn=false;

            //オブジェクトをもとの位置に戻す
            GameObject[] movableObjs = GameObject.FindGameObjectsWithTag("Movable");
            foreach (GameObject obj in movableObjs)
            { 
                //Debug.Log(obj.name);
                try
                {
                    StartCoroutine(obj.GetComponent<TotalWeight>().Respawn());
                }
                catch { }
            }

            GameObject[] movingFloors = GameObject.FindGameObjectsWithTag("Platform");
            foreach (GameObject obj in movingFloors)
            {
                //Debug.Log(obj.name);
                try
                {
                    obj.GetComponent<MoveObjectWithRoute>().Respawn();
                }
                catch { }
            }


            //SE
            GetComponent<AudioSource>().PlayOneShot(GetComponent<AudioSource>().clip, 0.2f);
        }
        

        /*if (respawning1 || respawn_index_current==0)
        {
            //Debug.Log("OK");
            playerAvatar.enabled = true; // SetActive(true);
            respawning1 = false;
            
        }

        if(respawning2 || respawn_index_current == 0)
        {
            //Debug.Log("OK2");
            playerController.SetCanMove(true); //canMove = true;
            respawning2 = false;
            resAnimation = false;
        }*/

        //canActive = false;
        
    }

    public bool GetRespawning1()
    {
        return respawning1;
    }

    public void SetRespawning1(bool value)
    {
        respawning1 = value;
    }

    public bool GetRespawning2()
    {
        return respawning2;
    }

    public void SetRespawning2(bool value)
    {
        respawning2 = value;
    }

    public bool GetRespawnAnimation()
    {
        return resAnimation;
    }
    public void SetRespawnAnimation(bool value)
    {
        resAnimation = value;
    }


    public int GetRespawnIndexCurrent()
    {
        return respawn_index_current;
    }
    public void SetRespawnIndexCurrent(int index)
    {
        respawn_index_current = index;
    }

    public GameObject GetRespawnPoint(int index)
    {
        return RespawnPointsList[index];
    }
    
}
