using System.Collections;
using System.Collections.Generic;
using UnityEngine;

public class CKB_ArtilleryCannon2 : MonoBehaviour
{
    public GameObject missile;
    public Transform muzzle;
    public float missileDeadline;
    public float rotSensitivity;

    GameObject shooter;
    GameObject target;
    float damage;

    void Start()  { }

    void Update()
    {
        TurretFollowTarget();
    }

    void TurretFollowTarget()
    {
        if (target == null)
            return;

        Vector3 targetDir = Vector3.ProjectOnPlane(target.transform.position - transform.position, Vector3.up).normalized;
        Quaternion destinationRot = Quaternion.LookRotation(targetDir);

        transform.rotation = Quaternion.Lerp(transform.rotation, destinationRot, Time.deltaTime * rotSensitivity);
    }

    public void Initialize(GameObject shooter)
    {
        this.shooter = shooter;
        this.damage = shooter.GetComponent<CKB_Artillery3>().damage;
    }

    public void Fire()
    {
        CKB_ArtilleryMissile artyMissile = Instantiate(missile, muzzle.position, muzzle.rotation).GetComponent<CKB_ArtilleryMissile>();
        
        artyMissile.Initialize(shooter, damage, target.transform.position, missileDeadline);
    }

    public void SetTarget(GameObject target)
    {
        this.target = target;
    }

    public IEnumerator RotateTurretToTarget(float angleUntil)
    {
        // 각도 차이가 angleUntil 보다 큰 동안 터렛을 회전한다.
        while (angleUntil < Quaternion.Angle(Quaternion.LookRotation(Vector3.ProjectOnPlane(transform.forward, Vector3.up)),
                    Quaternion.LookRotation(Vector3.ProjectOnPlane(target.transform.position - transform.position, Vector3.up).normalized)))
            yield return null;
    }
}
