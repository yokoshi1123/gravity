using System.Collections;
using System.Collections.Generic;
using UnityEngine;

public class RespawnUpdater : MonoBehaviour
{
    [SerializeField] private GameObject SE;
    [SerializeField] private Animator animator;
    private bool change = false;
    public bool respawned = false;
    private GameObject resManager;
    private Vector3 posi;
    private Transform myTransform;
    


    void Awake()
    {
        resManager = GameObject.Find("RespawnManager");
        
    }

    void Update()
    {
        myTransform = this.transform;
        posi = myTransform.position;
    }

    private void OnCollisionEnter2D(Collision2D collision)
    {
        if (collision.gameObject.tag == "Player")
        {
            change = true;
            animator.SetBool("change", change);
            collision.gameObject.GetComponent<PlayerController>().respawnPoint = transform.position;
            SE.SetActive(true);

            //change = true;
            //animator.SetBool("change", change);

        }
    }

    public void RespawnAnimation1End()
    {
        //posi.z = 1.0f;
        //resManager.GetComponent<RespawnManager>().respawned = true;
    }

    public void RespawnAnimation2End()
    {
        //posi.z = -1.0f;
    }

}
