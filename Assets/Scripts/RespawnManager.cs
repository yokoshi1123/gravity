using System.Collections;
using System.Collections.Generic;
using System.Runtime.CompilerServices;
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
    //public bool respawnchanged = false;



    ///*[SerializeField]*/ private GameObject playerAvatar;
    private SpriteRenderer playerAvatar;
    private PlayerController playerController;


    // Start is called before the first frame update
    void Start()
    {
        playerAvatar = GameObject.FindWithTag("Player").transform.GetChild(0).gameObject.GetComponent<SpriteRenderer>();
        playerController = GameObject.FindWithTag("Player").GetComponent<PlayerController>();
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
            playerAvatar.enabled = false; // SetActive(false);
            playerController.SetCanMove(false); //canMove = false;
            //Debug.Log("������");
            respawn=false;

            //�I�u�W�F�N�g�����Ƃ̈ʒu�ɖ߂�
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
        

        if (respawning1 || (changePosi==0))
        {
            //Debug.Log("OK");
            playerAvatar.enabled = true; // SetActive(true);
            respawning1 = false;
            
        }

        if(respawning2 || (changePosi�@==0))
        {
            //Debug.Log("OK2");
            playerController.SetCanMove(true); //canMove = true;
            respawning2 = false;
            resAnimation = false;
        }

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


    
}
