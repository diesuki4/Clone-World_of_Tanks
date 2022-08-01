using UnityEngine;
using System.Collections;
using HWRWeaponSystem;

[RequireComponent (typeof(AudioSource))]
[RequireComponent (typeof(WeaponController))]
public class Tank : DamageManager
{
	public GameObject Turret;
	public GameObject MainGun;
	public Vector3 TurretBaseAxis = new Vector3(0, 1, 0);
	public Vector3 MainGunBaseAxis = new Vector3(1, 0, 0);

	public float TurretSpeed = 20;
	public float TankSpeed = 5;
	public float TurnSpeed = 20;

	public float MaingunMinTurnX = -10;
	public float MaingunMaxTurnX = 45;

	public float FlipRatio = 0.7f;
	public float FlipDamage = 3;

	float rotationX = 0;
	Vector3 turrentEulerAngle;
	Vector3 maingunEulerAngle;
	TankTrackAnimation track;
	AudioSource audioSource;
	Vector3 positionTemp;

	[HideInInspector]
	public WeaponController weapon;
	[HideInInspector]
	public float AimingAngle;

	void Awake()
	{
		track = GetComponentInChildren<TankTrackAnimation>();
		audioSource = GetComponent<AudioSource>();
		weapon = GetComponentInChildren<WeaponController>();
	}

	void Start()
	{
		// Save all position and rotation of the gun.
		if (Turret != null)
			turrentEulerAngle = Turret.transform.localEulerAngles;
			
		if (MainGun != null)
			maingunEulerAngle = MainGun.transform.localEulerAngles;
	}

	void Update()
	{
		// Arccos(FlipRatio) 이상 뒤집어졌으면 매 프레임 체력이 FlipDamage 만큼 감소한다.
		if (Vector3.Dot(transform.up, Vector3.up) < FlipRatio);
			// ApplyDamage(FlipDamage);
	}

	// 무기 발사
	public void FireWeapon()
	{
		if (weapon)
			weapon.LaunchWeapon();
	}

	// 해당 무기 번호에 해당하는 무기 발사
	public void FireWeapon(int index)
	{
		if (weapon)
			weapon.LaunchWeapon(index);
	}

	// 무기 변경
	public void SwitchWeapon()
	{
		weapon.SwitchWeapon();
	}

	// 이동 (한 프레임)
	public void Move(Vector2 moveVector)
	{
		// Arccos(FlipRatio) 이상 뒤집어졌으면 움직이지 않음
		if (Vector3.Dot(transform.up, Vector3.up) < FlipRatio)
			return;

		positionTemp = transform.position;
		// 좌우 회전
		transform.Rotate(new Vector3(0, moveVector.x * TurnSpeed * Time.deltaTime, 0));
		// 전진, 후진
		transform.position += transform.forward * moveVector.y * TankSpeed * Time.deltaTime;

		// 무한궤도 효과
		if (track)
		{
			float moveanim = moveVector.x + moveVector.y;

			if (moveVector.x <= 0 && moveVector.y > 0)
				moveanim = -1;

			if (moveVector.x > 0 && moveVector.y <= 0)
				moveanim = 1;

			// 무한궤도 효과 적용
			track.MoveTrack(new Vector2(Mathf.Clamp(moveanim, -1, 1), 0));
		}

		// 등록된 사운드가 있을 경우
		if (audioSource)
		{
			// 움직일 때 Pitch 가 증가한다.
			audioSource.pitch = Mathf.Clamp((transform.position - positionTemp).magnitude * 100, 1, 2f);
			// 거리에 따른 3D 사운드 효과 적용
			audioSource.spatialBlend = 1;
		}
	}

	// NavMeshAgent
	public void MoveTo(NavigatorInstance navigator)
	{
		// This tank move along with navigator object.
		if (Vector3.Dot(transform.up, Vector3.up) < FlipRatio)
			return;

		Quaternion rotationTarget = navigator.transform.rotation;
		rotationTarget.eulerAngles = new Vector3(transform.eulerAngles.x, rotationTarget.eulerAngles.y, transform.eulerAngles.z);
		transform.rotation = Quaternion.Lerp(transform.rotation, rotationTarget, TurnSpeed * 0.1f * Time.deltaTime);
		positionTemp = transform.position;
		transform.position += transform.forward * TankSpeed * Time.deltaTime;

		// Track animation working
		if (track)
			track.MoveTrack(new Vector2(1, 0));

		// increase Pitch when moving
		if(audioSource)
		{
			audioSource.pitch = Mathf.Clamp((transform.position - positionTemp).magnitude * 100, 1, 2f);
			audioSource.spatialBlend = 1;
		}
	}

	public void Aim(Vector3 target)
	{
		// Aim to specific position
		float aimAngleTurret = 0;
		float aimAngleMaingun = 0;

		if (Turret != null)
		{
			Vector3 localTarget = Turret.transform.parent.InverseTransformPoint(target);
			Quaternion targetlook = Quaternion.LookRotation(localTarget - Turret.transform.localPosition);

			targetlook.eulerAngles = TurretBaseAxis * targetlook.eulerAngles.y + Vector3.Scale(Vector3.one - TurretBaseAxis, turrentEulerAngle);

			Turret.transform.localRotation = Quaternion.Lerp(Turret.transform.localRotation, targetlook, TurretSpeed * Time.deltaTime * 0.1f);
			aimAngleTurret = Quaternion.Angle(Turret.transform.localRotation, targetlook);
		}

		if (MainGun != null && aimAngleTurret < 3)
		{
			Vector3 localTarget = MainGun.transform.parent.InverseTransformPoint(target);
			float distance = Vector2.Distance(new Vector2(localTarget.x, localTarget.z), new Vector2(MainGun.transform.localPosition.x, MainGun.transform.localPosition.z));
			float angle = Mathf.Atan(distance / localTarget.y) * Mathf.Rad2Deg;

			if (target.y < MainGun.transform.position.y)
				angle = -angle;
				
			Quaternion targetlook = MainGun.transform.localRotation;
			targetlook.eulerAngles = new Vector3((MainGunBaseAxis.x * angle) + maingunEulerAngle.x, (MainGunBaseAxis.y * angle) + maingunEulerAngle.y, (MainGunBaseAxis.z * angle) + maingunEulerAngle.z);
			MainGun.transform.localRotation = Quaternion.Lerp(MainGun.transform.localRotation, targetlook, TurretSpeed * Time.deltaTime * 0.1f);
			aimAngleMaingun = Quaternion.Angle(MainGun.transform.localRotation, targetlook);
		}

		AimingAngle = aimAngleTurret + aimAngleMaingun;
	}

	public void Aim(Vector2 aimVector)
	{
		// aim with cursor vector
		if (Turret != null)
		{
			float rotationY = Turret.transform.localEulerAngles.y + aimVector.x * TurretSpeed * Time.deltaTime;
			Turret.transform.localEulerAngles = TurretBaseAxis * rotationY + Vector3.Scale(Vector3.one - TurretBaseAxis, turrentEulerAngle);
		}

		if (MainGun != null)
		{
			rotationX += aimVector.y * TurretSpeed * Time.deltaTime;
			rotationX = Mathf.Clamp(rotationX, MaingunMinTurnX, MaingunMaxTurnX);

			MainGun.transform.localEulerAngles = MainGunBaseAxis * -rotationX + maingunEulerAngle;
		}
	}
}
