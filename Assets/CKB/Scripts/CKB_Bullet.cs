using System.Collections;
using System.Collections.Generic;
using UnityEngine;

public class CKB_Bullet : MonoBehaviour
{
    [HideInInspector]
    public GameObject Owner;
    [HideInInspector]
    public string TargetTag;
    public GameObject Effect;

    [Header("데미지")]
    public int Damage = 20;
    public float ExplosionRadius = 20;
    public float ExplosionForce = 1000;

    void Start()
    {
        Ray ray = new Ray(transform.position, transform.forward);
        RaycastHit hitInfo;

        if (Physics.Raycast(ray, out hitInfo))
        {
            GameObject hitObj = hitInfo.transform.gameObject;

            Instantiate(Effect, hitInfo.point, Quaternion.LookRotation(hitInfo.normal));

            if (DoDamageCheck(hitObj))
                BulletDamage(hitObj, hitInfo.point);
        }

        // 소멸
        Destroy(gameObject);
    }

    void Update() { }

    void BulletDamage(GameObject go, Vector3 position)
    {
        // AI 이면 ExplosionForce 를 가한다.
        if (!IsPlayer(go))
        {
            Rigidbody rb = go.GetComponent<Rigidbody>();

            if (rb)
                rb.AddExplosionForce(ExplosionForce, position, ExplosionRadius, 3.0f);
        }

        if (IsPlayer(go))
        {
            // 플레이어에게 데미지를 적용한다.
            Debug.Log("[TODO] [CKB_Bullet.cs] 플레이어 데미지 처리");
        }
        else
        {
            // AI 에게 데미지를 적용한다.
            CKB_HPManager ckbHPManager = go.GetComponent<CKB_HPManager>();

            if (ckbHPManager)
                ckbHPManager.ApplyDamage(Owner, Damage);
        }
    }

    // 데미지 처리가 가능한지 검사 (false : 불가, true : 가능)
    public bool DoDamageCheck(GameObject go)
    {
        // 아직 죽지 않았고 상대 팀이면 가능
        if (go && GetTankTransform(go.transform).CompareTag(TargetTag))
            return true;
        // 나머지는 불가능
        else
            return false;
    }

    // 게임 오브젝트가 플레이어인지 확인
    bool IsPlayer(GameObject go)
    {
        Debug.Log("[TODO] [CKB_Bullet.cs] 플레이어인지 확인");
        return false;
    }

	Transform GetTankTransform(Transform tr)
	{
        if (tr.GetComponent<CKB_Tank>() || tr == tr.root)
            return tr;

		return GetTankTransform(tr.parent);
	}
}
