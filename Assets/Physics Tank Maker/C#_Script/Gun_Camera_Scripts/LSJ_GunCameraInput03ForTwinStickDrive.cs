using System.Collections;
using UnityEngine;


public class LSJ_GunCameraInput03ForTwinStickDrive : LSJ_GunCameraInput00Base
{

    public override void Get_Input()
    {
        // Turn on / off.
        if (Input.GetButtonDown("Stick Press L"))
        {
            if (gunCameraScript.Gun_Camera.enabled)
            {
                gunCameraScript.Switch_Mode(1); // Off
            }
            else
            {
                gunCameraScript.Switch_Mode(2); // On
            }
        }


        // Zoom.
        if (gunCameraScript.Gun_Camera.enabled)
        { // The gun camera is enabled.
            var inputValue = 0.0f;
            if (Input.GetKey(LSJ_GeneralSettings.Camera_Zoom_In_Pad_Button))
            {
                inputValue = -1.0f;
            }
            else if (Input.GetKey(LSJ_GeneralSettings.Camera_Zoom_Out_Pad_Button))
            {
                inputValue = 1.0f;
            }

            gunCameraScript.Zoom_Input = inputValue * 0.05f;
        }
    }

}

