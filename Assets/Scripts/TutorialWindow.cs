using System.Collections;
using System.Collections.Generic;
using UnityEngine;

public class TutorialWindow : MonoBehaviour
{
    private Animator tutorialAnimator;
    private AudioSource tutorialSource;

    [SerializeField] private int tutorialNum = 0;
    
    // Start is called before the first frame update
    void Start()
    {
        tutorialAnimator = transform.GetChild(0).GetComponent<Animator>();
        tutorialAnimator.SetInteger("tutorialNum", tutorialNum);
        tutorialSource = GetComponent<AudioSource>();
    }

    // Update is called once per frame
    //void Update() { }
    private void OnTriggerEnter2D(Collider2D collision)
    {
        if (collision.transform.parent.CompareTag("Player"))
        {
            tutorialAnimator.SetBool("open", true);
            tutorialSource.PlayOneShot(tutorialSource.clip, 0.2f);
            //Debug.Log("On");
        }

    }

    private void OnTriggerStay2D(Collider2D collision)
    {
        if (collision.transform.parent.CompareTag("Player"))
        {
            tutorialAnimator.SetBool("open", true);
            //Debug.Log("On");
        }

    }

    private void OnTriggerExit2D(Collider2D collision)
    {
        if (collision.transform.parent.CompareTag("Player"))
        {
            tutorialAnimator.SetBool("open", false);
            //Debug.Log("Off");
        }
    }
}
