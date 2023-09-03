using System.Collections;
using System.Collections.Generic;
using UnityEngine;


public class LSJ_EventTrigger0202DestroyANDNecessary : LSJ_EventTrigger0200DestroyBase
{

    public override void Check_Trigger()
    {
        if (allTriggersAreReady == false)
        { // All the trigger tanks have not been spawned yet.
            Observe_Trigger_Tanks();
        }

        if (spawnedTankCount >= eventControllerScript.Necessary_Num)
        { // The necessary number of trigger tanks have been spawned. 
            if (Check_Destroy_AND_Necessary(eventControllerScript))
            { // The necessary number of trigger tanks have been destroyed.

                // Remove this reference in the "Event_Controller_CS".
                eventControllerScript.Trigger_Script = null; // (Note.) The "Trigger_Script" must be set to null before starting the event.
                Destroy(this);

                // Start the event.
                eventControllerScript.Start_Event();
            }
        }
    }


    bool Check_Destroy_AND_Necessary(LSJ_EventController eventControllerScript)
    { // Check the necessary number of trigger tanks are destroyed.
        int destroyedCount = 0;
        for (int i = 0; i < triggerTanksList.Count; i++)
        {
            if (triggerTanksList[i].Root_Transform == null)
            { // The tank has already been removed from the scene.
                destroyedCount += 1;
                continue;
            }
            if (triggerTanksList[i].Root_Transform.tag == "Finish")
            { // The tank has been destroyed.
                destroyedCount += 1;
                if (triggerTanksList[i].Respawn_Script && triggerTanksList[i].Respawn_Script.Respawn_Times > 0)
                { // The "Respawn_Times" remains.
                    destroyedCount -= 1;
                }
            }
        }
        return (destroyedCount >= eventControllerScript.Necessary_Num); // The necessary number of trigger tanks have been destroyed or removed, or not.
    }

}

