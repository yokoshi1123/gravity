using System;
using System.Collections;
using System.Collections.Generic;
using TMPro;
using UnityEngine;
using UnityEngine.UIElements;
using UnityEngine.SceneManagement;
using System.Linq;

public class LastMovieController : MonoBehaviour
{
    private Animator animator;
    private Animator UIanime;
    private FadeManager fadeManager;

    private bool isChangeable = false;
    private int mouseWheel = 2;

    private GameObject gFieldUI;
    private TextMeshProUGUI gravityValueText;

    //[SerializeField] private float span = 0.3f;

    const int convertionConstant = 65248;

    //private IEnumerator routine = null;

    [SerializeField] private bool build = false;
    private SaveDataManager saveDataManager;

    private GameObject gravityField;
    private List<GameObject> gFields = new();
    private const int gFieldNum = 32;

    // Start is called before the first frame update
    void Awake()
    {
        animator = GameObject.Find("LastMovie").GetComponent<Animator>();
        fadeManager = GameObject.FindWithTag("Fade").GetComponent<FadeManager>();
        fadeManager.FadeIn();

        if (build)
        {
            saveDataManager = GameObject.Find("SaveDataManager").GetComponent<SaveDataManager>();
            saveDataManager.SaveGameData("LastScene", 0, true);
        }

        gFieldUI = GameObject.Find("/Canvas/GFieldUI");
        UIanime = gFieldUI.GetComponent<Animator>();
        gravityValueText = GameObject.Find("/Canvas/GFieldUI/GravityText/GravityValue").GetComponent<TextMeshProUGUI>();
        gFieldUI.SetActive(true);

        gravityField = (GameObject)Resources.Load("GravityField");
        StartCoroutine(Movie1());
    }

    // Update is called once per frame
    void Update()
    {
        if (isChangeable)
        {
            if (Input.GetAxis("Mouse ScrollWheel") != 0)
            {
                StartCoroutine(MouseWheelWait());
                //Debug.Log(Input.GetAxis("Mouse ScrollWheel"));
                mouseWheel = (int)Mathf.Max((float)mouseWheel - (int)Input.GetAxis("Mouse ScrollWheel"), 1);
                ChangeGravity();
            }

            if (mouseWheel >= 20)
            {
                animator.SetBool("next", true);
                UIanime.SetInteger("Effect_id", 0);
                isChangeable = false;
                StartCoroutine(Movie2());
            }
        }

        //if (mouseWheel > 3072) StartCoroutine(Movie3());
        if (mouseWheel > 3072)
        {
            for (int i = 0; i < gFieldNum; i++) Destroy(gFields[i]);
            StartCoroutine(Movie3());
            animator.SetInteger("phase", 7);             
        }
        else if (mouseWheel > 2048) animator.SetInteger("phase", 6);
        else if (mouseWheel > 1024) animator.SetInteger("phase", 5);
        else if (mouseWheel > 768) animator.SetInteger("phase", 4);
        else if (mouseWheel > 512) animator.SetInteger("phase", 3);
        else if (mouseWheel > 256) animator.SetInteger("phase", 2);
        else if (mouseWheel > 64) animator.SetInteger("phase", 1);

        if (mouseWheel >= 1000) gFieldUI.GetComponent<RectTransform>().SetSizeWithCurrentAnchors(RectTransform.Axis.Horizontal, 290);

        //if (mouseWheel > 4096 && build) SceneManager.LoadScene("NewsScene");
    }

    public void ChangeGravity()
    {
        if (mouseWheel == 1)
        {
            for (int i = 0; i < gFieldNum; i++) gFields[i].GetComponent<GravityFieldTexture>().SetGPattern(1);
            gravityValueText.text = "‚O.‚TG / @  Light";
        }
        else if (mouseWheel >= 2)
        {
            for (int i = 0; i < gFieldNum; i++) gFields[i].GetComponent<GravityFieldTexture>().SetGPattern(2);
            gravityValueText.text = "";
            for (int i = 0; i < mouseWheel.ToString().Length; i++)
            {
                gravityValueText.text += (char)(mouseWheel.ToString()[i] + convertionConstant);
            }
            if (mouseWheel < 10)
            {
                gravityValueText.text += ".‚O";
            }
            gravityValueText.text += "G /     Heavy";
        }
    }

    private IEnumerator MouseWheelWait() // ƒ}ƒEƒXƒzƒC[ƒ‹‚©‚ç‚Ì“ü—Í‚ð0.2•b–³Ž‹‚·‚é
    {
        isChangeable = false;
        yield return new WaitForSecondsRealtime(0.07f); // 0.17f);
        isChangeable = true;
    }

    private IEnumerator Movie1()
    {
        yield return new WaitForSecondsRealtime(7.3f);
        fadeManager.FadeOut();
        yield return new WaitForSecondsRealtime(3 * fadeManager.fadeSpeed);
        fadeManager.FadeIn();
        yield return new WaitForSecondsRealtime(fadeManager.fadeSpeed);
        for (int i = 0; i < gFieldNum; i++)
        {
            Vector3 pos = new(9f * Mathf.Cos(2 * Mathf.PI * i / gFieldNum), 10f + 9f * Mathf.Sin(2 * Mathf.PI * i / gFieldNum), -1f);
            GameObject gFieldClone = Instantiate(gravityField, pos, Quaternion.Euler(0, 0, -90 + 360 * i / gFieldNum));
            gFields.Add(gFieldClone);
            gFieldClone.transform.localScale = new(0.5f, 4f, 1f);
            gFieldClone.GetComponent<GravityFieldTexture>().SetIsFixed(true);
            gFieldClone.GetComponent<GravityFieldTexture>().SetGPattern(2);
            gFieldClone.GetComponent<SpriteRenderer>().color = new(1f, 1f, 1f, 0.3f);
            //gFieldClone.transform.rotation = Quaternion.Euler(0, 0, -90 + 360 * i / 32);
        }
        UIanime.SetInteger("Effect_id", 1);
        isChangeable = true;
    }

    private IEnumerator Movie2() 
    {
        while (mouseWheel <= 3072) //mouseWheel < 999)
        {
            yield return new WaitForSeconds(2f / mouseWheel);
            mouseWheel++;
            ChangeGravity();
        }
    }

    private IEnumerator Movie3()
    {
        yield return new WaitForSecondsRealtime(0.125f);
        gFieldUI.SetActive(false);
        yield return new WaitForSecondsRealtime(1.95f);
        fadeManager.FadeOut();
        yield return new WaitForSecondsRealtime(5*fadeManager.fadeSpeed);
        SceneManager.LoadScene("NewsScene");
    }
}
