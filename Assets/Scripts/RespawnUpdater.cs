using System.Collections;
using System.Collections.Generic;
using UnityEngine;

public class RespawnUpdater : MonoBehaviour
{
    [SerializeField] private GameObject SE;
    [SerializeField] private Animator animator;
    private bool change = false;
    private bool resAnimation;

    //public bool respawned = false;
    private GameObject resManager;
    private Vector3 posi;
    private Transform myTransform;
    


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
        myTransform = this.transform;
        posi = myTransform.position;
        resAnimation = resManager.GetComponent<RespawnManager>().resAnimation;

        if (resAnimation)
        {
            posi.z = 2.0f;
            animator.SetBool("resAnimation", resAnimation);
        }


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
        resManager.GetComponent<RespawnManager>().respawning1 = true;
    }

    public void RespawnAnimation2End()
    {
        posi.z = -1.0f;
        resManager.GetComponent<RespawnManager>().respawning2 = true;
    }

}
