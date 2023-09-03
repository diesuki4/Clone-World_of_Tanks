using System.Collections;
using UnityEngine;


public class LSJ_DamageControl02Turret : LSJ_DamageControl00Base
{

    public int Turret_Index; // 0 = Main turret.


    public override bool Get_Damage(float damage, int bulletType)
    { // Called from "Bullet_Control_CS", when the bullet hits this collider.
      // Send the damage value to the "Damage_Control_Center_CS".
        return centerScript.Receive_Damage(damage, 1, Turret_Index); // type = 1 (Turret), index = Turret_ID (0 = Main turret).
    }


    void MainBody_Destroyed_Linkage()
    { // Called from "Damage_Control_Center_CS", when the MainBody has been destroyed.
        Destroy(this);
    }


    void Turret_Destroyed_Linkage()
    { // Called from "Damage_Control_Center_CS", when this turret or the parent turret has been destroyed.
        Destroy(this);
    }
}

