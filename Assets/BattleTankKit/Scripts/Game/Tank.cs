using UnityEngine;
using System.Collections;
using HWRWeaponSystem;

[RequireComponent (typeof(AudioSource))]
[RequireComponent (typeof(WeaponController))]
public class Tank : DamageManager
{
	[Header("탱크 상부")]
	public GameObject Turret;
	[Header("포신")]
	public GameObject MainGun;
	// 탱크 상부의 회전은 좌우 회전만 적용한다.
	public Vector3 TurretBaseAxis = new Vector3(0, 1, 0);
	// 포신의 회전은 상하 회전만 적용한다.
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
	// 포신과 타겟의 가로, 세로 각도 차의 합
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
		// Arccos(FlipRatio) 이상 뒤집어졌으면 움직이지 않음
		if (Vector3.Dot(transform.up, Vector3.up) < FlipRatio)
			return;

		// NavigatorInstance 의 회전값
		Quaternion rotationTarget = navigator.transform.rotation;
		// 타겟의 회전값에 탱크의 X, Z 회전값을 적용 (좌우 회전만 변경하겠다)
		rotationTarget.eulerAngles = new Vector3(transform.eulerAngles.x, rotationTarget.eulerAngles.y, transform.eulerAngles.z);
		// 좌우 회전만 서서히 변경
		transform.rotation = Quaternion.Lerp(transform.rotation, rotationTarget, TurnSpeed * 0.1f * Time.deltaTime);
		positionTemp = transform.position;
		// 전진
		transform.position += transform.forward * TankSpeed * Time.deltaTime;

		// 무한궤도 효과
		if (track)
			track.MoveTrack(new Vector2(1, 0));

		// 등록된 사운드가 있을 경우
		if(audioSource)
		{
			// 움직일 때 Pitch 가 증가한다.
			audioSource.pitch = Mathf.Clamp((transform.position - positionTemp).magnitude * 100, 1, 2f);
			// 거리에 따른 3D 사운드 효과 적용
			audioSource.spatialBlend = 1;
		}
	}

	// target 에 따라 서서히 회전
	public void Aim(Vector3 target)
	{
		// 탱크 상부와 타겟의 각도
		float aimAngleTurret = 0;
		// 포신과 타겟의 각도
		float aimAngleMaingun = 0;

		// 등록된 탱크 상부가 있으면
		if (Turret != null)
		{
			// target 의 위치를 탱크 기준 로컬 위치로 변환
			Vector3 localTarget = Turret.transform.parent.InverseTransformPoint(target);
			// localTarget 을 보고 있을 때의 각도
			Quaternion targetlook = Quaternion.LookRotation(localTarget - Turret.transform.localPosition);
			// targetlook 에 현재 각도에서 target 의 Y축 각도를 적용한 것을 대입한다.
			targetlook.eulerAngles = TurretBaseAxis * targetlook.eulerAngles.y + Vector3.Scale(Vector3.one - TurretBaseAxis, turrentEulerAngle);

			// 탱크 상부를 targetlook 방향으로 서서히 회전
			Turret.transform.localRotation = Quaternion.Lerp(Turret.transform.localRotation, targetlook, TurretSpeed * Time.deltaTime * 0.1f);
			// 탱크 상부와 타겟의 각도
			aimAngleTurret = Quaternion.Angle(Turret.transform.localRotation, targetlook);
		}

		// 등록된 포신이 있고 탱크 상부와 타겟의 각도가 3도 미만이면
		if (MainGun != null && aimAngleTurret < 3)
		{
			// target 의 위치를 탱크 상부 기준 로컬 위치로 변환
			Vector3 localTarget = MainGun.transform.parent.InverseTransformPoint(target);
			// 포신과 타겟과의 거리
			float distance = Vector2.Distance(new Vector2(localTarget.x, localTarget.z), new Vector2(MainGun.transform.localPosition.x, MainGun.transform.localPosition.z));
			// 포신과 타겟의 수직(세로) 각도 차이
			float angle = Mathf.Atan(distance / localTarget.y) * Mathf.Rad2Deg;

			// 타겟이 아래에 있으면 각도에 - 적용
			if (target.y < MainGun.transform.position.y)
				angle = -angle;
				
			// localTarget 을 보고 있을 때의 각도
			Quaternion targetlook = MainGun.transform.localRotation;
			// angle 만큼 회전한 각도를 target 각도로 지정
			targetlook.eulerAngles = MainGunBaseAxis * angle + maingunEulerAngle;
			// 포신을 targetlook 방향으로 서서히 회전
			MainGun.transform.localRotation = Quaternion.Lerp(MainGun.transform.localRotation, targetlook, TurretSpeed * Time.deltaTime * 0.1f);
			// 포신과 타겟의 각도
			aimAngleMaingun = Quaternion.Angle(MainGun.transform.localRotation, targetlook);
		}

		// 포신과 타겟의 가로, 세로 각도 차의 합
		AimingAngle = aimAngleTurret + aimAngleMaingun;
	}

	// aimVector 에 따라 서서히 회전
	public void Aim(Vector2 aimVector)
	{
		// 등록된 탱크 상부가 있으면
		if (Turret != null)
		{
			// aimVector 와 TurretSpeed 속도에 따라 서서히 회전한다.
			float rotationY = Turret.transform.localEulerAngles.y + aimVector.x * TurretSpeed * Time.deltaTime;
			// Y축 회전만을 변경한다.
			Turret.transform.localEulerAngles = TurretBaseAxis * rotationY + Vector3.Scale(Vector3.one - TurretBaseAxis, turrentEulerAngle);
		}

		// 등록된 포신이 있으면
		if (MainGun != null)
		{
			// aimVector 와 TurretSpeed 속도에 따라 서서히 회전한다.
			rotationX += aimVector.y * TurretSpeed * Time.deltaTime;
			// 회전 각도를 제한
			rotationX = Mathf.Clamp(rotationX, MaingunMinTurnX, MaingunMaxTurnX);
			// X축 회전만을 변경한다.
			MainGun.transform.localEulerAngles = MainGunBaseAxis * -rotationX + maingunEulerAngle;
		}
	}
}
