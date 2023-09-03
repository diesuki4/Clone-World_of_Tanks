using System.Collections;
using UnityEngine;


public class LSJ_EventEvent07ShowResultCanvas : LSJ_EventEvent00Base
{

    public override void Prepare_Event(LSJ_EventController eventControllerScript)
    {
        // Store the reference to "Event_Controller_CS".
        this.eventControllerScript = eventControllerScript;

        // Check the "Result_Canvas".
        if (eventControllerScript.Result_Canvas == null)
        {
            Debug.LogWarning("The event (" + this.name + ") cannot be executed, because the 'Result Canvas' is not assigned.");
            Destroy(eventControllerScript);
            Destroy(this);
            return;
        }

        // Disable the result canvas.
        eventControllerScript.Result_Canvas.enabled = false;
    }


    public override void Execute_Event()
    {
        // Call the "Game_Controller_CS" in the scene to disallow the pause.
        if (LSJ_GameController.Instance)
        {
            LSJ_GameController.Instance.Allow_Pause = false;
        }

        // Enable the result canvas.
        eventControllerScript.Result_Canvas.enabled = true;

        // Show cursor.
        Cursor.lockState = CursorLockMode.None;
        Cursor.visible = true;

        // End the event.
        Destroy(this.gameObject);
    }

}


