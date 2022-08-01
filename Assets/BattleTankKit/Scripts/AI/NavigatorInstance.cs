using UnityEngine;
using UnityEngine.AI;
using System.Collections;

public class NavigatorInstance : MonoBehaviour
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
		if (Navigator)
			Navigator.SetDestination(target);
	}

	// 사용되지 않음
	public Vector3 GetDirection()
	{
		if (Navigator)
			return (Navigator.velocity - Owner.transform.position).normalized;
		else	
			return Vector3.zero;
	}

	void Update ()
	{
		// 탱크가 죽었으면
		if (!Owner)
		{
			// 소멸
			Destroy(gameObject);
		}
	}
}
