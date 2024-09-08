using System;
using System.Collections;
using System.Collections.Generic;
using UnityEditorInternal;
using UnityEngine;

public class MoveObjectWithRoute : MonoBehaviour
{
    [Header ("移動経路")] public GameObject[] movePoint;
    [Header("速さ")] public float speed = 1.0f;

    private Rigidbody2D rb;
    private int nowPoint = 0;
    private int nextPoint = 0;
    private bool returnPoint = false;

    private Vector2 toVector;
    private Vector2 mFloorVelocity = Vector2.zero;
    private Vector2 oldPosition = Vector2.zero;

    private int delay = 0;
    private const int DELAY = 8;

    // Start is called before the first frame update
    void Start()
    {
        rb = GetComponent<Rigidbody2D>();

        if (movePoint != null && movePoint.Length > 0 && rb != null)
        {
            transform.position = movePoint[0].transform.position;
        }
    }

    // Update is called once per frame
    void FixedUpdate()
    {
        if (movePoint != null && movePoint.Length > 1 && rb != null)
        {
            mFloorVelocity = (rb.position - oldPosition) / Time.deltaTime;
            if (mFloorVelocity.y >= 0)
            {
                mFloorVelocity.y = -speed;
            }
            //Debug.Log(mFloorVelocity);
            oldPosition = rb.position;

            // 通常進行
            if (!returnPoint && delay >= DELAY)
            {
                nextPoint = nowPoint + 1;

                // 目標ポイントとの誤差がわずかになるまで移動
                if (Vector2.Distance(transform.position, movePoint[nextPoint].transform.position) > 0.1f)
                {
                    toVector = Vector2.MoveTowards(transform.position, movePoint[nextPoint].transform.position, speed * Time.deltaTime);
                    rb.MovePosition(toVector);
                }

                else
                {
                    rb.MovePosition(movePoint[nextPoint].transform.position);
                    nowPoint++;
                    delay = 0;

                    if (nowPoint + 1 >= movePoint.Length)
                    {
                        returnPoint = true;
                    }
                }
            }
            else if (delay >= DELAY)
            {
                // 折り返し進行
                nextPoint = nowPoint - 1;

                // 目標ポイントとの誤差がわずかになるまで移動
                if (Vector2.Distance(transform.position, movePoint[nextPoint].transform.position) > 0.1f)
                {
                    toVector = Vector2.MoveTowards(transform.position, movePoint[nextPoint].transform.position, speed * Time.deltaTime);
                    rb.MovePosition(toVector);
                }

                else
                {
                    rb.MovePosition(movePoint[nextPoint].transform.position);
                    nowPoint--;
                    delay = 0;

                    if (nowPoint <= 0)
                    {
                        returnPoint = false;
                    }
                }
            }
            else
            {
                delay++;
            }

            //Debug.Log("toVector : " + toVector);
        }
    }

    public Vector2 GetMFloorVelocity()
    {
        return mFloorVelocity;
    }
}
