using System.Collections;
using System.Collections.Generic;
using UnityEngine;

public class RespawnUpdater : MonoBehaviour
{
    [SerializeField] private GameObject SE;
    [SerializeField] private Animator animator;
    private bool change = false;
    private bool resAnimation = false;

    [SerializeField] private int respawn_index;

    //現在のリスポーン地点がここであることを示す。
    //private bool current = false;

    //collisionしている間true;
    //private bool collisioning = false;

    //public bool respawned = false;
    private RespawnManager respawnManager;
    //private Vector3 posi;
    //private Transform myTransform;

    private int resposi;
    


    void Awake()
    {
        respawnManager = GameObject.Find("RespawnManager").GetComponent<RespawnManager>();
        /*if(respawnManager == null)
        {
            Debug.Log("NULL");
        }*/
    }

    void Update()
    {
        
        resAnimation = respawnManager.GetRespawnAnimation();
        if (resAnimation)
        {
            //Debug.Log("resAnimation is true");
            transform.position += new Vector3(0, 0, -transform.position.z - 2.0f);
        }
        else
        {
            transform.position += new Vector3(0, 0, -transform.position.z + 0.1f);
            //animator.SetBool("resAnimation", resAnimation);
        }

        /*if (respawnManager.respawnchanged)
        {
            if(!collisioning)
            {
                current = false;
            }
        
        }*/

        if (respawn_index == respawnManager.GetRespawnIndexCurrent())
        {
            //Debug.Log("now="+respawn_index);
            animator.SetBool("resAnimation", resAnimation);
        }
        //respawnManager.resAnimation = false;
    }

    private void OnCollisionEnter2D(Collision2D collision)
    {
        if (collision.gameObject.CompareTag("Player"))
        {
            change = true;
            //current = true;
            //collisioning = true;
            //respawnManager.respawnchanged = true;

            animator.SetBool("change", change);
            //collision.gameObject.GetComponent<PlayerController>().respawnPoint = transform.position + new Vector3(0, 0.99f, 0);
            SE.SetActive(true);

            respawnManager.SetRespawnIndexCurrent(respawn_index);
            //respawnManager.changePosi++;

            //resposi = respawnManager.changePosi;
            //change = true;
            //animator.SetBool("change", change);
        }
    }

    /*private void OnCollisionExit2d(Collision2D collision)
    {
        if(collision.gameObject.tag == "Player")
        {
            collisioning = false;
        }
    }*/

    public void RespawnAnimation1End()
    {
        Debug.Log("Animation1 End");
        respawnManager.SetRespawning1(true);
    }

    public void RespawnAnimation2End()
    {
        Debug.Log("Animation2 End");
        transform.position += new Vector3(0, 0, -transform.position.z + 1.0f);
        respawnManager.SetRespawning2(true);
    }

    public void SetRespawnIndex(int index)
    {
        respawn_index = index;
    }

    public int GetRespawnIndex()
    {
        return respawn_index;
    }
}
