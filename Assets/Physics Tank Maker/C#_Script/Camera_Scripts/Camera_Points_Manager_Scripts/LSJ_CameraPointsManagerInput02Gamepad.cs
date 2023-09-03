using System.Collections;
using UnityEngine;


public class LSJ_CameraPointsManagerInput02Gamepad : LSJ_CameraPointsManagerInput00Base
{

    protected bool dPadPressed;


    public override void Get_Input()
    {
        // Switch
        if (dPadPressed == false && Input.GetAxis(LSJ_GeneralSettings.Camera_Switch_Pad_Axis) == LSJ_GeneralSettings.Camera_Switch_Pad_Axis_Direction)
        {
            dPadPressed = true;
            managerScript.Switch_Camera_Point();
        }
        else if (dPadPressed == true && Input.GetAxis(LSJ_GeneralSettings.Camera_Switch_Pad_Axis) == 0.0f)
        {
            dPadPressed = false;
        }
    }
}

