using System.Collections;
using System.Collections.Generic;
using UnityEngine;

[RequireComponent(typeof(Rigidbody2D))]

public class GravityObserver : MonoBehaviour
{
    [SerializeField] private Rigidbody2D rb;

    public GravityManager gravityManager;

    [SerializeField] private float OBJ_WEIGHT;

    // private bool isReverse = false;
    // private Vector3 scale;

    void Awake()
    {
        rb.gravityScale = gravityManager.G_SCALE;
        rb.mass = OBJ_WEIGHT;
        // isReverse = gravityManager.isReverse;
    }

    /*void Update()
    {
        scale = gameObject.transform.localScale;
        if (!isReverse && scale.y < 0)
        {
            scale.y *= -1;
            gameObject.transform.localScale = scale;
        }
    }*/

    private void OnTriggerStay2D(Collider2D collision)
    {
        if (collision.CompareTag("GravityField")) // 重力場中にあるとき、gravityManagerでの変更を読み込む
        {
            rb.gravityScale = gravityManager.gravityScale; // * OBJ_WEIGHT;
            rb.mass = OBJ_WEIGHT * Mathf.Min(0.5f, Mathf.Abs(rb.gravityScale / gravityManager.G_SCALE));
            /*isReverse = gravityManager.isReverse;
            scale = gameObject.transform.localScale;
            if (isReverse && scale.y == 1)
            {
                scale.y = -1;
                gameObject.transform.localScale = scale;
            }*/
        }
    }

    private void OnTriggerExit2D(Collider2D collision)
    {
        if (collision.CompareTag("GravityField")) // 重力場から出たとき、デフォルトに戻す
        {
            rb.gravityScale = gravityManager.G_SCALE;
            rb.mass = OBJ_WEIGHT;
            // isReverse = false;
        }

    }
}
