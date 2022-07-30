using UnityEngine;
using System.Collections;

// 플레이어 탱크 구분을 위한 컴포넌트
[RequireComponent (typeof(Tank))]
public class TankPlayer : MonoBehaviour
{
	[Header("이름")]
	public string Name;
	[Header("플레이어 탱크 여부")]
	public bool IsMine;
	// 탱크 컴포넌트
	[HideInInspector]
	public Tank tank;

	void Awake()
	{
		tank = this.GetComponent<Tank>();
	}

	void Start() { }
}
