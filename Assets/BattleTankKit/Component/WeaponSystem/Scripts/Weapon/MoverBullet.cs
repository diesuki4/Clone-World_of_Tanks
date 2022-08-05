using UnityEngine;
using System.Collections;

// 미사일의 이동
namespace HWRWeaponSystem
{
    public class MoverBullet : WeaponBase
    {
        [Header("소멸 시간")]
        public int Lifetime;
        [Header("초기 속도")]
        public float Speed = 80;
        [Header("최대 속도")]
        public float SpeedMax = 80;
        [Header("초당 증가하는 속도")]
        public float SpeedMult = 1;

        float speedTemp;
        Rigidbody rigidBody;

        void Awake()
        {
            rigidBody = GetComponent<Rigidbody>();
            speedTemp = Speed;
            objectPool = GetComponent<ObjectPool>();
        }

        void Start()
        {
            // 오브젝트 풀을 사용 중이면
            if (objectPool && WeaponSystem.Pool != null)
                // Lifetime 뒤에 비활성화
                objectPool.SetDestroy(Lifetime);
            // 풀을 사용하지 않고 있으면
            else
                // LifeTime 뒤에 소멸
                Destroy(gameObject, Lifetime);
        }

        // 활성화 시 (오브젝트 풀에서 재활용할 시)
        void OnEnable()
        {
            // 초기 속도 복원
            Speed = speedTemp;

            // 오브젝트 풀을 사용 중이면
            if (objectPool && WeaponSystem.Pool != null)
                // Lifetime 뒤에 비활성화
                objectPool.SetDestroy(Lifetime);
        }

        void FixedUpdate()
        {
            if (WeaponSystem.Pool != null && objectPool != null && !objectPool.Active)
                return;

            // Rigidbody 모드가 켜져있으면
            if (!RigidbodyProjectile)
            {
                if (rigidBody)
                    // 증가하는 Speed 에 따라 velocity 를 적용한다.
                    rigidBody.velocity = transform.forward * Speed;
                else
                    // 증가하는 Speed 에 따라 위치를 이동시킨다.
                    transform.position += transform.forward * Speed * Time.deltaTime;
            }

            // 최대 속도로 Clamp
            if (Speed < SpeedMax)
                Speed += SpeedMult * Time.fixedDeltaTime;
        }
    }
}
