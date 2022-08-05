using System.Collections;
using System.Collections.Generic;
using UnityEngine;

public class CKB_Cannon : MonoBehaviour
{		/*
    [HideInInspector]
    public GameObject Owner;

    [Header("Aiming")]
    public bool Seeker;
    public float AimDirection = 0.8f;

    [Header("Projectile")]
    public Transform[] MissileOuter;
    public GameObject Missile;
    public float FireRate = 0.1f;
    public float Spread = 1;
    public float ForceShoot = 8000;
    public int Ammo = 10;
    public int AmmoMax = 10;
    public float ReloadTime = 1;

    [Header("HUD")]
    public float DistanceLock = 200;
    public float TimeToLock = 2;

    [Header("Other FX")]
    public GameObject Muzzle;
    public float MuzzleLifeTime = 2;

    [Header("Sound FX")]
    public AudioClip SoundGun;

    float timetolockcount = 0;
    float nextFireTime = 0;
    GameObject target;
    float reloadTimeTemp;
    AudioSource audioSource;
    [HideInInspector]
    public bool Reloading;

    void Start()
    {
        Owner = transform.root.gameObject;

        audioSource = GetComponent<AudioSource>();

        if (!WeaponSystem.Finder)
            Debug.LogWarning("Need Weapon System object in the scene, you have to place it from WeaponSystem/WeaponSystem.prefab");
    }

    [HideInInspector]
    public Vector3 AimPoint;
    [HideInInspector]
    public GameObject AimObject;

    void Update()
    {
        // 지정된 타겟이 있으면
        if (target)
        {
            // 타겟과의 거리
            float targetdistance = Vector3.Distance(transform.position, target.transform.position);
            // 타겟으로의 방향
            Vector3 dir = (target.transform.position - transform.position).normalized;
            float direction = Vector3.Dot(dir, transform.forward);

            // 타겟과의 거리가 Lock 거리보다 멀거나 Arccos(AimDirection) 각도보다 크면
            if (targetdistance > DistanceLock || direction <= AimDirection)
                // Unlock
                Unlock();
        }

        // 재장전 중이면
        if (Reloading)
        {
            // ReloadTime 이 지났으면
            if (Time.time >= reloadTimeTemp + ReloadTime)
            {
                // 재장전 완료
                Reloading = false;

                // 탄창 재충전
                Ammo = AmmoMax;
            }
        }
        // 재장전 중이 아닐 때
        else
        {
            // 탄창이 다 떨어졌으면
            if (Ammo <= 0)
            {
                // Unlock
                Unlock();
                // 장전 중인 상태
                Reloading = true;
                reloadTimeTemp = Time.time;
            }
        }
    }

    void Unlock()
    {
        timetolockcount = Time.time;
        target = null;
    }

    public void Shoot()
    {
        // 총알이 있으면
        if (Ammo > 0)
        {
            // FireRate 마다 총알을 발사한다. (재장전 시간과는 별개)
            if (Time.time > nextFireTime + FireRate)
            {
                --Ammo;
                nextFireTime = Time.time;

                // 발사체를 포신의 위치와 회전값으로 초기화
                Vector3 missileposition = transform.position;
                Quaternion missilerotate = transform.rotation;

                // Muzzle Flash 이펙트가 있을 때
                if (Muzzle)
                {
                    // Muzzle 효과를 생성한다.
                    GameObject muzzle = Instantiate(Muzzle, missileposition, missilerotate);
                    // MuzzleLifeTime 뒤에 소멸하도록 지정한다.
                    Destroy(muzzle, MuzzleLifeTime);
                    // 하이어라키에 계속 차오르지 않도록 자식으로 등록한다.
                    muzzle.transform.parent = transform;
                }

                // 탄퍼짐 정도를 설정할 수 있다.
                Vector3 spread = new Vector3(Random.Range(-Spread, Spread), Random.Range(-Spread, Spread), Random.Range(-Spread, Spread)) / 100;
                // 발사체가 날아갈 방향
                Vector3 direction = transform.forward + spread;
                missilerotate = Quaternion.LookRotation(direction);

                // 발사체를 생성한다.
                GameObject missile = Instantiate(Missile, missileposition, missilerotate);

                // 발사체 이동 제어용 컴포넌트 (MoverBullet / MoverMissile : WeaponBase 상속)
                CKB_Missile ckbMissile = missile.GetComponent<CKB_Missile>();

                // 쏜 탱크는 나다.
                ckbMissile.Owner = Owner;
                ckbMissile.Target = target;
                ckbMissile.IgnoreSelf(gameObject);

                Rigidbody bulletRB = missile.GetComponent<Rigidbody>();
                Rigidbody ownerRB = Owner.GetComponent<Rigidbody>();

                // 발사체의 velocity 를 탱크의 velocity 로 설정한다.
                bulletRB.velocity = ownerRB.velocity;
                // 발사체의 방향으로 힘을 가한다.
                bulletRB.AddForce(direction * ForceShoot, ForceMode.Acceleration);
                // 발사체의 앞 방향을 조정
                missile.transform.forward = direction;

                // 사운드 효과 재생
                audioSource.PlayOneShot(SoundGun);

                nextFireTime += FireRate;
            }
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
    }*/
}
