using System.Collections;
using System.Collections.Generic;
using UnityEngine;
using TMPro;

public class WeightButton : MonoBehaviour
{
    private SpriteRenderer buttonRenderer;
    
    [SerializeField] private List<GameObject> otherObjs = new();
    private readonly Dictionary<string, float> addedObjs = new();

    private TotalWeight otherTW;

    [SerializeField]
    private float totalWeight = 0f;

    private const float DEFAULTWEIGHT = 0f;

    //private BoxCollider2D bc2D;

    [SerializeField] private TurnOn to;

    //[SerializeField] private float weight = 0f;
    [SerializeField] private float onValue = 10f;

    [SerializeField] private TextMeshProUGUI weightText;
    [SerializeField] private TextMeshProUGUI onValueText;

    [SerializeField] private Sprite offSprite;
    [SerializeField] private Sprite onSprite;

    // Start is called before the first frame update
    void Start()
    {
        totalWeight = DEFAULTWEIGHT;

        buttonRenderer = GetComponent<SpriteRenderer>();
    }

    // Update is called once per frame
    void Update()
    {
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
            totalWeight = 0f;
        }

        weightText.text = totalWeight.ToString();
        onValueText.text = onValue.ToString();
        if (totalWeight >= onValue)
        {
            to.SetTurnOn(true);
            buttonRenderer.sprite = onSprite;
        }
        else
        {
            to.SetTurnOn(false);
            buttonRenderer.sprite = offSprite;
        }
    }
    private void OnTriggerEnter2D(Collider2D other)
    {
        otherTW = other.gameObject.GetComponent<TotalWeight>();

        if (otherTW != null && !otherObjs.Contains(other.gameObject))
        {
            otherObjs.Add(other.gameObject);
        }
    }

    private void OnTriggerExit2D(Collider2D other)
    {
        if (otherObjs.Contains(other.gameObject))
        {
            if (addedObjs.ContainsKey(other.gameObject.name))
            {
                otherTW = other.gameObject.GetComponent<TotalWeight>();
                totalWeight -= otherTW.GetTWeight();
                addedObjs.Remove(other.gameObject.name);
                otherTW.SetIsAdded(false);
                //Debug.Log(other.gameObject.name + " is removed");
            }
            otherObjs.Remove(other.gameObject);

        }
    }
}
