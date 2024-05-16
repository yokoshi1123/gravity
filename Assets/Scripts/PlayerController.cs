using System.Collections;
using System.Collections.Generic;
using Unity.Burst.CompilerServices;
using Unity.VisualScripting;
using UnityEngine;
using UnityEngine.UIElements;

[RequireComponent(typeof(Rigidbody2D))]
[RequireComponent(typeof(CapsuleCollider2D))]

public class PlayerController : MonoBehaviour
{
    [SerializeField] private Rigidbody2D rb;
    [SerializeField] private Animator animator;
    [SerializeField] private BoxCollider2D grabCollider;

    public GravityManager gravityManager; // EnvironmentManagerを廃止

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
    private float grabWidth;
    private Vector3 grabPos;
    private Vector3 grabScale;


    void Awake()
    {
        moveSpeed = gravityManager.M_SPEED;
        rb.gravityScale = gravityManager.G_SCALE;
        isReverse = gravityManager.isReverse; 
    }

    void Update()
    {
        float horizontal = Input.GetAxis("Horizontal");
        // Debug.Log(horizontal);
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
            // Debug.Log(scale.y);
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
        /*if (isGrabbing)
        {
            grabObj.GetComponent<Rigidbody2D>().velocity = new Vector2(horizontal * Mathf.Max(1.0f, moveSpeed - objWeight), grabObj.GetComponent<Rigidbody2D>().velocity.y);
            // Debug.Log(rb.velocity);
        }*/

        if (Input.GetKeyDown(KeyCode.G) && !isJumping)
        {
            Grab();
        }       

        animator.SetBool("isWalking", isWalking);
        animator.SetBool("isJumping", isJumping);
    }



    private void Jump()
    {
        isJumping = true;
        // Debug.Log("Jumping");
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

                //grabWidth = grabObj.GetComponent<Collider2D>().bounds.extents.x / grabObj.transform.localScale.x;
                grabPos = grabObj.transform.position;
                grabPos.x += 0.1f * scale.x; // grabWidth * scale.x;
                grabObj.transform.position = new Vector2(grabPos.x, grabPos.y+0.2f);
                grabObj.transform.SetParent(transform);

                grabObj.GetComponent<BoxCollider2D>().enabled = false;
                //grabCollider.offset = grabObj.GetComponent<BoxCollider2D>().transform.position - grabPoint.position; // transform.position;
                //grabCollider.size = grabObj.GetComponent<BoxCollider2D>().size;
                //grabCollider.enabled = true;

                //objWeight = grabObj.GetComponent<Rigidbody2D>().mass / 5.0f;
                //grabObj.GetComponent<Rigidbody2D>().sharedMaterial.friction = 0f;
                //Debug.Log("Mass:" + objWeight);
                isGrabbing = true;
            }
        }
        else
        {
            grabObj.GetComponent<Rigidbody2D>().isKinematic = false;
            //grabCollider.enabled = false;
            //grabObj.GetComponent<BoxCollider2D>().enabled = true;

            //grabObj.GetComponent<CollisionController>().isGrabbed = true;
            //grabObj.GetComponent<Rigidbody2D>().sharedMaterial.friction = 1.0f;
            /*grabScale = grabObj.transform.localScale;
            if (grabScale.y < 0)
            {
                grabScale.y *= -1;
            }
            grabObj.transform.localScale = grabScale;*/
            grabObj.transform.SetParent(null);
            objWeight = 0.0f;
            grabObj = null;
            isGrabbing = false;
        }

        //Debug.Log(grabObj);
        animator.SetBool("isGrabbing", isGrabbing);
    }

    private void OnTriggerStay2D(Collider2D collision)
    {
        if (collision.CompareTag("Stage"))
        {
            isJumping = false;
            // Debug.Log("On the ground");
        }

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
        }

        if (collision.CompareTag("Stage"))//空中にいるときはisJumpingをtrue
        {
            isJumping = true;
            // Debug.Log("In the air");
        }
    }
}
