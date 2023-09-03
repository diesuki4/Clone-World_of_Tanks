using System.Collections;
using UnityEngine;


public class LSJ_CameraPointsManagerInput00Base : MonoBehaviour
{

    protected LSJ_CameraPointsManager managerScript;


    public virtual void Prepare(LSJ_CameraPointsManager managerScript)
    {
        this.managerScript = managerScript;
    }


    public virtual void Get_Input()
    {
    }

}


