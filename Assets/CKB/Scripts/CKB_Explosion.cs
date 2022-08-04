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
    [Header("랜덤하게 재생할 사운드")]
    public AudioClip[] Sounds;

    // 현재 위치에서 폭발 반경 내에 있는 오브젝트들 중
    // 힘을 가할 수 있는 오브젝트들에 Force 만큼 힘을 가한다.
    void OnEnable ()
    {
        Vector3 explosionPos = transform.position;
        Collider[] colliders = Physics.OverlapSphere(explosionPos, Radius);

        if (Sounds.Length > 0)
            AudioSource.PlayClipAtPoint(Sounds[Random.Range(0, Sounds.Length)], transform.position);

        foreach (Collider hit in colliders)
        {
            Rigidbody rb = hit.GetComponent<Rigidbody>();

            if (rb)
                rb.AddExplosionForce(Force, explosionPos, Radius, 3.0f);
        }
    }

    void Start () { }
}
