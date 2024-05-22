using System.Collections;
using System.Collections.Generic;
using UnityEngine;

public class BackGroundController : MonoBehaviour
{
    [SerializeField] float yMin;
    [SerializeField] float yMax;
    [SerializeField] private Transform player;

    void Start()
    {
        transform.position = new Vector2(player.position.x, Mathf.Max(6.0f, player.position.y + 1.0f));
        transform.position = new Vector2(transform.position.x, Mathf.Min(10.0f, transform.position.y));
    }


    void Update()
    {
        transform.position = new Vector2(player.position.x, Mathf.Max(6.0f, player.position.y + 1.0f));
        transform.position = new Vector2(transform.position.x, Mathf.Min(10.0f, transform.position.y));
    }
}