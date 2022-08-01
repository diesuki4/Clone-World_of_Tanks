using UnityEngine;
using System.Collections;
using HWRWeaponSystem;

public class DamageDecay : MonoBehaviour
{
	// HP가 이 값보다 작을 경우 불(파손) 이펙트가 표시된다.
	// 여러 개를 지정할 수 있으므로 HP에 따라 여러 개의 이펙트를 표시할 수 있다.
	[Header("이펙트 표시에 사용되는 기준 체력들")]
	public int[] DamageLowerThan = {10};
	// HP가 DamageLowerThan[i]보다 낮을 경우 표시될 불(파손) 이펙트
	[Header("기준 체력에 따라 표시되는 이펙트들")]
	public GameObject[] DecayObject;

	DamageManager damage;

	void Start()
	{
		damage = GetComponent<DamageManager>();
	}

	void Update()
	{
		if (damage == null ||
			/* 설정한 HP의 개수와 이펙트 개수가 다르거나 */
			DecayObject.Length != DamageLowerThan.Length ||
			/* 등록한 이펙트가 없으면 */
			DecayObject.Length <= 0)
			// 아무 것도 하지 않는다.
			return;

		for (int i = 0; i < DecayObject.Length; ++i)
			if (damage.HP > DamageLowerThan[i])
				DecayObject[i].SetActive(false);

		for (int i = 0; i < DecayObject.Length; ++i)
			if (damage.HP < DamageLowerThan[i])
				DecayObject[i].SetActive(true);
	}
}
