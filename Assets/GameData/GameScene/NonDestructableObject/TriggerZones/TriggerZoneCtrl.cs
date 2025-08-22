using System.Collections;
using System.Collections.Generic;
using UnityEngine;

public class TriggerZoneCtrl : CoreMonoBehaviour
{
    [SerializeField] public bool isTriggered = false;
    [SerializeField] public bool canTriggerMultipleTimes = false;
    

    protected void OnTriggerExit2D(Collider2D other)
    {
        if (!this.canTriggerMultipleTimes) return;
        CharController charController = other.GetComponent<CharController>();
        if (charController == null) return;
        this.isTriggered = false;
    }
}