using System.Collections;
using System.Collections.Generic;
using UnityEngine;

public class CKB_HPManager : MonoBehaviour
{
    [Header("죽었을 때 변경되는 모델")]
    public GameObject Effect;
    [Header("체력")]
    public float MaxHP = 200;
    // [HideInInspector]
    public float HP;
    // 마지막으로 자신을 맞춘 탱크
    [HideInInspector]
    public GameObject LatestHit;

    void Start()
    {
        HP = MaxHP;

        LatestHit = null;
    }

    void Update() { }

    // 데미지 적용
    public void ApplyDamage(GameObject shooter, float damage)
    {
        if (HP <= 0)
            return;

        // 마지막으로 자신을 맞춘 탱크를 저장하고
        LatestHit = shooter;
        // 데미지 적용 (오버라이딩)
        ApplyDamage(damage);
    }

    // 데미지 적용 (오버라이딩)
    public void ApplyDamage(float damage)
    {
        if (HP <= 0)
            return;

        // HP를 감소시킨다.
        HP -= damage;
        
        
        if(GetComponent<LSJ_MainBodySetting>())
        {
            LSJ_UIAnim.Instance.ShowLog(LSJ_UIAnim.PanelType.Right);
        }
        // 체력이 없으면 죽는다.
        if (HP <= 0)
            Dead();
    }

    // 죽음 처리
    public void Dead()
    {
        // 자신을 죽인 탱크의 점수를 증가시킨다.
        if (LatestHit)
            ++LatestHit.GetComponent<CKB_Tank>().killScore;

        // 등록된 모델을 생성한다.
        Instantiate(Effect, transform.position, transform.rotation);

        string o_tag = gameObject.tag;

        gameObject.tag = "Untagged";
        CKB_TagObjectFinder.Instance.UpdateTargetList(o_tag);

        HP = 0;

        //Destroy(gameObject);
        gameObject.SetActive(false);
    }
}
