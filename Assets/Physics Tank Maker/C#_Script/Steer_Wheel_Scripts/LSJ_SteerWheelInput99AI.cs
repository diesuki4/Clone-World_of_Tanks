using System.Collections;
using UnityEngine;


public class LSJ_SteerWheelInput99AI : LSJ_SteerWheelInput00Base
{

    LSJ_AI aiScript;


    public override void Prepare(LSJ_SteerWheel steerScript)
    {
        this.steerScript = steerScript;

        aiScript = transform.root.GetComponentInChildren<LSJ_AI>();
    }


    public override void Get_Input()
    {
        steerScript.Horizontal_Input = aiScript.Turn_Order;
    }

}

