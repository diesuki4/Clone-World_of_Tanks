using System.Collections;
using UnityEngine;


public class LSJ_SteerWheelInput03ForTwinSticksDrive : LSJ_SteerWheelInput00Base
{

    public override void Get_Input()
    {
        steerScript.Horizontal_Input = Input.GetAxis("Horizontal2");
    }

}
