using System.Collections;
using UnityEngine;


public class LSJ_CannonFireInput01Mouse : LSJ_CannonFireInput00Base
{

    protected LSJ_TurretHorizontal turretScript;


    public override void Prepare(LSJ_CannonFire cannonFireScript)
    {
        this.cannonFireScript = cannonFireScript;

        turretScript = GetComponentInParent<LSJ_TurretHorizontal>();
    }


    public override void Get_Input()
    {
        // Fire.
        if (turretScript.Is_Ready && Input.GetKey(LSJ_GeneralSettings.Fire_Key))
        {
            cannonFireScript.Fire();
        }

        // Switch the bullet type.
        if (Input.GetKeyDown(LSJ_GeneralSettings.Switch_Bullet_Key))
        {
            // Call the "Bullet_Generator_CS" scripts.
            for (int i = 0; i < cannonFireScript.Bullet_Generator_Scripts.Length; i++)
            {
                if (cannonFireScript.Bullet_Generator_Scripts[i] == null)
                {
                    continue;
                }
                cannonFireScript.Bullet_Generator_Scripts[i].Switch_Bullet_Type();
            }

            // Reload.
            cannonFireScript.StartCoroutine("Reload");
        }
    }

}

