using UnityEngine;
using System.Collections;


[RequireComponent(typeof(Light))]
public class LSJ_MainLightControl : MonoBehaviour
{
    /*
     * This script is attached to the main light in the dark night scene (09_01_Escape_Mission).
     * The instance of this script is used for adjusting the particle alpha by "Emission_Control_CS" in the tank.
     * Attach this script to the main light when the particle is too bright in dark scenes.
    */

    public Light Main_Light;

    public static LSJ_MainLightControl Instance;


    void Awake()
    {
        if (Main_Light == null)
        {
            Main_Light = GetComponent<Light>();
        }

        Instance = this;
    }

}
