using UnityEngine;
using System.Collections;


public class LSJ_ExtraColliderCollisionControl : MonoBehaviour
{
    /*
     * This script is attached to the "Extra_Collier" in the tank.
    */


    public float Collision_Impact_Force;


    LSJ_DriveControl driveScript;
    Transform bodyTransform;


    void Start()
    {
        GetComponent<Collider>().isTrigger = true;

        driveScript = GetComponentInParent<LSJ_DriveControl>();
        bodyTransform = driveScript.transform;
    }


    void OnTriggerStay(Collider collider)
    {
        var dir = (collider.attachedRigidbody.position - bodyTransform.position).normalized;
        collider.attachedRigidbody.AddForce(dir * Collision_Impact_Force * (driveScript.Current_Velocity / driveScript.Max_Speed), ForceMode.Impulse);
    }


    void MainBody_Destroyed_Linkage()
    { // Called from "Damage_Control_Center_CS".

        Destroy(this.gameObject);
    }
}
