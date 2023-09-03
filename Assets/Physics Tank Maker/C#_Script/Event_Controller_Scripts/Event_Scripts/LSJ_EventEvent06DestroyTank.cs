using System.Collections;
using UnityEngine;


public class LSJ_EventEvent06DestroyTank : LSJ_EventEvent00Base
{

    public override void Execute_Event()
    {
        for (int i = 0; i < eventControllerScript.Target_Tanks.Length; i++)
        {
            // Get the "Damage_Control_Center_CS" in the target, and send it infinite damage values.
            var damageScript = eventControllerScript.Target_Tanks[i].GetComponentInChildren<LSJ_DamageControlCenter>();
            if (damageScript)
            {
                damageScript.Receive_Damage(Mathf.Infinity, 0, 0);
            }
        }

        // End the event.
        // (Note.) This event can be repeatedly executed.
        if (eventControllerScript.Trigger_Script == null)
        { // All the triggers are pulled.
            Destroy(this.gameObject);
        }
    }

}

