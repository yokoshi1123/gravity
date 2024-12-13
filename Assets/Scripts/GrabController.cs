using System.Collections;
using System.Collections.Generic;
using UnityEngine;

public class GrabController : MonoBehaviour
{
    /*[SerializeField]*/ private PlayerController playerController;
    /*[SerializeField]*/ private Vector3 footPos;

    private Vector3 OFFSET = new(0.9f, -0.6f, 0f);

    private BoxCollider2D grabCollider;

    private bool isJumping = false;
    private bool canMove;

    private Vector3 scale;

    private const float RAYDISTANCE = 0.2f;
    private GameObject grabObj;
    private RaycastHit2D hit;
    private Transform formerParent;

    private Rigidbody2D rb;
    private GravityManager gravityManager;

    private bool isAvailable = false;

    private float moveSpeed;
    private int gravityDirection = 1;

    [SerializeField] private float OBJ_MASS = 1;

    // Start is called before the first frame update
    void Awake()
    {
        playerController = transform.parent.GetComponent<PlayerController>();
        grabCollider = GetComponent<BoxCollider2D>();

        rb = transform.parent.GetComponent<Rigidbody2D>();
        gravityManager = GameObject.Find("GravityManager").GetComponent<GravityManager>();
        (rb.gravityScale, moveSpeed, gravityDirection) = gravityManager.GetDefaultValue();
        rb.mass = OBJ_MASS;
        playerController.SetGState(moveSpeed, gravityDirection);
    }

    // Update is called once per frame
    void Update()
    {
        scale = transform.parent.transform.localScale;
        footPos = transform.parent.GetChild(3).position;
        (isJumping, canMove) = playerController.GetBools_Grab();
        //canMove = playerController.GetCanMove();
        if (canMove && Input.GetKeyDown(KeyCode.G))
        {
            Grab();
        }
    }

    public void Grab()
    {
        if (grabObj == null && !isJumping)
        {
            int layerMask = ~(1 << 2 | 1 << 6 | 1 << 8);
            hit = Physics2D.Raycast(transform.position, Vector2.right * scale.x, RAYDISTANCE, layerMask);
            //Debug.DrawRay(grabPoint.position, RAYDISTANCE * scale.x * Vector2.right, Color.green, 0.015f);
            if (hit.collider != null && hit.collider.CompareTag("Movable") && Mathf.Abs((hit.collider.transform.position.y - footPos.y) * scale.y -2.0f) <= 0.07f)
            {
                //Debug.Log("Grabbed");
                //Debug.Log(hit.collider.gameObject.name);
                grabObj = (hit.collider.name.Contains("BOX")) ? hit.collider.gameObject : hit.collider.transform.parent.gameObject;
                grabObj.GetComponent<Rigidbody2D>().isKinematic = true;
                grabObj.GetComponent<BoxCollider2D>().enabled = false;
                formerParent = grabObj.transform.parent;
                grabObj.transform.SetParent(transform.parent);
                grabObj.transform.position += new Vector3(0, 0.005f * scale.y, 0);
                //Debug.Log(transform.position + ", " + grabObj.transform.position);
                transform.position = grabObj.transform.position;
                //Debug.Log(grabObj.transform.localScale.x * grabObj.GetComponent<BoxCollider2D>().size.x);
                //grabCollider.size = new Vector2(Mathf.Abs(grabObj.transform.localScale.x) * grabObj.GetComponent<BoxCollider2D>().size.x, Mathf.Abs(grabObj.transform.localScale.y) * grabObj.GetComponent<BoxCollider2D>().size.y);
                grabCollider.size = new Vector2(Mathf.Abs(grabObj.transform.GetChild(0).lossyScale.x), Mathf.Abs(grabObj.transform.GetChild(0).lossyScale.y));
                grabCollider.enabled = true;
                grabObj.GetComponent<BoxCollider2D>().size = new Vector2(grabObj.GetComponent<BoxCollider2D>().size.x, 0.95f);
                grabObj.transform.GetChild(0).gameObject.SetActive(false);
                grabObj.GetComponent<BoxCollider2D>().enabled = true;
                playerController.SetIsGrabbing(true);
            }
        }
        else if (grabObj != null && !isJumping)
        {
            //Debug.Log("Release");
            grabObj.GetComponent<Rigidbody2D>().isKinematic = false;
            grabObj.transform.GetChild(0).gameObject.SetActive(true);
            grabObj.GetComponent<BoxCollider2D>().size = new Vector2(grabObj.GetComponent<BoxCollider2D>().size.x, 1f);

            grabCollider.enabled = false;
            grabCollider.size = Vector3.one;
            grabObj.transform.SetParent(formerParent);
            grabObj.GetComponent<Rigidbody2D>().velocity = Vector2.zero;
            grabObj = null;
            transform.position = transform.parent.transform.position + Vector3.Scale(OFFSET, scale);
            //Debug.Log(Vector3.Scale(OFFSET, scale));
            playerController.SetIsGrabbing(false);
        }
    }

    private void OnTriggerStay2D(Collider2D collision)
    {
        if (collision.CompareTag("GravityField") && grabCollider.enabled) // 重力場中にあるとき、gravityManagerでの変更を読み込む
        {
            //Debug.Log("GField Stay");
            if (!isAvailable)
            {
                gravityManager.SetGScale(collision.GetComponent<GravityFieldTexture>().GetGPattern());
                gravityManager.ChangeGravity();
            }
            (rb.gravityScale, moveSpeed, gravityDirection) = gravityManager.GetValue();
            playerController.SetGState(moveSpeed, gravityDirection);
        }
    }
    private void OnTriggerExit2D(Collider2D collision)
    {
        if (collision.CompareTag("GravityField") && grabCollider.enabled) // 重力場から出たとき、デフォルトに戻す
        {
            //Debug.Log("GField Exit : " + collision.name);
            (rb.gravityScale, moveSpeed, gravityDirection) = gravityManager.GetDefaultValue();
            playerController.SetGState(moveSpeed, gravityDirection);
        }
    }
}
