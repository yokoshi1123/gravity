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
            // Scene��J�ڂ��Ă��I�u�W�F�N�g�������Ȃ��悤�ɂ���
            DontDestroyOnLoad(this);
        }
    }
}
