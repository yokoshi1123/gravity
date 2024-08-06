using System.Collections;
using System.Collections.Generic;
using UnityEngine;

public class WeightScale : MonoBehaviour
{
    private TotalMass totalMass;
    private GravityManager gravityManager;
    private Rigidbody2D rb;

    private float totalWeight = 0f;

    [SerializeField] private Vector2 defaultPosition;

    //[SerializeField] private float MOVELENGTH = 0.1f;

    private int direction;

    private int i = 0;
    
    // Start is called before the first frame update
    void Awake()
    {
        totalMass = GetComponent<TotalMass>();
        gravityManager = GameObject.Find("GravityManager").GetComponent<GravityManager>();
        rb = GetComponent<Rigidbody2D>();
        defaultPosition = transform.position;
    }

    // Update is called once per frame
    void Update()
    {
        totalWeight = (int)(totalMass.GetMass()) * rb.gravityScale / gravityManager.GetDeFaultGravityScale();
        //Debug.Log("Total Weight is " + totalWeight);
        Debug.Log(i);
        i++;

        //if (Mathf.Abs(transform.position.y - defaultPosition.y) < totalWeight * MOVELENGTH)
        //{
        //    direction = (transform.position.y >= defaultPosition.y) ? 1 : -1;
        //    rb.MovePosition(new Vector2(transform.position.x, transform.position.y) + Vector2.down * direction);
        //}
    }
}
