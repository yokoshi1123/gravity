using System.Collections;
using System.Collections.Generic;
using UnityEngine;

[RequireComponent(typeof(Rigidbody2D))]
[RequireComponent(typeof(CircleCollider2D))]

public class PlayerController : MonoBehaviour
{
    [SerializeField] private Rigidbody2D rb;

    public EnvironmentManager environmentManager;

    [SerializeField] private float moveSpeed;
    [SerializeField] private float jumpForce;
    [SerializeField] private float gravityDefault;

    private bool isJumping = false;
    private bool isHeavy = false;

    void Awake()
    {
        moveSpeed = environmentManager.moveSpeed;
        jumpForce = environmentManager.jumpForce;
        gravityDefault = environmentManager.gravityDefault;
    }

    // Update is called once per frame
    void Update()
    {
        if (Input.GetKeyDown(KeyCode.G))
        {
            ChangeGravity();
        }

        if (Input.GetKeyDown(KeyCode.Space) && !isJumping && !(rb.velocity.y < -0.5f))
        {
            Jump();
        }
        
        rb.velocity = new Vector2(Input.GetAxis("Horizontal") * moveSpeed, rb.velocity.y);
    }

    void Jump()
    {
        isJumping = true;

        rb.AddForce(Vector2.up * jumpForce, ForceMode2D.Impulse);
    }

    private void OnTriggerEnter2D(Collider2D collision)
    {
        if (collision.CompareTag("Stage"))
        {
            isJumping = false;
        }
    }

    void ChangeGravity()
    {
        if (isHeavy)
        {
            isHeavy = false;
            rb.gravityScale = gravityDefault;
        }
        else if (!isHeavy)
        {
            isHeavy = true;
            rb.gravityScale = gravityDefault * 2;
        }
    }
}
