using System.Collections;
using UnityEngine;


public class LSJ_GunCameraInput00Base : MonoBehaviour
{

    protected LSJ_GunCamera gunCameraScript;


    public void Prepare(LSJ_GunCamera gunCameraScript)
    {
        this.gunCameraScript = gunCameraScript;
    }


    public virtual void Get_Input()
    {
    }

}

