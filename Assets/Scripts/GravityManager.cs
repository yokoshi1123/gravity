using System.Collections;
using System.Collections.Generic;
using UnityEngine;
using TMPro;

[RequireComponent(typeof(AudioSource))]

public class GravityManager : MonoBehaviour
{
    [SerializeField] private bool isAvailable = false;

    private const float G_SCALE = 1.0f;
    private const float M_SPEED = 10.0f;

    [SerializeField] private float gravityScale;
    [SerializeField] private float moveSpeed;
    
    //public bool isReverse = false;
    private int gravityDirection = 1;

    private Vector2 startMPosition = Vector2.zero;
    private Vector2 endMPosition = Vector2.zero;
    private const float CAMERAZPOSITION = -20f;
    private GameObject destroyGF;

    private int gScale = 1;
    //private float magnification = 0.5f;

    private bool isChangeable = true;
    private int mouseWheel = 1;

    [SerializeField] private GameObject gFieldUI;
    [SerializeField] private TextMeshProUGUI gravityValueText;

    private IEnumerator routine;

    void Awake()
    {
        moveSpeed = M_SPEED;
        gravityScale = G_SCALE;
        //gravityValueText.text = "重力場：--";
        gFieldUI = GameObject.Find("/Canvas/GFieldUI");
        gravityValueText = GameObject.Find("/Canvas/GFieldUI/GravityText/GravityValue").GetComponent<TextMeshProUGUI>();
        gFieldUI.SetActive(isAvailable);

        ChangeGravity();
    }
    void Update()
    {
        gFieldUI.SetActive(isAvailable);
        if (isAvailable)
        {           
            GameObject gravityField = (GameObject)Resources.Load("GravityField"); //"Square");//

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
                DestroyGF();
                // GravityFieldのクローンを作成
                GameObject gField = (GameObject)Instantiate(gravityField, (startMPosition2 + endMPosition2) / 2, Quaternion.identity);
                gField.transform.position = gField.transform.position + new Vector3(0, 0, -gField.transform.position.z - 2f);
                // ドラッグしたサイズに拡大
                //gField.transform.localScale = new Vector2(Mathf.Abs(endMPosition2.x - startMPosition2.x), Mathf.Abs(endMPosition2.y - startMPosition2.y));
                gField.GetComponent<SpriteRenderer>().size = new Vector2(Mathf.Abs(endMPosition2.x - startMPosition2.x), Mathf.Abs(endMPosition2.y - startMPosition2.y));
                gField.GetComponent<BoxCollider2D>().size = new Vector2(Mathf.Abs(endMPosition2.x - startMPosition2.x), Mathf.Abs(endMPosition2.y - startMPosition2.y));

                // 効果音
                GetComponent<AudioSource>().Play();

                routine = WaitAndDestroy();
                StartCoroutine(routine);
            }

            if (Input.GetMouseButtonUp(0))
            {
                try
                {
                    GameObject.FindWithTag("GravityField").GetComponent<BoxCollider2D>().enabled = true;
                }
                catch { }
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
    }
    public void ChangeGravity()
    {
        // Debug.Log("gscale: " + gScale);

        //isReverse = (gScale == 0) ? true : false;
        //magnification = 1.5f * gScale - 1.0f;
        //gravityDirection = (int)Mathf.Sign(magnification);
        switch (gScale)
        {
            case 0: // x(-1.0)
                gravityScale = G_SCALE * (-1.0f);
                moveSpeed = M_SPEED;
                gravityDirection = -1;
                if (isAvailable/* || isAvailable*/) { gravityValueText.text = "-１.０G / Reversal"; } // string.Format("{ 0,6}","-1.0G") + " / 反転";//"重力場：-1.0G / 反転";
                
                break;
            case 1: // x0.5
                gravityScale = G_SCALE * 0.5f;
                moveSpeed = M_SPEED * 0.75f;
                gravityDirection = 1;
                if (isAvailable/* || isAvailable*/) { gravityValueText.text = "０.５G / 　  Light"; }
                // string.Format("{0,6}","0.5G") + " / 　軽"; //"重力場：0.5G / 軽";
                break;
            case 2: // x2.0
                gravityScale = G_SCALE * 2.0f;
                moveSpeed = M_SPEED * 1.5f;
                gravityDirection = 1;
                if (isAvailable/* || isAvailable*/) { gravityValueText.text = "２.０G /     Heavy"; } // string.Format("{ 0,6}", "2.0G") + " / 　重";//"重力場：2.0G / 重";
                break;
            default:
                Debug.Log("ChangeGravityでエラー");
                break;
        }
    }
    public void DestroyGF()
    {
        destroyGF = GameObject.FindWithTag("GravityField");
        if (destroyGF != null)
        {
            StopCoroutine(routine);
            routine = null;
            Destroy(destroyGF);
        }
    }

    public float GetGravityScale()
    {
        return gravityScale;
    }

    public float GetDeFaultGravityScale()
    {
        return G_SCALE;
    }

    //public (float, float, float) GetValue()
    //{
    //    return (gravityScale, moveSpeed, magnification);
    //}
    //public (float, float, float) GetDefaultValue()
    //{
    //    return (G_SCALE, M_SPEED, 1f);
    //}
    public (float, float, int) GetValue()
    {
        return (gravityScale, moveSpeed, gravityDirection);
    }

    public (float, float, int) GetDefaultValue()
    {
        return (G_SCALE, M_SPEED, 1);
    }

    public int GetGScale()
    {
        return gScale;
    }

    public void SetGScale(int value)
    {
        gScale = value;
    }

    //public float GetMagnification()
    //{
    //    return magnification;
    //}
    public bool GetIsAvailable()
    {
        return isAvailable;
    }

    public void SetIsAvailable(bool value)
    {
        isAvailable = value;
    }

    private IEnumerator WaitAndDestroy()
    {
        yield return new WaitForSecondsRealtime(4f); // 5秒遅延          
        for (int i = 0; i < 5; i++)
        {
            destroyGF = GameObject.FindWithTag("GravityField");
            if (destroyGF != null)
            {
                SpriteRenderer myRenderer = destroyGF.GetComponent<SpriteRenderer>();
                myRenderer.color += new Color(0, 0, 0, -myRenderer.color.a + 0.25f);
                yield return new WaitForSeconds(0.1f);
            }
            destroyGF = GameObject.FindWithTag("GravityField");
            if (destroyGF != null)
            {
                SpriteRenderer myRenderer = destroyGF.GetComponent<SpriteRenderer>();
                myRenderer.color += new Color(0, 0, 0, -myRenderer.color.a + 0.5f);
                yield return new WaitForSeconds(0.1f);
            }
        }
        DestroyGF();
    }
    private IEnumerator MouseWheelWait() // マウスホイールからの入力を0.2秒無視する
    {
        isChangeable = false;
        yield return new WaitForSecondsRealtime(0.17f);
        isChangeable = true;
    }
}
