using System.Collections;
using System.Collections.Generic;
using UnityEngine;


public class Trigger_Tank_Information
{
    public Transform Root_Transform;
    public LSJ_RespawnController Respawn_Script;
    public bool Is_Registered;
}


public class LSJ_EventTrigger0200DestroyBase : LSJ_EventTrigger00Base
{

    protected List<Trigger_Tank_Information> triggerTanksList = new List<Trigger_Tank_Information>();
    protected bool allTriggersAreReady;
    protected int spawnedTankCount;


    public override void Prepare_Trigger(LSJ_EventController eventControllerScript)
    {
        // Store the reference to "Event_Controller_CS".
        this.eventControllerScript = eventControllerScript;

        // Create "triggerTanksList".
        for (int i = 0; i < eventControllerScript.Trigger_Tanks.Length; i++)
        {
            if (eventControllerScript.Trigger_Tanks[i] == null)
            {
                continue;
            }
            var triggerTankInformation = new Trigger_Tank_Information();
            triggerTankInformation.Root_Transform = eventControllerScript.Trigger_Tanks[i];
            triggerTanksList.Add(triggerTankInformation);
        }
    }


    public virtual void Observe_Trigger_Tanks()
    { // Check the trigger tanks have been spawned or not.
        for (int i = 0; i < triggerTanksList.Count; i++)
        {
            if (triggerTanksList[i].Is_Registered)
            { // The tank has already been registered.
                continue;
            }

            // Check the trigger tank is exist.
            if (triggerTanksList[i].Root_Transform == null)
            { // The tank should have been removed from the scene.
                triggerTanksList[i].Is_Registered = true;
                spawnedTankCount += 1;
                continue;
            }

            // Get components in the spawned tank.
            var respawnScript = triggerTanksList[i].Root_Transform.GetComponentInChildren<LSJ_RespawnController>();
            if (respawnScript)
            {
                triggerTanksList[i].Respawn_Script = respawnScript;
                triggerTanksList[i].Is_Registered = true;
                spawnedTankCount += 1;
            }
        }

        // Set flags.
        allTriggersAreReady = (spawnedTankCount == triggerTanksList.Count); // All the trigger tanks have been spawned or not.
    }

}

