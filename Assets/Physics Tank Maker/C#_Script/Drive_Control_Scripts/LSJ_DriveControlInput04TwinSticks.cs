using System.Collections;
using UnityEngine;


public class LSJ_DriveControlInput04TwinSticks : LSJ_DriveControlInput03SingleStick
{

    public override void Drive_Input()
    {


        if (Input.GetButton("Bumper L") == false)
        {
            vertical = Input.GetAxis("Vertical");
            horizontal = Input.GetAxis("Horizontal2");
        }
        else
        {
            vertical = 0.0f;
            horizontal = 0.0f;
        }

        Set_Values();
    }

}

