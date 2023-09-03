using System.Collections;
using UnityEngine;


public class LSJ_CameraZoomInput01Mouse : LSJ_CameraZoomInput00Base
{

    public override void Get_Input()
    {
        zoomScript.Zoom_Input = -Input.GetAxis("Mouse ScrollWheel") * 2.0f;
    }

}

