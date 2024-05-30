using System.Collections;
using System.Collections.Generic;
using UnityEngine;
using TMPro;

[RequireComponent(typeof(AudioSource))]

public class GravityManager : MonoBehaviour
{
    public float M_SPEED = 10.0f;
    public float G_SCALE = 5.0f;
    public float moveSpeed;
    public float gravityScale;
    public bool isReverse = false;

    private Vector2 startMPosition = Vector2.zero;
    private Vector2 endMPosition = Vector2.zero;
    private float CAMERAZPOSITION = -20f;
    private GameObject destroyGF;

    private int gScale = 2;

    public TextMeshProUGUI gravityText;

    void Awake()
    {
        moveSpeed = M_SPEED;
        gravityScale = G_SCALE;
    }


    void Update()
    {
        GameObject gravityField = (GameObject)Resources.Load("GravityField");

        if (Input.GetMouseButtonDown(0)) // マウスの左ボタンを押した時の座標を取得
        { 
            startMPosition = Input.mousePosition;
            // スクリーン座標からワールド座標に変換
            startMPosition = Camera.main.ScreenToWorldPoint(new Vector3(startMPosition.x, startMPosition.y, CAMERAZPOSITION)); 
            // Debug.Log("Start:(" + startMPosition.x + ", " + startMPosition.y + ")");

        }

        if (Input.GetMouseButton(0)) // マウスの左ボタンを離した時の座標を取得
        {
            endMPosition = Input.mousePosition;
            // スクリーン座標からワールド座標に変換
            endMPosition = Camera.main.ScreenToWorldPoint(new Vector3(endMPosition.x, endMPosition.y, CAMERAZPOSITION));
            // Debug.Log("End:(" + endMPosition.x + ", " + endMPosition.y + ")");

            Vector2 startMPosition2 = startMPosition;
            Vector2 endMPosition2 = endMPosition;
            if (startMPosition.x > endMPosition.x) // startMPosition.x > endMPosition.x なら値を入れ替え
            {
                // Debug.Log(startMPosition + ", " + endMPosition);
                (startMPosition2, endMPosition2) = (endMPosition, startMPosition);
                // Debug.Log(startMPosition + ", " + endMPosition);
            }
            if (startMPosition.y > endMPosition.y) // startMPosition.y > endMPosition.y なら値を入れ替え
            {
                // Debug.Log(startMPosition + ", " + endMPosition);
                (startMPosition2.y, endMPosition2.y) = (endMPosition.y, startMPosition.y);
                // Debug.Log(startMPosition + ", " + endMPosition);
            }

            // 既にGravityFieldのクローンがあれば削除
            destroyGF = GameObject.FindWithTag("GravityField");
            if (destroyGF != null)
            {
                Destroy(destroyGF);
            }
            // GravityFieldのクローンを作成
            GameObject gField = (GameObject)Instantiate(gravityField, (startMPosition2 + endMPosition2) / 2, Quaternion.identity);
            // 横幅をドラッグした幅に変更
            gField.transform.localScale = new Vector2(Mathf.Abs(endMPosition2.x - startMPosition2.x), Mathf.Abs(endMPosition2.y - startMPosition2.y));

            // 効果音
            GetComponent<AudioSource>().Play();
        }

        //if (Input.GetMouseButtonUp(0)) // マウスの左ボタンを離した時の座標を取得
        //{
        //    endMPosition = Input.mousePosition;
        //    // スクリーン座標からワールド座標に変換
        //    endMPosition = Camera.main.ScreenToWorldPoint(new Vector3(endMPosition.x, endMPosition.y, CAMERAZPOSITION));
        //    // DebugLog("End:(" + endMPosition.x + ", " + endMPosition.y + ")");

        //    if (startMPosition.x > endMPosition.x) // startMPosition.x > endMPosition.x なら値を入れ替え
        //    {
        //        // Debug.Log(startMPosition + ", " + endMPosition);
        //        (startMPosition, endMPosition) = (endMPosition, startMPosition);
        //        // Debug.Log(startMPosition + ", " + endMPosition);
        //    }
        //    if (startMPosition.y > endMPosition.y) // startMPosition.y > endMPosition.y なら値を入れ替え
        //    {
        //        // Debug.Log(startMPosition + ", " + endMPosition);
        //        (startMPosition.y, endMPosition.y) = (endMPosition.y, startMPosition.y);
        //        // Debug.Log(startMPosition + ", " + endMPosition);
        //    }

        //    // 既にGravityFieldのクローンがあれば削除
        //    destroyGF = GameObject.FindWithTag("GravityField");
        //    if (destroyGF != null) 
        //    {
        //        Destroy(destroyGF);
        //    }
        //    // GravityFieldのクローンを作成
        //    GameObject gField = (GameObject)Instantiate(gravityField, (startMPosition + endMPosition) / 2, Quaternion.identity);
        //    // 横幅をドラッグした幅に変更
        //    gField.transform.localScale = new Vector2(Mathf.Abs(endMPosition.x - startMPosition.x), Mathf.Abs(endMPosition.y - startMPosition.y));

        //}

        // 上下キーでgravityScaleの変更
        if (Input.GetKeyDown(KeyCode.UpArrow))
        {
            gScale = (gScale + 399999) % 4;
            ChangeGravity();
        }

        if (Input.GetKeyDown(KeyCode.DownArrow))
        {
            gScale = (gScale + 400001) % 4;
            ChangeGravity();
        }
    }

    void ChangeGravity()
    {
        // Debug.Log("gscale: " + gScale);
        // jumpDirection = (gScale == 0) ? -1 : 1;

        isReverse = (gScale == 0) ? true : false;
        switch (gScale)
        {
            case 0: // x(-1.0)
                gravityScale = G_SCALE * (-1.0f);
                moveSpeed = M_SPEED;
                gravityText.text = "GRAVITY : -1.0";
                break;
            case 1: // x0.5
                gravityScale = G_SCALE * 0.5f;
                moveSpeed = M_SPEED * 1.3f;
                gravityText.text = "GRAVITY : 0.5";
                break;
            case 2: // x1.0
                gravityScale = G_SCALE;
                moveSpeed = M_SPEED;
                gravityText.text = "GRAVITY : 1.0";
                break;
            case 3: // x2.0
                gravityScale = G_SCALE * 2.0f;
                moveSpeed = M_SPEED * 0.7f;
                gravityText.text = "GRAVITY : 2.0";
                break;
            default:
                Debug.Log("ChangeGravityでエラー");
                break;
        }


    }
}
