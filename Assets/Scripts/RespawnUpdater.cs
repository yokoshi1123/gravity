using System.Collections;
using System.Collections.Generic;
using UnityEngine;

public class RespawnUpdater : MonoBehaviour
{
    [SerializeField] private GameObject SE;
    [SerializeField] private Animator animator;
    private bool change = false;
    private bool resAnimation = false;

    //現在のリスポーン地点がここであることを示す。
    //private bool current = false;

    //collisionしている間true;
    //private bool collisioning = false;

    //public bool respawned = false;
    private GameObject resManager;
    //private Vector3 posi;
    //private Transform myTransform;

    private int resposi;
    


    void Awake()
    {
        resManager = GameObject.Find("RespawnManager");
        /*if(resManager == null)
        {
            Debug.Log("NULL");
        }*/
    }

    void Update()
    {
        
        resAnimation = resManager.GetComponent<RespawnManager>().resAnimation;
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

        /*if (resManager.GetComponent<RespawnManager>().respawnchanged)
        {
            if(!collisioning)
            {
                current = false;
            }
        
        }*/

        if (resposi == resManager.GetComponent<RespawnManager>().changePosi)
        {
            animator.SetBool("resAnimation", resAnimation);
        }
        //resManager.GetComponent<RespawnManager>().resAnimation = false;
    }

    private void OnCollisionEnter2D(Collision2D collision)
    {
        if (collision.gameObject.CompareTag("Player"))
        {
            change = true;
            //current = true;
            //collisioning = true;
            resManager.GetComponent<RespawnManager>().respawnchanged = true;

            animator.SetBool("change", change);
            collision.gameObject.GetComponent<PlayerController>().respawnPoint = transform.position + new Vector3(0, 0.99f, 0);
            SE.SetActive(true);
            resManager.GetComponent<RespawnManager>().changePosi++;

            resposi = resManager.GetComponent<RespawnManager>().changePosi;
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
        resManager.GetComponent<RespawnManager>().respawning1 = true;
    }

    public void RespawnAnimation2End()
    {
        Debug.Log("Animation2 End");
        transform.position += new Vector3(0, 0, -transform.position.z + 1.0f);
        resManager.GetComponent<RespawnManager>().respawning2 = true;
    }

}
