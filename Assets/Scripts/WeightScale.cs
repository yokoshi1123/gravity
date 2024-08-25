using System.Collections;
using System.Collections.Generic;
using UnityEditor;
using UnityEngine;

public class WeightScale : MonoBehaviour
{
    //private TotalMass totalMass;
    //private GravityManager gravityManager;
    private Rigidbody2D rb;

    //private float totalWeight = 0f;

    //[SerializeField] private Vector2 defaultPosition;

    //[SerializeField] private float MOVELENGTH = 0.1f;

    //private int direction;

    //private int i = 0;

    //[SerializeField] private float POWER = 100000;
    
    // Start is called before the first frame update
    void Awake()
    {
        //totalMass = GetComponent<TotalMass>();
        rb = GetComponent<Rigidbody2D>();
        //defaultPosition = transform.position;
    }

    // Update is called once per frame
    void Update()
    {
        //totalWeight = (int)(totalMass.GetMass()) * rb.gravityScale / gravityManager.GetDeFaultGravityScale();
        ////Debug.Log("Total Weight is " + totalWeight);
        //Debug.Log(i);
        //i++;
        //rb.velocity = Vector2.zero;
        rb.AddForce(-Physics.gravity.y * rb.mass * Vector2.up, ForceMode2D.Force);

        //if (Mathf.Abs(transform.position.y - defaultPosition.y) < totalWeight * MOVELENGTH)
        //{
        //    direction = (transform.position.y >= defaultPosition.y) ? 1 : -1;
        //    rb.MovePosition(new Vector2(transform.position.x, transform.position.y) + Vector2.down * direction);
        //}
    }
}
