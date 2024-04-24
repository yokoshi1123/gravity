using System.Collections;
using System.Collections.Generic;
using UnityEngine;
using UnityEngine.UIElements;
// using TMPro;

[RequireComponent(typeof(Rigidbody2D))]
[RequireComponent(typeof(BoxCollider2D))]
[RequireComponent(typeof(Animator))]

public class PlayerController : MonoBehaviour
{
    [SerializeField] private Rigidbody2D rb;
    [SerializeField] private Animator animator;

    // public EnvironmentManager environmentManager;
    public GravityManager gravityManager; // EnvironmentManagerを廃止
    // public TextMeshProUGUI gravityText;

    [SerializeField] private float moveSpeed;
    // [SerializeField] private float gravityScale;
    [SerializeField] private float jumpForce = 20.0f;

    private bool isMoving = false;
    private bool isJumping = false;
    private bool isReverse = false;
    private bool isUpsideDown = false;
    // private bool inField= false;
    private int jumpDirection = 1;
    // private int gScale = 2;

    void Awake()
    {
        /* moveSpeed = environmentManager.moveSpeed;
        jumpForce = environmentManager.jumpForce;
        gravityDefault = environmentManager.gravityDefault; */
        moveSpeed = gravityManager.M_SPEED;
        rb.gravityScale = gravityManager.G_SCALE;
        isReverse = gravityManager.isReverse;
        // gravityText.text = "Gravity * 1.0";
        animator = GetComponent<Animator>();

    }

    // Update is called once per frame
    void Update()
    {
        float horizontal = Input.GetAxis("Horizontal");
        // Debug.Log(horizontal);
        isMoving = horizontal != 0;

        if (isMoving)
        {
            Vector3 scale = gameObject.transform.localScale;
            if (horizontal < 0 && scale.x > 0 || horizontal > 0 && scale.x < 0)
            {
                scale.x *= -1;
            }
            gameObject.transform.localScale = scale;
        }

        if (isReverse && !isUpsideDown)
        {
            Vector3 scale = gameObject.transform.localScale;
            Debug.Log(scale.y);
            scale.y *= -1;
            gameObject.transform.localScale = scale;
            isUpsideDown = true;
        }

        /*if (Input.GetKeyDown(KeyCode.UpArrow))
        {
            gScale = (gScale + 399999) % 4;
            ChangeGravity();
        }
        if (Input.GetKeyDown(KeyCode.DownArrow))
        {
            gScale = (gScale + 400001) % 4;
            ChangeGravity();
        }*/

        //ジャンプ
        if (Input.GetKeyDown(KeyCode.Space) && !isJumping && !(rb.velocity.y < -0.5f))
        {
            Jump();
        }
        
        //プレイヤーの移動
        rb.velocity = new Vector2(horizontal * moveSpeed, rb.velocity.y);

        animator.SetBool("isMoving", isMoving);
        animator.SetBool("isJumping", isJumping);

        /* if (inField)
        {
            rb.gravityScale = gravityDefault * (-1.0f);
            // moveSpeed = environmentManager.moveSpeed * 0.7f;
        }
        else if (!inField)
        {
            rb.gravityScale = gravityDefault ;
            // moveSpeed = environmentManager.moveSpeed ;
        } */
    }



    void Jump()
    {
        isJumping = true;
        // Debug.Log("Jumping");
        jumpDirection = (rb.gravityScale == gravityManager.G_SCALE * (-1.0f)) ? -1 : 1; 
        rb.AddForce(Vector2.up * jumpForce * jumpDirection, ForceMode2D.Impulse);
    }

    private void OnTriggerEnter2D(Collider2D collision)
    {
        if (collision.CompareTag("Stage"))
        {
            isJumping = false;
            // Debug.Log("On the ground");
        }

        /* if (collision.CompareTag("GravityField"))
        {
            // inField = true;
            moveSpeed = gravityManager.moveSpeed;
            gravityScale = gravityManager.gravityScale;
        } */
    }
    private void OnTriggerStay2D(Collider2D collision)
    {
        if (collision.CompareTag("GravityField")) // 重力場中にあるとき、gravityManagerでの変更を読み込む
        {
            moveSpeed = gravityManager.moveSpeed;
            rb.gravityScale = gravityManager.gravityScale;
            isReverse = gravityManager.isReverse;
            // Debug.Log("In the gravity field: " + rb.gravityScale);
        }
    }

    private void OnTriggerExit2D(Collider2D collision)
    {
        if (collision.CompareTag("GravityField")) // 重力場から出たとき、デフォルトに戻す
        {
            // inField = false;
            //Debug.Log("すり抜け終えた");
            moveSpeed = gravityManager.M_SPEED;
            rb.gravityScale = gravityManager.G_SCALE;
            isReverse = false;
            isUpsideDown = false;
        }

        if (collision.CompareTag("Stage"))//空中にいるときはisJumpingをtrue
        {
            isJumping = true;
            // Debug.Log("In the air");
        }

    }

    /* void ChangeGravity()
    {
        // Debug.Log("gscale: " + gScale);
        // jumpDirection = (gScale == 0) ? -1 : 1;
        
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
        

    }*/
}
