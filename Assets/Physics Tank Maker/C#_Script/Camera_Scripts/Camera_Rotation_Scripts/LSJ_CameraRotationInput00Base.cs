using System.Collections;
using UnityEngine;


public class LSJ_CameraRotationInput00Base : MonoBehaviour
{

    protected LSJ_CameraRotation rotationScript;
    protected float multiplier;


    public virtual void Prepare(LSJ_CameraRotation rotationScript)
    {
        this.rotationScript = rotationScript;
    }


    public virtual void Get_Input()
    {
    }

}

