using System.Collections;
using System.Collections.Generic;
using UnityEngine;

public class RespawnUpdater : MonoBehaviour
{
    [SerializeField] private GameObject SE;
    [SerializeField] private Animator animator;
    private bool change = false;

    void Awake()
    {
        
    }
    private void OnCollisionEnter2D(Collision2D collision)
    {
        if (collision.gameObject.tag == "Player")
        {
            change = true;
            animator.SetBool("change", change);
            collision.gameObject.GetComponent<PlayerController>().respawnPoint = transform.position;
            SE.SetActive(true);
            change = true;

            animator.SetBool("change", change);

        }
    }

}
