using System.Collections;
using System.Collections.Generic;
using UnityEngine;
using UnityEngine.UIElements;

public class EnvironmentManager : MonoBehaviour
{
    public float moveSpeed = 10.0f;
    public float jumpForce = 20.0f;
    public float gravityDefault = 5.0f;

    /*void Update()
    {
        if (Input.GetKeyDown(KeyCode.UpArrow))
        {
            gScale = (gScale + 400001) % 4;
            ChangeGravity();
        }
        if (Input.GetKeyDown(KeyCode.DownArrow))
        {
            gScale = (gScale + 399999) % 4;
            ChangeGravity();
        }
    }
    
    void ChangeGravity()
    {
        // Debug.Log("gscale: " + gScale);
        switch (gScale)
        {
            case 0: // x(-1.0)
                rb.gravityScale = gravityDefault * (-1.0f);
                moveSpeed = environmentManager.moveSpeed;
                break;
            case 1: // x0.5
                rb.gravityScale = gravityDefault * 0.5f;
                moveSpeed = environmentManager.moveSpeed * 2.0f;
                break;
            case 2: // x1.0
                rb.gravityScale = gravityDefault;
                moveSpeed = environmentManager.moveSpeed;
                break;
            case 3: // x2.0
                rb.gravityScale = gravityDefault * 2.0f;
                moveSpeed = environmentManager.moveSpeed * 0.5f;
                break;
            default:
                Debug.Log("ChangeGravityÇ≈ÉGÉâÅ[");
                break;
        }
    }*/
}
