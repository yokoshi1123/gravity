using System.Collections;
using System.Collections.Generic;
using UnityEngine;

public class BackGroundController : MonoBehaviour
{
    [SerializeField] float xMin = 11f;
    [SerializeField] float xMax = 133f;
    [SerializeField] float yMin = 6f;
    [SerializeField] float yMax = 10f;
    [SerializeField] private Transform player;

    void Start()
    {
        //Debug.Log(xMin + "," + xMax +"," + yMin + "," +yMax);
        transform.position = new Vector2(Mathf.Max(xMin, player.position.x), Mathf.Max(yMin, player.position.y + 2.0f));
        transform.position = new Vector2(Mathf.Min(xMax, transform.position.x), Mathf.Min(yMax, transform.position.y));
    }


    void Update()
    {
        transform.position = new Vector2(Mathf.Max(xMin, player.position.x), Mathf.Max(yMin, player.position.y + 2.0f));
        transform.position = new Vector2(Mathf.Min(xMax, transform.position.x), Mathf.Min(yMax, transform.position.y));
    }
}