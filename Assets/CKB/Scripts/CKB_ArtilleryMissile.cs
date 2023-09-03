using System.Collections;
using System.Collections.Generic;
using UnityEngine;

public class CKB_ArtilleryMissile : MonoBehaviour
{
    GameObject shooter;

    public float curvature;
    public float damageRadius;
    public GameObject explosionVFX;

    Vector3 destination;
    Vector3 originPos;
    Vector3 prevPos;

    float damage;
    float deadline;
    float t;

    void Start() { }

    void Update()
    {
        t += Time.deltaTime;

        Vector3 center = (originPos + destination) * 0.5f - new Vector3(0, curvature, 0);
        Vector3 relOriginPos = originPos - center;
        Vector3 relDestination = destination - center;

        transform.position = Vector3.Slerp(relOriginPos, relDestination, t / deadline) + center;
        transform.forward = (transform.position - prevPos).normalized;

        if (deadline <= t)
            Explosion();
    }

    public void Initialize(GameObject shooter, float damage, Vector3 destination, float deadline)
    {
        this.shooter = shooter;
        this.damage = damage;
        this.destination = destination;
        this.deadline = deadline;

        originPos = prevPos = transform.position;
    }

    public bool IsDamageAvailable(GameObject go)
    {
        if (go && GetTankTransform(go.transform).CompareTag(CKB_TagObjectFinder.Instance.OpponentTag(shooter.tag)))
            return true;
        else
            return false;
    }

    Transform GetTankTransform(Transform tr)
	{
        if (tr.GetComponent<CKB_HPManager>() || tr == tr.root)
            return tr;

		return GetTankTransform(tr.parent);
	}

    void Explosion()
    {
        GameObject expVFX = Instantiate(explosionVFX, transform.position, transform.rotation);
        Destroy(expVFX, 3);

        Collider[] hitCols = Physics.OverlapSphere(transform.position, damageRadius, 1 << CKB_TagObjectFinder.Instance.OpponentLayer(shooter.layer));

        foreach (Collider col in hitCols)
        {
            if (!col)
                continue;

            if (IsDamageAvailable(col.gameObject))
            {
                if (!shooter || (shooter && col.gameObject != shooter.gameObject))
                {
                    CKB_HPManager ckbHPManager = col.transform.GetComponent<CKB_HPManager>();

                    if (ckbHPManager)
                        ckbHPManager.ApplyDamage((int)(Mathf.Lerp(damage, 0f, Vector3.Distance(transform.position, col.transform.position) / damageRadius)));
                }
            }
        }

        Destroy(gameObject);
    }

    void OnCollisionEnter(Collision other)
    {
        if (IsDamageAvailable(other.gameObject) && other.gameObject.tag != gameObject.tag)
            Explosion();
    }
}
