using System.Collections;
using System.Collections.Generic;
using UnityEngine;

public class TurnOn : MonoBehaviour
{
    [SerializeField] private bool turnOn;
    public bool GetTurnOn()
    {
        return turnOn;
    }

    public void SetTurnOn(bool value)
    {
        turnOn = value;
    }
}
