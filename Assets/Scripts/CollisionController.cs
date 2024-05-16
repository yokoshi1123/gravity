using System.Collections;
using System.Collections.Generic;
using UnityEngine;

[RequireComponent(typeof(BoxCollider2D))]
public class CollisionController : MonoBehaviour
{
    private GameObject root;
    [SerializeField] private string pName;
    [SerializeField] private Rigidbody2D rb;

    void Awake()
    {
        root = transform.root.gameObject;
    }
    
    void Update()
    {
       root = transform.root.gameObject;
    }

    private void OnTriggerStay2D(Collider2D collision)
    {
        if (root.name != pName && collision.CompareTag("Stage"))
        {
            rb.isKinematic = false;
            //Debug.Log("Collision");
        }
    }

    private void OnTriggerExit2D(Collider2D collision)
    {
        if (root.name != pName && collision.CompareTag("Stage"))
        {
            rb.isKinematic = true;
            //Debug.Log("N");
        }
    }
}
