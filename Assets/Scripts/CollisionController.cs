using System.Collections;
using System.Collections.Generic;
using UnityEngine;
using UnityEngine.SceneManagement;

public class CollisionController : MonoBehaviour
{
    private PlayerController playerController;
    
    //private bool isJumping = false;
    //private bool isJumpActive = false;

    [SerializeField] private string sceneName;

    //private MoveObjectWithRoute movingFloor;
    //private Vector2 mFloorVelocity;

    [Header("電気柵")][SerializeField] private AudioClip spikeSE;
    [Header("ゴール")][SerializeField] private AudioClip warpSE;

    // Start is called before the first frame update
    void Start()
    {
        playerController = transform.parent.GetComponent<PlayerController>();
    }

    // Update is called once per frame
    //void Update()
    //{
    //    //playerController.SetBools_Collision(isJumping, isJumpActive);
    //}

    private void OnCollisionEnter2D(Collision2D collision)
    {
        //Debug.Log(collision.gameObject.tag);
        if (collision.gameObject.CompareTag("Goal"))
        {
            transform.parent.GetComponent<AudioSource>().PlayOneShot(warpSE, 0.5f);
            SceneManager.LoadScene(sceneName);
        }

        if (collision.gameObject.CompareTag("Toxic"))
        {
            //Debug.Log(collision.gameObject.name);
            transform.parent.GetComponent<AudioSource>().PlayOneShot(spikeSE, 0.4f);
            StartCoroutine(playerController.Respawn());
            //StartCoroutine(Test());
        }

        if (collision.gameObject.CompareTag("Abyss"))
        {
            StartCoroutine(playerController.Respawn());
        }
    }

    private void OnTriggerEnter2D(Collider2D collision)
    {
        if (collision.gameObject.CompareTag("Toxic"))
        {
            //Debug.Log(collision.gameObject.name);
            transform.parent.GetComponent<AudioSource>().PlayOneShot(spikeSE, 0.4f);
            StartCoroutine(playerController.Respawn());
            //StartCoroutine(Test());
        }
    }

    //    if (collision.gameObject.CompareTag("Toxic"))
    //    {
    //        //Vector2 hitPos = collision.ClosestPoint(grabPoint.position);
    //        ////Debug.Log(hitPos);
    //        //if (isGrabbing && (hitPos.x - transform.position.x) * scale.x > 0.65f)
    //        //{
    //        //    //Debug.Log("Safe");
    //        //    return;
    //        //}
    //        //Debug.Log(collision.gameObject.name);

    //        transform.parent.GetComponent<AudioSource>().PlayOneShot(spikeSE, 0.4f);
    //        StartCoroutine(playerController.Respawn());
    //        //StartCoroutine(Test());
    //    }
    //}
}
