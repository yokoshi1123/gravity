using System.Collections;
using System.Collections.Generic;
using UnityEngine;

[RequireComponent(typeof(TurnOn))]
public class Shutter : MonoBehaviour
{

    [SerializeField]
    private GameObject shutterCollider;
    /*[SerializeField] */private Animator animator;

    //[SerializeField] private GameObject Switch;

    private bool turnOn = false;
    private bool oldTO;
    //private Transform defaultTransform;

    private TurnOn to;

    private IEnumerator routine = null;

    void Awake()
    {
        shutterCollider = transform.GetChild(0).gameObject; // GameObject.Find("/" + this.name + "/Collider");
        animator = GetComponent<Animator>();
        
        to = GetComponent<TurnOn>();
        oldTO = turnOn;
    }

    // Update is called once per frame
    void Update()
    {
        //turnOn = Switch.turnOn;
        turnOn = to.GetTurnOn();
        animator.SetBool("turnOn", turnOn);

        //if(turnOn)
        //{
        //    ShutterCollider.SetActive(false);
        //}
        //else
        //{
        //    ShutterCollider.SetActive(true);
        //}

        if (turnOn != oldTO)
        {
            if (routine != null) { StopCoroutine(routine); }
            routine = ChangeSprite(turnOn);
            StartCoroutine(routine);
        }
        oldTO = turnOn;
    }

    private IEnumerator ChangeSprite(bool value)
    {
        yield return (value ? new WaitForSeconds(0.35f) : null);
        //Debug.Log((value ? "Op" : "Clo"));
        shutterCollider.SetActive(!value);
        //Debug.Log((value ? "en" : "se"));
    }
}
