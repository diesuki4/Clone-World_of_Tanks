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
		// Passing Target Tag parameter to Weapon System
		if (tank && tank.weapon)
			tank.weapon.TargetTag = TargetTag;
			
		// spawn Navigator object
		if (Navigator != null)
		{
			GameObject navigatorObj = Instantiate(Navigator.gameObject, transform.position, Navigator.transform.rotation);
			navigator = navigatorObj.GetComponent<NavigatorInstance>();
			navigator.Navigator.speed = tank.TankSpeed;
			navigator.Owner = gameObject;
		}
	}

	Vector3 positionAround;
	Vector3 aimAround;

	void Update()
	{
		// Calculate distance from target position
		float distanceToPoint = Vector3.Distance(transform.position, positionAround);

		if (currentTarget == null)
		{
			// Always finding some target
			FindNewTarget ();

			switch (aiMoveState)
			{
			case 0:
				// Create position offset for AI this randomly making AI looks more natural
				positionAround = DetectGround(transform.position + new Vector3(Random.Range(-PatrolDistance, PatrolDistance), 1000, Random.Range(-PatrolDistance, PatrolDistance)));
				aiMoveState = 1;
				aiTime = Random.Range(0, PatrolDurationMax);
				break;
			case 1:
				//just Move the tank along with navigator object
				if (navigator != null)
				{
					if (Vector3.Distance(navigator.transform.position, transform.position) > 1)
						navigator.transform.position = transform.position;
						
					// if AI is far from destination. so keep moving
					if (distanceToPoint > StopDistance)
					{
						navigator.SetDestination(positionAround);
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

				if (Vector3.Distance (navigator.transform.position, transform.position) > 1)
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

	void FindNewTarget()
	{
		if (WeaponSystem.Finder == null)
			return;
		
		currentTarget = null;

		for (int i = 0; i < TargetTag.Length; ++i)
		{
			TargetCollector targetcollector = WeaponSystem.Finder.FindTargetTag(TargetTag[i]);

			if (targetcollector != null)
			{
				GameObject[] targets = targetcollector.Targets;
				float closerDistance = float.MaxValue;

				for (int j = 0; j < targets.Length; ++j)
				{
					if (targets[j] != null && targets[j] != gameObject)
					{
						float distance = Vector3.Distance(transform.position, targets[j].transform.position);
						
						if (distance < closerDistance)
						{
							currentTarget = targets[j];
							closerDistance = distance;
						}
					}
				}
			}
		}
	}

	Vector3 DetectGround(Vector3 position)
	{
		RaycastHit hit;

		if (Physics.Raycast(position, -Vector3.up, out hit, float.MaxValue))
			return hit.point;
			
		return position;
	}
}
