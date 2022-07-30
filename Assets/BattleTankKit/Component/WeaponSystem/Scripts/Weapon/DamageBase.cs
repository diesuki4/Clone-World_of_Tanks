using UnityEngine;
using System.Collections;

namespace HWRWeaponSystem
{
	public class DamageBase : MonoBehaviour
	{
		[Header("발사체 타격 효과")]
		public GameObject Effect;
		[HideInInspector]
		public GameObject Owner;
		[Header("데미지")]
		public int Damage = 20;
		[HideInInspector]
		public ObjectPool objectPool;
		public string[] TargetTag = {"Enemy"};
		[Header("충돌하지 않을 태그")]
		public string[] IgnoreTag;
	
		// 데미지 처리가 가능한지 검사 (false : 불가, true : 가능)
		public bool DoDamageCheck(GameObject gob)
		{
			// 아군이면 불가
			if (gob.tag == Owner.tag)
				return false;

			// IgnoreTag 에 등록되어 있으면 불가
			for (int i = 0; i < IgnoreTag.Length; ++i)
				if (IgnoreTag[i] == gob.tag)
					return false;

			// 나머지는 가능
			return true;
		}
		
		// 발사체를 쏜 탱크 자신과는 충돌하지 않도록 처리
		public void IgnoreSelf(GameObject owner)
		{
			Collider col = GetComponent<Collider>();

			if (col && owner)
			{
				Collider ownrCol = owner.GetComponent<Collider>();

				if (ownrCol)
					Physics.IgnoreCollision(col, ownrCol);

				if (Owner.transform.root)
					foreach (Collider collider in Owner.transform.root.GetComponentsInChildren<Collider>())
						Physics.IgnoreCollision(col, collider);
			}
		}
	}

	// 데미지 처리용 구조체
	public struct DamagePack
	{
		// 데미지
		public int Damage;
		// 쏜 탱크
		public GameObject Owner;
	}
}
