using System.Collections;
using System.Collections.Generic;
using UnityEngine;

public class BGMManager : Singleton<BGMManager>
{
    public bool DontDestroyEnabled = true;

    // Use this for initialization
    void Start()
    {
        if (DontDestroyEnabled)
        {
            // Sceneを遷移してもオブジェクトが消えないようにする
            DontDestroyOnLoad(this);
        }
    }
}
