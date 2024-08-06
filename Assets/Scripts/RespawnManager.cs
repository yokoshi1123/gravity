using System.Collections;
using System.Collections.Generic;
using UnityEngine;

public class RespawnManager : MonoBehaviour
{
    //à»â∫ÇÕPlayerÇ…ìnÇ∑bool
    public bool respawn = false;
    public bool canMove = true;
    //public bool canActive = false;
    

    //à»â∫ÇÕAnimationÇÃÇ¬Ç»Ç¨ÇÃbool
    public bool resAnimation = false;


    //à»â∫ÇÕAnimationÇ©ÇÁìnÇ≥ÇÍÇÈbool
    public bool respawning1 = false;
    public bool respawning2 = false;

    //à»â∫ÇÕèâä˙à íuÇ©Ç«Ç§Ç©ÇîªíËÇ∑ÇÈint
    public int changePosi = 0;

    [SerializeField] private GameObject PlayerAvatar;


    // Start is called before the first frame update
    void Start()
    {
       
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
            PlayerAvatar.SetActive(false);
            canMove = false;
            //Debug.Log("è¡Ç¶ÇÈ");
            respawn=false;
        }
        
        if (respawning1 || (changePosi==0))
        {
            //Debug.Log("OK");
            PlayerAvatar.SetActive(true);
            respawning1 = false;
        }

        if(respawning2 || (changePosiÅ@==0))
        {
            //Debug.Log("OK2");
            canMove = true;
            respawning2 = false;
            resAnimation = false;
        }

        //canActive = false;
        
    }

    
}
