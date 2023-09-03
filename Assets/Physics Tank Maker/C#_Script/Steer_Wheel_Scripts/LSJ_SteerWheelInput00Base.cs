using System.Collections;
using UnityEngine;


public class LSJ_SteerWheelInput00Base : MonoBehaviour
{

    protected LSJ_SteerWheel steerScript;


    public virtual void Prepare(LSJ_SteerWheel steerScript)
    {
        this.steerScript = steerScript;
    }


    public virtual void Get_Input()
    {
    }

}

