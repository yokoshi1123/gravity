using System.Collections;
using System.Collections.Generic;
using UnityEngine;

public class GravityFieldCollision : MonoBehaviour
{    
    private void OnTriggerStay2D(Collider2D collision)
    {
        if (collision.name == "Player") // || collision.name == "GrabPoint")
        {
            //Debug.Log(collision.name + " : Stay");
            collision.gameObject.GetComponent<PlayerController>().SetIsPlayer(true);
        }
    }
    private void OnTriggerExit2D(Collider2D collision)
    {
        if (collision.name == "Player") // || collision.name == "GrabPoint")
        {
            //Debug.Log(collision.name + " : Exit");
            collision.gameObject.GetComponent<PlayerController>().SetIsPlayer(false);
        }
    }
}
