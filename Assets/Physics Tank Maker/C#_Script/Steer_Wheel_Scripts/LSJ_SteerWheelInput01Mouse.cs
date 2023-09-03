using System.Collections;
using UnityEngine;


public class LSJ_SteerWheelInput01Mouse : LSJ_SteerWheelInput00Base
{

    public override void Get_Input()
    {
        if (Input.GetKey(LSJ_GeneralSettings.Drive_Left_Key))
        {
            steerScript.Horizontal_Input = -1.0f;
        }
        else if (Input.GetKey(LSJ_GeneralSettings.Drive_Right_Key))
        {
            steerScript.Horizontal_Input = 1.0f;
        }
        else
        {
            steerScript.Horizontal_Input = 0.0f;
        }
    }

}


