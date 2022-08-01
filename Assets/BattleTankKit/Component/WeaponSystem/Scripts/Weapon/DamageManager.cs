using UnityEngine;
using System.Collections;

namespace HWRWeaponSystem
{
	public class DamageManager : MonoBehaviour
	{
		[Header("죽었을 때 변경되는 모델")]
		public GameObject Effect;
		[Header("체력")]
		public int HP = 100;
		// 마지막으로 자신을 맞춘 탱크
		[HideInInspector]
		public GameObject LatestHit;

		ObjectPool objPool;

		void Awake()
		{
			objPool = GetComponent<ObjectPool>();	
		}
	
		void Start() { }

		// 데미지 적용
		public void ApplyDamage(DamagePack damage)
		{
			if (HP < 0)
				return;

			// 마지막으로 자신을 맞춘 탱크를 저장하고
			LatestHit = damage.Owner;
			// HP를 감소시킨다.
			HP -= damage.Damage;

			// 체력이 없으면 죽는다.
			if (HP <= 0)
				Dead();
		}

		// 데미지 적용 (오버라이딩)
		public void ApplyDamage(int damage)
		{
			if (HP < 0)
				return;

			// HP를 감소시킨다.
			HP -= damage;

			// 체력이 없으면 죽는다.
			if (HP <= 0)
				Dead();
		}

		// 죽음 처리
		public void Dead()
		{
			// 등록된 모델이 있을 때
			if (Effect)
			{
				// 오브젝트 풀을 사용 중이면
				if (WeaponSystem.Pool != null && Effect.GetComponent<ObjectPool>())
					// 풀에서 모델을 가져온다.
					WeaponSystem.Pool.Instantiate(Effect, transform.position, transform.rotation);
				// 오브젝트 풀을 사용 안 하고 있으면
				else
					// 그냥 생성한다.
					Instantiate(Effect, transform.position, transform.rotation);
			}

			// 게임 오브젝트를 삭제
			if (objPool != null)
				objPool.Destroying();
			else
				Destroy(gameObject);

			// 플레이어 점수를 증가시키는 루틴
			gameObject.SendMessage("OnDead", SendMessageOptions.DontRequireReceiver);
		}
	}
}
