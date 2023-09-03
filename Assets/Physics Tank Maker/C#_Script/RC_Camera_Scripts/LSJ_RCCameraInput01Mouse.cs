using System.Collections;
using UnityEngine;



public class LSJ_RCCameraInput01Mouse : LSJ_RCCameraInput00Base
{

    public override void Prepare(LSJ_RCCamera rcCameraScript)
    {
        this.rcCameraScript = rcCameraScript;

        rcCameraScript.Use_Analog_Stick = false;
    }


    public override void Get_Input()
    {
        // Switch
        if (Input.GetKeyDown(LSJ_GeneralSettings.RC_Camera_Switch_Key))
        {
            rcCameraScript.Switch_RC_Camera();
        }

        // Check the RC camera is enabled.
        if (rcCameraScript.RC_Camera.enabled == false)
        {
            return;
        }

        // Zoom
        rcCameraScript.Zoom_Input = -Input.GetAxis("Mouse ScrollWheel") * 2.0f;

        // Turn
        if (Input.GetKey(LSJ_GeneralSettings.RC_Camera_Rotate_Key))
        {
            rcCameraScript.Is_Turning = true;
            var multiplier = Mathf.Lerp(0.2f, 4.0f, rcCameraScript.RC_Camera.fieldOfView / 50.0f); // Change the rotation speed according to the FOV of the RC camera.
            rcCameraScript.Horizontal_Input = Input.GetAxis("Mouse X") * multiplier;
            rcCameraScript.Vertical_Input = Input.GetAxis("Mouse Y") * multiplier;
        }
        else
        {
            rcCameraScript.Is_Turning = false;
            rcCameraScript.Horizontal_Input = 0.0f;
            rcCameraScript.Vertical_Input = 0.0f;
        }
    }

}

