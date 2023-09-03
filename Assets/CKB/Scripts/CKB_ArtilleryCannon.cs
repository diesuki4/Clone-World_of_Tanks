using System.Collections;
using System.Collections.Generic;
using UnityEngine;

public class CKB_ArtilleryCannon : MonoBehaviour
{
    public GameObject artilleryMissile;
    public GameObject bullet;
    public Transform muzzle;
    public float missileDeadline;
    public float rotSensitivity;

    GameObject shooter;
    GameObject target;
    float damage;

    float artilleryModeTurretAngleX;
    
    void Start()
    {
        artilleryModeTurretAngleX = -transform.GetChild(0).eulerAngles.x;
    }

    void Update()
    {/*
        Vector3 targetDir = Vector3.ProjectOnPlane(target.transform.position - transform.position, Vector3.up).normalized;
        Quaternion destinationRot = Quaternion.LookRotation(targetDir);

        transform.rotation = Quaternion.Lerp(transform.rotation, destinationRot, Time.deltaTime * rotSensitivity);
    */}

    public void Initialize(GameObject shooter)
    {
        this.shooter = shooter;
        this.damage = shooter.GetComponent<CKB_Artillery>().damage;
    }

    public void Fire(CKB_Artillery.ShootMode shootMode)
    {
        if (shootMode == CKB_Artillery.ShootMode.Artillery)
        {
            CKB_ArtilleryMissile artyMissile = Instantiate(artilleryMissile, muzzle.position, muzzle.rotation).GetComponent<CKB_ArtilleryMissile>();
            
            artyMissile.Initialize(shooter, damage, target.transform.position, missileDeadline);
        }
        else if (shootMode == CKB_Artillery.ShootMode.Normal)
        {
            CKB_Bullet ckbBullet = Instantiate(bullet, muzzle.transform.position, Quaternion.LookRotation(muzzle.transform.forward)).GetComponent<CKB_Bullet>();

            ckbBullet.Initialize(shooter, damage);
        }
    }

    public void SetTarget(GameObject target)
    {
        this.target = target;
    }

    // 현재 모드에 따라 터렛 내리기 / 올리기
    // target 을 인자로 넘겨주면 해당 방향으로 Y축 회전도 적용
    IEnumerator RotateTurret(CKB_Artillery.ShootMode shootMode)
    {
        // 목표 각도를 정한다.
        float destinationAngleX = (shootMode == CKB_Artillery.ShootMode.Artillery) ? (0) : (artilleryModeTurretAngleX);
        float destinationAngleY = 0;
        Quaternion destinationRot = Quaternion.Euler(Vector3.zero);

        do
        {
            // 타겟이 있으면
            if (target)
                // 타겟 방향의 Y축 회전도 적용
                destinationAngleY = Vector3.ProjectOnPlane(target.transform.position - transform.position, Vector3.up).normalized.y;
            // 타겟이 없으면
            else
                // Y축 회전하지 않음
                destinationAngleY = transform.eulerAngles.y;
            // 목표 회전 값을 정한다.
            // X축, Y축 회전만 변경
            destinationRot = Quaternion.Euler(destinationAngleX, destinationAngleY, transform.eulerAngles.z);
            // 서서히 목표 회전값으로 회전한다.
            transform.rotation = Quaternion.Lerp(transform.rotation, destinationRot, Time.deltaTime * rotSensitivity);
            // 한 프레임을 넘긴다.
            yield return null;
        }
        // 각도 차이가 1도보다 큰 동안 터렛을 회전한다.
        while (1 < Quaternion.Angle(transform.rotation, destinationRot));
        
        // 최종 회전 값을 적용한다.
        transform.rotation = destinationRot;
    }
}
