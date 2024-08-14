using System.Collections;
using System.Collections.Generic;
using UnityEngine;

public class DisappearingFloor : MonoBehaviour
{
    [SerializeField] private float OFFSET = 0f;
    [SerializeField] private float ONDURATION = 3f;
    [SerializeField] private float OFFDURATION = 3f;

    private BoxCollider2D myCollider;
    private SpriteRenderer myRenderer;

    void Awake()
    {
        myCollider = GetComponent<BoxCollider2D>();
        myRenderer = GetComponent<SpriteRenderer>();
    }

    void Start()
    {
        StartCoroutine(DisappearCycle());
    }

    private IEnumerator DisappearCycle()
    {
        yield return new WaitForSeconds(OFFSET);
        while (true)
        {
            yield return new WaitForSeconds(ONDURATION-1f);
            for (int i = 0; i < 5; i++)
            {
                myRenderer.color += new Color(0, 0, 0, -myRenderer.color.a + 0.4f);
                yield return new WaitForSeconds(0.1f);
                myRenderer.color += new Color(0, 0, 0, -myRenderer.color.a + 0.8f);
                yield return new WaitForSeconds(0.1f);
            }
            //Debug.Log("ON -> OFF");
            myRenderer.color += new Color(0, 0, 0, -myRenderer.color.a + 0f);
            myCollider.enabled = false;
            yield return new WaitForSeconds(OFFDURATION - 1f);
            for (int i = 0; i < 5; i++)
            {
                myRenderer.color += new Color(0, 0, 0, -myRenderer.color.a + 0.8f);
                yield return new WaitForSeconds(0.1f);
                myRenderer.color += new Color(0, 0, 0, -myRenderer.color.a + 0.4f);
                yield return new WaitForSeconds(0.1f);
            }
            //Debug.Log("OFf -> ON");
            myRenderer.color = myRenderer.color + new Color(0, 0, 0, -myRenderer.color.a + 1f);
            myCollider.enabled = true;
        }
    }
}
