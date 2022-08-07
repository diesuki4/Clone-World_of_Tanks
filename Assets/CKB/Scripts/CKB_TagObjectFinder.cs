using System;
using System.Linq;
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
    Dictionary<string, GameObject[]> TargetList;
    List<bool> IsActive;

    void Start()
    {
        TargetList = new Dictionary<string, GameObject[]>();
        IsActive = new List<bool>();
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
                int idx = Array.IndexOf(TargetList.Keys.ToArray(), tag);

                // 활성화 상태로 바꾸고
                IsActive[idx] = true;
                // 반환한다.
                return objsWithTag;
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
            // 상태도 추가한다.
            IsActive.Add(false);

            // 반환한다.
            return objsWithTag;
        }
    }

    // 매 프레임 마다
    void Update()
    {
        int length = TargetList.Count;

        // 딕셔너리의 모든 목록 중에
        for (int i = 0; i < length; ++i)
        {
            if (TargetList.ElementAt(i).Value != null)
            {
                // 목록이 활성화 상태이면
                if (IsActive[i])
                {
                    string key = TargetList.ElementAt(i).Key;
                    // 목록을 갱신하고
                    TargetList[key] = GameObject.FindGameObjectsWithTag(key);
                    // 비활성화 상태로 바꾼다.
                    IsActive[i] = false;
                }
            }
        }
    }
}
