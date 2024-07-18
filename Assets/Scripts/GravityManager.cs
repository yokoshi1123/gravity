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
        //gravityText.text = "�d�͏�F--";
        ChangeGravity();
    }


    void Update()
    {
        GameObject gravityField = (GameObject)Resources.Load("Square");//"GravityField");

        if (Input.GetMouseButtonDown(0)) // �}�E�X�̍��{�^�������������̍��W���擾
        { 
            startMPosition = Input.mousePosition;
            // �X�N���[�����W���烏�[���h���W�ɕϊ�
            startMPosition = Camera.main.ScreenToWorldPoint(new Vector3(startMPosition.x, startMPosition.y, CAMERAZPOSITION)); 
            // Debug.Log("Start:(" + startMPosition.x + ", " + startMPosition.y + ")");

        }

        if (Input.GetMouseButton(0)) // �}�E�X�̍��{�^���𗣂������̍��W���擾
        {
            endMPosition = Input.mousePosition;
            // �X�N���[�����W���烏�[���h���W�ɕϊ�
            endMPosition = Camera.main.ScreenToWorldPoint(new Vector3(endMPosition.x, endMPosition.y, CAMERAZPOSITION));
            // Debug.Log("End:(" + endMPosition.x + ", " + endMPosition.y + ")");

            Vector2 startMPosition2 = startMPosition;
            Vector2 endMPosition2 = endMPosition;

            // ����GravityField�̃N���[��������΍폜
            destroyGF = GameObject.FindWithTag("GravityField");
            if (destroyGF != null)
            {
                Destroy(destroyGF);
                StopCoroutine(routine);
                routine = null;
            }
            // GravityField�̃N���[�����쐬
            GameObject gField = (GameObject)Instantiate(gravityField, (startMPosition2 + endMPosition2) / 2, Quaternion.identity);
            gField.transform.position = gField.transform.position + new Vector3(0, 0, -gField.transform.position.z - 0.1f);
            // �h���b�O�����T�C�Y�Ɋg��
            //gField.transform.localScale = new Vector2(Mathf.Abs(endMPosition2.x - startMPosition2.x), Mathf.Abs(endMPosition2.y - startMPosition2.y));
            gField.GetComponent<SpriteRenderer>().size = new Vector2(Mathf.Abs(endMPosition2.x - startMPosition2.x), Mathf.Abs(endMPosition2.y - startMPosition2.y));
            gField.GetComponent<BoxCollider2D>().size = new Vector2(Mathf.Abs(endMPosition2.x - startMPosition2.x), Mathf.Abs(endMPosition2.y - startMPosition2.y));

            // ���ʉ�
            GetComponent<AudioSource>().Play();

            routine = WaitAndDestroy();
            StartCoroutine(routine);
        }

        // ���L�[/S�L�[��gravityScale�̕ύX
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
                gravityText.text = "-1.0G / ���]"; // string.Format("{ 0,6}","-1.0G") + " / ���]";//"�d�͏�F-1.0G / ���]";
                break;
            case 1: // x0.5
                gravityScale = G_SCALE * 0.5f;
                moveSpeed = M_SPEED * 1.3f;
                gravityText.text = "0.5G / �@�y"; // string.Format("{0,6}","0.5G") + " / �@�y"; //"�d�͏�F0.5G / �y";
                break;
            case 2: // x2.0
                gravityScale = G_SCALE * 2.0f;
                moveSpeed = M_SPEED * 0.7f;
                gravityText.text = "2.0G / �@�d"; // string.Format("{ 0,6}", "2.0G") + " / �@�d";//"�d�͏�F2.0G / �d";
                break;
            default:
                Debug.Log("ChangeGravity�ŃG���[");
                break;
        }
    }

    private IEnumerator WaitAndDestroy()
    {
        yield return new WaitForSecondsRealtime(5); // 5�b�x��
        destroyGF = GameObject.FindWithTag("GravityField");
        if (destroyGF != null)
        {
            Destroy(destroyGF);
        }
    }

    private IEnumerator MouseWheelWait() // �}�E�X�z�C�[������̓��͂�0.2�b��������
    {
        isChangeable = false;
        yield return new WaitForSecondsRealtime(0.2f);
        isChangeable = true;
    }
}
