using System.Collections;
using System.Collections.Generic;
using UnityEngine;
using UnityEngine.AI;

// 네비게이터를 이동시키고 탱크가 따라가는 방식으로 작동
public class CKB_Navigator_old : MonoBehaviour
{
	[Header("탱크 자신")]
	public GameObject Owner;
	public NavMeshAgent Navigator;

	void Start()
	{
		Navigator = GetComponent<NavMeshAgent>();
	}

	// NavMeshAgent 목적지 지정
	public void SetDestination(Vector3 target)
	{
		Navigator.SetDestination(target);
	}

	void Update ()
	{
		// 탱크가 죽었으면
		if (!Owner)
			// 소멸
			Destroy(gameObject);
	}
}
