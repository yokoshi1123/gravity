using System.Collections;
using System.Collections.Generic;
using UnityEngine;
using TMPro;

[RequireComponent(typeof(Rigidbody2D))]
[RequireComponent(typeof(CircleCollider2D))]

public class PlayerController : MonoBehaviour
{
    [SerializeField] private Rigidbody2D rb;

    public EnvironmentManager environmentManager;
    public TextMeshProUGUI gravityText;

    [SerializeField] private float moveSpeed;
    [SerializeField] private float jumpForce;
    [SerializeField] private float gravityDefault;

    private bool isJumping = false;
    private bool inField= false;
    private int jumpDirection = 1;
    private int gScale = 2;

    void Awake()
    {
        moveSpeed = environmentManager.moveSpeed;
        jumpForce = environmentManager.jumpForce;
        gravityDefault = environmentManager.gravityDefault;
        gravityText.text = "Gravity * 1.0";
    }

    // Update is called once per frame
    void Update()
    {
        if (Input.GetKeyDown(KeyCode.UpArrow))
        {
            gScale = (gScale + 399999) % 4;
            ChangeGravity();
        }
        if (Input.GetKeyDown(KeyCode.DownArrow))
        {
            gScale = (gScale + 400001) % 4;
            ChangeGravity();
        }

        //ジャンプ
        if (Input.GetKeyDown(KeyCode.Space) && !isJumping && !(rb.velocity.y < -0.5f))
        {
            Jump();
        }
        
        //プレイヤーの移動
        rb.velocity = new Vector2(Input.GetAxis("Horizontal") * moveSpeed, rb.velocity.y);

        /*if (inField)
        {
            rb.gravityScale = gravityDefault * 0.5f;
            moveSpeed = environmentManager.moveSpeed * 0.7f;
        }
        else if (!inField)
        {
            rb.gravityScale = gravityDefault ;
            moveSpeed = environmentManager.moveSpeed ;
        }*/
    }



    void Jump()
    {
        isJumping = true;
        rb.AddForce(Vector2.up * jumpForce * jumpDirection, ForceMode2D.Impulse);
    }

    private void OnTriggerEnter2D(Collider2D collision)
    {
        if (collision.CompareTag("Stage"))
        {
            isJumping = false;
        }

        if (collision.CompareTag("GravityField"))
        {
            inField = true;
        }


    }
    private void OnTriggerExit2D(Collider2D collision)
    {
        
        if (collision.CompareTag("GravityField"))
        {
            inField = false;
            //Debug.Log("すり抜け終えた");
        }
        
    }

    void ChangeGravity()
    {
        // Debug.Log("gscale: " + gScale);
        jumpDirection = (gScale == 0) ? -1 : 1;
        
        switch (gScale)
        {
            case 0: // x(-1.0)
                rb.gravityScale = gravityDefault * (-1.0f);
                moveSpeed = environmentManager.moveSpeed;
                gravityText.text = "Gravity * (-1.0)";
                break;
            case 1: // x0.5
                rb.gravityScale = gravityDefault * 0.5f;
                moveSpeed = environmentManager.moveSpeed * 0.7f;
                gravityText.text = "Gravity * 0.5";
                break;
            case 2: // x1.0
                rb.gravityScale = gravityDefault;
                moveSpeed = environmentManager.moveSpeed;
                gravityText.text = "Gravity * 1.0";
                break;
            case 3: // x2.0
                rb.gravityScale = gravityDefault * 2.0f;
                moveSpeed = environmentManager.moveSpeed * 0.5f;
                gravityText.text = "Gravity * 2.0";
                break;
            default:
                Debug.Log("ChangeGravityでエラー");
                break;
        }
        

    }
}
