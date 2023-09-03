using System.Collections;
using UnityEngine;

public class LSJ_EventEvent00Base : MonoBehaviour
{

    protected LSJ_EventController eventControllerScript;


    public virtual void Prepare_Event(LSJ_EventController eventControllerScript)
    {
        // Store the reference to "Event_Controller_CS".
        this.eventControllerScript = eventControllerScript;
    }


    public virtual void Execute_Event()
    {
    }

}
