using System;
using System.Collections;
using System.Collections.Generic;
using TMPro;
using UnityEngine;
using UnityEngine.UIElements;
using UnityEngine.SceneManagement;

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

    // Start is called before the first frame update
    void Start()
    {
        animator = GameObject.Find("LastMovie").GetComponent<Animator>();
        fadeManager = GameObject.FindWithTag("Fade").GetComponent<FadeManager>();
        fadeManager.fadeIn();

        if (build)
        {
            saveDataManager = GameObject.Find("SaveDataManager").GetComponent<SaveDataManager>();
            saveDataManager.SaveGameData("LastScene", 0, true);
        }

        gFieldUI = GameObject.Find("/Canvas/GFieldUI");
        UIanime = gFieldUI.GetComponent<Animator>();
        gravityValueText = GameObject.Find("/Canvas/GFieldUI/GravityText/GravityValue").GetComponent<TextMeshProUGUI>();
        gFieldUI.SetActive(true);
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

        if (mouseWheel >= 1000)
        {
            gFieldUI.GetComponent<RectTransform>().SetSizeWithCurrentAnchors(RectTransform.Axis.Horizontal, 290);
        }

        if (mouseWheel > 4096 && build)
        {
            SceneManager.LoadScene("NewsScene");
        }
    }

    public void ChangeGravity()
    {
        if (mouseWheel == 1)
        {
            gravityValueText.text = "‚O.‚TG / @  Light";
        }
        else if (mouseWheel >= 2)
        {
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
        yield return new WaitForSecondsRealtime(0.12f); // 0.17f);
        isChangeable = true;
    }

    private IEnumerator Movie1()
    {
        yield return new WaitForSecondsRealtime(8f);
        fadeManager.fadeOut();
        yield return new WaitForSecondsRealtime(fadeManager.fadeSpeed * 3);
        fadeManager.fadeIn();
        yield return new WaitForSecondsRealtime(fadeManager.fadeSpeed);
        UIanime.SetInteger("Effect_id", 1);
        isChangeable = true;
    }

    private IEnumerator Movie2() 
    {
        while (true) //mouseWheel < 999)
        {
            yield return new WaitForSeconds(1f / mouseWheel);
            mouseWheel++;
            ChangeGravity();
        }
    }
}
