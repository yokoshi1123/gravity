using System.Collections;
using System.Collections.Generic;
using Unity.VisualScripting;
using UnityEngine;
using UnityEngine.UIElements;

public class GravityObserverPL : MonoBehaviour
{
    private Rigidbody2D rb;
    private GravityManager gravityManager;
    private PlayerController playerController;

    private bool isAvailable = false;

    private float moveSpeed;
    private int gravityDirection = 1;


    [SerializeField] private float OBJ_MASS = 1;

    // Start is called before the first frame update
    void Awake()
    {
        rb = transform.parent.GetComponent<Rigidbody2D>();
        gravityManager = GameObject.Find("GravityManager").GetComponent<GravityManager>();
        (rb.gravityScale, moveSpeed, gravityDirection) = gravityManager.GetDefaultValue();
        rb.mass = OBJ_MASS;
        playerController = transform.parent.GetComponent<PlayerController>();
        playerController.SetGState(moveSpeed, gravityDirection);
    }

    // Update is called once per frame
    void Update()
    {
        isAvailable = gravityManager.GetIsAvailable();
        playerController.SetIsAvailable(isAvailable);
    }

    private void OnTriggerStay2D(Collider2D collision)
    {
        if (collision.CompareTag("GravityField")) // 重力場中にあるとき、gravityManagerでの変更を読み込む
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
        if (collision.CompareTag("GravityField")) // 重力場から出たとき、デフォルトに戻す
        {
            //Debug.Log("GField Exit : " + collision.name);
            (rb.gravityScale, moveSpeed, gravityDirection) = gravityManager.GetDefaultValue();
            playerController.SetGState(moveSpeed, gravityDirection);
        }
    }
}
