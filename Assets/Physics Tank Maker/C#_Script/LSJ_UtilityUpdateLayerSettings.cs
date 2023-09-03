using System.Collections;
using System.Collections.Generic;
using UnityEngine;


public class LSJ_UtilityUpdateLayerSettings : MonoBehaviour
{

    [ContextMenu("Update_Layer_Settings")]


    void Update_Layer_Settings()
    {

        // MainBody.
        var bodyObject = GetComponentInChildren<Rigidbody>().gameObject;
        bodyObject.layer = LSJ_LayerSettings.Body_Layer;


        // Road Wheels.
        var createRoadWheelScripts = GetComponentsInChildren<LSJ_CreateRoadWheel>();
        foreach (var createRoadWheelScript in createRoadWheelScripts)
        {
            var children = createRoadWheelScript.GetComponentsInChildren<Transform>();
            foreach (var child in children)
            {
                if (child.GetComponent<LSJ_DriveWheel>())
                { // The child is a wheel.
                    child.gameObject.layer = LSJ_LayerSettings.Wheels_Layer;
                }
                else
                { // The child is a suspension.
                    child.gameObject.layer = LSJ_LayerSettings.Reinforce_Layer;
                }
            }

        }


        // Road Wheels Type89.
        var createRoadWheelType89Scripts = GetComponentsInChildren<LSJ_CreateRoadWheelType89>();
        foreach (var createRoadWheelType89Script in createRoadWheelType89Scripts)
        {
            var children = createRoadWheelType89Script.GetComponentsInChildren<Transform>();
            foreach (var child in children)
            {
                if (child.GetComponent<LSJ_DriveWheel>())
                { // The child is a wheel.
                    child.gameObject.layer = LSJ_LayerSettings.Wheels_Layer;
                }
                else
                { // The child is a suspension.
                    child.gameObject.layer = LSJ_LayerSettings.Reinforce_Layer;
                }
            }

        }


        // Sprocket Wheels.
        var createSprocketWheelScripts = GetComponentsInChildren<LSJ_CreateSprocketWheel>();
        foreach (var createSprocketWheelScript in createSprocketWheelScripts)
        {
            var children = createSprocketWheelScript.GetComponentsInChildren<Transform>();
            foreach (var child in children)
            {
                if (child.GetComponent<LSJ_DriveWheel>() || child.GetComponent<LSJ_StaticWheel>())
                { // The child is a wheel.
                    child.gameObject.layer = LSJ_LayerSettings.Wheels_Layer;
                } // The child is a tensioner arm. >> Default layer.
            }
        }


        // Idler Wheels.
        var createIdlerWheelScripts = GetComponentsInChildren<LSJ_CreateIdlerWheel>();
        foreach (var createIdlerWheelScript in createIdlerWheelScripts)
        {
            var children = createIdlerWheelScript.GetComponentsInChildren<Transform>();
            foreach (var child in children)
            {
                if (child.GetComponent<LSJ_DriveWheel>() || child.GetComponent<LSJ_StaticWheel>())
                { // The child is a wheel.
                    child.gameObject.layer = LSJ_LayerSettings.Wheels_Layer;
                } // The child is a tensioner arm. >> Default layer.
            }
        }


        // Support Wheels.
        var createSupportWheelScripts = GetComponentsInChildren<LSJ_CreateSupportWheel>();
        foreach (var createSupportWheelScript in createSupportWheelScripts)
        {
            var children = createSupportWheelScript.GetComponentsInChildren<Transform>();
            foreach (var child in children)
            {
                // All the children are wheels.
                child.gameObject.layer = LSJ_LayerSettings.Wheels_Layer;
            }

        }


        // Swing Balls.
        var createSwingBallScripts = GetComponentsInChildren<LSJ_CreateSwingBall>();
        foreach (var createSwingBallScript in createSwingBallScripts)
        {
            var children = createSwingBallScript.GetComponentsInChildren<Transform>();
            foreach (var child in children)
            {
                if (child.gameObject.layer == 2)
                { // The child's layer is set to 2 (Ignore Raycast).
                  // No need to change the layer.
                    continue;
                } // The child's layer should be set to the old Reinforce layer.
                child.gameObject.layer = LSJ_LayerSettings.Reinforce_Layer;
            }
        }


        // Reinforce objects in Physics Track Pieces.
        var createTrackBeltScripts = GetComponentsInChildren<LSJ_CreateTrackBelt>();
        foreach (var createTrackBeltScript in createTrackBeltScripts)
        {
            // (Note.)The reinforce object must have a SphereCollider.
            var sphereColliders = createTrackBeltScript.GetComponentsInChildren<SphereCollider>();
            foreach (var sphereCollider in sphereColliders)
            {
                sphereCollider.gameObject.layer = LSJ_LayerSettings.Reinforce_Layer;
            }
        }


        /* (Note.)
         *  The layers of "Armor_Collider", "Extra_Collider" and Bullets are set automatically at the start by attached scripts.
        */

    }
}
