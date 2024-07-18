using System.Collections;
using System.Collections.Generic;
using UnityEngine;

public class RespawnManager : MonoBehaviour
{
    //�ȉ���Player�ɓn��bool
    public bool respawn = false;
    public bool canMove = true;
    public bool canActive = false;

    //�ȉ���Animation�̂Ȃ���bool
    public bool resAnimation = false;


    //�ȉ���Animation����n�����bool
    public bool respawning1 = false;
    public bool respawning2 = false;

    //�ȉ��͏����ʒu���ǂ����𔻒肷��int
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
            //Debug.Log("������");
        }
        
        if (respawning1 || changePosi==0)
        {
            PlayerAvatar.SetActive(true);
        }

        if(respawning2 || changePosi�@==0)
        {
            canMove = true;
        }

        canActive = false;
        
    }

    
}
