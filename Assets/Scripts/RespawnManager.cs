using System.Collections;
using System.Collections.Generic;
using UnityEngine;

public class RespawnManager : MonoBehaviour
{
    //ˆÈ‰º‚ÍPlayer‚É“n‚·bool
    public bool respawn = false;
    public bool canMove = true;
    public bool canActive = false;

    //ˆÈ‰º‚ÍAnimation‚Ì‚Â‚È‚¬‚Ìbool
    public bool resAnimation = false;


    //ˆÈ‰º‚ÍAnimation‚©‚ç“n‚³‚ê‚ébool
    public bool respawning1 = false;
    public bool respawning2 = false;

    //ˆÈ‰º‚Í‰ŠúˆÊ’u‚©‚Ç‚¤‚©‚ğ”»’è‚·‚éint
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
            //Debug.Log("Á‚¦‚é");
        }
        
        if (respawning1 || changePosi==0)
        {
            PlayerAvatar.SetActive(true);
        }

        if(respawning2 || changePosi@==0)
        {
            canMove = true;
        }

        canActive = false;
        
    }

    
}
