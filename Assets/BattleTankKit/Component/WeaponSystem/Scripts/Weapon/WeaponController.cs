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
	
		void Awake()
		{
			WeaponLauncher[] weas = transform.GetComponentsInChildren<WeaponLauncher>();

			// find all attached weapons.
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

		public WeaponLauncher GetCurrentWeapon()
		{
			if (CurrentWeapon < WeaponLists.Length && WeaponLists[CurrentWeapon] != null)
				return WeaponLists[CurrentWeapon];
				
			return null;
		}
	
		void Start()
		{
			for (int i = 0; i < WeaponLists.Length; ++i)
			{
				if (WeaponLists[i] != null)
				{
					WeaponLists[i].TargetTag = TargetTag;
					WeaponLists[i].ShowCrosshair = ShowCrosshair;
				}
			}
		}

		void Update()
		{
			for (int i = 0; i < WeaponLists.Length; ++i)
				if (WeaponLists[i] != null)
					WeaponLists[i].OnActive = false;

			if (CurrentWeapon < WeaponLists.Length && WeaponLists[CurrentWeapon] != null)
				WeaponLists[CurrentWeapon].OnActive = true;
		}
	
		public void LaunchWeapon(int index)
		{
			CurrentWeapon = index;

			if (CurrentWeapon < WeaponLists.Length && WeaponLists [index] != null)
				WeaponLists [index].Shoot();
		}
	
		public void SwitchWeapon()
		{
			++CurrentWeapon;

			if (CurrentWeapon >= WeaponLists.Length)
				CurrentWeapon = 0;

			for (int i = 0; i < WeaponLists.Length; ++i)
			{
				if (CurrentWeapon == i)
					WeaponLists[i].OnActive = true;
					//HideWeapon(WeaponLists[i].gameObject,true);
				else
					//HideWeapon(WeaponLists[i].gameObject,false);
					WeaponLists[i].OnActive = false;
			}
		}
	
		public void HideWeapon(GameObject weapon, bool show)
		{
			foreach (Renderer render in weapon.GetComponentsInChildren<Renderer>())
				render.enabled = show;
		}
	
		public void LaunchWeapon()
		{
			if (CurrentWeapon < WeaponLists.Length && WeaponLists[CurrentWeapon] != null)
				WeaponLists[CurrentWeapon].Shoot();
		}
	}
}
