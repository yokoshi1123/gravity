using System.Collections;
using System.Collections.Generic;
using Unity.VisualScripting;
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
    public RespawnManager respawnManager;

    public Vector2 respawnPoint = new(0, 2);

    /*[SerializeField] */
    private GameObject pauseButton;

    //private TotalMass totalMass;
    private TotalWeight totalWeight;

    private float moveSpeed;
    private float magnification;
    private int gravityDirection;
    private float oldMag = 1f;
    private bool isAvailable;

    private const float JUMPFORCE = 20.5f;
    [SerializeField] private float OBJ_MASS = 1f;

    [SerializeField] private Transform grabPoint;

    private bool isWalking = false;
    private bool isJumping = false;
    private bool isGrabbing = false;
    private bool isJumpActive = false;
    private bool Grabfront = false;
    private bool canMove;

    private Vector3 scale;

    private const float RAYDISTANCE = 0.2f;
    private GameObject grabObj;
    private RaycastHit2D hit;
    //private Vector3 grabPos;
    //private float grabMass;
    [SerializeField] private bool isPlayer = false;

    private MoveObjectWithRoute movingFloor;
    private Vector2 mFloorVelocity;

    [SerializeField] private BoxCollider2D pushBc;
    //[SerializeField] private BoxCollider2D footBc;

    [SerializeField] private string sceneName;

    [Header("ジャンプ")][SerializeField] private AudioClip jumpSE;
    [Header("電気柵")][SerializeField] private AudioClip spikeSE;
    [Header("リスポーン")][SerializeField] private AudioClip respawnSE;
    [Header("ゴール")][SerializeField] private AudioClip warpSE;

    void Awake()
    {
        gravityManager = GameObject.Find("GravityManager").GetComponent<GravityManager>();
        respawnManager = GameObject.Find("RespawnManager").GetComponent<RespawnManager>();
        pauseButton = GameObject.Find("/Canvas/PauseButton");
        rb = GetComponent<Rigidbody2D>();
        //totalMass = GetComponent<TotalMass>();
        totalWeight = GetComponent<TotalWeight>();
        
        (rb.gravityScale, moveSpeed, magnification) = gravityManager.GetDefaultValue();
        gravityDirection = (int)Mathf.Sign(magnification);
        isAvailable = gravityManager.GetIsAvailable();

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

            //ジャンプ
            if ((Input.GetKeyDown(KeyCode.Space) || Input.GetKeyDown(KeyCode.UpArrow) || Input.GetKeyDown(KeyCode.W)) && !isJumping && !isGrabbing)
            {
                GetComponent<AudioSource>().PlayOneShot(jumpSE, 0.3f);
                //Debug.Log(rb.velocity.y);
                Jump();
            }

            //if (isGrabbing)
            //{
            //    totalMass.SetMass(grabObj.GetComponent<TotalMass>().GetMass() - grabMass, true);
            //    //rb.mass = OBJ_MASS * Mathf.Abs(magnification) + grabMass;
            //    grabMass = grabObj.GetComponent<TotalMass>().GetMass();
            //}

            //プレイヤーの移動
            if (!isGrabbing || !isJumping)
            {
                //Debug.Log(totalMass.GetMass() + ", " + Mathf.Max(1.0f, moveSpeed / totalMass.GetMass()));
                float load = Mathf.Max(1.0f, moveSpeed / ((totalWeight.GetTWeight() == 0f) ? 1f : Mathf.Abs(totalWeight.GetTWeight())));
                rb.velocity = new Vector2(horizontal * Mathf.Max(1.0f, load /*totalMass.GetMass() /* Mathf.Abs(magnification)*/), rb.velocity.y);
                //footBc.enabled = false;
            }
            else
            {
                rb.velocity = new Vector2(0, rb.velocity.y);
                //Debug.Log("Stop walking");
                //footBc.enabled = true;
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


            Grabfront = horizontal * scale.x >= 0;

            animator.SetBool("isWalking", isWalking);
            animator.SetBool("isJumping", isJumping);
            animator.SetBool("isJumpActive", isJumpActive);
            animator.SetBool("Grabfront", Grabfront);
            pushBc.enabled = (!isJumping && !isGrabbing);
        }

        //canMove = falseのとき、速度0にし続ける
        if(!canMove)
        {
            rb.velocity = Vector2.zero;
        }

    }
    private void Jump()
    {
        isJumpActive = true;
        isJumping = true;
        rb.velocity = new(rb.velocity.x, 0f);
        rb.AddForce(JUMPFORCE * gravityDirection /* magnification*/ * Vector2.up, ForceMode2D.Impulse);
        //if (movingFloor != null)
        //{
        //    Debug.Log(mFloorVelocity);
        //}
    }
    private void Grab()
    {
        if (grabObj == null && !isJumping)
        {
            int layerMask = ~(1 << 2 | 1 << 6 | 1 << 8);
            hit = Physics2D.Raycast(grabPoint.position, Vector2.right * scale.x, RAYDISTANCE, layerMask);
            Debug.DrawRay(grabPoint.position, RAYDISTANCE * scale.x * Vector2.right, Color.green, 0.015f);
            //if (hit.collider != null)
            //{
            //    Debug.Log(hit.collider.name + ", " + hit.collider.transform.position.y + ", " + grabPoint.position.y);
            //}
            if (hit.collider != null && hit.collider.CompareTag("Movable") && (hit.collider.transform.position.y - grabPoint.position.y) * scale.y >= 0)
            {
                //Debug.Log("Grabbed");
                grabObj = (hit.collider.name.Contains("BOX")) ? hit.collider.gameObject : hit.collider.transform.parent.gameObject;
                //grabPos = grabObj.transform.position;

                pushBc.enabled = false;
                //grabObj.transform.position = new Vector2(grabPos.x + 0.1f * scale.x, grabPos.y);
                grabObj.GetComponent<Rigidbody2D>().isKinematic = true;
                transform.position += new Vector3(-0.1f * scale.x, 0f, 0f);
                grabObj.transform.position += new Vector3(0, 0.005f * scale.y, 0);
                grabObj.transform.SetParent(transform);
                //grabObj.transform.position = new Vector2(grabPos.x, grabPos.y + 0.08f * scale.y);
                //Debug.Log(grabObj.name + grabObj.transform.localScale);

                //grabCollider.offset = (grabObj.transform.position - transform.position) * (Vector2)scale;
                //grabCollider.size = grabObj.transform.localScale;
                grabPoint.position = grabObj.transform.position;
                grabPoint.localScale = new Vector2(grabObj.transform.localScale.x * grabObj.GetComponent<BoxCollider2D>().size.x, grabObj.transform.localScale.y * grabObj.GetComponent<BoxCollider2D>().size.y);
                grabCollider.enabled = true;
                grabObj.GetComponent<BoxCollider2D>().size = new Vector2(grabObj.GetComponent<BoxCollider2D>().size.x, 0.95f);
                grabObj.transform.GetChild(0).gameObject.SetActive(false);

                //grabMass = grabObj.GetComponent<TotalMass>().GetMass();
                //totalMass.SetMass(grabMass, true);
                //rb.mass = OBJ_MASS * magnification + grabMass;
                //grabPoint.position = grabObj.transform.position;
                //grabPoint.localScale = grabObj.transform.localScale;
                //Debug.Log(grabCollider.name + ", " + grabCollider.tag);

                isGrabbing = true;
            }
        }
        else if (grabObj != null)
        {
            grabObj.GetComponent<Rigidbody2D>().isKinematic = false;
            grabObj.transform.GetChild(0).gameObject.SetActive(true);
            grabObj.GetComponent<BoxCollider2D>().size = new Vector2(grabObj.GetComponent<BoxCollider2D>().size.x, 1f);
            
            grabCollider.enabled = false;
            grabPoint.position = transform.position + new Vector3(0.9f * scale.x, -0.5f * scale.y, 0);
            grabPoint.localScale = Vector3.one;
            //grabMass = grabObj.GetComponent<TotalMass>().GetMass();
            //totalMass.SetMass(-grabMass, true);
            //rb.mass = OBJ_MASS * Mathf.Abs(magnification);
            //grabPos = grabObj.transform.position;
            //grabObj.transform.position = new Vector2(grabPos.x - 0.1f * scale.x, grabPos.y);
            grabObj.transform.SetParent(null);
            grabObj.GetComponent<Rigidbody2D>().velocity = Vector2.zero;
            grabObj = null;
            transform.position += new Vector3(0.1f * scale.x, 0f, 0f);
            pushBc.enabled = false;

            isGrabbing = false;
            //footBc.enabled = false;
        }

        animator.SetBool("isGrabbing", isGrabbing);
    }
    public bool GetIsGrabbing()
    {
        return isGrabbing;
    }
    public void SetIsPlayer(bool value)
    {
        isPlayer = value;
    }
    public IEnumerator Respawn()
    {
        pauseButton.SetActive(false);
        Time.timeScale = 0;
        yield return new WaitForSecondsRealtime(1); // 1秒遅延
        //Debug.Log("Died");
        Time.timeScale = 1;
        pauseButton.SetActive(true);
        GameObject destroyGF = GameObject.FindWithTag("GravityField");
        if (destroyGF != null && isAvailable)
        {
            Destroy(destroyGF);
        }
        if (isGrabbing)
        {
            Grab();
        }
        rb.velocity = Vector2.zero;
        respawnManager.respawn = true;
        transform.position = respawnPoint;
        transform.localScale += new Vector3(0f, -transform.localScale.y + 1f, 0f);
        respawnManager.resAnimation = true;

        //if(respawnManager.respawn)
        //{
        //    Debug.Log("respawn");
        //}
        //if (respawnManager.resAnimation)
        //{
        //    Debug.Log("resAnimation");
        //}

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
            Debug.Log(collision.gameObject.name);
            GetComponent<AudioSource>().PlayOneShot(spikeSE, 0.4f);
            StartCoroutine(Respawn());
            //StartCoroutine(Test());
        }

        if (collision.gameObject.CompareTag("Abyss"))
        {
            isJumping = true;
            StartCoroutine(Respawn());
        }

        if (collision.gameObject.CompareTag("Platform") && transform.position.y > collision.gameObject.transform.position.y)
        {
            //Debug.Log(transform.position.y + ", " + collision.gameObject.transform.position.y);
            isJumping = false;
            isJumpActive = false;
            movingFloor = collision.gameObject.GetComponent<MoveObjectWithRoute>();
        }

        if (collision.gameObject.CompareTag("Movable") && isJumping)
        { 
        }
    }

    private void OnTriggerEnter2D(Collider2D collision)
    {
        
        if (collision.gameObject.CompareTag("Toxic"))
        {           
            //Vector2 hitPos = collision.ClosestPoint(grabPoint.position);
            ////Debug.Log(hitPos);
            //if (isGrabbing && (hitPos.x - transform.position.x) * scale.x > 0.65f)
            //{
            //    //Debug.Log("Safe");
            //    return;
            //}
            //Debug.Log(collision.gameObject.name);

            GetComponent<AudioSource>().PlayOneShot(spikeSE, 0.4f);
            StartCoroutine(Respawn());
            //StartCoroutine(Test());
        }
    }
    private void OnTriggerStay2D(Collider2D collision)
    {
        if (collision.CompareTag("GravityField") && (isPlayer /*!isGCollider*/ || gravityManager.GetMagnification() == -1f)) // 重力場中にあるとき、gravityManagerでの変更を読み込む
        {
            //Debug.Log("GField Stay");
            if (!isAvailable)
            {
                gravityManager.SetGScale(collision.GetComponent<GravityFieldTexture>().GetGPattern());
                gravityManager.ChangeGravity();
            }
            (rb.gravityScale, moveSpeed, magnification) = gravityManager.GetValue();
            gravityDirection = (int)Mathf.Sign(magnification);
            if (magnification != oldMag)
            {
                //totalMass.SetMass(-rb.mass, true);
                //rb.mass = OBJ_MASS * Mathf.Abs(magnification);
                //totalMass.SetMass(rb.mass, true);
                //Debug.Log(magnification + ": " + totalMass.GetMass());
                oldMag = magnification;
            }
            scale = gameObject.transform.localScale;
            scale.y = gravityDirection;
            gameObject.transform.localScale = scale;
        }
        else if (!collision.CompareTag("Platform") && !collision.CompareTag("Tutorial"))
        {
            isJumping = false;
        }

        //if (collision.CompareTag("Platform"))
        //{
        //    if (transform.position.y > collision.gameObject.transform.position.y)
        //    {
        //        isJumping = false;
        //        movingFloor = collision.gameObject.GetComponent<MoveObjectWithRoute>();
        //    }
        //    //Debug.Log(transform.position.y + ", " + collision.gameObject.transform.position.y + ": higher");
        //}

        //if (collision.CompareTag("Platform") && transform.position.y > collision.gameObject.transform.position.y + 2f)
        //{
        //    isJumping = false;
        //    movingFloor = collision.gameObject.GetComponent<MoveObjectWithRoute>();
        //    //Debug.Log(transform.position.y + ", " + collision.gameObject.transform.position.y + ": higher");
        //}
    }
    private void OnTriggerExit2D(Collider2D collision)
    {
        if (collision.CompareTag("GravityField"))// && !isPlayer /*!isGCollider*/) // 重力場から出たとき、デフォルトに戻す
        {
            //Debug.Log("GField Exit : " + collision.name);
            (rb.gravityScale, moveSpeed, magnification) = gravityManager.GetDefaultValue();
            gravityDirection = 1;
            //rb.mass = OBJ_MASS;
            oldMag = 1f;
        }
        //else if (collision.CompareTag("GravityField") && !isPlayer)
        //{

        //}
        else if (!collision.CompareTag("Tutorial") && Mathf.Abs(rb.velocity.y) > 3f)
        {
            isJumping = true;
            //Debug.Log("In the air : T");
        }

        if (collision.CompareTag("Platform"))
        {
            movingFloor = null;
        }       
    }

    //private void OnCollisionExit2D(Collision2D collision)
    //{
    //    if (collision.gameObject.CompareTag("GravityField") && !isPlayer /*!isGCollider*/) // 重力場から出たとき、デフォルトに戻す
    //    {
    //        (rb.gravityScale, moveSpeed, magnification) = gravityManager.GetDefaultValue();
    //        gravityDirection = 1;
    //        //rb.mass = OBJ_MASS;
    //        oldMag = 1f;
    //    }
    //    else
    //    {
    //        isJumping = true;
    //        Debug.Log("In the air : C");
    //    }

    //    if (collision.gameObject.CompareTag("Platform"))
    //    {
    //        movingFloor = null;
    //    }
    //}
}
