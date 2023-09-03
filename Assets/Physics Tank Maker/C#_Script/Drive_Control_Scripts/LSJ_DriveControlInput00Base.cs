using System.Collections;
using UnityEngine;

public class LSJ_DriveControlInput00Base : MonoBehaviour
{

    protected LSJ_DriveControl controlScript;

    protected int speedStep;


    public virtual void Prepare(LSJ_DriveControl controlScript)
    {
        this.controlScript = controlScript;
    }


    public virtual void Drive_Input()
    {
    }

}
