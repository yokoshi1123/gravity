using System.Collections;
using System.Collections.Generic;
using UnityEngine;

public class NewsBelt : MonoBehaviour
{
    private bool open = false;

    private bool animEnd = false;

    private Animator beltAnim;
    // Start is called before the first frame update
    void Start()
    {
        beltAnim = GetComponent<Animator>();
    }

    // Update is called once per frame
    void Update()
    {
        beltAnim.SetBool("open", open);
    }

    public void SetOpen(bool value)
    {
        open = value;
    }

    public void EndAnim()
    {
        animEnd = true;
        //Debug.Log("Hello1");
    }

    public void SetAnimEnd(bool value)
    {
        animEnd = value;
    }

    public bool GetAnimEnd()
    {
        return animEnd;
    }
}
