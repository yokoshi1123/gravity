using System.Collections;
using System.Collections.Generic;
using System.Linq;
using UnityEngine;
public class TotalMass : MonoBehaviour
{
    //private GravityManager gravityManager;

    [SerializeField]
    private List<GameObject> otherObjs = new();
    private readonly Dictionary<string, float> addedObjs = new();

    private Vector2 myPosition;
    private Vector2 otherPosition = Vector2.zero;

    private TotalMass otherTM;

    [SerializeField] 
    private float totalMass;

    [SerializeField]
    private bool isAdded = false;

    private float DEFAULTMASS;
    private float oldMass;
    private Vector2 defaultPosition;

    [SerializeField] private bool noRb = false;

    private void Start()
    {
        //gravityManager = GameObject.Find("GravityManager").GetComponent<GravityManager>();
        myPosition = transform.position;
        defaultPosition = transform.localPosition;
        try
        {
            DEFAULTMASS = GetComponent<Rigidbody2D>().mass;
        }
        catch
        {
            DEFAULTMASS = 0f;
            noRb = true;
        }

        //if (GetComponent<Rigidbody2D>().isKinematic)
        //{ 
        //    DEFAULTMASS = 0f;
        //    noRb = true;
        //}

        totalMass = DEFAULTMASS;
        oldMass = DEFAULTMASS;
    }
    private void Update()
    {
        myPosition = transform.position;

        if (!noRb && oldMass != GetComponent<Rigidbody2D>().mass)
        {
            totalMass += GetComponent<Rigidbody2D>().mass - oldMass;
            oldMass = GetComponent<Rigidbody2D>().mass;
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
    public bool GetIsAdded()
    {
        return isAdded;
    }
    public void SetIsAdded(bool value)
    {
        isAdded = value;
        //Debug.Log(gameObject.name + ": " + value);
    }
    public float GetMass()
    {
        return totalMass;
    }
    public float GetDefaultMass()
    {
        return DEFAULTMASS;
    }
    public void SetMass(float value, bool isAddition = false)
    {
        totalMass = (isAddition) ? totalMass + value : value;
    }
    private void OnTriggerEnter2D(Collider2D other)
    {
        otherTM = other.gameObject.GetComponent<TotalMass>();
        otherPosition = other.transform.position;
        //if (this.gameObject.name == "Player")
        //{
        //   Debug.Log("Collided with " + other.gameObject.name);
        //}

        try
        {
            if (otherTM != null && ((otherPosition.y - myPosition.y) * Mathf.Sign(GetComponent<Rigidbody2D>().gravityScale) > 0) && GetComponent<Rigidbody2D>().gravityScale * other.gameObject.GetComponent<Rigidbody2D>().gravityScale > 0 && !otherObjs.Contains(other.gameObject) && (this.gameObject.name == "Player" || !otherTM.GetIsAdded()))
            {
                //if (this.gameObject.name == "Player" && !GetComponent<PlayerController>().GetIsGrabbing())
                //{ 
                //    return;
                //}
                otherObjs.Add(other.gameObject);
                otherTM.SetIsAdded(true);
                Debug.Log(this.gameObject.name + " : " + other.gameObject.name + " added : Try");
            }
        }
        catch
        {
            if (otherTM != null && (otherPosition.y - myPosition.y > 0) && noRb && !otherObjs.Contains(other.gameObject) && !otherTM.GetIsAdded()) // (myPosition.y <= otherPosition.y)
            {
                otherObjs.Add(other.gameObject);
                otherTM.SetIsAdded(true);
                Debug.Log(this.gameObject.name + " : " + other.gameObject.name + " added : Catch");
            }
        }
        //if (noRb)
        //{

        //}
        //else if (otherTM != null && ((otherPosition.y - myPosition.y) * Mathf.Sign(other.gameObject.GetComponent<Rigidbody2D>().gravityScale) > 0) && GetComponent<Rigidbody2D>().gravityScale * other.gameObject.GetComponent<Rigidbody2D>().gravityScale > 0 && !otherObjs.Contains(other.gameObject) && !otherTM.GetIsAdded())
        //{
        //    Debug.Log("Error");
        //}

        if (this.gameObject.name != "Player" && other.gameObject.CompareTag("Abyss"))
        {
            StartCoroutine(Respawn());
        }
    }
    private void OnTriggerExit2D(Collider2D other)
    {
        if (otherObjs.Contains(other.gameObject))
        {
            if (addedObjs.ContainsKey(other.gameObject.name))
            {
                otherTM = other.gameObject.GetComponent<TotalMass>();
                totalMass -= otherTM.GetMass();
                addedObjs.Remove(other.gameObject.name);
                otherTM.SetIsAdded(false);
                Debug.Log(this.gameObject.name + " : removed");
            }
            otherObjs.Remove(other.gameObject);

        }
    }


    //private void OnTriggerEnter2D(Collider2D other)
    //{
    //    otherTM = other.gameObject.GetComponent<TotalMass>();
    //    otherPosition = other.transform.position;

    //    try
    //    {
    //        if (otherTM != null && ((otherPosition.y - myPosition.y) * Mathf.Sign(GetComponent<Rigidbody2D>().gravityScale) > 0) && GetComponent<Rigidbody2D>().gravityScale * other.gameObject.GetComponent<Rigidbody2D>().gravityScale > 0 && !otherObjs.Contains(other.gameObject) && !otherTM.GetIsAdded())
    //        {
    //            otherObjs.Add(other.gameObject);
    //            otherTM.SetIsAdded(true);
    //        }
    //    }
    //    catch
    //    {
    //        if (otherTM != null && (otherPosition.y - myPosition.y > 0) && noRb && !otherObjs.Contains(other.gameObject) && !otherTM.GetIsAdded()) // (myPosition.y <= otherPosition.y)
    //        {
    //            otherObjs.Add(other.gameObject);
    //            otherTM.SetIsAdded(true);
    //            //Debug.Log(other.gameObject.name + " is added : T");
    //        }
    //    }

    //    if (this.gameObject.name != "Player" && other.gameObject.CompareTag("Abyss"))
    //    {
    //        StartCoroutine(Respawn());
    //    }
    //}
    //private void OnTriggerExit2D(Collider2D other)
    //{
    //    if (otherObjs.Contains(other.gameObject))
    //    {
    //        if (addedObjs.ContainsKey(other.gameObject.name))
    //        {
    //            otherTM = other.gameObject.GetComponent<TotalMass>();
    //            totalMass -= otherTM.GetMass();
    //            addedObjs.Remove(other.gameObject.name);
    //            otherTM.SetIsAdded(false);
    //            //Debug.Log("Removed : " + totalMass);
    //            //Debug.Log(other.gameObject.name + " is removed");
    //        }
    //        otherObjs.Remove(other.gameObject);

    //    }
    //}
    public IEnumerator Respawn()
    {
        yield return null;
        transform.position = defaultPosition;
    }
}
