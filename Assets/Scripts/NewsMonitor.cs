using System.Collections;
using System.Collections.Generic;
using UnityEngine;

public class NewsMonitor : MonoBehaviour
{
    private Animator monitorAnim;

    private bool open = false;
    private int animIndex = 0;

    private bool animEnd = false;

    // Start is called before the first frame update
    void Start()
    {
        monitorAnim = GetComponent<Animator>();
    }

    // Update is called once per frame
    void Update()
    {
        monitorAnim.SetBool("open", open);
        monitorAnim.SetInteger("animIndex", animIndex);
    }

    public void SetOpen(bool value)
    {
        open = value;
    }

    public void EndAnim()
    {
        animEnd = true;
        //Debug.Log("Hello");
    }

    public void SetAnimEnd(bool value)
    {
        animEnd = value;
    }

    public bool GetAnimEnd()
    {
        return animEnd;
    }

    public void SetAnimIndex(int num)
    {
        animIndex = num;
    }



}
