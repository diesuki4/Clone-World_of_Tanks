using System.Collections;
using UnityEngine;


public class LSJ_DamageControl09ArmorCollider : LSJ_DamageControl00Base
{

    public float Damage_Multiplier = 1.0f;


    LSJ_DamageControl00Base parentDamageScript;


    protected override void Start()
    {
        // Set the layer.
        gameObject.layer = LSJ_LayerSettings.Armor_Collider_Layer;

        // Make this invisible.
        MeshRenderer meshRenderer = GetComponent<MeshRenderer>();
        if (meshRenderer)
        {
            meshRenderer.enabled = false;
        }

        // Find the "Damage_Control_##_##_CS" script in the parent object.
        parentDamageScript = transform.parent.GetComponent<LSJ_DamageControl00Base>();
        if (parentDamageScript == null)
        {
            Destroy(this.gameObject);
        }
    }


    public override bool Get_Damage(float damage, int bulletType)
    { // Called from "Bullet_Control_CS".

        // Apply the multiplier.
        if (bulletType == 0)
        { // AP
            damage *= Damage_Multiplier;
        }

        // Send the damage value to the parent "Damage_Control_##_##_CS" script.
        if (parentDamageScript == null)
        {
            return false;
        }
        return parentDamageScript.Get_Damage(damage, bulletType);
    }


    void MainBody_Destroyed_Linkage()
    { // Called from "Damage_Control_Center_CS", when the MainBody has been destroyed.
        Destroy(this.gameObject);
    }


    void Turret_Destroyed_Linkage()
    { // Called from "Damage_Control_Center_CS", when this turret or the parent turret has been destroyed.
        Destroy(this.gameObject);
    }

}
