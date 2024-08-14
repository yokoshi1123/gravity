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

    private float moveSpeed;
    private float magnification;
    private int gravityDirection;
    private const float JUMPFORCE = 20.1f;
    [SerializeField] private float OBJ_MASS = 1f;

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
    private float grabMass;

    public Vector2 respawnPoint = new(0, 2);

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
        
        (rb.gravityScale, moveSpeed, magnification) = gravityManager.GetDefaultValue();
        gravityDirection = 1;
        rb.mass = OBJ_MASS;
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

            //�W�����v
            if ((Input.GetKeyDown(KeyCode.Space) || Input.GetKeyDown(KeyCode.UpArrow) || Input.GetKeyDown(KeyCode.W)) && !isJumping && !isGrabbing)
            {
                GetComponent<AudioSource>().PlayOneShot(jumpSE, 0.3f);
                //Debug.Log(rb.velocity.y);
                Jump();
            }

            if (isGrabbing)
            {
                totalMass.PlusMass(grabObj.GetComponent<TotalMass>().GetMass() - grabMass);
                grabMass = grabObj.GetComponent<TotalMass>().GetMass();
            }

            //�v���C���[�̈ړ�
            if (!isGrabbing || !isJumping)
            {
                rb.velocity = new Vector2(horizontal * Mathf.Max(1.0f, moveSpeed/totalMass.GetMass() * Mathf.Abs(magnification)), rb.velocity.y);
            }
            else
            {
                rb.velocity = new Vector2(0, rb.velocity.y);
            }

            if (movingFloor != null && !isWalking && !isJumping)
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

        //canMove = false�̂Ƃ��A���x0�ɂ�������
        if(!canMove)
        {
            rb.velocity = Vector2.zero;
        }

    }

    private void Jump()
    {
        isJumping = true;
        rb.velocity = new(rb.velocity.x, 0f);
        rb.AddForce(JUMPFORCE * Vector2.up * magnification, ForceMode2D.Impulse);
        //if (movingFloor != null)
        //{
        //    Debug.Log(mFloorVelocity);
        //}
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
                grabMass = grabObj.GetComponent<TotalMass>().GetDefaultMass();
                totalMass.PlusMass(grabMass);
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
            totalMass.PlusMass(-totalMass.GetMass() + OBJ_MASS * Mathf.Abs(magnification));
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
        yield return new WaitForSecondsRealtime(1); // 1�b�x��
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

        if(respawnManager.respawn)
        {
            Debug.Log("respawn");
        }
        if (respawnManager.resAnimation)
        {
            Debug.Log("resAnimation");
        }

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
    //private void OnTriggerEnter2D(Collider2D collision)
    //{
    //    if (collision.CompareTag("Platform") && transform.position.y > collision.gameObject.transform.position.y)
    //    {
    //        isJumping = false;
    //        movingFloor = collision.gameObject.GetComponent<MoveObjectWithRoute>();
    //        //Debug.Log(transform.position.y + ", " + collision.gameObject.transform.position.y + ": higher");
    //    }
    //    //else if (collision.CompareTag("Platform"))
    //    //{
    //    //    Debug.Log(transform.position.y + ", " + collision.gameObject.transform.position.y);
    //    //}

    //}

    private void OnTriggerStay2D(Collider2D collision)
    {
        if (collision.CompareTag("GravityField")) // �d�͏ꒆ�ɂ���Ƃ��AgravityManager�ł̕ύX��ǂݍ���
        {
            (rb.gravityScale, moveSpeed, magnification) = gravityManager.GetValue();
            gravityDirection = (int)Mathf.Sign(magnification);
            rb.mass = (isGrabbing) ? totalMass.GetMass() * Mathf.Abs(magnification) : OBJ_MASS * Mathf.Abs(magnification);
            scale = gameObject.transform.localScale;
            scale.y = gravityDirection;
            gameObject.transform.localScale = scale;
        }
        else if (!collision.CompareTag("Platform"))
        {
            isJumping = false;
        }

        if (collision.CompareTag("Platform") && transform.position.y > collision.gameObject.transform.position.y)
        {
            isJumping = false;
            movingFloor = collision.gameObject.GetComponent<MoveObjectWithRoute>();
            //Debug.Log(transform.position.y + ", " + collision.gameObject.transform.position.y + ": higher");
        }
    }

    private void OnTriggerExit2D(Collider2D collision)
    {
        if (collision.CompareTag("GravityField")) // �d�͏ꂩ��o���Ƃ��A�f�t�H���g�ɖ߂�
        {
            (rb.gravityScale, moveSpeed, magnification) = gravityManager.GetDefaultValue();
            gravityDirection = 1;
            rb.mass = OBJ_MASS;
        }
        else
        {
            isJumping = true;
        }

        if (collision.CompareTag("Platform"))
        {
            movingFloor = null;
        }       
    }
}
