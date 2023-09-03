using System.Collections;
using UnityEngine;


public class LSJ_CameraPointsManagerInput01Mouse : LSJ_CameraPointsManagerInput00Base
{

    public override void Get_Input()
    {
        if (Input.GetKeyDown(LSJ_GeneralSettings.Camera_Switch_Key))
        {
            managerScript.Switch_Camera_Point();
        }
    }

}

