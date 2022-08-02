using UnityEngine;
using System.Collections;
using HWRWeaponSystem;

[RequireComponent (typeof(Tank))]
public class TankAI : MonoBehaviour
{
	Tank tank;
	GameObject currentTarget;
	NavigatorInstance navigator;
	float aiTime = 0;
	int aiMoveState = 0;
	float aiFireDelay = 0;
	float aiFireTime = 0;

	public int PatrolDurationMax = 20;
	public int PatrolDistance = 100;
	public float StopDistance = 2;
	public float FiringDurationMax = 5;
	public float FiringDelay = 10;
	public float FireDistance = 20;
	public int FiringSpread = 500;
	public float SwitchWeaponChance = 49;
	public NavigatorInstance Navigator;
	public string[] TargetTag = {"Enemy"};

	void Awake()
	{
		tank = GetComponent<Tank>();
	}

	void Start()
	{
		// 무기에 타겟으로 정할 오브젝트들의 태그를 지정한다.
		if (tank && tank.weapon)
			tank.weapon.TargetTag = TargetTag;
			
		// 네비게이터 생성
		if (Navigator != null)
		{
			// 네비게이터는 프리팹으로 존재한다.
			GameObject navigatorObj = Instantiate(Navigator.gameObject, transform.position, Navigator.transform.rotation);
			navigator = navigatorObj.GetComponent<NavigatorInstance>();
			// NavMeshAgent 의 속도 지정
			navigator.Navigator.speed = tank.TankSpeed;
			// 이 네비게이터의 소유자는 나다.
			navigator.Owner = gameObject;
		}
	}

	Vector3 positionAround;
	Vector3 aimAround;

	void Update()
	{
		// 반경 PatrolDistance 이내의 랜덤 지점까지의 거리
		float distanceToPoint = Vector3.Distance(transform.position, positionAround);

		// 현재 등록된 타겟이 없으면
		if (currentTarget == null)
		{
			// TargetTag 에 등록된 태그를 갖는 오브젝트 중 가장 가까운 오브젝트를 타겟으로 갱신
			FindNewTarget();

			switch (aiMoveState)
			{
			// AI의 랜덤 움직임을 초기화 하는 상태
			case 0:
				// 현재 위치 기준 반경 PatrolDistance 이내의 지점을 랜덤하게 지정한다.
				positionAround = DetectGround(transform.position + new Vector3(Random.Range(-PatrolDistance, PatrolDistance), 1000, Random.Range(-PatrolDistance, PatrolDistance)));
				// 상태를 움직이는 상태로 전환
				aiMoveState = 1;
				// PatrolDurationMax 미만의 시간(초)로 지정한 타겟으로 움직일 시간 지정
				aiTime = Random.Range(0, PatrolDurationMax);
				break;
			case 1:
				if (navigator != null)
				{
					// 네비게이터의 위치가 탱크에서 1m 이상 떨어졌으면 위치를 동일하게 지정
					if (Vector3.Distance(navigator.transform.position, transform.position) > 1)
						navigator.transform.position = transform.position;
						
					// 설정한 목적지가 아직 멀었으면
					if (distanceToPoint > StopDistance)
					{
						// 네비게이터를 목적지로 계속 이동시킨다.
						navigator.SetDestination(positionAround);
						// 탱크는 네비게이터를 따라간다.
						tank.MoveTo(navigator);
					}
				}

				// aim turret to position offset
				tank.Aim(positionAround);
				break;
			}

			// count down for new state
			if (aiTime <= 0)
				aiMoveState = 0;
			else
				aiTime -= Time.deltaTime;
		}
		else
		{
			// calculate distance from target
			float targetDistance = Vector3.Distance(transform.position, currentTarget.transform.position);
			Vector3 targetDirection = (currentTarget.transform.position - transform.position).normalized;

			RaycastHit hit;
			bool canFire = false; 
			// Raycasting object in front. to make sure the sight is clean before firing.
			if (Physics.Raycast(currentTarget.transform.position + Vector3.up - (targetDirection * 2), -targetDirection, out hit))
				if (hit.collider.gameObject == gameObject)
					if (targetDistance <= FireDistance)
						canFire = true;

			switch (aiMoveState)
			{
			case 0:
				// Create position offset for AI this randomly making AI looks more natural
				aimAround = (new Vector3(Random.Range(-FiringSpread, FiringSpread), Random.Range(0, FiringSpread) + targetDistance, Random.Range(-FiringSpread, FiringSpread))) * 0.001f;
				positionAround = currentTarget.transform.position + new Vector3(Random.Range(-FireDistance, FireDistance), 0, Random.Range(-FireDistance, FireDistance));
				aiMoveState = 1;
				aiTime = Random.Range(0, PatrolDurationMax);
				break;
			case 1:
				// do some works.
				break;
			}

			//just Move the tank along with navigator object
			if (navigator != null && (!canFire || (canFire && distanceToPoint > FireDistance / 2)))
			{
				navigator.SetDestination(positionAround);
				tank.MoveTo(navigator);

				if (Vector3.Distance(navigator.transform.position, transform.position) > 1)
					navigator.transform.position = transform.position;
			}

			// aim to the target plus with position offset
			tank.Aim(currentTarget.transform.position + aimAround);

			// sight is clean and gun is straight to the target
			if (canFire && tank.AimingAngle < 5)
			{
				if (aiFireDelay <= 0)
				{
					aiFireTime = Random.Range(0, FiringDurationMax);
					aiFireDelay = Random.Range(0, FiringDelay);

					if (tank.weapon)
						if (Random.Range(0, 100) > Mathf.Clamp(100 - SwitchWeaponChance, 0, 100))
							tank.weapon.SwitchWeapon();
				}
				else
				{
					aiFireDelay -= 1 * Time.deltaTime;
				}
				
				if (aiFireTime > 0)
				{
					tank.FireWeapon();
					aiFireTime -= Time.deltaTime;
				}
			}

			// count down to the new state
			if (aiTime <= 0)
			{
				aiMoveState = 0;
				FindNewTarget();
			}
			else
			{
				aiTime -= Time.deltaTime;
			}
		}
	}

	// TargetTag 에 등록된 태그를 갖는 오브젝트 중 가장 가까운 오브젝트를 타겟으로 지정
	void FindNewTarget()
	{
		// Finder 가 활성화돼있지 않으면 실행 안 함
		if (WeaponSystem.Finder == null)
			return;
		
		// 현재 타겟 해제
		currentTarget = null;

        // TargetTag 에 등록된 태그만큼 순회
		for (int i = 0; i < TargetTag.Length; ++i)
		{
			// i 번째 태그를 갖는 오브젝트 목록을 가져온다.
			TargetCollector targetcollector = WeaponSystem.Finder.FindTargetTag(TargetTag[i]);

			if (targetcollector != null)
			{
				// 해당 태그를 갖는 게임 오브젝트 배열
				GameObject[] targets = targetcollector.Targets;
				float closerDistance = float.MaxValue;

				// TargetTag 의 i 번째 태그를 갖는 오브젝트들을 순회
				for (int j = 0; j < targets.Length; ++j)
				{
					// j 번째 오브젝트가 자기 자신이 아니고
					if (targets[j] != null && targets[j] != gameObject)
					{
						// targets[j] 와의 거리
						float distance = Vector3.Distance(transform.position, targets[j].transform.position);
						
						// 현재 target 보다 가까이 있으면
						if (distance < closerDistance)
						{
							// TargetTag 의 i번째 태그를 갖는 오브젝트 중 j번째 오브젝트를 현재 타겟으로 지정
							currentTarget = targets[j];
							closerDistance = distance;
						}
					}
				}
			}
		}
	}

	// position 에서의 바닥의 위치 반환
	Vector3 DetectGround(Vector3 position)
	{
		RaycastHit hit;

		if (Physics.Raycast(position, -Vector3.up, out hit, float.MaxValue))
			return hit.point;
			
		return position;
	}
}
