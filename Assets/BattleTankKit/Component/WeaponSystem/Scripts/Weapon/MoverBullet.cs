using UnityEngine;
using System.Collections;

namespace HWRWeaponSystem
{
    public class MoverBullet : WeaponBase
    {
        public int Lifetime;
        public float Speed = 80;
        public float SpeedMax = 80;
        public float SpeedMult = 1;
        private float speedTemp;
        private Rigidbody rigidBody;

        private void Awake()
        {
            rigidBody = this.GetComponent<Rigidbody>();
            speedTemp = Speed;
            objectPool = this.GetComponent<ObjectPool>();
        }

        private void Start()
        {
            if (objectPool && WeaponSystem.Pool != null)
            {
                objectPool.SetDestroy(Lifetime);
            }
            else
            {
                Destroy(gameObject, Lifetime);
            }
        }

        public void OnEnable()
        {
            Speed = speedTemp;
            if (objectPool && WeaponSystem.Pool != null)
            {
                objectPool.SetDestroy(Lifetime);
            }
        }

        private void FixedUpdate()
        {
            if (WeaponSystem.Pool != null && objectPool != null && !objectPool.Active)
                return;

            if (!RigidbodyProjectile)
            {
                if (rigidBody)
                {
                    rigidBody.velocity = transform.forward * Speed;
                }
                else
                {
                    this.transform.position += transform.forward * Speed * Time.deltaTime;
                }
            }

            if (Speed < SpeedMax)
            {
                Speed += SpeedMult * Time.fixedDeltaTime;
            }
        }
    }
}
