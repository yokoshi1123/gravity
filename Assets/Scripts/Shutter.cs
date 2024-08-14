using System.Collections;
using System.Collections.Generic;
using UnityEngine;



public class Shutter : MonoBehaviour
{

    [SerializeField] private GameObject ShutterCollider;
    [SerializeField] private Animator animator;
    //[SerializeField] private GameObject Switch;

    public bool turnon = false;
    //private Transform defaultTransform;

    // Start is called before the first frame update
    void Start()
    {
        
    }

    // Update is called once per frame
    void Update()
    {
        //turnon = Switch.turnon;
        animator.SetBool("turnon", turnon);

        if(turnon)
        {
            ShutterCollider.SetActive(false);
        }
        else
        {
            ShutterCollider.SetActive(true);
        }

    }
}
