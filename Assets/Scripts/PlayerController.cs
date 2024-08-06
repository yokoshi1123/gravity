using System.Collections;
using System.Collections.Generic;
using UnityEngine;
using UnityEngine.SceneManagement;

[RequireComponent(typeof(Rigidbody2D))]
[RequireComponent(typeof(AudioSource))]

public class PlayerController : MonoBehaviour
{
    [SerializeField] private Rigidbody2D rb;
    [SerializeField] private Animator animator;
    [SerializeField] private BoxCollider2D grabCollider;

    private GravityManager gravityManager;
    [SerializeField] private GameObject pauseButton;

    public RespawnManager respawnManager;

    [SerializeField] private float moveSpeed;
    [SerializeField] private int gravityDirection;
    private float jumpForce = 20.0f;

    [SerializeField] private Transform grabPoint;

    private bool isWalking = false;
    private bool isJumping = false;
    private bool isGrabbing = false;
    //private bool isReverse = false;
    private bool canMove;
    //private int jumpDirection = 1;

    private Vector3 scale;

    private float rayDistance = 0.2f;
    private GameObject grabObj;
    private float objWeight = 0.0f;
    private RaycastHit2D hit;
    //private float grabWidth;
    private Vector3 grabPos;
   // private Vector3 grabScale;

    //[SerializeField] private float ABYSS = 10.0f;
    public Vector2 respawnPoint = new Vector2(0, 2);

    [SerializeField] private string sceneName;

    [SerializeField] private AudioClip jumpSE;
    [SerializeField] private AudioClip spikeSE;
    [SerializeField] private AudioClip respawnSE;
    [SerializeField] private AudioClip warpSE;

    void Awake()
    {
        gravityManager = GameObject.Find("GravityManager").GetComponent<GravityManager>();
        //moveSpeed = gravityManager.M_SPEED;
        //rb.gravityScale = gravityManager.G_SCALE;
        //isReverse = gravityManager.isReverse;
        (rb.gravityScale, moveSpeed, gravityDirection) = gravityManager.GetDefaultValue();
        respawnPoint = transform.position;
    }

    void Update()
    {
        canMove = respawnManager.canMove;


        if (canMove)
        {
            float horizontal = Input.GetAxisRaw("Horizontal");
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

            //if (!isReverse && scale.y == -1)
            //{
            //    scale.y = 1;
            //    gameObject.transform.localScale = scale;
            //}
            scale.y = gravityDirection;
            gameObject.transform.localScale = scale;

            //ジャンプ
            if ((Input.GetKeyDown(KeyCode.Space) || Input.GetKeyDown(KeyCode.UpArrow) || Input.GetKeyDown(KeyCode.W)) && !isJumping && !isGrabbing && !(rb.velocity.y < -0.5f))
            {
                GetComponent<AudioSource>().PlayOneShot(jumpSE, 0.3f);
                Jump();
            }

            //プレイヤーの移動
            if (!isGrabbing || !isJumping)
            {
                rb.velocity = new Vector2(horizontal * Mathf.Max(1.0f, moveSpeed - objWeight), rb.velocity.y);
            }
            else
            {
                rb.velocity = new Vector2(0, rb.velocity.y);
            }


            if (Input.GetKeyDown(KeyCode.G))
            {
                Grab();
            }

            animator.SetBool("isWalking", isWalking);
            animator.SetBool("isJumping", isJumping);

            //if (transform.position.y < ABYSS)
            //{
            //    StartCoroutine(Respawn()); //Respawn();
            //}
        }

        //canMove = falseのとき、速度0にし続ける
        if(!canMove)
        {
            rb.velocity = Vector2.zero;
        }

    }

    private void Jump()
    {
        isJumping = true;
        //jumpDirection = (isReverse) ? -1 : 1;
        rb.AddForce(Vector2.up * jumpForce * gravityDirection, ForceMode2D.Impulse);
    }

    private void Grab()
    {
        if (grabObj == null && !isJumping)
        {
            hit = Physics2D.Raycast(grabPoint.position, transform.right, rayDistance);
            if (hit.collider != null && hit.collider.tag == "Movable")
            {
                grabObj = hit.collider.gameObject;
                grabObj.GetComponent<Rigidbody2D>().isKinematic = true;
                grabPos = grabObj.transform.position;
                grabObj.transform.position = new Vector2(grabPos.x + 0.15f * scale.x, grabPos.y + 0.2f * scale.y);

                grabCollider.offset = (grabObj.transform.position - grabPoint.position) * new Vector2(scale.x, scale.y);
                grabCollider.size = grabObj.transform.localScale;
                //Debug.Log(grabObj.name + grabObj.transform.localScale);
                grabObj.GetComponent<BoxCollider2D>().enabled = false;
                grabCollider.enabled = true;
                grabObj.transform.SetParent(transform);



                isGrabbing = true;
            }
        }
        else if (grabObj != null)
        {
            grabObj.GetComponent<Rigidbody2D>().isKinematic = false;
            grabCollider.enabled = false;
            grabObj.GetComponent<BoxCollider2D>().enabled = true;
            grabPos = grabObj.transform.position;
            grabObj.transform.position = new Vector2(grabPos.x - 0.15f * scale.x, grabPos.y + 0.2f*scale.y);
            grabObj.transform.SetParent(null);
            objWeight = 0.0f;
            grabObj = null;
            isGrabbing = false;
        }

        animator.SetBool("isGrabbing", isGrabbing);
    }

    private IEnumerator Respawn()
    {
        pauseButton.SetActive(false);
        Time.timeScale = 0;
        yield return new WaitForSecondsRealtime(1); // 1秒遅延
        //Debug.Log("Died");
        Time.timeScale = 1;
        pauseButton.SetActive(true);
        GameObject destroyGF = GameObject.FindWithTag("GravityField");
        if (destroyGF != null)
        {
            Destroy(destroyGF);
        }
        rb.velocity = Vector2.zero;
        respawnManager.respawn = true;
        transform.position = respawnPoint;
        respawnManager.resAnimation = true;

        /*if(respawnManager.respawn)
        {
            Debug.Log("respawn");
        }
        if (respawnManager.resAnimation)
        {
            Debug.Log("resAnimation");
        }*/

        int i = 0;


        
        //Debug.Log(respawnManager.changePosi);
        if (respawnManager.changePosi >= 1)
        {

            while (!respawnManager.respawning2 && i <= 30)
            {
                yield return new WaitForSecondsRealtime(0.05f);
                i++;
            }
        }

        /*if (i >= 25)
        {
            Debug.Log("i=15");
        }*/

        //respawnManager.resAnimation = false;
        


        //Debug.Log("before:" + isJumping);
        GetComponent<AudioSource>().PlayOneShot(respawnSE, 0.2f);
        //Debug.Log("after:" + isJumping);
        //respawnManager.canActive = true;
        //Debug.Log("canActive");
    }

    /*private IEnumerator Test()
    {
        for (int i = 0; i < 20; i++)
        {
            yield return new WaitForSecondsRealtime(0.05f);
            Debug.Log(i);
        }
    }*/

    private void OnCollisionEnter2D(Collision2D collision)
    {
        if (collision.gameObject.tag == "Goal")
        {
            GetComponent<AudioSource>().PlayOneShot(warpSE, 0.5f);
            SceneManager.LoadScene(sceneName);
        }

        if (collision.gameObject.tag == "Toxic")
        {
            GetComponent<AudioSource>().PlayOneShot(spikeSE, 0.4f);
            StartCoroutine(Respawn());
            //StartCoroutine(Test());
        }

        if (collision.gameObject.tag == "Abyss")
        {
            isJumping = true;
            StartCoroutine(Respawn());
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
            //moveSpeed = gravityManager.moveSpeed;
            //rb.gravityScale = gravityManager.gravityScale;
            //isReverse = gravityManager.isReverse;
            (rb.gravityScale, moveSpeed, gravityDirection) = gravityManager.GetValue();
            scale = gameObject.transform.localScale;
            //if (isReverse && scale.y == 1)
            //{
            //    scale.y = -1;
            //    gameObject.transform.localScale = scale;
            //}
            scale.y = gravityDirection;
            gameObject.transform.localScale = scale;
        }

        else if (!collision.CompareTag("Platform") || (collision.CompareTag("Platform") && transform.position.y > 2 + collision.gameObject.transform.position.y))
        {
            isJumping = false;
        }
    }

    private void OnTriggerExit2D(Collider2D collision)
    {
        if (collision.CompareTag("GravityField")) // 重力場から出たとき、デフォルトに戻す
        {
            //moveSpeed = gravityManager.M_SPEED;
            //rb.gravityScale = gravityManager.G_SCALE;
            //isReverse = false;
            (rb.gravityScale, moveSpeed, gravityDirection) = gravityManager.GetDefaultValue();
        }

        //if (collision.CompareTag("Stage"))//空中にいるときはisJumpingをtrue
        else
        {
            isJumping = true;
        }
    }
}
