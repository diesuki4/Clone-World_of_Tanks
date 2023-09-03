using UnityEngine;
using System.Collections;


public class LSJ_ExtraCollider : MonoBehaviour
{
    /*
     * This script is attached to the "Extra_Collier" in the tank.
     * This script only sets the Layer of this gameobject.
    */


    void Start()
    {
        // Set the layer.
        gameObject.layer = LSJ_LayerSettings.Extra_Collider_Layer;

        Destroy(this);
    }

}
