using UnityEngine;
using System.Collections;
using HWRWeaponSystem;
using CnControls;

public class TankController : MonoBehaviour
{
	public bool UseMobileControl;
	private TankCamera tankCamera;
	private RadarSystem radar;
	public Tank TargetTank;

	void Start ()
	{
		tankCamera = this.GetComponent<TankCamera> ();
		radar = this.GetComponent<RadarSystem> ();
		if (tankCamera != null && TargetTank != null)
			tankCamera.AddCameras (TargetTank.gameObject);

		if (UseMobileControl) {
			Debug.LogWarning ("No MobileControllGUI object on the scene! please place it from Prefab/Game/MobileControllGUI.prefab");
		}
	}

	void KeyController ()
	{
		// This is an Input controller
		if (TargetTank == null || Time.timeScale == 0 || tankCamera == null)
			return;

		if (!UseMobileControl) {
			MouseLock.MouseLocked = true;

			if (Input.GetButton ("Fire1")) {
				FireWeapon ();
			}

			if (Input.GetButtonDown ("Fire2")) {
				SwithCamera ();
			}

			if (Input.GetKeyDown (KeyCode.Q)) {
				SwithGun ();
			}

			if (Input.GetKeyDown (KeyCode.Escape)) {
				TankGame.TankGameManager.Pause ();
			}
	
			Vector2 moveVector = new Vector2 (Input.GetAxis ("Horizontal"), Input.GetAxis ("Vertical"));
			TargetTank.Move (moveVector);

			Vector2 aimVector = new Vector2 (Input.GetAxis ("Mouse X"), Input.GetAxis ("Mouse Y"));
			TargetTank.Aim (aimVector);
		} else {
			// Need Mobile controller object in the scene.

			if (CnInputManager.GetButton ("Fire3")) {
				FireWeapon ();
			}
			if (CnInputManager.GetButtonDown ("Fire2")) {
				SwithCamera ();
			}

			Vector2 moveVector = new Vector2 (CnInputManager.GetAxis ("Horizontal"), CnInputManager.GetAxis ("Vertical"));
			Vector2 aimVector = new Vector2 (CnInputManager.GetAxis ("Horizontal2"), CnInputManager.GetAxis ("Vertical2")) * 0.1f;

			TargetTank.Aim (aimVector);
			TargetTank.Move (moveVector);
		}
	}

	public void SwithGun ()
	{
		if (TargetTank == null)
			return;
		TargetTank.SwitchWeapon ();
	}

	public void SwithCamera ()
	{
		if (TargetTank == null)
			return;
		tankCamera.SwitchCameras ();
	}

	public void FireWeapon ()
	{
		if (TargetTank == null)
			return;
		TargetTank.FireWeapon ();
	}


	void Update ()
	{

		if (TargetTank == null) {
			TankPlayer tankTarget = (TankPlayer)GameObject.FindObjectOfType (typeof(TankPlayer));
			if (tankTarget != null && tankTarget.IsMine && tankTarget.tank != null) {
				TargetTank = tankTarget.tank;
				if (tankCamera != null)
					tankCamera.AddCameras (TargetTank.gameObject);
			}

			if (tankCamera != null) {
				if (tankCamera.Target == null && tankTarget != null && tankTarget.tank != null) {
					if (tankTarget.tank.MainGun) {
						tankCamera.Target = TargetTank.MainGun.gameObject;
					} else {
						if (tankTarget.tank.Turret) {
							tankCamera.Target = TargetTank.Turret.gameObject;
						} else {
							tankCamera.Target = TargetTank.gameObject;
						}
					}
				}
			}
		}
		if (radar && TargetTank)
			radar.Player = TargetTank.gameObject;
		KeyController ();
	}
}
