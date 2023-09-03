using System.Linq;
using System.Collections;
using System.Collections.Generic;
using UnityEngine;
using UnityEngine.AI;

public class CKB_Artillery2 : MonoBehaviour
{
    public CKB_ArtilleryCannon2 ckbArtyCannon;
    public float fireEverySecs;
    public float minFireRange;
    public float maxFireRange;
    public float damage = 120;

    [HideInInspector]
    // UI 표시용 발사 쿨타임
    public float fireCoolTime;

    IEnumerator Start()
    {
        ckbArtyCannon.Initialize(gameObject);

        fireCoolTime = fireEverySecs;

        while (true)
            yield return StartCoroutine(IEFire());
    }

    void Update() { fireCoolTime += Time.deltaTime; }

    IEnumerator IEFire()
    {
        // 지정 가능한 타겟을 찾는다.
        GameObject target = FindTarget(minFireRange, maxFireRange);
        // 찾은 타겟이 있으면
        if (target)
        {
            // 터렛에 타겟을 지정하고
            ckbArtyCannon.SetTarget(target);
            // 터렛이 타겟 방향으로 Y축 회전한다.
            yield return StartCoroutine(ckbArtyCannon.RotateTurretToTarget(1));
            // 미사일 발사
            ckbArtyCannon.Fire();
            // 표시용 발사 쿨타임을 초기화
            fireCoolTime = 0;
            // 발사 쿨타임 동안 대기한다.
            yield return new WaitForSeconds(fireEverySecs);
        }
        // 찾은 타겟이 없으면
        else
        {
            // 한 프레임을 넘긴다.
            yield return null;
        }
    }

    // minDistance ~ maxDistance 내에 있는 전차를 랜덤하게 타겟으로 지정한다.
    // bClosest 가 true 이면 가장 가까운 전차를 지정한다.
    // 범위 내에 전차가 없을 시 null
    GameObject FindTarget(float minDistance, float maxDistance)
    {
        // 상대의 태그를 찾는다.
        string opponentTag = CKB_TagObjectFinder.Instance.OpponentTag(gameObject.tag);
        // 해당 태그를 가진 모든 오브젝트를 찾는다.
        GameObject[] objsWithTag = CKB_TagObjectFinder.Instance.FindObjectWithTag(opponentTag);
        // minDistance ~ maxDistance 내에 있는 오브젝트를 필터링한다.
        GameObject[] inRangeObjs = objsWithTag.Where(x => minDistance <= Vector3.Distance(transform.position, x.transform.position)
                                                        &&  Vector3.Distance(transform.position, x.transform.position) <= maxDistance).ToArray();
        // 후보가 1대 이상일 때
        if (1 <= inRangeObjs.Length)
            // 랜덤하게 타겟을 지정한다.
            return inRangeObjs[Random.Range(0, inRangeObjs.Length)];
        // 후보를 찾지 못했으면
        else
            // null 반환
            return null;
    }
}
