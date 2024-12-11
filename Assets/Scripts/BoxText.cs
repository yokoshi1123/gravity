using System.Collections;
using System.Collections.Generic;
using UnityEngine;
using TMPro;


public class BoxText : MonoBehaviour
{
    [SerializeField] private TextMeshProUGUI weightText;
    //private GravityObserver gravityObserver;
    private float objectMass;



    // Start is called before the first frame update
    void Start()
    {
        objectMass = GetComponent<GravityObserver>().GetMass();
        weightText.text = objectMass.ToString();
    }

    // Update is called once per frame
    //void Update()
    //{

    //}
}
