using System.Collections;
using System.Collections.Generic;
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
    public bool respawnchanged = false;



    /*[SerializeField]*/ private GameObject PlayerAvatar;


    // Start is called before the first frame update
    void Start()
    {
        PlayerAvatar = GameObject.FindWithTag("Player").transform.GetChild(0).gameObject;
    }

    // Update is called once per frame
    void Update()
    {
        
        /*if (respawning1)
        {
            Debug.Log("respawning1=true");
        }*/
        
        if (respawn)
        {
            //Debug.Log("respawn");
            PlayerAvatar.SetActive(false);
            canMove = false;
            //Debug.Log("消える");
            respawn=false;
        }
        
        if (respawning1 || (changePosi==0))
        {
            //Debug.Log("OK");
            PlayerAvatar.SetActive(true);
            respawning1 = false;
        }

        if(respawning2 || (changePosi　==0))
        {
            //Debug.Log("OK2");
            canMove = true;
            respawning2 = false;
            resAnimation = false;
        }

        //canActive = false;
        
    }

    
}
