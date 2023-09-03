using System.Collections;
using UnityEngine;


public class LSJ_CameraZoomInput02GamePad : LSJ_CameraZoomInput00Base
{

    public override void Get_Input()
    {
        var inputValue = 0.0f;
        if (Input.GetKey(LSJ_GeneralSettings.Camera_Zoom_In_Pad_Button))
        {
            inputValue = -1.0f;
        }
        else if (Input.GetKey(LSJ_GeneralSettings.Camera_Zoom_Out_Pad_Button))
        {
            inputValue = 1.0f;
        }

        zoomScript.Zoom_Input = inputValue * 0.05f;
    }

}
