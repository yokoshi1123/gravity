using System.Collections;
using System.Collections.Generic;
using UnityEngine;

public class RespawnManager : MonoBehaviour
{
    //�ȉ���Player�ɓn��bool
    public bool respawn = false;
    public bool canMove = true;

    //�ȉ���Animation�̂Ȃ���bool
    public bool resAnimation = false;


    //�ȉ���Animation����n�����bool
    public bool respawning1 = false;
    public bool respawning2 = false;
    


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
        
        if (respawning1)
        {
            PlayerAvatar.SetActive(true);
        }

        if(respawning2)
        {
            canMove = true;
        }
        
    }

    
}
