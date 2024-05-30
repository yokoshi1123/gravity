using System.Collections;
using System.Collections.Generic;
using UnityEngine;
using UnityEngine.SceneManagement;

[RequireComponent(typeof(Rigidbody2D))]

public class PlayerController : MonoBehaviour
{
    [SerializeField] private Rigidbody2D rb;
    [SerializeField] private Animator animator;
    [SerializeField] private BoxCollider2D grabCollider;

    public GravityManager gravityManager;

    [SerializeField] private float moveSpeed;
    private float jumpForce = 20.0f;

    [SerializeField] private Transform grabPoint;

    private bool isWalking = false;
    private bool isJumping = false;
    private bool isGrabbing = false;
    private bool isReverse = false;
    private int jumpDirection = 1;

    private Vector3 scale;

    private float rayDistance = 0.2f;
    private GameObject grabObj;
    private float objWeight = 0.0f;
    private RaycastHit2D hit;
    //private float grabWidth;
    private Vector3 grabPos;
   // private Vector3 grabScale;

    [SerializeField] private float ABYSS = 10.0f;
    public Vector2 respawnPoint = new Vector2(0, 2);

    [SerializeField] private string sceneName;

    void Awake()
    {
        moveSpeed = gravityManager.M_SPEED;
        rb.gravityScale = gravityManager.G_SCALE;
        isReverse = gravityManager.isReverse; 
    }

    void Update()
    {
        float horizontal = Input.GetAxis("Horizontal");
        isWalking = horizontal != 0;

        scale = gameObject.transform.localScale;
        if (isWalking && !isGrabbing)
        {
            if (horizontal < 0 && scale.x > 0 || horizontal > 0 && scale.x < 0)
            {
                scale.x *= -1;
            }
            gameObject.transform.localScale = scale;
        }

        if (!isReverse && scale.y == -1)
        {
            scale.y = 1;
            gameObject.transform.localScale = scale;
        }

        //ジャンプ
        if (Input.GetKeyDown(KeyCode.Space) && !isJumping && !isGrabbing && !(rb.velocity.y < -0.5f))
        {
            Jump();
        }
        
        //プレイヤーの移動
        rb.velocity = new Vector2(horizontal * Mathf.Max(1.0f, moveSpeed - objWeight), rb.velocity.y);
        
        if (Input.GetKeyDown(KeyCode.G) && !isJumping)
        {
            Grab();
        }       

        animator.SetBool("isWalking", isWalking);
        animator.SetBool("isJumping", isJumping);

        if (transform.position.y < ABYSS)
        {
            Respawn();
        }
    }



    private void Jump()
    {
        isJumping = true;
        jumpDirection = (isReverse) ? -1 : 1; 
        rb.AddForce(Vector2.up * jumpForce * jumpDirection, ForceMode2D.Impulse);
    }

    private void Grab()
    {
        if (grabObj == null)
        {
            hit = Physics2D.Raycast(grabPoint.position, transform.right, rayDistance);
            if (hit.collider != null && hit.collider.tag == "Movable")
            {
                grabObj = hit.collider.gameObject;
                grabObj.GetComponent<Rigidbody2D>().isKinematic = true;

                //grabObj.GetComponent<CollisionController>().isGrabbed = true;

                grabPos = grabObj.transform.position;
                grabPos.x += 0.1f * scale.x;
                grabObj.transform.position = new Vector2(grabPos.x, grabPos.y+0.2f);
                grabObj.transform.SetParent(transform);

                grabObj.GetComponent<BoxCollider2D>().enabled = false;
                //grabCollider.offset = grabObj.GetComponent<BoxCollider2D>().transform.position - grabPoint.position; // transform.position;
                //grabCollider.size = grabObj.GetComponent<BoxCollider2D>().size;
                //grabCollider.enabled = true;

                isGrabbing = true;
            }
        }
        else
        {
            grabObj.GetComponent<Rigidbody2D>().isKinematic = false;
            //grabCollider.enabled = false;
            //grabObj.GetComponent<BoxCollider2D>().enabled = true;

            //grabObj.GetComponent<CollisionController>().isGrabbed = true;
            grabObj.transform.SetParent(null);
            objWeight = 0.0f;
            grabObj = null;
            isGrabbing = false;
        }

        animator.SetBool("isGrabbing", isGrabbing);
    }
    private void Respawn()
    {
        rb.velocity = Vector2.zero;
        transform.position = respawnPoint;
    }

    private void OnCollisionEnter2D(Collision2D collision)
    {
        if (collision.gameObject.tag == "Goal")
        {
            SceneManager.LoadScene(sceneName);
        }

        if (collision.gameObject.tag == "Toxic")
        {
            Respawn();
        }
    }

    private void OnTriggerStay2D(Collider2D collision)
    {
        /*if (collision.CompareTag("Stage"))
        {
            isJumping = false;
        }*/

        if (collision.CompareTag("GravityField")) // 重力場中にあるとき、gravityManagerでの変更を読み込む
        {
            moveSpeed = gravityManager.moveSpeed;
            rb.gravityScale = gravityManager.gravityScale;
            isReverse = gravityManager.isReverse;
            scale = gameObject.transform.localScale;
            if (isReverse && scale.y == 1)
            {
                scale.y = -1;
                gameObject.transform.localScale = scale;
            }
        }

        else
        {
            isJumping = false;
        }
    }

    private void OnTriggerExit2D(Collider2D collision)
    {
        if (collision.CompareTag("GravityField")) // 重力場から出たとき、デフォルトに戻す
        {
            moveSpeed = gravityManager.M_SPEED;
            rb.gravityScale = gravityManager.G_SCALE;
            isReverse = false;
        }

        //if (collision.CompareTag("Stage"))//空中にいるときはisJumpingをtrue
        else
        {
            isJumping = true;
        }
    }
}
