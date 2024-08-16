using System.Collections;
using System.Collections.Generic;
using UnityEngine;
using UnityEngine.UIElements;

public class PressureButton : MonoBehaviour
{
    //private GravityManager gravityManager;

    //[SerializeField] private GameObject target;
    //[SerializeField] private List<GameObject> targets = new();

    private Rigidbody2D targetRb;
    private TotalMass targetMass;
    private Vector2 offPosition;
    //[SerializeField] private float speed = 10f;
    private BoxCollider2D bc2D;
    private TotalMass totalMass;

    //[SerializeField] private GameObject button;
    [SerializeField] private float pressure = 0f;
    [SerializeField] private float onValue = 10f;
    private bool isPressed = false;

    void Awake()
    {
        offPosition = transform.position;
        //gravityManager = GameObject.Find("GravityManager").GetComponent<GravityManager>();

        bc2D = GetComponent<BoxCollider2D>();
        totalMass = GetComponent<TotalMass>();
    }
    void Update()
    {
        //if (targets != null)
        //{
        //pressure = 0f;
        //foreach (GameObject target in targets)
        //{
        //    targetRb = target.GetComponent<Rigidbody2D>();
        //    targetMass = target.GetComponent<TotalMass>();
        //    if (targetRb != null && targetMass != null)
        //    {
        //        pressure += targetMass.GetMass(); // * targetRb.gravityScale / gravityManager.GetDeFaultGravityScale();
        //    }
        //}

        //if (pressure >= onValue)
        //{
        //    bc2D.enabled = true;
        //    transform.position = offPosition + new Vector2(0, -0.49f);
        //    isPressed = true;
        //}
        //else
        //{
        //    transform.position = offPosition;
        //    bc2D.enabled = false;
        //    isPressed = false;
        //}

        //if (targetRb != null && targetMass != null && targetMass.GetMass() * targetRb.gravityScale / gravityManager.GetDeFaultGravityScale() >= onValue)
        //{
        //    //Vector2 toVector = Vector2.MoveTowards(offPosition, offPosition + new Vector2(0, -0.49f), speed * Time.deltaTime);
        //    //rb.MovePosition(toVector);
        //    //targetRb.MovePosition(toVector);
        //    //rb.MovePosition(offPosition + new Vector2(0, -0.49f));
        //    bc2D.enabled = true;
        //    transform.position = offPosition + new Vector2(0, -0.49f);
        //    //targetRb.velocity += new Vector2(0, -0.49f);

        //    isPressed = true;
        //    //transform.localScale = new Vector2(1f, 0.4f);
        //    Debug.Log(targetMass.GetMass() * targetRb.gravityScale / gravityManager.GetDeFaultGravityScale() + ": The button is pressed");
        //}
        //else
        //{
        //    transform.position = offPosition;
        //    bc2D.enabled = false;
        //    //rb.MovePosition(offPosition);
        //    //Vector2 toVector = Vector2.MoveTowards(offPosition + new Vector2(0, -0.49f), offPosition, speed * Time.deltaTime);
        //    //rb.MovePosition(toVector);
        //    isPressed = false;

        //    //transform.localScale = Vector2.one;
        //}
        //}
        pressure = totalMass.GetMass();
        if (!isPressed && pressure >= onValue)
        {
            bc2D.enabled = true;
            transform.position = offPosition + new Vector2(0, -0.49f);
            isPressed = true;
        }
        else if (isPressed && pressure < onValue)
        {
            transform.position = offPosition;
            bc2D.enabled = false;
            isPressed = false;
        }
    }
    public bool GetIsPressed()
    {
        return isPressed;
    }

    //private void OnCollisionEnter2D(Collision2D collision)
    //{
    //    if (!collision.gameObject.CompareTag("Stage") && !targets.Contains(collision.gameObject))
    //    {
    //        targets.Add(collision.gameObject);
    //    }
    //}

    //private void OnTriggerEnter2D(Collider2D collision)
    //{
    //    if (!collision.gameObject.CompareTag("Stage") && !targets.Contains(collision.gameObject))
    //    {
    //        //target = collision.gameObject;
    //        targets.Add(collision.gameObject);
    //        //targetRb = collision.gameObject.GetComponent<Rigidbody2D>();
    //        //if (targetRb.targetMass * targetRb.gravityScale >= onValue)
    //        //{
    //        //    button.transform.localScale = new Vector2(0.4f, 1f);
    //        //    Debug.Log("The button is pressed");
    //        //}
    //        //else
    //        //{
    //        //    button.transform.localScale = Vector2.one;
    //        //}
    //    }
    //}

    //private void OnTriggerExit2D(Collider2D collision)
    //{
    //    if (!collision.gameObject.CompareTag("Stage") && targets.Contains(collision.gameObject))
    //    {
    //        //target = null;
    //        //targetRb = null;
    //        //targetMass = null;
    //        targets.Remove(collision.gameObject);

    //        //transform.position = offPosition;
    //        //bc2D.enabled = false;
    //        //Vector2 toVector = Vector2.MoveTowards(offPosition + new Vector2(0, -0.49f), offPosition, speed * Time.deltaTime);
    //        //rb.MovePosition(toVector);
    //        //rb.MovePosition(offPosition);
    //        //isPressed = false;
    //        //transform.localScale = Vector2.one;
    //    }
    //}
    //private void OnTriggerEnter2D(Collider2D other)
    //{
    //    Debug.Log(other.gameObject.name + " : Enter");
    //    otherTM = other.gameObject.GetComponent<TotalMass>();
    //    otherPosition = other.transform.position;
    //    if (otherTM != null && ((otherPosition.y - myPosition.y) * Mathf.Sign(other.gameObject.GetComponent<Rigidbody2D>().gravityScale) > 0) && !otherObjs.Contains(other.gameObject) && !otherTM.GetIsAdded()) // (myPosition.y <= otherPosition.y)
    //    {
    //        otherObjs.Add(other.gameObject);
    //        otherTM.SetIsAdded(true);
    //    }

    //    if (this.gameObject.name != "Player" && other.gameObject.CompareTag("Abyss"))
    //    {
    //        StartCoroutine(Respawn());
    //    }
    //}
    //private void OnTriggerExit2D(Collider2D other)
    //{
    //    Debug.Log(other.gameObject.name + " : Exit");
    //    if (otherObjs.Contains(other.gameObject))
    //    {
    //        if (addedObjs.ContainsKey(other.gameObject.name))
    //        {
    //            otherTM = other.gameObject.GetComponent<TotalMass>();
    //            totalMass -= otherTM.GetMass();
    //            addedObjs.Remove(other.gameObject.name);
    //            otherTM.SetIsAdded(false);
    //            Debug.Log("Removed : " + totalMass);
    //        }
    //        otherObjs.Remove(other.gameObject);

    //    }
    //}
}
