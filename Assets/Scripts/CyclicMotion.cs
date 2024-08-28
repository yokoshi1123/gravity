using System.Collections;
using System.Collections.Generic;
using System.Runtime.CompilerServices;
using Unity.VisualScripting;
using UnityEngine;

[RequireComponent(typeof(TurnOn))]
[RequireComponent(typeof(Animator))]

public class CyclicMotion : MonoBehaviour
{
    [SerializeField] private float OFFSET = 1f;
    [SerializeField] private float ONDURATION = 2f;
    [SerializeField] private float OFFDURATION = 3f;

    private const float OFF2ON = 1.3f;
    private const float ON2OFF = 0.3f;

    private TurnOn to;
    private Animator animator;

    // Start is called before the first frame update
    void Start()
    {
        to = GetComponent<TurnOn>();
        animator = GetComponent<Animator>();
        animator.SetBool("turnOn", to.GetTurnOn());
        
        StartCoroutine(Cycle());
    }

    private IEnumerator Cycle()
    {
        yield return new WaitForSeconds(OFFSET);
        while (true)
        {
            animator.SetBool("turnOn", true);
            yield return new WaitForSeconds(OFF2ON + ONDURATION);
            animator.SetBool("turnOn", false);
            yield return new WaitForSeconds(ON2OFF + OFFDURATION);

        }
    }
}
