using System.Collections;
using UnityEngine;


public class LSJ_AimingControlInput00Base : MonoBehaviour
{

    protected LSJ_AimingControl aimingScript;


    public virtual void Prepare(LSJ_AimingControl aimingScript)
    {
        this.aimingScript = aimingScript;
    }


    public virtual void Get_Input()
    {
    }

}
