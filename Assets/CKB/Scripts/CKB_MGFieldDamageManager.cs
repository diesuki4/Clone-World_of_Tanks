using System.Linq;
using System.Collections;
using System.Collections.Generic;
using UnityEngine;

public class CKB_MGFieldDamageManager : MonoBehaviour
{
    public static CKB_MGFieldDamageManager Instance;

    void Awake()
    {
        if (!Instance)
            Instance = this;
    }

    public LayerMask layerMask;
    public GameObject[] tanks;
    public float damagePerSec = 10;
    public float damageInterval = 1;
    [HideInInspector]
    public float fieldDistance;

    Ray ray;
    float rayDistance = 2000;
    float t;

    void Start()
    {
        ray = new Ray(transform.position, transform.forward);
        ray.origin = ray.GetPoint(rayDistance);
        ray.direction = -ray.direction;

        fieldDistance = 309.2057f;
    }

    void Update()
    {
        t += Time.deltaTime;

        if (damageInterval <= t)
        {
            RaycastHit hitInfo;

            if (Physics.Raycast(ray, out hitInfo, rayDistance, layerMask))
            {
                fieldDistance = Vector3.Distance(transform.position, hitInfo.point);

                GameObject[] outRangeTanks = tanks.Where(x => x && fieldDistance <= Vector3.Distance(transform.position, x.transform.position)).ToArray();

                foreach (GameObject tank in outRangeTanks)
                {
                    CKB_HPManager ckbHPManager = tank.GetComponent<CKB_HPManager>();

                    if (ckbHPManager)
                        ckbHPManager.ApplyDamage(damagePerSec);
                }
            }

            t = 0;
        }
    }
}
