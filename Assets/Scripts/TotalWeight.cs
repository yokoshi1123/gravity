using System.Collections;
using System.Collections.Generic;
using UnityEngine;
using UnityEngine.UIElements;

[RequireComponent(typeof(Rigidbody2D))]
public class TotalWeight : MonoBehaviour
{
    [SerializeField]
    protected List<GameObject> otherObjs = new();
    private readonly Dictionary<string, float> addedObjs = new();

    private Rigidbody2D rb;
    
    [SerializeField] private float totalWeight;
    private float oldWeight;

    [SerializeField] private bool isAdded = false;

    private Vector2 defaultPosition;

    // Start is called before the first frame update
    void Start()
    {
        defaultPosition = transform.localPosition;
        rb = GetComponent<Rigidbody2D>();
        oldWeight = rb.mass * rb.gravityScale;
        totalWeight = oldWeight;;
    }

    // Update is called once per frame
    void Update()
    {
        if (oldWeight != rb.mass * rb.gravityScale)
        {
            totalWeight += rb.mass * rb.gravityScale - oldWeight;
            oldWeight = rb.mass * rb.gravityScale;
        }

        if (otherObjs.Count > 0)
        {
            foreach (GameObject otherObj in otherObjs)
            {
                TotalWeight otherTW = otherObj.GetComponent<TotalWeight>();
                if (!addedObjs.ContainsKey(otherObj.name) && !otherTW.GetIsAdded())
                {
                    otherTW.SetIsAdded(true);
                    totalWeight += otherTW.GetTWeight();
                    addedObjs.Add(otherObj.name, otherTW.GetTWeight());
                }
                else if (!addedObjs.ContainsKey(otherObj.name) && otherTW.GetIsAdded())
                {
                    addedObjs.Remove(otherObj.name);
                }
                else if (addedObjs.ContainsKey(otherObj.name) && otherTW.GetTWeight() != addedObjs[otherObj.name])
                {
                    totalWeight += otherTW.GetTWeight() - addedObjs[otherObj.name];
                    addedObjs[otherObj.name] = otherTW.GetTWeight();
                }
            }
        }
        else
        {
            totalWeight = oldWeight;
        }
    }
    public bool GetIsAdded()
    {
        return isAdded;
    }
    public void SetIsAdded(bool value)
    {
        isAdded = value;
    }
    public float GetTWeight()
    {
        return totalWeight;
    }
    public void SetTWeight(float value)
    {
        totalWeight = value;
    }

    private void OnTriggerEnter2D(Collider2D other)
    {
        TotalWeight otherTW = other.gameObject.GetComponent<TotalWeight>();

        if (otherTW != null && (other.transform.position.y - transform.position.y) * other.gameObject.GetComponent<Rigidbody2D>().gravityScale > 0 && !otherObjs.Contains(other.gameObject))
        {
            otherObjs.Add(other.gameObject);
        }

        if (!this.gameObject.name.Contains("Player") && other.gameObject.CompareTag("Abyss"))
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
                TotalWeight otherTW = other.gameObject.GetComponent<TotalWeight>();
                totalWeight -= otherTW.GetTWeight();
                addedObjs.Remove(other.gameObject.name);
                otherTW.SetIsAdded(false);
            }
            otherObjs.Remove(other.gameObject);
        }
    }
    public IEnumerator Respawn()
    {
        yield return null;
        transform.position = defaultPosition;
        rb.velocity = Vector2.zero;
    }
}
