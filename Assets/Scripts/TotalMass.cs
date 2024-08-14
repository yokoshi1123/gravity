using System.Collections;
using System.Collections.Generic;
using System.Linq;
using UnityEngine;
public class TotalMass : MonoBehaviour
{
    //private GravityManager gravityManager;

    //[SerializeField] 
    private List<GameObject> otherObjs = new();
    private Dictionary<string, float> addedObjs = new();

    private Vector2 myPosition;
    private Vector2 otherPosition = Vector2.zero;

    private TotalMass otherTM;

    [SerializeField] 
    private float totalMass;

    [SerializeField]
    private bool isAdded = false;

    private float defaultMass;


    void Start()
    {
        //gravityManager = GameObject.Find("GravityManager").GetComponent<GravityManager>();
        myPosition = transform.position;
        defaultMass = GetComponent<Rigidbody2D>().mass;
        totalMass = defaultMass;
    }

    void Update()
    {
        myPosition = transform.position;

        if (defaultMass != GetComponent<Rigidbody2D>().mass)
        {
            totalMass += GetComponent<Rigidbody2D>().mass - defaultMass;
            defaultMass = GetComponent<Rigidbody2D>().mass;
        }

        foreach (GameObject other in otherObjs)
        {
            otherTM = other.GetComponent<TotalMass>();
            if (!otherTM.GetBool())
            {
                totalMass += otherTM.GetMass();
                addedObjs.Add(other.name, otherTM.GetMass());
                otherTM.SetBool(true);
                //Debug.Log("Added");
            }
            else if (addedObjs.ContainsKey(other.name) && otherTM.GetMass() != addedObjs[other.name])
            {
                totalMass += otherTM.GetMass() - addedObjs[other.name];
                //Debug.Log(otherTM.GetMass() + ", " + addedObjs[other.name]);
                addedObjs[other.name] = otherTM.GetMass();
                //Debug.Log("Updated");
            }
        }
    }

    public bool GetBool()
    {
        return isAdded;
    }
    public void SetBool(bool value)
    {
        isAdded = value;
    }

    public float GetMass()
    {
        return totalMass;
    }

    public void PlusMass(float value)
    {
        totalMass += value;
    }

    private void OnCollisionEnter2D(Collision2D other)
    {
        otherTM = other.gameObject.GetComponent<TotalMass>();
        otherPosition = other.transform.position;
        if (otherTM != null && ((otherPosition.y - myPosition.y) * Mathf.Sign(other.gameObject.GetComponent<Rigidbody2D>().gravityScale) >= 0) && GetComponent<Rigidbody2D>().gravityScale * other.gameObject.GetComponent<Rigidbody2D>().gravityScale > 0 && !otherObjs.Contains(other.gameObject)) // (myPosition.y <= otherPosition.y)
        {
            otherObjs.Add(other.gameObject);
        }     
    }
    private void OnCollisionExit2D(Collision2D other)
    {
        if (otherObjs.Contains(other.gameObject))
        {
            if (addedObjs.ContainsKey(other.gameObject.name))
            { 
                otherTM = other.gameObject.GetComponent<TotalMass>();
                totalMass -= otherTM.GetMass();
                addedObjs.Remove(other.gameObject.name);
                otherTM.SetBool(false);
            }
            otherObjs.Remove(other.gameObject);
            //Debug.Log("Removed");
        }
    }
}
