using System.Collections;
using System.Collections.Generic;
using UnityEngine;
using UnityEngine.Tilemaps;

public class HiddenRoute : MonoBehaviour
{
    private Tilemap hiddenRoute;

    private void Awake()
    {
        hiddenRoute = GetComponent<Tilemap>();    
    }

    private void OnTriggerStay2D(Collider2D collision)
    {
        if (collision.transform.parent.CompareTag("Player"))
        {
            hiddenRoute.color = new Color(0.5f, 0.5f, 0.5f, 1f);
            transform.position += new Vector3(0, 0, -transform.position.z + 0.2f);
        }
    }

    private void OnTriggerExit2D(Collider2D collision)
    {
        if (collision.transform.parent.CompareTag("Player"))
        {
            hiddenRoute.color = Color.white;
            transform.position += new Vector3(0, 0, -transform.position.z + 0.1f);
        }
    }

}
