using System.Collections;
using UnityEngine;


public class LSJ_DamageControl00Base : MonoBehaviour
{

    protected LSJ_DamageControlCenter centerScript;


    protected virtual void Start()
    {
        centerScript = GetComponentInParent<LSJ_DamageControlCenter>();
    }


    public virtual bool Get_Damage(float damage, int bulletType)
    {
        return false;
    }

}
