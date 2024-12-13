using System.Collections;
using System.Collections.Generic;
using UnityEngine;

public class Singleton<T> : MonoBehaviour where T : Singleton<T>
{
    protected static T instance;
    public static T Instance
    {
        get
        {
            if (instance == null)
            {
                instance = (T)FindObjectOfType(typeof(T));

                if (instance == null) Debug.LogWarning(typeof(T) + "is nothing");
            }

            return instance;
        }
    }
    
    protected void Awake()
    {
        CheckInstance();
    }

    protected bool CheckInstance()
    {
        if (instance == null)
        {
            instance = (T)this;
            //Debug.Log("New one set");
            return true;
        }
        else if (Instance == this)
        {
            //Debug.Log("Only this");
            return true;
        }

        //Debug.Log("Other already exists");
        Destroy(this.gameObject);
        return false;
    }
}
