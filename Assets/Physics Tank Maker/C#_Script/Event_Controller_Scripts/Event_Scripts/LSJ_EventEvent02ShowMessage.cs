using System.Collections;
using UnityEngine;


public class LSJ_EventEvent02ShowMessage : LSJ_EventEvent00Base
{

    public override void Prepare_Event(LSJ_EventController eventControllerScript)
    {
        // Store the reference to "Event_Controller_CS".
        this.eventControllerScript = eventControllerScript;

        if (eventControllerScript.Event_Text == null)
        {
            Debug.LogWarning("The event (" + this.name + ")  cannot be executed, because the 'Text' is not assigned.");
            Destroy(eventControllerScript);
            Destroy(this);
        }
    }


    public override void Execute_Event()
    {
        // Send message to "UI_Text_Control_CS".
        var textScript = eventControllerScript.Event_Text.GetComponent<LSJ_UITextControl>();
        if (textScript == null)
        {
            textScript = eventControllerScript.Event_Text.gameObject.AddComponent<LSJ_UITextControl>();
        }
        textScript.Receive_Text(eventControllerScript.Event_Message, eventControllerScript.Event_Message_Color, eventControllerScript.Event_Message_Time);

        // End the event.
        // (Note.) This event can be repeatedly executed.
        if (eventControllerScript.Trigger_Script == null)
        { // All the triggers are pulled.
            Destroy(this.gameObject);
        }
    }

}

