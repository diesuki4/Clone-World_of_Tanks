using System.Collections;
using UnityEngine;


public class LSJ_RCCameraInput00Base : MonoBehaviour
{

    protected LSJ_RCCamera rcCameraScript;


    public virtual void Prepare(LSJ_RCCamera rcCameraScript)
    {
        this.rcCameraScript = rcCameraScript;
    }


    public virtual void Get_Input()
    {
    }

}


