using System.Collections;
using System.Collections.Generic;
using UnityEngine;

public class CKB_HPManager : MonoBehaviour
{
    [Header("죽었을 때 변경되는 모델")]
    public GameObject Effect;
    [Header("체력")]
    public int HP = 100;
    // 마지막으로 자신을 맞춘 탱크
    [HideInInspector]
    public GameObject LatestHit;

    void Start() { }

    void Update() { }

    // 데미지 적용
    public void ApplyDamage(GameObject shooter, int damage)
    {
        if (HP < 0)
            return;

        // 마지막으로 자신을 맞춘 탱크를 저장하고
        LatestHit = shooter;
        // 데미지 적용 (오버라이딩)
        ApplyDamage(damage);
    }

    // 데미지 적용 (오버라이딩)
    public void ApplyDamage(int damage)
    {
        if (HP < 0)
            return;

        // HP를 감소시킨다.
        HP -= damage;

        // 체력이 없으면 죽는다.
        if (HP <= 0)
            Dead();
    }

    // 죽음 처리
    public void Dead()
    {
        // 등록된 모델을 생성한다.
        Instantiate(Effect, transform.position, transform.rotation);

        Destroy(gameObject);
    }
}
