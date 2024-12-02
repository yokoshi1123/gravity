using System.Collections;
using System.Collections.Generic;
using UnityEngine;

[System.Serializable]
public class SaveData
{
    public string stageName;
    public int respawnIndex = 0;
    public int totalProgress = 0;
    public bool isAvailable = false;
}
