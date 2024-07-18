using System.Collections;
using System.Collections.Generic;
using UnityEngine;
using TMPro;
using UnityEditor.UIElements;

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

    private int gScale = 1;

    private bool isChangeable = true;
    private int mouseWheel = 1;

    public TextMeshProUGUI gravityText;

    private IEnumerator routine;

    void Awake()
    {
        moveSpeed = M_SPEED;
        gravityScale = G_SCALE;
        //gravityText.text = "重力場：--";
        ChangeGravity();
    }


    void Update()
    {
        GameObject gravityField = (GameObject)Resources.Load("Square");//"GravityField");

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

            // 既にGravityFieldのクローンがあれば削除
            destroyGF = GameObject.FindWithTag("GravityField");
            if (destroyGF != null)
            {
                Destroy(destroyGF);
                StopCoroutine(routine);
                routine = null;
            }
            // GravityFieldのクローンを作成
            GameObject gField = (GameObject)Instantiate(gravityField, (startMPosition2 + endMPosition2) / 2, Quaternion.identity);
            gField.transform.position = gField.transform.position + new Vector3(0, 0, -gField.transform.position.z - 0.1f);
            // ドラッグしたサイズに拡大
            //gField.transform.localScale = new Vector2(Mathf.Abs(endMPosition2.x - startMPosition2.x), Mathf.Abs(endMPosition2.y - startMPosition2.y));
            gField.GetComponent<SpriteRenderer>().size = new Vector2(Mathf.Abs(endMPosition2.x - startMPosition2.x), Mathf.Abs(endMPosition2.y - startMPosition2.y));
            gField.GetComponent<BoxCollider2D>().size = new Vector2(Mathf.Abs(endMPosition2.x - startMPosition2.x), Mathf.Abs(endMPosition2.y - startMPosition2.y));

            // 効果音
            GetComponent<AudioSource>().Play();

            routine = WaitAndDestroy();
            StartCoroutine(routine);
        }

        // 下キー/SキーでgravityScaleの変更
        //if (Input.GetKeyDown(KeyCode.DownArrow) || Input.GetKeyDown(KeyCode.S))
        //{
        //    gScale = (gScale + 300001) % 3; // (gScale + 400001) % 4;
        //    ChangeGravity();
        //}

        if (Input.GetAxis("Mouse ScrollWheel") != 0 && isChangeable)
        {
            StartCoroutine(MouseWheelWait());
            //Debug.Log(Input.GetAxis("Mouse ScrollWheel"));
            mouseWheel -= (int)Input.GetAxis("Mouse ScrollWheel");
            gScale = ((int)mouseWheel + 30000) % 3;      
            ChangeGravity();
        }
    }

    void ChangeGravity()
    {
        // Debug.Log("gscale: " + gScale);

        isReverse = (gScale == 0) ? true : false;
        switch (gScale)
        {
            case 0: // x(-1.0)
                gravityScale = G_SCALE * (-1.0f);
                moveSpeed = M_SPEED;
                gravityText.text = "-1.0G / 反転"; // string.Format("{ 0,6}","-1.0G") + " / 反転";//"重力場：-1.0G / 反転";
                break;
            case 1: // x0.5
                gravityScale = G_SCALE * 0.5f;
                moveSpeed = M_SPEED * 1.3f;
                gravityText.text = "0.5G / 　軽"; // string.Format("{0,6}","0.5G") + " / 　軽"; //"重力場：0.5G / 軽";
                break;
            case 2: // x2.0
                gravityScale = G_SCALE * 2.0f;
                moveSpeed = M_SPEED * 0.7f;
                gravityText.text = "2.0G / 　重"; // string.Format("{ 0,6}", "2.0G") + " / 　重";//"重力場：2.0G / 重";
                break;
            default:
                Debug.Log("ChangeGravityでエラー");
                break;
        }
    }

    private IEnumerator WaitAndDestroy()
    {
        yield return new WaitForSecondsRealtime(5); // 5秒遅延
        destroyGF = GameObject.FindWithTag("GravityField");
        if (destroyGF != null)
        {
            Destroy(destroyGF);
        }
    }

    private IEnumerator MouseWheelWait() // マウスホイールからの入力を0.2秒無視する
    {
        isChangeable = false;
        yield return new WaitForSecondsRealtime(0.2f);
        isChangeable = true;
    }
}
