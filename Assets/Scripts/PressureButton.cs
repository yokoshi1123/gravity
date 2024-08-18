using System.Collections;
using System.Collections.Generic;
using UnityEngine;
using UnityEngine.UIElements;

public class PressureButton : MonoBehaviour
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

    private float DEFAULTMASS = 0f;
    private float oldMass;

    private Vector2 defaultPosition;
    private BoxCollider2D bc2D;

    [SerializeField] private TurnOn to;

    private void Awake()
    {
        myPosition = transform.position;

        defaultPosition = transform.position;

        bc2D = GetComponent<BoxCollider2D>();

        totalMass = DEFAULTMASS;
        oldMass = DEFAULTMASS;
    }
    private void Update()
    {
        myPosition = transform.position;

        foreach (GameObject other in otherObjs)
        {
            otherTM = other.GetComponent<TotalMass>();
            if (!addedObjs.ContainsKey(other.name))
            {
                totalMass += otherTM.GetMass();
                addedObjs.Add(other.name, otherTM.GetMass());

            }
            else if (addedObjs.ContainsKey(other.name) && otherTM.GetMass() != addedObjs[other.name])
            {
                totalMass += otherTM.GetMass() - addedObjs[other.name];
                addedObjs[other.name] = otherTM.GetMass();
            }
        }

        pressure = totalMass;
        if (!to.GetTurnOn()/*isPressed*/ && pressure >= onValue)
        {
            bc2D.enabled = true;
            //transform.position = defaultPosition + new Vector2(0, -0.49f);
            transform.Translate(new Vector2(0, -0.5f));
            to.SetTurnOn(true);
            //isPressed = true;
        }
        else if (to.GetTurnOn()/*isPressed*/ && pressure < onValue)
        {
            //transform.position = defaultPosition;
            transform.Translate(new Vector2(0, 0.5f));
            bc2D.enabled = false;
            to.SetTurnOn(false);
            //isPressed = false;
        }

        //foreach (var addedObj in addedObjs)
        //{
        //    Debug.Log(addedObj);
        //}
    }
    public float GetMass()
    {
        return totalMass;
    }
    public float GetDefaultMass()
    {
        return DEFAULTMASS;
    }

    private void OnCollisionStay2D(Collision2D other)
    {
        otherTM = other.gameObject.GetComponent<TotalMass>();
        otherPosition = other.transform.position;

        if (otherTM != null && (otherPosition.y - myPosition.y > 0) && !otherObjs.Contains(other.gameObject) && !otherTM.GetIsAdded()) // (myPosition.y <= otherPosition.y)
        {
            if (other.gameObject.name == "Player" && other.gameObject.GetComponent<PlayerController>().GetIsGrabbing())
            {
                Debug.Log("Grabbing");
                return;
            }
            otherObjs.Add(other.gameObject);
            //otherTM.SetIsAdded(true);
            //Debug.Log(other.gameObject.name + " is added : C");
        }
    }

    private void OnTriggerEnter2D(Collider2D other)
    {
        otherTM = other.gameObject.GetComponent<TotalMass>();
        otherPosition = other.transform.position;

        if (otherTM != null && (otherPosition.y - myPosition.y > 0) && !otherObjs.Contains(other.gameObject) && !otherTM.GetIsAdded()) // (myPosition.y <= otherPosition.y)
        {
            if (other.gameObject.name == "Player" && other.gameObject.GetComponent<PlayerController>().GetIsGrabbing())
            {
                Debug.Log("Grabbing");
                return;
            }
            otherObjs.Add(other.gameObject);
            //otherTM.SetIsAdded(true);
            //Debug.Log(other.gameObject.name + " is added : T");
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
                //otherTM.SetIsAdded(false);
                //Debug.Log(other.gameObject.name + " is removed");
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

    //private Vector2 defaultPosition;
    //private BoxCollider2D bc2D;
    //private TotalMass totalMass;

    [SerializeField] private float pressure = 0f;
    [SerializeField] private float onValue = 10f;
    private bool isPressed = false;

    //void Awake()
    //{
    //    defaultPosition = transform.position;

    //    bc2D = GetComponent<BoxCollider2D>();
    //    totalMass = GetComponent<TotalMass>();
    //}
    //void Update()
    //{
    //    pressure = totalMass.GetMass();
    //    if (!isPressed && pressure >= onValue)
    //    {
    //        bc2D.enabled = true;
    //        transform.position = defaultPosition + new Vector2(0, -0.49f);
    //        isPressed = true;
    //    }
    //    else if (isPressed && pressure < onValue)
    //    {
    //        transform.position = defaultPosition;
    //        bc2D.enabled = false;
    //        isPressed = false;
    //    }
    //}
    public bool GetIsPressed()
    {
        return isPressed;
    }
}
