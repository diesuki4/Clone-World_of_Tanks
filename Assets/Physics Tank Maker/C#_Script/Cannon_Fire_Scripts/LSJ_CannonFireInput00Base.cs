using System.Collections;
using UnityEngine;


public class LSJ_CannonFireInput00Base : MonoBehaviour
{

    protected LSJ_CannonFire cannonFireScript;


    public virtual void Prepare(LSJ_CannonFire cannonFireScript)
    {
        this.cannonFireScript = cannonFireScript;
    }


    public virtual void Get_Input()
    {
    }

}

