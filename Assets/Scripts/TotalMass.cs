using System.Collections;
using System.Collections.Generic;
using System.Linq;
using UnityEngine;
public class TotalMass : MonoBehaviour
{
    //private GravityManager gravityManager;

    [SerializeField]
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
    private Vector2 defaultPosition;


    void Start()
    {
        //gravityManager = GameObject.Find("GravityManager").GetComponent<GravityManager>();
        myPosition = transform.position;
        defaultPosition = transform.localPosition;
        defaultMass = GetComponent<Rigidbody2D>().mass;
        totalMass = defaultMass;
    }

    void Update()
    {
        myPosition = transform.position;

        if (defaultMass != GetComponent<Rigidbody2D>().mass)
        {
            //Debug.Log("Before: " + totalMass + ", After: " + (totalMass + GetComponent<Rigidbody2D>().mass - defaultMass));
            totalMass += GetComponent<Rigidbody2D>().mass - defaultMass;
            defaultMass = GetComponent<Rigidbody2D>().mass;
        }

        foreach (GameObject other in otherObjs)
        {
            otherTM = other.GetComponent<TotalMass>();
            if (!addedObjs.ContainsKey(other.name))
            {
                totalMass += otherTM.GetMass();
                addedObjs.Add(other.name, otherTM.GetMass());
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

    public float GetDefaultMass()
    {
        return defaultMass;
    }

    public void PlusMass(float value)
    {
        totalMass += value;
    }

    private void OnCollisionEnter2D(Collision2D other)
    {
        otherTM = other.gameObject.GetComponent<TotalMass>();
        otherPosition = other.transform.position;
        if (otherTM != null && ((otherPosition.y - myPosition.y) * Mathf.Sign(other.gameObject.GetComponent<Rigidbody2D>().gravityScale) > 0) && GetComponent<Rigidbody2D>().gravityScale * other.gameObject.GetComponent<Rigidbody2D>().gravityScale > 0 && !otherObjs.Contains(other.gameObject) && !otherTM.GetBool()) // (myPosition.y <= otherPosition.y)
        {
            //if (other.gameObject.name != "Player")
            //{
            //    other.gameObject.transform.SetParent(this.gameObject.transform);
            //}
            otherObjs.Add(other.gameObject);
            otherTM.SetBool(true);
        }

        if (this.gameObject.name != "Player" && other.gameObject.CompareTag("Abyss"))
        {
            StartCoroutine(Respawn());
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
                //if (other.gameObject.name != "Player")
                //{
                //    other.gameObject.transform.SetParent(null);
                //}
                otherTM.SetBool(false);
            }
            otherObjs.Remove(other.gameObject);
            //Debug.Log("Removed");
        }
    }

    private IEnumerator Respawn()
    {
        yield return null;
        transform.position = defaultPosition;
    }
}
