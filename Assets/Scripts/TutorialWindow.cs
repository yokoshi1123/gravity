using System.Collections;
using System.Collections.Generic;
using UnityEngine;

public class TutorialWindow : MonoBehaviour
{
    private Animator tutorialAnimator;

    [SerializeField] private int tutorialNum = 0;
    
    // Start is called before the first frame update
    void Start()
    {
        tutorialAnimator = transform.GetChild(0).GetComponent<Animator>();
        tutorialAnimator.SetInteger("tutorialNum", tutorialNum);
    }

    // Update is called once per frame
    void Update()
    {
        
    }

    private void OnTriggerStay2D(Collider2D collision)
    {
        if (collision.CompareTag("Player"))
        {
            tutorialAnimator.SetBool("open", true);
            Debug.Log("On");
        }

    }

    private void OnTriggerExit2D(Collider2D collision)
    {
        if (collision.CompareTag("Player"))
        {
            tutorialAnimator.SetBool("open", false);
            Debug.Log("Off");
        }
    }
}
