using System.Collections;
using System.Collections.Generic;
using UnityEngine;

public class RespawnUpdater : MonoBehaviour
{
    [SerializeField] private GameObject SE;
    [SerializeField] private Animator animator;
    private bool change = false;
    private bool resAnimation = false;

    //public bool respawned = false;
    private GameObject resManager;
    private Vector3 posi;
    //private Transform myTransform;
    


    void Awake()
    {
        resManager = GameObject.Find("RespawnManager");
        if(resManager == null)
        {
            Debug.Log("NULL");
        }

        

    }

    void Update()
    {
        
        resAnimation = resManager.GetComponent<RespawnManager>().resAnimation;

        if (resAnimation)
        {
            transform.position += new Vector3(0, 0, -transform.position.z - 2.0f);
        }
        else
        {
            //animator.SetBool("resAnimation", resAnimation);
        }

        animator.SetBool("resAnimation", resAnimation);
        resManager.GetComponent<RespawnManager>().resAnimation = false;
    }

    private void OnCollisionEnter2D(Collision2D collision)
    {
        if (collision.gameObject.tag == "Player")
        {
            change = true;

            animator.SetBool("change", change);
            collision.gameObject.GetComponent<PlayerController>().respawnPoint = transform.position;
            SE.SetActive(true);
            resManager.GetComponent<RespawnManager>().changePosi++;

            //change = true;
            //animator.SetBool("change", change);

        }
    }

    public void RespawnAnimation1End()
    {
        Debug.Log("Animation1 End");
        resManager.GetComponent<RespawnManager>().respawning1 = true;
    }

    public void RespawnAnimation2End()
    {
        transform.position += new Vector3(0, 0, -transform.position.z + 1.0f);
        resManager.GetComponent<RespawnManager>().respawning2 = true;
    }

}
