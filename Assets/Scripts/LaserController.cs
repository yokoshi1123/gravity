using System.Collections;
using System.Collections.Generic;
using UnityEngine;

public class LaserController : MonoBehaviour
{
    [SerializeField] private float offset = 0;
    [SerializeField] private float onDuration = 3;
    [SerializeField] private float offDuration = 9;
    private float TRANSITION = 20;
    private float THICKNESS = 0.6f;

    void Start()
    {
        StartCoroutine(LaserCycle());
    }

    private IEnumerator LaserCycle()
    {
        yield return new WaitForSeconds(offset);
        while (true)
        {
            for (int i = 1; i <= TRANSITION; i++)
            {
                yield return new WaitForSeconds(1 / TRANSITION);
                gameObject.transform.localScale = gameObject.transform.localScale +  new Vector3(-gameObject.transform.localScale.x + THICKNESS * i / TRANSITION, 0, 0);
            }
            yield return new WaitForSeconds(onDuration);
            for (int i = 1; i <= TRANSITION; i++)
            {
                yield return new WaitForSeconds(1 / TRANSITION);
                gameObject.transform.localScale = gameObject.transform.localScale +  new Vector3(-gameObject.transform.localScale.x + THICKNESS * (1 - i / TRANSITION), 0, 0);
            }
            yield return new WaitForSeconds(offDuration);
        }
    }
}
