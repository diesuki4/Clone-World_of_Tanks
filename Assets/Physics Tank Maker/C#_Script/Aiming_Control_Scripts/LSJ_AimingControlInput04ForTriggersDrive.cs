using System.Collections;
using UnityEngine;

public class LSJ_AimingControlInput04ForTriggersDrive : LSJ_AimingControlInput03ForTwinSticksDrive
{

    public override void Get_Input()
    {
        // Rotate the turret and the cannon manually.
        aimingScript.Turret_Turn_Rate = Input.GetAxis("Horizontal");
        aimingScript.Cannon_Turn_Rate = -Input.GetAxis("Vertical");
    }

}

