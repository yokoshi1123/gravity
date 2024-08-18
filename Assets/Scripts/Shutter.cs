using System.Collections;
using System.Collections.Generic;
using UnityEngine;

[RequireComponent(typeof(TurnOn))]
public class Shutter : MonoBehaviour
{

    [SerializeField] private GameObject ShutterCollider;
    [SerializeField] private Animator animator;

    [SerializeField] private PressureButton pButton;
    //[SerializeField] private GameObject Switch;

    private bool turnOn = false;
    //private Transform defaultTransform;

    private TurnOn to;

    void Awake()
    {
        to = GetComponent<TurnOn>();
    }

    // Update is called once per frame
    void Update()
    {
        //turnOn = Switch.turnOn;
        //turnOn = pButton.GetIsPressed();
        turnOn = to.GetTurnOn();
        animator.SetBool("turnOn", turnOn);

        if(turnOn)
        {
            ShutterCollider.SetActive(false);
        }
        else
        {
            ShutterCollider.SetActive(true);
        }

    }
}
