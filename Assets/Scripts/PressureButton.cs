using System.Collections;
using System.Collections.Generic;
using UnityEngine;

public class PressureButton : MonoBehaviour
{
    private GameObject target;
    private GravityManager gravityManager;
    private Rigidbody2D rb;
    private TotalMass mass;
    [SerializeField] private GameObject button;
    [SerializeField] private float onValue = 10f; 

    void Awake()
    {
        gravityManager = GameObject.Find("GravityManager").GetComponent<GravityManager>();
    }
    void Update()
    {
        if (target != null)
        {
            rb = target.GetComponent<Rigidbody2D>();
            mass = target.GetComponent<TotalMass>();

            if (rb != null && mass != null && mass.GetMass() * rb.gravityScale / gravityManager.GetDeFaultGravityScale() >= onValue) //(rb.mass * rb.gravityScale >= onValue)
            {
                button.transform.localScale = new Vector2(0.4f, 1f);
                Debug.Log(mass.GetMass() * rb.gravityScale / gravityManager.GetDeFaultGravityScale() + ": The button is pressed");
            }
            else
            {
                button.transform.localScale = Vector2.one;
            }
        }
        
    }

    private void OnTriggerEnter2D(Collider2D collision)
    {
        if (!collision.CompareTag("Stage"))
        {
            target = collision.gameObject;
            //rb = collision.gameObject.GetComponent<Rigidbody2D>();
            //if (rb.mass * rb.gravityScale >= onValue)
            //{
            //    button.transform.localScale = new Vector2(0.4f, 1f);
            //    Debug.Log("The button is pressed");
            //}
            //else
            //{
            //    button.transform.localScale = Vector2.one;
            //}
        }
    }

    private void OnTriggerExit2D(Collider2D collision)
    {
        if (!collision.CompareTag("Stage"))
        {
            target = null;
            button.transform.localScale = Vector2.one;
        }
    }

}
