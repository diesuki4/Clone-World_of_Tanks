using System.Collections;
using UnityEngine;

public class LSJ_CannonFireInput02ForSticksDrive : LSJ_CannonFireInput00Base
{

    public override void Get_Input()
    {
        // Fire.
        if (Input.GetKey(LSJ_GeneralSettings.Fire_Pad_Button))
        {
            cannonFireScript.Fire();
        }

        // Switch the bullet type.
        if (Input.GetKeyDown(LSJ_GeneralSettings.Switch_Bullet_Pad_Button))
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

