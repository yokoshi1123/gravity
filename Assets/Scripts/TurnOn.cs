using System.Collections;
using System.Collections.Generic;
using UnityEngine;

public class TurnOn : MonoBehaviour
{
    [SerializeField] private bool turnOn;
    [SerializeField] private bool not = false;
    public bool GetTurnOn()
    {
        return (not) ? !turnOn : turnOn;
    }

    public void SetTurnOn(bool value)
    {
        turnOn = value;
    }
}
