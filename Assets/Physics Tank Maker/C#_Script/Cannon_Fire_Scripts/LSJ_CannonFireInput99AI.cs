using System.Collections;
using UnityEngine;


public class LSJ_CannonFireInput99AI : LSJ_CannonFireInput00Base
{

    LSJ_AI aiScript;
    LSJ_TurretHorizontal turretScript;
    LSJ_CannonVertical cannonScript;
    LSJ_AimingControl aimingScript;
    float obstacleCount;
    float waitingCount;
    float aimingCount;


    public override void Prepare(LSJ_CannonFire cannoFireScript)
    {
        this.cannonFireScript = cannoFireScript;

        aiScript = transform.root.GetComponentInChildren<LSJ_AI>();
        turretScript = GetComponentInParent<LSJ_TurretHorizontal>();
        cannonScript = GetComponent<LSJ_CannonVertical>();
        aimingScript = GetComponentInParent<LSJ_AimingControl>();
    }


    public override void Get_Input()
    {
        // Check the AI gives an oder to fire.
        if (aiScript.OpenFire_Flag == false)
        { // The AI does not give an oder to fire.
            return;
        }

        // Check the turret and the cannon are ready to fire.
        if (turretScript.Is_Ready && cannonScript.Is_Ready)
        { // The turret and the cannon are ready to fire.
            if (aiScript.Direct_Fire == true)
            { // The tank aims a target directly.
              // Check all the "Bullet_Generator" in the children can aim the target.
                for (int i = 0; i < cannonFireScript.Bullet_Generator_Scripts.Length; i++)
                {
                    if (cannonFireScript.Bullet_Generator_Scripts[i].Can_Aim == false)
                    { // At least one of the "Bullet_Generator" in the children cannot aim the target.
                      // Change the aiming offset.
                        obstacleCount += Time.deltaTime;
                        if (obstacleCount > 1.0f)
                        {
                            obstacleCount = 0.0f;
                            aimingScript.AI_Random_Offset();
                        }
                        return;
                    }
                }
                // All the "Bullet_Generator" in the children can aim the target.
                obstacleCount = 0.0f;
            }

            // Count the aiming time.
            aimingCount += Time.deltaTime;
            if (aimingCount > aiScript.Fire_Count)
            {
                // Fire.
                cannonFireScript.Fire();
                aimingCount = 0.0f;
                aimingScript.AI_Random_Offset();
            }
        }
        else
        { // The turret and the cannon are not ready to fire.
            aimingCount = 0.0f;

            if (aiScript.Direct_Fire == true)
            { // The tank aims a target directly.
              // Count the waiting time.
                waitingCount += Time.deltaTime;
                if (waitingCount > 2.0f)
                { // The target might be in the dead angle.
                  // Change the aiming offset.
                    waitingCount = 0.0f;
                    aimingScript.AI_Random_Offset();
                }
            }
        }

    }

}
