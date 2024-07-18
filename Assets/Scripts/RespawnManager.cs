using System.Collections;
using System.Collections.Generic;
using UnityEngine;

public class RespawnManager : MonoBehaviour
{
    //以下はPlayerに渡すbool
    public bool respawn = false;
    public bool canMove = true;
    public bool canActive = false;

    //以下はAnimationのつなぎのbool
    public bool resAnimation = false;


    //以下はAnimationから渡されるbool
    public bool respawning1 = false;
    public bool respawning2 = false;

    //以下は初期位置かどうかを判定するint
    public int changePosi = 0;

    [SerializeField] private GameObject PlayerAvatar;


    // Start is called before the first frame update
    void Start()
    {
       
    }

    // Update is called once per frame
    void Update()
    {
        if (respawn)
        {
            PlayerAvatar.SetActive(false);
            canMove = false;
            //Debug.Log("消える");
        }
        
        if (respawning1 || changePosi==0)
        {
            PlayerAvatar.SetActive(true);
        }

        if(respawning2 || changePosi　==0)
        {
            canMove = true;
        }

        canActive = false;
        
    }

    
}
