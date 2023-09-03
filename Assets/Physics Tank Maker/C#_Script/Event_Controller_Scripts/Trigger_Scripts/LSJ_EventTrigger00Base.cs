using System.Collections;
using UnityEngine;


public class LSJ_EventTrigger00Base : MonoBehaviour
{

    protected LSJ_EventController eventControllerScript;


    public virtual void Prepare_Trigger(LSJ_EventController eventControllerScript)
    {
        // Store the reference to "Event_Controller_CS".
        this.eventControllerScript = eventControllerScript;
    }


    public virtual void Check_Trigger()
    {
    }

}

