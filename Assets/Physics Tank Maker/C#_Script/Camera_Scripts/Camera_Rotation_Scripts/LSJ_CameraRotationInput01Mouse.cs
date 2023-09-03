using System.Collections;
using UnityEngine;


public class LSJ_CameraRotationInput01Mouse : LSJ_CameraRotationInput00Base
{

    public override void Get_Input()
    {
        // Change the rotation speed according to the FOV of the main camera.
        multiplier = Mathf.Lerp(0.1f, 2.0f, rotationScript.Main_Camera.fieldOfView / 15.0f);

        rotationScript.Horizontal_Input = Input.GetAxis("Mouse X") * multiplier;
        rotationScript.Vertical_Input = Input.GetAxis("Mouse Y") * multiplier;
    }

}

