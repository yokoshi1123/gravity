using System.Collections;
using System.Collections.Generic;
using UnityEngine;
using UnityEngine.UI;

public class BlackOut : MonoBehaviour
{
    private Image im;
    
    // Start is called before the first frame update
    void Start()
    {
        im = GetComponent<Image>();
        Debug.Log("Start");
        StartCoroutine(Effect());
    }

    private IEnumerator Effect()
    {
        while (true)
        {
            for (int i = 0; i < 20; i++)
            {
                im.color += new Color(0, 0, 0, 0.00625f);
                yield return new WaitForSeconds(0.025f);
            }
            yield return new WaitForSeconds(0.1f);
            for (int i = 0; i < 20; i++)
            {
                im.color -= new Color(0, 0, 0, 0.00625f);
                yield return new WaitForSeconds(0.025f);
            }
            yield return new WaitForSeconds(0.1f);
        }        
    }
}
