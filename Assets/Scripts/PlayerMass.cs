using System.Collections;
using System.Collections.Generic;
using UnityEngine;
using UnityEngine.UIElements;

public class PlayerMass : TotalMass
{
    private void OnTriggerEnter2D(Collider2D other)
    {
        otherTM = other.gameObject.GetComponent<TotalMass>();
        otherPosition = other.transform.position;

        try
        {
            if (otherTM != null && ((otherPosition.y - myPosition.y) * Mathf.Sign(GetComponent<Rigidbody2D>().gravityScale) > (transform.localScale.y + other.gameObject.transform.localScale.y) / 2.0f) && GetComponent<Rigidbody2D>().gravityScale * other.gameObject.GetComponent<Rigidbody2D>().gravityScale > 0 && !otherObjs.Contains(other.gameObject) && (this.gameObject.name == "Player" || !otherTM.GetIsAdded()))
            {
                //if (this.gameObject.name == "Player" && !GetComponent<PlayerController>().GetIsGrabbing())
                //{ 
                //    return;
                //}
                otherObjs.Add(other.gameObject);
                otherTM.SetIsAdded(true);
                //Debug.Log(this.gameObject.name + " : " + other.gameObject.name + " added : Try");
            }
        }
        catch
        {
            if (otherTM != null && (otherPosition.y - myPosition.y > 0) && !otherObjs.Contains(other.gameObject) && !otherTM.GetIsAdded()) // (myPosition.y <= otherPosition.y)
            {
                otherObjs.Add(other.gameObject);
                otherTM.SetIsAdded(true);
                //Debug.Log(this.gameObject.name + " : " + other.gameObject.name + " added : Catch");
            }
        }
    }
}
