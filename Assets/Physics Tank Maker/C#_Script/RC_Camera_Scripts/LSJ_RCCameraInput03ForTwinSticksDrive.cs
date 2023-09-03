using System.Collections;
using UnityEngine;


public class LSJ_RCCameraInput03ForTwinSticksDrive : LSJ_RCCameraInput02ForSingleStickDrive
{

    public override void Get_Input()
    {
        // Switch
        if (dPadPressed == false && Input.GetAxis(LSJ_GeneralSettings.RC_Camera_Switch_Pad_Axis) == LSJ_GeneralSettings.RC_Camera_Switch_Pad_Axis_Direction)
        {
            dPadPressed = true;
            rcCameraScript.Switch_RC_Camera();
        }
        else if (dPadPressed == true && Input.GetAxis(LSJ_GeneralSettings.RC_Camera_Switch_Pad_Axis) == 0.0f)
        {
            dPadPressed = false;
        }

        // Check the RC camera is enabled.
        if (rcCameraScript.RC_Camera.enabled == false)
        {
            return;
        }

        // Zoom
        var inputValue = 0.0f;
        if (Input.GetKey(LSJ_GeneralSettings.Camera_Zoom_In_Pad_Button))
        {
            inputValue = -1.0f;
        }
        else if (Input.GetKey(LSJ_GeneralSettings.Camera_Zoom_Out_Pad_Button))
        {
            inputValue = 1.0f;
        }
        rcCameraScript.Zoom_Input = inputValue * 0.05f;

        // Turn
        if (Input.GetButton("Bumper L") == false)
        {
            var multiplier = Mathf.Lerp(0.01f, 1.0f, rcCameraScript.RC_Camera.fieldOfView / 50.0f); // Change the rotation speed according to the FOV of the RC camera.
            rcCameraScript.Horizontal_Input = Input.GetAxis("Horizontal") * multiplier;
            rcCameraScript.Vertical_Input = Input.GetAxis("Vertical2") * multiplier;
        }
        rcCameraScript.Is_Turning = (rcCameraScript.Horizontal_Input != 0.0f || rcCameraScript.Vertical_Input != 0.0f);
    }

}
