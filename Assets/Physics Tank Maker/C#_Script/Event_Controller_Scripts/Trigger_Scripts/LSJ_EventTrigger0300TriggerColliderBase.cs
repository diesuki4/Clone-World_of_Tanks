using System.Collections;
using System.Collections.Generic;
using UnityEngine;


public class LSJ_EventTrigger0300TriggerColliderBase : LSJ_EventTrigger00Base
{

    public override void Prepare_Trigger(LSJ_EventController eventControllerScript)
    {
        // Store the reference to "Event_Controller_CS".
        this.eventControllerScript = eventControllerScript;

        // Send this reference to all the "Trigger_Collider_CS" in the "Trigger_Collider_Scripts".
        for (int i = 0; i < eventControllerScript.Trigger_Collider_Scripts.Length; i++)
        {
            if (eventControllerScript.Trigger_Collider_Scripts[i])
            {
                eventControllerScript.Trigger_Collider_Scripts[i].Get_Trigger_Script(this);
            }
        }
    }


    public virtual void Detect_Collider(Transform detectedTransform)
    { // Called from "Trigger_Collider_CS".
    }

}

