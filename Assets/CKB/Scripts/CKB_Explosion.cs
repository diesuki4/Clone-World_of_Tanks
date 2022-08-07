using System.Collections;
using System.Collections.Generic;
using UnityEngine;

// 폭발 소리 재생 및 물리 효과 적용
public class CKB_Explosion : MonoBehaviour
{
    [Header("폭발력")]
    public int Force;
    [Header("폭발 반경")]
    public int Radius;

    // 현재 위치에서 폭발 반경 내에 있는 오브젝트들 중
    // 힘을 가할 수 있는 오브젝트들에 Force 만큼 힘을 가한다.
    void Start()
    {
        Vector3 explosionPos = transform.position;
        Collider[] colliders = Physics.OverlapSphere(explosionPos, Radius);

        foreach (Collider hit in colliders)
        {
            Rigidbody rb = hit.GetComponent<Rigidbody>();

            if (!IsPlayer(hit.gameObject) && rb)
                rb.AddExplosionForce(Force, explosionPos, Radius, 3.0f);
        }
    }

    void Update() { }

    // 게임 오브젝트가 플레이어인지 확인
    bool IsPlayer(GameObject go)
    {
        Debug.Log("[TODO] [CKB_Explosion.cs] 플레이어인지 확인");
        return false;
    }
}
