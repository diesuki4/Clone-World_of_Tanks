using System.Collections;
using System.Collections.Generic;
using UnityEngine;

// 데미지 전달
// 메인바디
public class LSJ_Bullet : MonoBehaviour
{
    float damage = 80f;
    GameObject target;

    private void Start()
    {
        
    }


    private void OnCollisionEnter(Collision other)
    {
        target = GameObject.Find("Cromwell_ST/MainBody");

        if (other.gameObject.tag == "Enemy")
        {
            CKB_HPManager ckbHPManager = other.gameObject.GetComponent<CKB_HPManager>();

            if (ckbHPManager)
                ckbHPManager.ApplyDamage(target, damage);
        }

         Destroy(gameObject);

    }
}
