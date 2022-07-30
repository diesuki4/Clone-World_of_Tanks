using System.Collections;
using System.Collections.Generic;
using UnityEngine;

// 아군은 Player, 적은 Enemy 태그를 갖는다.
// 모든 AI가 상대를 찾기 위해 FindGameObjectsWithTag() 를 호출하면
// 부하가 너무 크기 때문에 최적화를 위해 작성된 풀이다.

namespace HWRWeaponSystem
{
	public class FinderPool : MonoBehaviour
	{
		// 태그 : 오브젝트 목록을 딕셔너리로 관리한다.
		public Dictionary<string, TargetCollector> TargetList = new Dictionary<string, TargetCollector>();
		public int TargetTypeCount = 0;

		// 초기화
		public void ClearTarget()
		{
			TargetList.Clear();
			TargetList = new Dictionary<string, TargetCollector>(1);
		}

		// 딕셔너리에서 태그에 해당하는 오브젝트 목록을 검색한다.
		public TargetCollector FindTargetTag(string tag)
		{
			// 태그가 있으면
			if (TargetList.ContainsKey(tag))
			{
				TargetCollector targetcollector;

				// 목록을 가져와서
				if (TargetList.TryGetValue(tag, out targetcollector))
				{
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
				TargetCollector targetcollector = new TargetCollector(tag);

				// 딕셔너리에 목록을 추가하고
				TargetList.Add(tag, targetcollector);

				// 반환한다.
				return targetcollector;
			}
		}

		// 매 프레임 마다
		void Update()
		{
			int count = 0;

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
						
						++count;
					}
				}
			}

			if (count > TargetTypeCount)
				TargetTypeCount = count;
		}
	}

	// 특정 태그를 갖는 오브젝트의 목록을 관리한다.
	public class TargetCollector
	{
		// 특정 태그를 갖는 오브젝트의 배열
		public GameObject[] Targets;
		public bool IsActive;

		public TargetCollector(string tag)
		{
			SetTarget(tag);
		}

		// 해당 태그를 갖는 코든 오브젝트를 배열에 저장한다.
		public void SetTarget(string tag)
		{
			Targets = GameObject.FindGameObjectsWithTag(tag);
		}
	}
}
