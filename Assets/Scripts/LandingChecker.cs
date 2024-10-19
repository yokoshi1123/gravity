using System.Collections;
using System.Collections.Generic;
using UnityEngine;

public class LandingChecker : MonoBehaviour
{
    private PlayerController playerController;

    private bool isJumping = false;
    private bool isJumpActive = false;

    void Awake()
    {
        playerController = transform.parent.GetComponent<PlayerController>();
    }

    void Update()
    {
        
    }

    private void OnTriggerStay2D(Collider2D collision)
    {
        if (!collision.CompareTag("GravityField") && !collision.CompareTag("Toxic") && !collision.CompareTag("Platform") && !collision.CompareTag("Tutorial"))
        {
            //Debug.Log("Can Jump");
            isJumping = false;
            isJumpActive = false;
            playerController.SetIsJumping(isJumping, isJumpActive);
        }

        if (collision.gameObject.CompareTag("Platform") && transform.position.y + 0.1f> collision.gameObject.transform.position.y)
        {
            //Debug.Log(transform.position.y + ", " + collision.gameObject.transform.position.y);
            isJumping = false;
            isJumpActive = false;
            playerController.SetIsJumping(isJumping, isJumpActive);
            playerController.SetMovingFloor(collision.gameObject.GetComponent<MoveObjectWithRoute>());
        }
    }
    private void OnTriggerExit2D(Collider2D collision)
    {
        if (!collision.CompareTag("GravityField") && !collision.CompareTag("Toxic") && !collision.CompareTag("Platform") && !collision.CompareTag("Tutorial"))
        {
            //Debug.Log("Can Jump");
            isJumping = true;
            playerController.SetIsJumping(isJumping);
        }

        if (collision.gameObject.CompareTag("Platform"))
        {
            isJumping = true;
            playerController.SetIsJumping(isJumping);
            playerController.SetMovingFloor(null);
        }
    }
}
