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

    //private MoveObjectWithRoute movingFloor;
    //private Vector2 mFloorVelocity;


    // Start is called before the first frame update
    void Awake()
    {
        playerController = transform.parent.GetComponent<PlayerController>();
        grabCollider = GetComponent<BoxCollider2D>();
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
                formerParent = grabObj.transform.parent;
                grabObj.transform.SetParent(transform.parent);
                transform.position = grabObj.transform.position;
                Debug.Log(grabObj.transform.localScale.x * grabObj.GetComponent<BoxCollider2D>().size.x);
                grabCollider.size = new Vector2(Mathf.Abs(grabObj.transform.localScale.x) * grabObj.GetComponent<BoxCollider2D>().size.x, Mathf.Abs(grabObj.transform.localScale.y) * grabObj.GetComponent<BoxCollider2D>().size.y);
                grabCollider.enabled = true;
                grabObj.transform.position += new Vector3(0, 0.005f * scale.y, 0);
                grabObj.GetComponent<BoxCollider2D>().size = new Vector2(grabObj.GetComponent<BoxCollider2D>().size.x, 0.95f);
                grabObj.transform.GetChild(0).gameObject.SetActive(false);
                playerController.SetIsGrabbing(true);
            }
        }
        else if (grabObj != null)
        {
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
}