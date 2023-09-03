using System.Collections;
using UnityEngine;


public class LSJ_CameraZoomInput00Base : MonoBehaviour
{

    protected LSJ_CameraZoom zoomScript;


    public virtual void Prepare(LSJ_CameraZoom zoomScript)
    {
        this.zoomScript = zoomScript;
    }


    public virtual void Get_Input()
    {
    }

}

