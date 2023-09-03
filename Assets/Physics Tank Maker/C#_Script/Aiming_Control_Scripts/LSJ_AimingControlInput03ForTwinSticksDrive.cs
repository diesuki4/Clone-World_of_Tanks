using System.Collections;
using UnityEngine;


public class LSJ_AimingControlInput03ForTwinSticksDrive : LSJ_AimingControlInput00Base
{

    public override void Prepare(LSJ_AimingControl aimingScript)
    {
        this.aimingScript = aimingScript;

        aimingScript.Use_Auto_Turn = false;
    }


    public override void Get_Input()
    {
        // Rotate the turret and the cannon manually.
        if (Input.GetButton("Bumper L"))
        {
            aimingScript.Turret_Turn_Rate = Input.GetAxis("Horizontal2");
            aimingScript.Cannon_Turn_Rate = -Input.GetAxis("Vertical2");
        }
        else
        {
            aimingScript.Turret_Turn_Rate = 0.0f;
            aimingScript.Cannon_Turn_Rate = 0.0f;
        }

    }

}

