using System.Collections;
using System.Collections.Generic;
using UnityEngine;

public class TouchButton : MonoBehaviour
{
    [SerializeField] private List<TurnOn> to = new();
    [SerializeField] private Sprite OffTexture;
    [SerializeField] private Sprite OnTexture;

    private SpriteRenderer Renderer;

    private bool turnon;



    // Start is called before the first frame update
    void Start()
    {
        Renderer = GetComponent<SpriteRenderer>();
    }

    // Update is called once per frame
    void Update()
    {
        turnon = to[0].GetTurnOn();

        if(turnon)
        {
            Renderer.sprite = OnTexture;
        }
        else
        {
            Renderer.sprite = OffTexture;
        }
        
    }

    //private void OnCollisionStay2D(Collision2D other)
    //{
    //    Debug.Log(other.transform.gameObject.name + ": C");
    //    if(other.transform.parent.gameObject != null && other.transform.parent.gameObject.CompareTag("Player") || other.transform.parent.gameObject.CompareTag("Movable"))
    //    {
    //        for(int i = 0; i < to.Count; i++)
    //        {
    //            to[i].SetTurnOn(true);
    //        }
    //    }
    //}

    private void OnCollisionExit2D(Collision2D other)
    {
        for (int i = 0; i < to.Count; i++)
        {
            to[i].SetTurnOn(false);
        }
    }

    private void OnTriggerStay2D(Collider2D other)
    {
        //try
        //{
        //Debug.Log(other.gameObject.name + ", " + other.transform.parent.gameObject.name + " : T");
        bool b1 = other.transform.parent.gameObject.CompareTag("Player");
        bool b2 = other.transform.parent.gameObject.CompareTag("Movable");

        if (other.transform.parent.gameObject != null && (b1 || b2)) //(other.transform.parent.gameObject.CompareTag("Player") || other.transform.parent.gameObject.CompareTag("Movable")))
        {
            for (int i = 0; i < to.Count; i++)
            {
                to[i].SetTurnOn(true);
                //Debug.Log("On");
            }
        }
        //}
        //catch
        //{
        //    Debug.Log(other.gameObject.name + " : T");
        //}
    }

    private void OnTriggerExit2D(Collider2D other)
    {
        for (int i = 0; i < to.Count; i++)
        {
            to[i].SetTurnOn(false);
            //Debug.Log("Off");
        }
    }
}
