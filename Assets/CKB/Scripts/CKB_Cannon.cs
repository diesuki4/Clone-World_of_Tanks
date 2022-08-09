using System.Collections;
using System.Collections.Generic;
using UnityEngine;

public class CKB_Cannon : MonoBehaviour
{
    [HideInInspector]
    public GameObject Owner;
    [Header("발사체")]
    public GameObject Projectile;
    [Header("총알")]
    public GameObject Bullet;
    public float FireRate = 0.1f;
    public float Spread = 1;
    public float ForceShoot = 8000;
    public int Ammo = 10;
    public int AmmoMax = 10;
    public float ReloadTime = 1;

    [Header("Muzzle 이펙트")]
    public GameObject Muzzle;
    public float MuzzleLifeTime = 2;

    [Header("사운드 효과")]
    public AudioClip SoundGun;

    [HideInInspector]
    public float nextFireTime = 0;
    float reloadTimeTemp;
    AudioSource audioSource;
    bool Reloading;
    string TargetTag;
    string TargetLayer;

    void Start()
    {
        Transform tankTr = GetTankTransform(transform);
        
        Owner = tankTr.gameObject;
        TargetTag = tankTr.GetComponent<CKB_TankAI>().TargetTag;
        TargetLayer = tankTr.GetComponent<CKB_TankAI>().TargetLayer;

        audioSource = GetComponent<AudioSource>();
    }

    void Update()
    {
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
                // 장전 중인 상태
                Reloading = true;
                reloadTimeTemp = Time.time;
            }
        }
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

                // Muzzle Flash 이펙트가 있을 때
                if (Muzzle)
                {
                    // Muzzle 효과를 자식으로 생성한다.
                    GameObject muzzle = Instantiate(Muzzle, transform.position, transform.rotation, transform);
                    // MuzzleLifeTime 뒤에 소멸하도록 지정한다.
                    Destroy(muzzle, MuzzleLifeTime);
                }

                // 탄퍼짐 정도를 설정할 수 있다.
                Vector3 spread = new Vector3(Random.Range(-Spread, Spread), Random.Range(-Spread, Spread), Random.Range(-Spread, Spread)) / 100;
                // 발사체가 날아갈 방향
                Vector3 direction = transform.forward + spread;

                // 발사체를 생성한다.
                GameObject projectile = Instantiate(Projectile, transform.position, Quaternion.LookRotation(direction));

                // 발사체 컴포넌트
                CKB_Projectile ckbProjectile = projectile.GetComponent<CKB_Projectile>();

                // 쏜 탱크는 나다.
                ckbProjectile.Owner = Owner;
                // 적의 태그
                ckbProjectile.TargetTag = TargetTag;
                // 적의 레이어
                ckbProjectile.TargetLayer = TargetLayer;
                // 자신과는 충돌하지 않는다.
                ckbProjectile.IgnoreSelf();

                Rigidbody projectileRB = projectile.GetComponent<Rigidbody>();
                Rigidbody ownerRB = Owner.GetComponent<Rigidbody>();

                // 발사체의 velocity 를 탱크의 velocity 로 설정한다.
                projectileRB.velocity = ownerRB.velocity;
                // 발사체의 방향으로 힘을 가한다.
                projectileRB.AddForce(direction * ForceShoot, ForceMode.Acceleration);
                // 발사체의 앞 방향을 조정
                projectile.transform.forward = direction;

                // 사운드 효과 재생
                audioSource.PlayOneShot(SoundGun);

                nextFireTime += FireRate;
            }
        }
    }

    public void ShootBullet()
    {
        // 총알이 있으면
        if (Ammo > 0)
        {
            // FireRate 마다 총알을 발사한다. (재장전 시간과는 별개)
            if (Time.time > nextFireTime + FireRate)
            {
                --Ammo;
                nextFireTime = Time.time;

                // Muzzle Flash 이펙트가 있을 때
                if (Muzzle)
                {
                    // Muzzle 효과를 자식으로 생성한다.
                    GameObject muzzle = Instantiate(Muzzle, transform.position, transform.rotation, transform);
                    // MuzzleLifeTime 뒤에 소멸하도록 지정한다.
                    Destroy(muzzle, MuzzleLifeTime);
                }

                // 탄퍼짐 정도를 설정할 수 있다.
                Vector3 spread = new Vector3(Random.Range(-Spread, Spread), Random.Range(-Spread, Spread), Random.Range(-Spread, Spread)) / 100;
                // 발사체가 날아갈 방향
                Vector3 direction = transform.forward + spread;

                // 발사체를 생성한다.
                GameObject bullet = Instantiate(Bullet, transform.position, Quaternion.LookRotation(direction));

                // 발사체 컴포넌트
                CKB_Bullet ckbBullet = bullet.GetComponent<CKB_Bullet>();

                // 쏜 탱크는 나다.
                ckbBullet.Owner = Owner;
                // 적의 태그
                ckbBullet.TargetTag = TargetTag;

                // 사운드 효과 재생
                audioSource.PlayOneShot(SoundGun);

                nextFireTime += FireRate;
            }
        }
    }

	Transform GetTankTransform(Transform tr)
	{
        if (tr.GetComponent<CKB_Tank>() || tr == tr.root)
            return tr;

		return GetTankTransform(tr.parent);
	}
}
