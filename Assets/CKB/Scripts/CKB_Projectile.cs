using System.Collections;
using System.Collections.Generic;
using UnityEngine;

public class CKB_Projectile : MonoBehaviour
{
    [HideInInspector]
    public GameObject Owner;
    [HideInInspector]
    public string TargetTag;
    [Header("발사체 타격 효과")]
    public GameObject Effect;

    [Header("데미지")]
    public int Damage = 20;
    [Header("소멸 시간")]
    public int Lifetime;
    [Header("초기 속도")]
    public float Speed = 80;
    [Header("최대 속도")]
    public float SpeedMax = 80;
    [Header("초당 증가하는 속도")]
    public float SpeedMult = 1;

    public float DamageRadius = 20;
    public float ExplosionRadius = 20;
    public float ExplosionForce = 1000;

    Rigidbody rigidBody;
    // 현재 위치 기준 back 방향 1unit 위치
    Vector3 prevpos;

    void Start()
    {
        rigidBody = GetComponent<Rigidbody>();

        prevpos = transform.position - transform.forward;

        // LifeTime 뒤에 소멸
        Destroy(gameObject, Lifetime);
    }

    void Update()
    {
        // 이전 프레임 위치와의 거리 + 1
        float mag = Vector3.Distance(transform.position, prevpos) + 1;

        // 이전 위치에서도 레이를 쏘아 감지한다.
        RaycastHit[] hits = Physics.RaycastAll(transform.position + (-transform.forward * mag), transform.forward, mag);

        foreach (RaycastHit hit in hits) {
            // 감지된 게 내가 아니고 쏜 탱크가 아니면
            if (hit.transform.root != transform.root && (!Owner || hit.transform.root != Owner.transform.root)) {
                // 이펙트 표시, 폭발 데미지 처리
                ProcessExplosion(hit.point);
                break;
            }
        }

        prevpos = transform.position;
    }

    void FixedUpdate()
    {
        // 증가하는 Speed 에 따라 velocity 를 적용한다.
        rigidBody.velocity = transform.forward * Speed;

        // 최대 속도로 Clamp
        Speed = Mathf.Clamp(Speed + SpeedMult * Time.fixedDeltaTime, 0, SpeedMax);
    }

    // 이펙트 표시, 폭발 데미지 처리 부분
    public void ProcessExplosion(Vector3 position)
    {
        // 이펙트를 생성하고
        GameObject effect = Instantiate(Effect, position, transform.rotation);
        // 3초 뒤에 소멸시킨다.
        Destroy(effect, 3);

        // 현재 위치에서 ExplosionRadius 내에 있는 모든 Collider
        Collider[] hitColliders = Physics.OverlapSphere(position, ExplosionRadius);

        for (int i = 0; i < hitColliders.Length; ++i)
        {
            Collider hit = hitColliders[i];
            Rigidbody hitRB = null;

            if (!hit)
                continue;
            else
                hitRB = hit.GetComponent<Rigidbody>();

            // Rigidbody 가 있고 AI 이면 ExplosionForce 를 가한다.
            if (hitRB && !IsPlayer(hit.gameObject))
                hitRB.AddExplosionForce(ExplosionForce, position, ExplosionRadius, 3.0f);
        }

        // 현재 위치에서 DamageRadius 내에 있는 모든 Collider
        Collider[] dmhitColliders = Physics.OverlapSphere(position, DamageRadius, 1 << LayerMask.NameToLayer(TargetTag));

        for (int i = 0; i < dmhitColliders.Length; ++i)
        {
            Collider hit = dmhitColliders[i];

            if (!hit)
                continue;

            // IgnoreTag, 피아 식별, 내가 포함됐는지 검사한다.
            if (DoDamageCheck(hit.gameObject))
                if (IsPlayer(hit.gameObject))
                    // 플레이어에게 데미지를 적용한다.
                    Debug.Log("[TODO] [CKB_Projectile.cs] 플레이어 데미지 처리");
                else if (!Owner || (Owner && hit.gameObject != Owner.gameObject))
                    // AI 에게 데미지를 적용한다.
                    hit.transform.GetComponent<CKB_HPManager>().ApplyDamage(Owner, Damage);
        }

        // 소멸
        Destroy(gameObject);
    }

    // 발사체를 쏜 탱크 자신과는 충돌하지 않도록 처리
    public void IgnoreSelf()
    {
        Collider myCol = GetComponent<Collider>();
        Collider[] childCols = Owner.transform.root.GetComponentsInChildren<Collider>();

        foreach (Collider childCol in childCols)
            Physics.IgnoreCollision(myCol, childCol);
    }

    // 데미지 처리가 가능한지 검사 (false : 불가, true : 가능)
    public bool DoDamageCheck(GameObject go)
    {
        // 이미 죽었거나 아군이면 불가
        if (!go || go.transform.root.tag == Owner.tag)
            return false;
        // 나머지는 가능
        else
            return true;
    }

    // 게임 오브젝트가 플레이어인지 확인
    bool IsPlayer(GameObject go)
    {
        Debug.Log("[TODO] [CKB_Projectile.cs] 플레이어인지 확인");
        return false;
    }

    void OnCollisionEnter(Collision collision)
    {
        // IgnoreTag, 피아 식별, 발사체끼리 충돌을 검사한다.
        if (DoDamageCheck(collision.gameObject) && collision.gameObject.tag != gameObject.tag)
            // 이펙트 표시, 폭발 데미지 처리
            ProcessExplosion(transform.position);
    }
}
