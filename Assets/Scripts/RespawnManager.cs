using System.Collections;
using System.Collections.Generic;
using UnityEngine;

public class RespawnManager : MonoBehaviour
{
    //�ȉ���Player�ɓn��bool
    public bool respawn = false;
    public bool canMove = true;
    //public bool canActive = false;
    

    //�ȉ���Animation�̂Ȃ���bool
    public bool resAnimation = false;


    //�ȉ���Animation����n�����bool
    public bool respawning1 = false;
    public bool respawning2 = false;

    //�ȉ��͏����ʒu���ǂ����𔻒肷��int
    public int changePosi = 0;

    //�ȉ���respawn�̈ʒu���ύX���ꂽ�Ƃ���true��Ԃ�
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
            //Debug.Log("������");
            respawn=false;
        }
        
        if (respawning1 || (changePosi==0))
        {
            //Debug.Log("OK");
            PlayerAvatar.SetActive(true);
            respawning1 = false;
        }

        if(respawning2 || (changePosi�@==0))
        {
            //Debug.Log("OK2");
            canMove = true;
            respawning2 = false;
            resAnimation = false;
        }

        //canActive = false;
        
    }

    
}
