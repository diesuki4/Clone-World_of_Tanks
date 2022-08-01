using System;
using UnityEngine;
using System.Collections;
using Random = UnityEngine.Random;

namespace HWRWeaponSystem
{
    [RequireComponent(typeof(AudioSource))]
    public class WeaponLauncher : WeaponBase
    {
        [Header("발사 가능 여부")]
        public bool OnActive;
        [Header("Aiming")]
        public bool Seeker;
        public bool OnScreenAiming;
        public float AimDirection = 0.8f;
        public int MaxAimRange = 10000;
        public bool SnapCrosshair = true;

        [Header("Projectile")]
        public Transform[] MissileOuter;
        public GameObject Missile;
        public float FireRate = 0.1f;
        public float Spread = 1;
        public float ForceShoot = 8000;
        public int NumBullet = 1;
        public int Ammo = 10;
        public int AmmoMax = 10;
        public bool InfinityAmmo = false;
        public float ReloadTime = 1;

        [Header("HUD")]
        public bool ShowHUD = true;
        public bool ShowCrosshair = true;
        public Texture2D CrosshairTexture;
        public Texture2D TargetLockOnTexture;
        public Texture2D TargetLockedTexture;
        public float DistanceLock = 200;
        public float TimeToLock = 2;

        [Header("Other FX")]
        public GameObject Shell;
        public float ShellLifeTime = 4;
        public Transform[] ShellOuter;
        public int ShellOutForce = 300;
        public GameObject Muzzle;
        public float MuzzleLifeTime = 2;
        public Vector3 ShakeForce = Vector3.up;

        [Header("Sound FX")]
        public AudioClip[] SoundGun;
        public AudioClip SoundReloading;
        public AudioClip SoundReloaded;

        float timetolockcount = 0;
        float nextFireTime = 0;
        GameObject target;
        Vector3 torqueTemp;
        float reloadTimeTemp;
        AudioSource audioSource;
        [HideInInspector]
        public bool Reloading;
        [HideInInspector]
        public float ReloadingProcess;
        public GameObject CrosshairObject;
        GameObject crosshair;
        public Texture2D Icon;

        void Start()
        {
            if (!Owner)
                Owner = transform.root.gameObject;

            if (!audioSource)
            {
                audioSource = GetComponent<AudioSource>();

                if (!audioSource)
                    gameObject.AddComponent<AudioSource>();
            }

            if (CrosshairObject)
                crosshair = Instantiate(CrosshairObject.gameObject, transform.position, CrosshairObject.transform.rotation);

            if (!WeaponSystem.Finder)
                Debug.LogWarning("Need Weapon System object in the scene, you have to place it from WeaponSystem/WeaponSystem.prefab");
        }

        [HideInInspector]
        public Vector3 AimPoint;
        [HideInInspector]
        public GameObject AimObject;

        void rayAiming()
        {
            RaycastHit hit;

            if (OnScreenAiming)
            {
                if (CurrentCamera)
                {
                    var ray = CurrentCamera.ScreenPointToRay(Input.mousePosition);

                    if (Physics.Raycast(ray, out hit, MaxAimRange) && SnapCrosshair)
                    {
                        if (Missile != null && hit.collider.tag != Missile.tag)
                        {
                            AimPoint = hit.point;
                            AimObject = hit.collider.gameObject;
                        }
                    }
                    else
                    {
                        AimPoint = ray.origin + (ray.direction * MaxAimRange);
                        AimObject = null;
                    }
                }
            }
            else
            {
                if (Physics.Raycast(transform.position, transform.forward, out hit, MaxAimRange) && SnapCrosshair)
                {
                    if (Missile != null && hit.collider.tag != Missile.tag)
                    {
                        AimPoint = hit.point;
                        AimObject = hit.collider.gameObject;
                    }
                }
                else
                {
                    AimPoint = transform.position + (transform.forward * MaxAimRange);
                    AimObject = null;
                }
            }

        }

        void FixedUpdate()
        {
            if (OnActive)
                rayAiming();
        }

        void Update()
        {
            if (!CurrentCamera)
            {
                CurrentCamera = Camera.main;

                if (!CurrentCamera)
                    CurrentCamera = Camera.current;
            }

            // 발사 가능 상태이면
            if (OnActive)
            {
                // 사용되지 않음
                if (TorqueObject)
                {
                    TorqueObject.transform.Rotate(torqueTemp * Time.deltaTime);

                    torqueTemp = Vector3.Lerp(torqueTemp, Vector3.zero, Time.deltaTime);
                }

                // Seeker 가 true 이면
                if (Seeker)
                {
                    // TargetTag 에 등록된 태그만큼 순회
                    for (int i = 0; i < TargetTag.Length; ++i)
                    {
                        // i 번째 태그를 갖는 오브젝트 목록을 가져온다.
                        TargetCollector collector = WeaponSystem.Finder.FindTargetTag(TargetTag[i]);

                        if (collector != null)
                        {
                            // 해당 태그를 갖는 게임 오브젝트 배열
                            GameObject[] objs = collector.Targets;
                            float distance = int.MaxValue;

                            // 설정된 AimObject 가 있고 TargetTag 에 해당 태그가 존재하면
                            if (AimObject != null && AimObject.tag == TargetTag[i])
                            {
                                // AimObject 와의 거리
                                float dis = Vector3.Distance(AimObject.transform.position, transform.position);

                                // Lock 거리보다 작고
                                if (DistanceLock > dis)
                                {
                                    // 현재 target 보다 가까이 있고
                                    if (distance > dis)
                                    {
                                        // TimeToLock 시간이 지났으면
                                        if (timetolockcount + TimeToLock < Time.time)
                                        {
                                            // AimObject 를 현재 타겟으로 지정
                                            distance = dis;
                                            target = AimObject;
                                        }
                                    }
                                }
                            }
                            // 설정된 AimObject 가 없거나 TargetTag 에 해당 태그가 존재하지 않으면
                            else
                            {
                                // TargetTag 의 i번째 태그를 갖는 오브젝트들을 순회
                                for (int j = 0; j < objs.Length; ++j)
                                {
                                    if (objs[j])
                                    {
                                        // objs[j] 로의 방향
                                        Vector3 dir = (objs[j].transform.position - transform.position).normalized;
                                        float direction = Vector3.Dot(dir, transform.forward);
                                        // objs[j] 와의 거리
                                        float dis = Vector3.Distance(objs[j].transform.position, transform.position);

                                        // objs[j] 와의 각도가 Arccos(AimDirection) 작고
                                        if (direction >= AimDirection)
                                        {
                                            // Lock 거리보다 작고
                                            if (DistanceLock > dis)
                                            {
                                                // 현재 target 보다 가까이 있고
                                                if (distance > dis)
                                                {
                                                    // TimeToLock 시간이 지났으면
                                                    if (timetolockcount + TimeToLock < Time.time)
                                                    {
                                                        // TargetTag 의 i번째 태그를 갖는 오브젝트 중 j번째 오브젝트를 현재 타겟으로 지정
                                                        distance = dis;
                                                        target = objs[j];
                                                    }
                                                }
                                            }
                                        }
                                    }
                                }
                            }
                        }
                    }
                }

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
                    // 현재 장전율 계산
                    ReloadingProcess = ((1 / ReloadTime) * (reloadTimeTemp + ReloadTime - Time.time));

                    // ReloadTime 이 지났으면
                    if (Time.time >= reloadTimeTemp + ReloadTime)
                    {
                        // 재장전 완료
                        Reloading = false;

                        // 장전 완료 소리 재생
                        if (SoundReloaded)
                            if (audioSource)
                                audioSource.PlayOneShot(SoundReloaded);

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

                        // 탄창 장전 소리 재생
                        if (SoundReloading)
                            if (audioSource)
                                audioSource.PlayOneShot(SoundReloading);
                    }
                }
            }
        }

        public Camera CurrentCamera;

        void DrawTargetLockon(Transform aimtarget, bool locked)
        {
            if (!ShowHUD)
                return;

            if (CurrentCamera)
            {
                if (crosshair)
                {
                    crosshair.transform.position = AimPoint;
                    crosshair.transform.forward = transform.forward;
                    //Quaternion lookat = Quaternion.LookRotation((crosshair.transform.position - CurrentCamera.transform.position).normalized);
                }

                Vector3 dir = (aimtarget.position - CurrentCamera.transform.position).normalized;
                float direction = Vector3.Dot(dir, CurrentCamera.transform.forward);

                if (direction > 0.5f)
                {
                    Vector3 screenPos = CurrentCamera.WorldToScreenPoint(aimtarget.transform.position);
                    float distance = Vector3.Distance(transform.position, aimtarget.transform.position);

                    if (locked)
                    {
                        if (TargetLockedTexture)
                            GUI.DrawTexture(new Rect(screenPos.x - TargetLockedTexture.width / 2, Screen.height - screenPos.y - TargetLockedTexture.height / 2, TargetLockedTexture.width, TargetLockedTexture.height), TargetLockedTexture);
                        
                        GUI.Label(new Rect(screenPos.x + 40, Screen.height - screenPos.y, 200, 30), aimtarget.name + " " + Mathf.Floor(distance) + "m.");
                    }
                    else
                    {
                        if (TargetLockOnTexture)
                            GUI.DrawTexture(new Rect(screenPos.x - TargetLockOnTexture.width / 2, Screen.height - screenPos.y - TargetLockOnTexture.height / 2, TargetLockOnTexture.width, TargetLockOnTexture.height), TargetLockOnTexture);
                    }
                }
            }
            else
            {
                //Debug.Log("Can't Find camera");
            }
        }

        Vector3 crosshairPos;

        void DrawCrosshair()
        {
            if (!ShowCrosshair)
                return;

            if (CurrentCamera)
            {
                Vector3 screenPosAim = CurrentCamera.WorldToScreenPoint(AimPoint);
                crosshairPos += ((screenPosAim - crosshairPos) / 5);

                if (CrosshairTexture)
                    GUI.DrawTexture(new Rect(crosshairPos.x - CrosshairTexture.width / 2, Screen.height - crosshairPos.y - CrosshairTexture.height / 2, CrosshairTexture.width, CrosshairTexture.height), CrosshairTexture);
            }
        }

        void OnGUI()
        {
            if (OnActive)
            {
                if (Seeker)
                {
                    if (target)
                        DrawTargetLockon(target.transform, true);

                    for (int t = 0; t < TargetTag.Length; ++t)
                    {
                        TargetCollector collector = WeaponSystem.Finder.FindTargetTag(TargetTag[t]);
                        
                        if (collector != null)
                        {
                            GameObject[] objs = collector.Targets;

                            for (int i = 0; i < objs.Length; ++i)
                            {
                                if (objs[i])
                                {
                                    Vector3 dir = (objs[i].transform.position - transform.position).normalized;
                                    float direction = Vector3.Dot(dir, transform.forward);

                                    if (direction >= AimDirection)
                                    {
                                        float dis = Vector3.Distance(objs[i].transform.position, transform.position);

                                        if (DistanceLock > dis)
                                            DrawTargetLockon(objs[i].transform, false);
                                    }
                                }
                            }
                        }
                    }
                }

                DrawCrosshair();
            }

        }

        void Unlock()
        {
            timetolockcount = Time.time;
            target = null;
        }

        int currentOuter = 0;

        public void Shoot()
        {
            // 무한 탄창
            if (InfinityAmmo)
                Ammo = 1;
                
            // 총알이 있으면
            if (Ammo > 0)
            {
                // FireRate 마다 총알을 발사한다. (재장전 시간과는 별개)
                if (Time.time > nextFireTime + FireRate)
                {
                    CameraEffects.Shake(ShakeForce, transform.position);
                    nextFireTime = Time.time;
                    torqueTemp = TorqueSpeedAxis;
                    Ammo -= 1;
                    // 발사체를 포신의 위치와 회전값으로 초기화
                    Vector3 missileposition = transform.position;
                    Quaternion missilerotate = transform.rotation;

                    // 포신의 개수가 여러 개일 때 차례대로 1발씩 발사하도록 하는 루틴인 것 같다.
                    // 게임에서는 1개이므로 의미 없음
                    if (MissileOuter.Length > 0)
                    {
                        missilerotate = MissileOuter[currentOuter].rotation;
                        missileposition = MissileOuter[currentOuter].position;
                    }

                    if (MissileOuter.Length > 0)
                    {
                        ++currentOuter;

                        if (currentOuter >= MissileOuter.Length)
                            currentOuter = 0;
                    }

                    // Muzzle Flash 이펙트가 있을 때
                    if (Muzzle)
                    {
                        GameObject muzzle;

                        // 오브젝트 풀을 사용 중이면
                        if (WeaponSystem.Pool)
                        {
                            // 오브젝트 풀에서 Muzzle 효과를 가져온다.
                            muzzle = WeaponSystem.Pool.Instantiate(Muzzle, missileposition, missilerotate, MuzzleLifeTime);
                        }
                        // 오브젝트 풀을 사용 안 하고 있으면
                        else
                        {
                            // Muzzle 효과를 그냥 생성한다.
                            muzzle = Instantiate(Muzzle, missileposition, missilerotate);
                            // MuzzleLifeTime 뒤에 소멸하도록 지정한다.
                            Destroy(muzzle, MuzzleLifeTime);
                        }

                        muzzle.transform.parent = transform;

                        if (MissileOuter.Length > 0)
                            muzzle.transform.parent = MissileOuter[currentOuter].transform;
                    }

                    // NumBullet 만큼 발사체가 발사된다.
                    // 게임에서는 1
                    for (int i = 0; i < NumBullet; ++i)
                    {
                        if (Missile)
                        {
                            // 탄퍼짐 정도를 설정할 수 있다.
                            Vector3 spread = new Vector3(Random.Range(-Spread, Spread), Random.Range(-Spread, Spread), Random.Range(-Spread, Spread)) / 100;
                            // 발사체가 날아갈 방향
                            Vector3 direction = transform.forward + spread;
                            missilerotate = Quaternion.LookRotation(direction);

                            GameObject bullet;

                            // 오브젝트 풀을 사용 중이면
                            if (WeaponSystem.Pool)
                                // 오브젝트 풀에서 가져온다.
                                bullet = WeaponSystem.Pool.Instantiate(Missile, missileposition, missilerotate);
                            // 오브젝트 풀 사용 중이 아니면
                            else
                                // 그냥 생성한다.
                                bullet = Instantiate(Missile, missileposition, missilerotate);

                            // 발사체가 생성 됐으면
                            if (bullet)
                            {
                                // 데미지 처리용 컴포넌트 (Damage : DamageBase 상속)
                                DamageBase damageBase = bullet.GetComponent<DamageBase>();

                                if (damageBase)
                                {
                                    // 쏜 탱크는 나다.
                                    damageBase.Owner = Owner;
                                    damageBase.TargetTag = TargetTag;
                                    // 이 태그를 가진 오브젝트는 무시
                                    damageBase.IgnoreTag = IgnoreTag;
                                    // 쏜 탱크와는 충돌하지 않는다.
                                    damageBase.IgnoreSelf(gameObject);
                                }

                                // 발사체 이동 제어용 컴포넌트 (MoverBullet / MoverMissile : WeaponBase 상속)
                                WeaponBase weaponBase = bullet.GetComponent<WeaponBase>();

                                if (weaponBase)
                                {
                                    // 쏜 탱크는 나다.
                                    weaponBase.Owner = Owner;
                                    weaponBase.Target = target;
                                    weaponBase.TargetTag = TargetTag;
                                    // 이 태그를 가진 오브젝트는 무시
                                    weaponBase.IgnoreTag = IgnoreTag;
                                }

                                // 발사체 Rigidbody 모드가 켜져있으면
                                if (RigidbodyProjectile)
                                {
                                    Rigidbody bulletRB = bullet.GetComponent<Rigidbody>();
                                    Rigidbody ownerRB = Owner.GetComponent<Rigidbody>();

                                    if (bulletRB)
                                    {
                                        if (Owner && ownerRB)
                                            // 발사체의 velocity 를 탱크의 velocity 로 설정한다.
                                            bulletRB.velocity = ownerRB.velocity;
                                        
                                        // 발사체의 방향으로 힘을 가한다.
                                        bulletRB.AddForce(direction * ForceShoot, ForceMode.Acceleration);
                                    }
                                }
                            }

                            // 발사체의 앞 방향을 조정
                            bullet.transform.forward = direction;
                        }
                    }

                    // 별도의 포탄 모델을 지정할 수 있다.
                    // 게임에서는 사용되지 않음
                    if (Shell)
                    {
                        Transform shelloutpos = transform;

                        if (ShellOuter.Length > 0)
                            shelloutpos = ShellOuter[currentOuter];

                        GameObject shell;
                        
                        if (WeaponSystem.Pool != null)
                        {
                            shell = WeaponSystem.Pool.Instantiate(Shell, shelloutpos.position, Random.rotation, ShellLifeTime);
                        }
                        else
                        {
                            shell = Instantiate(Shell, shelloutpos.position, Random.rotation);
                            Destroy(shell.gameObject, ShellLifeTime);
                        }

                        Rigidbody shellRB = shell.GetComponent<Rigidbody>();

                        if (shellRB)
                            shellRB.AddForce(shelloutpos.forward * ShellOutForce);
                    }

                    // 사운드 효과 재생
                    if (SoundGun.Length > 0)
                        if (audioSource)
                            audioSource.PlayOneShot(SoundGun[Random.Range(0, SoundGun.Length)]);

                    nextFireTime += FireRate;
                }
            }
        }

        void OnDrawGizmos()
        {
            Gizmos.color = Color.yellow;

            Gizmos.DrawLine(transform.position + (transform.right * 0.1f), transform.position + transform.forward * 2);
            Gizmos.DrawLine(transform.position + (-transform.right * 0.1f), transform.position + transform.forward * 2);
            Gizmos.DrawLine(transform.position, transform.position + transform.forward * 2);
        }
    }
}
