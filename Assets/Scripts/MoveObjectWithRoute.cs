using System;
using System.Collections;
using System.Collections.Generic;
using UnityEngine;

public class MoveObjectWithRoute : MonoBehaviour
{
    /*[Header ("移動経路")]*/ [SerializeField] private GameObject[] movePoint;
    /*[Header("速さ")]*/ [SerializeField] private float speed = 1.0f;
    [SerializeField] private bool loop = false;

    private Rigidbody2D rb;
    private TurnOn to;
    [SerializeField] private int nowPoint = 0;
    [SerializeField] private int nextPoint = 0;
    private bool returnPoint = false;

    private Vector2 toVector;
    private Vector2 mFloorVelocity = Vector2.zero;
    private Vector2 oldPosition = Vector2.zero;

    [SerializeField] private int delay = 0;
    private const int DELAY = 6;

    // Start is called before the first frame update
    void Start()
    {
        rb = GetComponent<Rigidbody2D>();
        to = GetComponent<TurnOn>();

        if (movePoint != null && movePoint.Length > 0 && rb != null)
        {
            transform.position = movePoint[0].transform.position;
        }

        delay *= -60;
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

            if (to.GetTurnOn())
            {
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
                    if (loop)
                    {
                        nowPoint = 0;
                        transform.position = movePoint[0].transform.position;
                        returnPoint = false;
                    }
                    else // 折り返し進行
                    {
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
                }
                else
                {
                    delay++;
                }
            }

            //Debug.Log("toVector : " + toVector);
        }
    }

    public Vector2 GetMFloorVelocity()
    {
        return mFloorVelocity;
    }

    public void Respawn()
    {
        if (movePoint != null && movePoint.Length > 0 && rb != null)
        {
            transform.position = movePoint[0].transform.position;
            nowPoint = 0;
            nextPoint = 1;
            returnPoint = false;
        }
    }
}
