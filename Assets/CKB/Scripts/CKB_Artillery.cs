using System.Linq;
using System.Collections;
using System.Collections.Generic;
using UnityEngine;
using UnityEngine.AI;

public class CKB_Artillery : MonoBehaviour
{
    public enum ShootMode
    {
        Normal = 1,
        Artillery = 2
    }
    public ShootMode shootMode;

    public CKB_ArtilleryCannon ckbArtyCannon;
    public float fireEverySecsNormal;
    public float fireEverySecsArtillery;
    public float maxFireRangeNormal;
    public float minFireRangeArtillery;
    public float maxFireRangeArtillery;
    public float damage = 120;

    public Transform[] destinations;
    int currentDestination;
    public bool switchMode;
    
    NavMeshAgent agent;

    IEnumerator Start()
    {
        shootMode = ShootMode.Normal;

        ckbArtyCannon.Initialize(gameObject);

        currentDestination = 0;

        agent = GetComponent<NavMeshAgent>();
        agent.SetDestination(destinations[currentDestination].position);

        while (true)
            yield return StartCoroutine(shootMode.ToString());
    }

    void Update() { }

    // 일반 모드 (다음 목적지로 이동)
    IEnumerator Normal()
    {
        // 거리가 0.1보다 큰 동안 목적지로 이동한다.
        while (0.1f < Vector3.Distance(transform.position, destinations[currentDestination].position))
        {
            // 지정 가능한 가장 가까운 타겟을 찾는다.
            GameObject _target = FindTarget(0, maxFireRangeNormal, true);
            // 찾은 타겟이 있으면
            if (_target)
            {
                // 타겟 방향으로 터렛이 회전한다.
                //yield return StartCoroutine(RotateTurret(_target));
                // 타겟 방향의 회전각
                Quaternion dir = Quaternion.LookRotation(_target.transform.position - ckbArtyCannon.muzzle.transform.position);
                // 회전 결과, 터렛과 타겟의 각도 차이가 2도 이하이면
                if (Quaternion.Angle(ckbArtyCannon.muzzle.transform.rotation, dir) <= 2)
                {
                    // 총알 발사
                    ckbArtyCannon.Fire(shootMode);
                    // 발사 쿨타임 동안 대기
                    yield return new WaitForSeconds(fireEverySecsNormal);
                }
            }
            // 한 프레임 넘기기
            yield return null;
        }

        // 최종 목적지 적용
        transform.position = destinations[currentDestination].position;
        // 도착했으니 자주포 모드로 전환
        shootMode = ShootMode.Artillery;
        // 지정 가능한 타겟을 찾는다.
        GameObject target = FindTarget(minFireRangeArtillery, maxFireRangeArtillery);
        // 터렛 올리기 (target 이 null 이면 X축 회전만 적용된다)
        //yield return StartCoroutine(RotateTurret(target));
        // 찾은 타겟이 있으면
        if (target)
            // 미사일 발사
            ckbArtyCannon.Fire(shootMode);
    }

    // 자주포 모드
    IEnumerator Artillery()
    {
        // 지정 가능한 타겟을 찾는다.
        GameObject target = FindTarget(minFireRangeArtillery, maxFireRangeArtillery);
        // 찾은 타겟이 있으면
        if (target)
        {
            // 터렛이 지속적으로 타겟 방향으로 Y축 회전한다.
            ckbArtyCannon.SetTarget(target);
            // 미사일 발사
            ckbArtyCannon.Fire(shootMode);
            // 발사 쿨타임 동안 대기한다.
            yield return new WaitForSeconds(fireEverySecsArtillery);
        }
        // 찾은 타겟이 없으면
        else
        {
            // 한 프레임을 넘긴다.
            yield return null;
        }

        // 이동 시간이 됐거나 자기장 영역이 근접했으면
        if (switchMode)
        {
            // 일반 모드로 전환하고
            shootMode = ShootMode.Normal;
            switchMode = false;
            // 터렛을 내리고
            //yield return StartCoroutine(RotateTurret());
            // 다음 목적지로 이동한다.
            agent.SetDestination(destinations[++currentDestination].position);
        }
    }

    // minDistance ~ maxDistance 내에 있는 전차를 랜덤하게 타겟으로 지정한다.
    // bClosest 가 true 이면 가장 가까운 전차를 지정한다.
    // 범위 내에 전차가 없을 시 null
    GameObject FindTarget(float minDistance, float maxDistance, bool bClosest = false)
    {
        // 상대방의 태그를 찾는다.
        string opponentTag = CKB_TagObjectFinder.Instance.OpponentTag(gameObject.tag);
        // 해당 태그를 가진 모든 오브젝트를 찾는다.
        GameObject[] objsWithTag = CKB_TagObjectFinder.Instance.FindObjectWithTag(opponentTag);
        // minDistance ~ maxDistance 내에 있는 오브젝트를 필터링한다.
        GameObject[] inRangeObjs = objsWithTag.Where(x => minDistance <= Vector3.Distance(transform.position, x.transform.position)
                                                        &&  Vector3.Distance(transform.position, x.transform.position) <= maxDistance).ToArray();
        // 후보가 1대 이상일 때
        if (1 <= inRangeObjs.Length)
            // bClosest 가 켜져 있으면 가장 가까운 전차를,
            // 아니면 그 중 랜덤하게 타겟을 지정한다.
            return (bClosest) ? inRangeObjs.OrderBy(x => Vector3.Distance(transform.position, x.transform.position)).First()
                                : inRangeObjs[Random.Range(0, inRangeObjs.Length)];
        // 후보를 찾지 못했으면
        else
            // null 반환
            return null;
    }
}
