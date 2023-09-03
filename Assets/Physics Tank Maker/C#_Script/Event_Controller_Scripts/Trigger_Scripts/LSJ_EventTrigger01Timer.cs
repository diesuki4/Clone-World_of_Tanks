using System.Collections;
using UnityEngine;


public class LSJ_EventTrigger01Timer : LSJ_EventTrigger00Base
{

    float triggerCurrentTime;


    public override void Check_Trigger()
    {
        // Count time.
        triggerCurrentTime += Time.deltaTime;
        if (triggerCurrentTime >= eventControllerScript.Trigger_Time)
        { // The time is over.
          // Remove this reference in the "Event_Controller_CS".
            eventControllerScript.Trigger_Script = null; // (Note.) The "Trigger_Script" must be set to null before starting the event.

            // Remove this script.
            Destroy(this);

            // Start the event.
            eventControllerScript.Start_Event();
        }
    }

}

