using UnityEngine;
using System.Collections;

namespace HWRWeaponSystem
{
	public class WeaponController : MonoBehaviour
	{
		public string[] TargetTag = {"Enemy"};
		public WeaponLauncher[] WeaponLists;
		public int CurrentWeapon = 0;
		public bool ShowCrosshair = true;
	
		// 존재하는 모든 WeaponLauncher에 TargetTag 등록
		void Awake()
		{
			WeaponLauncher[] weas = transform.GetComponentsInChildren<WeaponLauncher>();

			if (weas.Length > 0)
			{
				WeaponLists = new WeaponLauncher[weas.Length];

				for (int i = 0; i < weas.Length; ++i)
				{
					WeaponLists[i] = weas[i].GetComponent<WeaponLauncher>();
					WeaponLists[i].TargetTag = TargetTag;
				}
			}
		}

		// 현재 무기 번호에 해당하는 WeaponLauncher 반환
		public WeaponLauncher GetCurrentWeapon()
		{
			if (CurrentWeapon < WeaponLists.Length && WeaponLists[CurrentWeapon] != null)
				return WeaponLists[CurrentWeapon];
				
			return null;
		}
	
		// 모든 WeaponLauncher에 TargetTag 등록
		void Start()
		{
			for (int i = 0; i < WeaponLists.Length; ++i)
			{
				if (WeaponLists[i] != null)
				{
					WeaponLists[i].TargetTag = TargetTag;
					// 크로스 헤어 표시 여부
					WeaponLists[i].ShowCrosshair = ShowCrosshair;
				}
			}
		}

		// 매 프레임마다 현재 무기 번호에 해당하는 무기만 활성화
		void Update()
		{
			for (int i = 0; i < WeaponLists.Length; ++i)
				if (WeaponLists[i] != null)
					WeaponLists[i].OnActive = false;

			if (CurrentWeapon < WeaponLists.Length && WeaponLists[CurrentWeapon] != null)
				WeaponLists[CurrentWeapon].OnActive = true;
		}
	
		// 무기 번호를 1씩 증가시켜 무기를 변경
		public void SwitchWeapon()
		{
			++CurrentWeapon;

			if (CurrentWeapon >= WeaponLists.Length)
				CurrentWeapon = 0;

			for (int i = 0; i < WeaponLists.Length; ++i)
			{
				// 해당 무기 번호만 발사 가능 상태로 즉시 전환
				if (CurrentWeapon == i)
					WeaponLists[i].OnActive = true;
					//HideWeapon(WeaponLists[i].gameObject,true);
				else
					//HideWeapon(WeaponLists[i].gameObject,false);
					WeaponLists[i].OnActive = false;
			}
		}
	
		// show 값에 따라 무기를 보이지 않게 처리
		public void HideWeapon(GameObject weapon, bool show)
		{
			foreach (Renderer render in weapon.GetComponentsInChildren<Renderer>())
				render.enabled = show;
		}
	
		// 현재 설정된 무기로 발사
		public void LaunchWeapon()
		{
			if (CurrentWeapon < WeaponLists.Length && WeaponLists[CurrentWeapon] != null)
				WeaponLists[CurrentWeapon].Shoot();
		}

		// index 번호에 해당하는 무기로 발사
		public void LaunchWeapon(int index)
		{
			CurrentWeapon = index;

			if (CurrentWeapon < WeaponLists.Length && WeaponLists [index] != null)
				WeaponLists[index].Shoot();
		}
	}
}
