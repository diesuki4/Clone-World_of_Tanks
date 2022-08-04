using UnityEngine;
using System.Collections;

namespace HWRWeaponSystem
{
	public class WeaponBase : MonoBehaviour
	{
		[HideInInspector]
		public GameObject Owner;
		[HideInInspector]
		public GameObject Target;
		[HideInInspector]
		public ObjectPool objectPool;
		public string[] TargetTag = {"Enemy"};
		[Header("충돌하지 않을 태그")]
		public string[] IgnoreTag;
		[Header("발사체 Rigidbody 모드")]
		public bool RigidbodyProjectile;
		[Header("사용되지 않음")]
		public Vector3 TorqueSpeedAxis;
		[Header("사용되지 않음")]
		public GameObject TorqueObject;
	
		// 데미지 처리가 가능한지 검사 (false : 불가, true : 가능)
		public bool DoDamageCheck(GameObject gob)
		{
			// 이미 죽었거나 아군이면 불가
			if (!gob || gob.transform.root.tag == Owner.tag)
				return false;

			// IgnoreTag 에 등록되어 있으면 불가
			for (int i = 0; i < IgnoreTag.Length; ++i)
				if (IgnoreTag[i] == gob.tag)
					return false;

			// 나머지는 가능
			return true;
		}

		public virtual void OnSpawn() { }
	}
}
