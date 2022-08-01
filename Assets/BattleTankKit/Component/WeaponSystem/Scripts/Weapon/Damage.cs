using UnityEngine;
using System.Collections;

namespace HWRWeaponSystem
{
    public class Damage : DamageBase
    {
        public bool Explosive;
        public float DamageRadius = 20;
        public bool RayChecker = false;
        public float ExplosionRadius = 20;
        public float ExplosionForce = 1000;
        public bool HitedActive = true;
        public float TimeActive = 0;
        float timetemp = 0;
        ObjectPool objPool;
        // 현재 위치 기준 back 방향 1unit 위치
        Vector3 prevpos;

        void Awake()
        {
            objPool = GetComponent<ObjectPool>();
        }

        void OnEnable()
        {
            prevpos = transform.position - transform.forward;
            timetemp = Time.time;
        }

        void Start()
        {
            if (!Owner || !Owner.GetComponent<Collider>())
                return;

            timetemp = Time.time;
        }

        void Update()
        {
            if (objPool && !objPool.Active)
                return;

            // 이전 프레임 위치와의 거리 + 1
            float mag = Vector3.Distance(transform.position, prevpos) + 1;

            if (RayChecker)
            {
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
            }

            prevpos = transform.position;

            // TimeActive 를 설정한 경우 해당 시간이 지나면 이펙트와 폭발 데미지 처리가 진행된다.
            if (!HitedActive || TimeActive > 0)
                if (Time.time >= timetemp + TimeActive)
                    Active(transform.position);
        }

        // 이펙트 표시, 폭발 데미지 처리 부분
        public void Active(Vector3 position)
        {
            // 등록된 이펙트가 있을 때
            if (Effect)
            {
                // 오브젝트 풀을 사용 중이면
                if (WeaponSystem.Pool != null)
                {
                    // 풀에서 가져온다.
                    WeaponSystem.Pool.Instantiate(Effect, transform.position, transform.rotation, 3);
                }
                // 풀을 사용하지 않으면
                else
                {
                    // 그냥 생성하고
                    GameObject obj = Instantiate(Effect, transform.position, transform.rotation);
                    // 3초 뒤에 소멸시킨다.
                    Destroy(obj, 3);
                }
            }

            // 폭발 발사체이면
            if (Explosive)
                // 폭발 데미지로 처리
                ExplosionDamage();

            // 오브젝트 풀 객체이면
            if (objPool)
                // 비활성화 시킨다.
                objPool.OnDestroyed();
            // 아니면
            else
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
                {
                    // 데미지와 쏜 탱크가 나라는 것을 저장한다.
                    DamagePack damagePack = new DamagePack();
                    damagePack.Damage = Damage;
                    damagePack.Owner = Owner;

                    // 데미지를 적용한다.
                    hit.gameObject.SendMessage("ApplyDamage", damagePack, SendMessageOptions.DontRequireReceiver);
                }
            }
        }

        // 비폭발 데미지
        void NormalDamage(Collision collision)
        {
            // IgnoreTag, 피아 식별을 검사한다.
            if (DoDamageCheck(collision.gameObject))
            {
                // 데미지와 쏜 탱크가 나라는 것을 저장한다.
                DamagePack damagePack = new DamagePack();
                damagePack.Damage = Damage;
                damagePack.Owner = Owner;

                // 데미지를 적용한다.
                collision.gameObject.SendMessage("ApplyDamage", damagePack, SendMessageOptions.DontRequireReceiver);
            }
        }

        void OnCollisionEnter(Collision collision)
        {
            if (objPool && !objPool.Active && WeaponSystem.Pool)
                return;

            if (HitedActive)
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
        }
    }
}
