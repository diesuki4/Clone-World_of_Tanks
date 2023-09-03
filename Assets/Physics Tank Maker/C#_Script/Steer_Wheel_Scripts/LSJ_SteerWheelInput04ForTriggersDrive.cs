using System.Collections;
using UnityEngine;


public class LSJ_SteerWheelInput04ForTriggersDrive : LSJ_SteerWheelInput00Base
{

    public override void Get_Input()
    {
        var leftRate = Input.GetAxis("Trigger L") - Input.GetAxis("Bumper L");
        var rightRate = Input.GetAxis("Trigger R") - Input.GetAxis("Bumper R");
        var tempHorizontal = Mathf.Clamp(leftRate - rightRate, -1.0f, 1.0f);
        if ((leftRate == 0.0f || rightRate == 0.0f) && (leftRate < 0.0f || rightRate < 0.0f))
        { // Backward turn.
            tempHorizontal = -tempHorizontal;
        }
        steerScript.Horizontal_Input = tempHorizontal;
    }

}

