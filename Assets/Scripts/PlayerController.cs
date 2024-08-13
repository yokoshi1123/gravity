using System.Collections;
using System.Collections.Generic;
using UnityEngine;
using UnityEngine.SceneManagement;

[RequireComponent(typeof(Rigidbody2D))]
[RequireComponent(typeof(AudioSource))]

public class PlayerController : MonoBehaviour
{
    private Rigidbody2D rb;
    [SerializeField] private Animator animator;
    [SerializeField] private BoxCollider2D grabCollider;

    private GravityManager gravityManager;
    [SerializeField] private GameObject pauseButton;

    public RespawnManager respawnManager;

    private TotalMass totalMass;

    [SerializeField] private float moveSpeed;
    [SerializeField] private int gravityDirection;
    private const float JUMPFORCE = 20.0f;

    [SerializeField] private Transform grabPoint;

    private bool isWalking = false;
    private bool isJumping = false;
    private bool isGrabbing = false;
    private bool canMove;

    private Vector3 scale;

    private const float RAYDISTANCE = 0.2f;
    private GameObject grabObj;
    private RaycastHit2D hit;
    private Vector3 grabPos;

    public Vector2 respawnPoint = new Vector2(0, 2);

    private MoveObjectWithRoute movingFloor;
    private Vector2 mFloorVelocity;

    [SerializeField] private string sceneName;

    [SerializeField] private AudioClip jumpSE;
    [SerializeField] private AudioClip spikeSE;
    [SerializeField] private AudioClip respawnSE;
    [SerializeField] private AudioClip warpSE;

    void Awake()
    {
        gravityManager = GameObject.Find("GravityManager").GetComponent<GravityManager>();
        rb = GetComponent<Rigidbody2D>();
        totalMass = GetComponent<TotalMass>();
        
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
                rb.velocity = new Vector2(horizontal * Mathf.Max(1.0f, moveSpeed/totalMass.GetMass()), rb.velocity.y);
            }
            else
            {
                rb.velocity = new Vector2(0, rb.velocity.y);
            }

            if (movingFloor != null && !isWalking)
            {
                mFloorVelocity = movingFloor.GetMFloorVelocity();
                //Debug.Log(mFloorVelocity);
                rb.velocity += mFloorVelocity;
            }

            if (Input.GetKeyDown(KeyCode.G))
            {
                Grab();
            }

            animator.SetBool("isWalking", isWalking);
            animator.SetBool("isJumping", isJumping);
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
        rb.AddForce(gravityDirection * JUMPFORCE * Vector2.up, ForceMode2D.Impulse);
    }

    private void Grab()
    {
        if (grabObj == null && !isJumping)
        {
            hit = Physics2D.Raycast(grabPoint.position, transform.right, RAYDISTANCE);
            if (hit.collider != null && hit.collider.CompareTag("Movable"))
            {
                grabObj = hit.collider.gameObject;
                grabPos = grabObj.transform.position;

                grabObj.GetComponent<Rigidbody2D>().isKinematic = true;
                grabObj.transform.position = new Vector2(grabPos.x + 0.15f * scale.x, grabPos.y);
                //grabCollider.offset = (grabObj.transform.position - grabPoint.position) * (Vector2)scale;
                //grabCollider.size = grabObj.transform.localScale;
                //Debug.Log(grabObj.name + grabObj.transform.localScale);
                grabCollider.enabled = true;
                grabObj.GetComponent<BoxCollider2D>().enabled = false;
                grabObj.transform.SetParent(transform);
                totalMass.PlusMass(grabObj.GetComponent<TotalMass>().GetMass());
                grabPoint.position = grabObj.transform.position;
                grabPoint.localScale = grabObj.transform.localScale;
                //Debug.Log(grabCollider.name + ", " + grabCollider.tag);

                isGrabbing = true;
            }
        }
        else if (grabObj != null)
        {
            grabObj.GetComponent<Rigidbody2D>().isKinematic = false;
            grabCollider.enabled = false;
            grabObj.GetComponent<BoxCollider2D>().enabled = true;
            grabPoint.position = transform.position + new Vector3(0.9f * scale.x, -0.5f * scale.y, 0);
            grabPoint.localScale = Vector3.one;
            totalMass.PlusMass(-grabObj.GetComponent<TotalMass>().GetMass());
            grabPos = grabObj.transform.position;
            grabObj.transform.position = new Vector2(grabPos.x - 0.15f * scale.x, grabPos.y);
            grabObj.transform.SetParent(null);
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
        Grab();

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
        if (collision.gameObject.CompareTag("Goal"))
        {
            GetComponent<AudioSource>().PlayOneShot(warpSE, 0.5f);
            SceneManager.LoadScene(sceneName);
        }

        if (collision.gameObject.CompareTag("Toxic"))
        {
            bool yRange = Mathf.Abs(collision.gameObject.transform.position.y - transform.position.y) <= transform.localScale.y;

            if (isGrabbing && (!yRange || yRange && (collision.gameObject.transform.position.x - grabObj.transform.position.x) * scale.x > 0))
            {
                //Debug.Log("Safe");
                return;
            }
            
            GetComponent<AudioSource>().PlayOneShot(spikeSE, 0.4f);
            StartCoroutine(Respawn());
            //StartCoroutine(Test());
        }

        if (collision.gameObject.CompareTag("Abyss"))
        {
            isJumping = true;
            StartCoroutine(Respawn());
        }
    }
    private void OnTriggerEnter2D(Collider2D collision)
    {
        if (collision.CompareTag("MovingFloor") && transform.position.y > collision.gameObject.transform.position.y)
        {
            isJumping = false;
            movingFloor = collision.gameObject.GetComponent<MoveObjectWithRoute>();
            Debug.Log(transform.position.y + ", " + collision.gameObject.transform.position.y + ": higher");
        }
        //else if (collision.CompareTag("MovingFloor"))
        //{
        //    Debug.Log(transform.position.y + ", " + collision.gameObject.transform.position.y);
        //}

    }

    private void OnTriggerStay2D(Collider2D collision)
    {
        if (collision.CompareTag("GravityField")) // 重力場中にあるとき、gravityManagerでの変更を読み込む
        {
            (rb.gravityScale, moveSpeed, gravityDirection) = gravityManager.GetValue();
            scale = gameObject.transform.localScale;
            scale.y = gravityDirection;
            gameObject.transform.localScale = scale;
        }
        else if (!collision.CompareTag("MovingFloor"))
        {
            isJumping = false;
        }
    }

    private void OnTriggerExit2D(Collider2D collision)
    {
        if (collision.CompareTag("GravityField")) // 重力場から出たとき、デフォルトに戻す
        {
            (rb.gravityScale, moveSpeed, gravityDirection) = gravityManager.GetDefaultValue();
        }
        else
        {
            isJumping = true;
        }

        if (collision.CompareTag("MovingFloor"))
        {
            movingFloor = null;
        }       
    }
}
