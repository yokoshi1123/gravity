using System.Collections;
using System.Collections.Generic;
using UnityEngine;

[RequireComponent(typeof(Rigidbody2D))]

public class OnTheMovingFloor : MonoBehaviour
{
    private Rigidbody2D rb;
    
    private MoveObjectWithRoute movingFloor;
    private Vector2 mFloorVelocity;
    
    // Start is called before the first frame update
    void Awake()
    {
        rb = GetComponent<Rigidbody2D>();
    }

    // Update is called once per frame
    void FixedUpdate()
    {
        if (movingFloor != null)
        {
            mFloorVelocity = movingFloor.GetMFloorVelocity();
            //Debug.Log(mFloorVelocity);
            rb.velocity = mFloorVelocity;
        }
    }

    public void OnCollisionEnter2D(Collision2D collision)
    {
        if (collision.gameObject.CompareTag("MovingFloor") && transform.position.y > transform.localScale.y/2 + collision.gameObject.transform.position.y)
        {
            movingFloor = collision.gameObject.GetComponent<MoveObjectWithRoute>();
        }
    }

    public void OnCollisionExit2D(Collision2D collision)
    {
        if (collision.gameObject.CompareTag("MovingFloor"))
        {
            movingFloor = null;
        }
    }
}
