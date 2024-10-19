using System.Collections;
using System.Collections.Generic;
using UnityEngine;

public class GravityFieldCollision : MonoBehaviour
{    
    private void OnTriggerStay2D(Collider2D collision)
    {
        //Debug.Log(collision.name + " : Stay");
        if (collision.CompareTag("Player")/*gameObject.name.Contains("Player")*/) // || collision.name == "GrabPoint")
        {
            //Debug.Log(collision.name + " : Stay");
            //collision.gameObject.GetComponent<PlayerController>().SetIsPlayer(true);
        }
    }
    private void OnTriggerExit2D(Collider2D collision)
    {
        if (collision.CompareTag("Player")/*.gameObject.name.Contains("Player")*/) // || collision.name == "GrabPoint")
        {
            //Debug.Log(collision.name + " : Exit");
            //collision.gameObject.GetComponent<PlayerController>().SetIsPlayer(false);
        }
    }
}
