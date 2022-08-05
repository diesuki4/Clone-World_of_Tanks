using System.Collections;
using System.Collections.Generic;
using UnityEngine;

public class CKB_Damage : MonoBehaviour
{
    [HideInInspector]
    public GameObject Owner;
    [Header("발사체 타격 효과")]
    public GameObject Effect;
    [Header("데미지")]
    public int Damage = 20;
    public bool Explosive;
    public float DamageRadius = 20;
    public float ExplosionRadius = 20;
    public float ExplosionForce = 1000;

    // 현재 위치 기준 back 방향 1unit 위치
    Vector3 prevpos;

    void Start()
    {
        if (!Owner || !Owner.GetComponent<Collider>())
            return;
        
        prevpos = transform.position - transform.forward;
    }

    void Update()
    {
        // 이전 프레임 위치와의 거리 + 1
        float mag = Vector3.Distance(transform.position, prevpos) + 1;

        // 이전 위치에서도 레이를 쏘아 감지한다.
        RaycastHit[] hits = Physics.RaycastAll(transform.position + (-transform.forward * mag), transform.forward, mag);

        foreach (RaycastHit hit in hits)
        {
            // 감지된 게 내가 아니고 쏜 탱크가 아니면
            if (hit.transform.root != transform.root && (!Owner || hit.transform.root != Owner.transform.root))
            {
                // 이펙트 표시, 폭발 데미지 처리
                Active(hit.point);
                break;
            }
        }

        prevpos = transform.position;
    }

    // 이펙트 표시, 폭발 데미지 처리 부분
    public void Active(Vector3 position)
    {
        // 등록된 이펙트가 있을 때
        if (Effect)
        {
            // 이펙트를 생성하고
            GameObject obj = Instantiate(Effect, transform.position, transform.rotation);
            // 3초 뒤에 소멸시킨다.
            Destroy(obj, 3);
        }

        // 폭발 발사체이면
        if (Explosive)
            // 폭발 데미지로 처리
            ExplosionDamage();

        // 소멸
        Destroy(gameObject);
    }

    // 폭발 데미지
    void ExplosionDamage()
    {
        // 현재 위치에서 ExplosionRadius 내에 있는 모든 Collider
        Collider[] hitColliders = Physics.OverlapSphere(transform.position, ExplosionRadius);

        for (int i = 0; i < hitColliders.Length; ++i)
        {
            Collider hit = hitColliders[i];
            Rigidbody hitRB = null;

            if (!hit)
                continue;
            else
                hitRB = hit.GetComponent<Rigidbody>();

            // Rigidbody 가 있으면 ExplosionForce 를 가한다.
            if (hitRB)
                hitRB.AddExplosionForce(ExplosionForce, transform.position, ExplosionRadius, 3.0f);
        }

        // 현재 위치에서 DamageRadius 내에 있는 모든 Collider
        Collider[] dmhitColliders = Physics.OverlapSphere(transform.position, DamageRadius);

        for (int i = 0; i < dmhitColliders.Length; ++i)
        {
            Collider hit = dmhitColliders[i];

            if (!hit)
                continue;

            // IgnoreTag, 피아 식별, 내가 포함됐는지 검사한다.
            if (DoDamageCheck(hit.gameObject) && (!Owner || (Owner && hit.gameObject != Owner.gameObject)))
                // 데미지를 적용한다.
                hit.gameObject.SendMessage("ApplyDamage", new object[]{Damage, Owner}, SendMessageOptions.DontRequireReceiver);
        }
    }

    // 비폭발 데미지
    void NormalDamage(Collision collision)
    {
        // IgnoreTag, 피아 식별을 검사한다.
        if (DoDamageCheck(collision.gameObject))
            // 데미지와 쏜 탱크가 나라는 것을 전달한다.
            collision.gameObject.SendMessage("ApplyDamage", new object[]{Damage, Owner}, SendMessageOptions.DontRequireReceiver);
    }

    void OnCollisionEnter(Collision collision)
    {
        // IgnoreTag, 피아 식별, 발사체끼리 충돌을 검사한다.
        if (DoDamageCheck(collision.gameObject) && collision.gameObject.tag != gameObject.tag)
        {
            // 폭발탄이 아닐 경우 일반 데미지로 처리
            if (!Explosive)
                NormalDamage(collision);

            // 이펙트 표시, 폭발 데미지 처리
            Active(transform.position);
        }
    }
    
    // 데미지 처리가 가능한지 검사 (false : 불가, true : 가능)
    public bool DoDamageCheck(GameObject gob)
    {
        // 이미 죽었거나 아군이면 불가
        if (!gob || gob.transform.root.tag == Owner.tag)
            return false;
        // 나머지는 가능
        else
            return true;
    }

    // 발사체를 쏜 탱크 자신과는 충돌하지 않도록 처리
    public void IgnoreSelf(GameObject owner)
    {
        Collider col = GetComponent<Collider>();

        if (col && owner)
        {
            Collider ownrCol = owner.GetComponent<Collider>();

            if (ownrCol)
                Physics.IgnoreCollision(col, ownrCol);

            if (Owner.transform.root)
                foreach (Collider collider in Owner.transform.root.GetComponentsInChildren<Collider>())
                    Physics.IgnoreCollision(col, collider);
        }
    }
}
