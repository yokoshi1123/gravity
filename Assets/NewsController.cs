using System.Collections;
using System.Collections.Generic;
using UnityEngine;

public class NewsController : MonoBehaviour
{
    private NewsBelt belt;
    private NewsMonitor monitor;
    private GameObject[] Text = new GameObject[4];
    private GameObject[] Clocks = new GameObject[3];

    [SerializeField] private int animIndex = 0;

    // Start is called before the first frame update
    void Start()
    {
        belt = transform.Find("belt").gameObject.GetComponent<NewsBelt>();
        monitor = transform.Find("monitor").gameObject.GetComponent<NewsMonitor>();
        Text[0] = transform.Find("Text0-1").gameObject;
        Text[1] = transform.Find("Text0-2").gameObject;
        Text[2] = transform.Find("Text1").gameObject;
        Text[3] = transform.Find("Text2").gameObject;

        monitor.SetAnimIndex(animIndex);

        int i;
        for(i = 0; i < 4 ; i++)
        {
            Text[i].SetActive(false);
        }

        for(i = 0; i < 3 ; i++)
        {
            Clocks[i] = transform.Find("clocks").gameObject.transform.GetChild(i).gameObject;
            Clocks[i].SetActive(false);
        }

        Clocks[animIndex].SetActive(true);

        switch (animIndex)
        {
            case 0:
                StartCoroutine(News0());
                break;
            case 1:
                StartCoroutine(News1());
                break;
            case 2:
                StartCoroutine(News2());
                break;
        }
    }

    // Update is called once per frame
    void Update()
    {
 
    
    }

    private IEnumerator News0()
    {
        belt.SetOpen(true);
        yield return new WaitUntil(() => belt.GetAnimEnd());//beltが開ききるまで
        belt.SetAnimEnd(false);
        Text[0].SetActive(true);//文字表示
        yield return new WaitForSeconds(2);//遅延
        monitor.SetOpen(true);
        yield return new WaitUntil(() => monitor.GetAnimEnd());//monitorが開き、最初の画像が終わるまで
        //Debug.Log("Hello");
        monitor.SetAnimEnd(false);
        Text[0].SetActive(false);
        yield return new WaitForSeconds(0.5f);
        Text[1].SetActive(true);

        yield return new WaitForSeconds(4);
    }

    private IEnumerator News1()
    {
        belt.SetOpen(true);
        yield return new WaitUntil(() => belt.GetAnimEnd());//beltが開ききるまで
        belt.SetAnimEnd(false);
        Text[2].SetActive(true);//文字表示
        yield return new WaitForSeconds(2);//遅延
        monitor.SetOpen(true);
        yield return new WaitUntil(() => monitor.GetAnimEnd());//monitorが開き終わるまで
        monitor.SetAnimEnd(false);
    }

    private IEnumerator News2()
    {
        belt.SetOpen(true);
        yield return new WaitUntil(() => belt.GetAnimEnd());//beltが開ききるまで
        belt.SetAnimEnd(false);
        Text[3].SetActive(true);//文字表示
        yield return new WaitForSeconds(2);//遅延
        monitor.SetOpen(true);
        yield return new WaitUntil(() => monitor.GetAnimEnd());//monitorが開き終わるまで
        monitor.SetAnimEnd(false);
    }
}
