using System.Collections;
using System.Collections.Generic;
using UnityEngine;

// 아군은 Player, 적은 Enemy 태그를 갖는다.
// 모든 AI가 상대를 찾기 위해 FindGameObjectsWithTag() 를 호출하면
// 부하가 너무 크기 때문에 최적화를 위해 작성된 풀이다.
public class CKB_TagObjectFinder : MonoBehaviour
{
    public static CKB_TagObjectFinder Instance;

    void Awake()
    {
        if (Instance == null)
            Instance = this;
    }

    // <태그 : 오브젝트 목록>을 딕셔너리로 관리한다.
    public Dictionary<string, GameObject[]> TargetList = new Dictionary<string, GameObject[]>();
    public List<bool> IsActive;

    void Start()
    {
        IsActive = new List<bool>();
    }

    // 초기화
    public void ClearTarget()
    {
        TargetList.Clear();
        TargetList = new Dictionary<string, GameObject[]>(1);
    }

    // 딕셔너리에서 태그에 해당하는 오브젝트 목록을 검색한다.
    public GameObject[] FindObjectWithTag(string tag)
    {
        // 태그가 있으면
        if (TargetList.ContainsKey(tag))
        {
            GameObject[] objsWithTag;

            // 목록을 가져와서
            if (TargetList.TryGetValue(tag, out objsWithTag))
            {
                Array.IndexOf(myDictionary.Keys.ToArray(), "a");

                // 활성화 상태로 바꾸고
                targetcollector.IsActive = true;
                // 반환한다.
                return targetcollector;
            }
            // 목록이 존재하지 않으면
            else
            {
                // null 반환.
                return null;
            }
        }
        // 태그가 없으면
        else
        {
            GameObject[] objsWithTag = GameObject.FindGameObjectsWithTag(tag);

            // 딕셔너리에 목록을 추가하고
            TargetList.Add(tag, objsWithTag);

            // 반환한다.
            return objsWithTag;
        }
    }

    // 매 프레임 마다
    void Update()
    {
        // 딕셔너리의 모든 목록 중에
        foreach (var target in TargetList)
        {
            if (target.Value != null)
            {
                // 목록이 활성화 상태이면
                if (target.Value.IsActive)
                {
                    // 목록을 갱신하고
                    target.Value.SetTarget(target.Key);
                    // 비활성화 상태로 바꾼다.
                    target.Value.IsActive = false;
                }
            }
        }
    }
}
