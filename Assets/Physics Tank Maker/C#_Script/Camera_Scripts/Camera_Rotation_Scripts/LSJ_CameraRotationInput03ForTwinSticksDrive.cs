using System.Collections;
using UnityEngine;

public class LSJ_CameraRotationInput03ForTwinSticksDrive : LSJ_CameraRotationInput02ForSingleStickDrive
{

    public override void Get_Input()
    {
        // Check the main camera is enabled.
        if (rotationScript.Main_Camera.enabled == false)
        {
            // Do not rotate.
            rotationScript.Horizontal_Input = 0.0f;
            rotationScript.Vertical_Input = 0.0f;
            return;
        }

        // Look forward.
        if (Input.GetKeyDown(LSJ_GeneralSettings.Camera_Look_Forward_Pad_Button))
        {
            rotationScript.Look_At_Target(bodyTransform.position + bodyTransform.forward * 64.0f);
        }

        // Rotation.
        if (Input.GetButton("Bumper L") == false)
        {
            multiplier = Mathf.Lerp(0.1f, 2.0f, rotationScript.Main_Camera.fieldOfView / 15.0f); // Change the rotation speed according to the FOV of the main camera.
            rotationScript.Horizontal_Input = Input.GetAxis("Horizontal") * multiplier;
            rotationScript.Vertical_Input = Input.GetAxis("Vertical2") * multiplier;
        }
        else
        {
            rotationScript.Horizontal_Input = 0.0f;
            rotationScript.Vertical_Input = 0.0f;
        }
    }

}

