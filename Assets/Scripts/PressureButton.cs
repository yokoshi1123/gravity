using System.Collections;
using System.Collections.Generic;
using UnityEngine;

public class PressureButton : MonoBehaviour
{ 
    private GravityManager gravityManager;

    [SerializeField] private GameObject target;
    [SerializeField] private Rigidbody2D targetRb;
    [SerializeField] private TotalMass targetMass;
    private Vector2 defaultPosition;
    //[SerializeField] private float speed = 10f;
    private BoxCollider2D bc2D;

    //[SerializeField] private GameObject button;
    [SerializeField] private float onValue = 10f;
    [SerializeField] private bool isPressed = false;

    void Awake()
    {
        defaultPosition = transform.position;
        gravityManager = GameObject.Find("GravityManager").GetComponent<GravityManager>();

        bc2D = GetComponent<BoxCollider2D>();
    }
    void Update()
    {
        if (target != null)
        {
            targetRb = target.GetComponent<Rigidbody2D>();
            targetMass = target.GetComponent<TotalMass>();

            if (targetRb != null && targetMass != null && targetMass.GetMass() * targetRb.gravityScale / gravityManager.GetDeFaultGravityScale() >= onValue) //(targetRb.targetMass * targetRb.gravityScale >= onValue)
            {
                //Vector2 toVector = Vector2.MoveTowards(defaultPosition, defaultPosition + new Vector2(0, -0.49f), speed * Time.deltaTime);
                //rb.MovePosition(toVector);
                //targetRb.MovePosition(toVector);
                //rb.MovePosition(defaultPosition + new Vector2(0, -0.49f));
                bc2D.enabled = true;
                transform.position = defaultPosition + new Vector2(0, -0.49f);
                //targetRb.velocity += new Vector2(0, -0.49f);

                isPressed = true;
                //transform.localScale = new Vector2(1f, 0.4f);
                Debug.Log(targetMass.GetMass() * targetRb.gravityScale / gravityManager.GetDeFaultGravityScale() + ": The button is pressed");
            }
            else
            {
                transform.position = defaultPosition;
                bc2D.enabled = false;
                //rb.MovePosition(defaultPosition);
                //Vector2 toVector = Vector2.MoveTowards(defaultPosition + new Vector2(0, -0.49f), defaultPosition, speed * Time.deltaTime);
                //rb.MovePosition(toVector);
                isPressed = false;
                
                //transform.localScale = Vector2.one;
            }
        }
        
    }

    public bool GetBool()
    {
        return isPressed;
    }

    private void OnTriggerEnter2D(Collider2D collision)
    {
        if (!collision.gameObject.CompareTag("Stage"))
        {
            target = collision.gameObject;
            //targetRb = collision.gameObject.GetComponent<Rigidbody2D>();
            //if (targetRb.targetMass * targetRb.gravityScale >= onValue)
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
        if (!collision.gameObject.CompareTag("Stage"))
        {
            target = null;
            targetRb = null;
            targetMass = null;

            transform.position = defaultPosition;
            bc2D.enabled = false;
            //Vector2 toVector = Vector2.MoveTowards(defaultPosition + new Vector2(0, -0.49f), defaultPosition, speed * Time.deltaTime);
            //rb.MovePosition(toVector);
            //rb.MovePosition(defaultPosition);
            isPressed = false;
            //transform.localScale = Vector2.one;
        }
    }
}
