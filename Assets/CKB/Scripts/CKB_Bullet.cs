using System.Collections;
using System.Collections.Generic;
using UnityEngine;

public class CKB_Bullet : MonoBehaviour
{
    GameObject shooter;
    float damage;

    public GameObject hitVFX;

    void Start()
    {
        Ray ray = new Ray(transform.position, transform.forward);
        RaycastHit hitInfo;

        if (Physics.Raycast(ray, out hitInfo))
        {
            GameObject hitObj = hitInfo.transform.gameObject;

            Instantiate(hitVFX, hitInfo.point, Quaternion.LookRotation(hitInfo.normal));

            if (IsDamageAvailable(hitObj))
                BulletDamage(hitObj);
        }

        // 소멸
        Destroy(gameObject);
    }

    void Update() { }

    public void Initialize(GameObject shooter, float damage)
    {
        this.shooter = shooter;
        this.damage = damage;
    }

    void BulletDamage(GameObject go)
    {
        CKB_HPManager ckbHPManager = go.GetComponent<CKB_HPManager>();

        if (ckbHPManager)
            ckbHPManager.ApplyDamage(shooter, damage);
    }

    public bool IsDamageAvailable(GameObject go)
    {
        if (go && GetTankTransform(go.transform).CompareTag(CKB_TagObjectFinder.Instance.OpponentTag(shooter.tag)))
            return true;
        else
            return false;
    }

    Transform GetTankTransform(Transform tr)
	{
        if (tr.GetComponent<CKB_HPManager>() || tr == tr.root)
            return tr;

		return GetTankTransform(tr.parent);
	}
}
