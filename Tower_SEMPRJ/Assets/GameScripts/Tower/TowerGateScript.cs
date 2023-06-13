using System;
using System.Collections;
using System.Collections.Generic;
using UnityEngine;

public class TowerGateScript : MonoBehaviour
{
    public bool isTriggered;

    private void Update()
    {
        Debug.Log($"Triggered = {isTriggered}");
    }

    private void OnTriggerEnter(Collider collidedWith)
    {
        isTriggered = true;
        Debug.Log("Triggered");
    }

    private void OnTriggerExit(Collider collidedWith)
    {
        isTriggered = false;
        Debug.Log("Triggered = false");
    }
}
