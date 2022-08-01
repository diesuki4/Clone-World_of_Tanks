using UnityEngine;
using System.Collections;

namespace HWRWeaponSystem
{
	public class MoverMissile : WeaponBase
	{
        [Header("타겟 방향으로 조정되는 속도")]
		public float Damping = 3;
        [Header("초기 속도")]
		public float Speed = 80;
        [Header("최대 속도")]
		public float SpeedMax = 80;
        [Header("초당 증가하는 속도")]
		public float SpeedMult = 1;
        [Header("노이즈")]
		public Vector3 Noise = new Vector3 (20, 20, 20);
		public float TargetLockDirection = 0.5f;
		public int DistanceLock = 70;
		public int DurationLock = 40;
		public bool Seeker;
        [Header("소멸 시간")]
		public float LifeTime = 5.0f;

		bool locked;
		int timetorock;
		float timeCount = 0;
		float speedTemp;
	
		void Awake()
		{
			// 초기 속도 백업
			speedTemp = Speed;
			objectPool = GetComponent<ObjectPool>();

            // 오브젝트 풀을 사용 중이면
			if (objectPool && WeaponSystem.Pool != null)
                // 오브젝트의 소멸 시간 지정
				objectPool.LifeTime = LifeTime;	
		}
	
		void Start()
		{
            // 오브젝트 풀을 사용 중이면
			if (objectPool && WeaponSystem.Pool != null)
			{
                // Lifetime 뒤에 비활성화
				objectPool.LifeTime = LifeTime;
				objectPool.SetDestroy(LifeTime);
			}
            // 풀을 사용하지 않고 있으면
			else
			{
                // LifeTime 뒤에 소멸
				Destroy(gameObject, LifeTime);
			}

			timeCount = Time.time;
		}
	
        // 활성화 시 (오브젝트 풀에서 재활용할 시)
		void OnEnable()
		{
			// 초기 속도 복원
			Speed = speedTemp;
			// 타겟 초기화
			Target = null;
			timeCount = Time.time;

            // 오브젝트 풀을 사용 중이면
			if (objectPool)
			{
                // Lifetime 뒤에 비활성화
				objectPool.LifeTime = LifeTime;
				objectPool.SetDestroy(LifeTime);
			}
		}
	
		void FixedUpdate()
		{
			Rigidbody rigidbody = GetComponent<Rigidbody>();

			// 증가하는 Speed 에 따라 velocity 를 적용한다.
			rigidbody.velocity = transform.forward * Speed * Time.fixedDeltaTime;
			// 노이즈 추가
			rigidbody.velocity += new Vector3(Random.Range(-Noise.x, Noise.x), Random.Range(-Noise.y, Noise.y), Random.Range(-Noise.z, Noise.z));
		
            // 최대 속도로 Clamp
			if (Speed < SpeedMax)
				Speed += SpeedMult * Time.fixedDeltaTime;
		}

		void Update()
		{
			if (WeaponSystem.Pool != null && objectPool != null && !objectPool.Active) 
				return;
		
			// 소멸 0.5초 전이면
			if (Time.time >= (timeCount + LifeTime) - 0.5f)
			{
				Damage damage = GetComponent<Damage>();

				if (damage)
				{
        			// 이펙트 표시, 폭발 데미지 처리 부분 실행
					damage.Active(transform.position);
					timeCount = Time.time;
				}
			}
		
			// 타겟이 지정되어 있으면
			if (Target)
			{
				// 미사일을 볼 때의 각도
				Quaternion rotation = Quaternion.LookRotation(Target.transform.position - transform.position);
				// 미사일 방향으로 서서히 회전
				transform.rotation = Quaternion.Slerp(transform.rotation, rotation, Time.deltaTime * Damping);
				// 타겟 방향
				Vector3 dir = (Target.transform.position - transform.position).normalized;
				float direction = Vector3.Dot(dir, transform.forward);
				
				// 타겟과의 각도가 Arccos(TargetLockDirection) 각도보다 크면
				if (direction < TargetLockDirection)
					// 타겟을 해제
					Target = null;
			}
		
			// Seeker 가 true 이고
			if (Seeker)
			{
				// DurationLock 프레임이 지났고
				if (timetorock > DurationLock)
				{
					// Lock 상태가 아니고 지정된 타겟이 없으면
					if (!locked && !Target)
					{
						float distance = int.MaxValue;

                    	// TargetTag 에 등록된 태그만큼 순회
						for (int i = 0; i < TargetTag.Length; ++i)
						{
                            // 해당 태그를 갖는 게임 오브젝트 배열
							GameObject[] objs = WeaponSystem.Finder.FindTargetTag(TargetTag[i]).Targets;

							if (objs.Length > 0)
							{
                                // TargetTag 의 i번째 태그를 갖는 오브젝트들을 순회
								for (int j = 0; j < objs.Length; ++j)
								{
									if (objs[j])
									{
                                        // objs[j] 로의 방향
										Vector3 dir = (objs[j].transform.position - transform.position).normalized;
										float direction = Vector3.Dot(dir, transform.forward);
                                        // objs[j] 와의 거리
										float dis = Vector3.Distance(objs[j].transform.position, transform.position);
										
                                        // objs[j] 와의 각도가 Arccos(TargetLockDirection) 작고
										if (direction >= TargetLockDirection)
										{
                                            // Lock 거리보다 작고
											if (DistanceLock > dis)
											{
                                                // 현재 target 보다 가까이 있으면
												if (distance > dis)
												{
													// TargetTag 의 i번째 태그를 갖는 오브젝트 중 j번째 오브젝트를 현재 타겟으로 지정
													distance = dis;
													Target = objs[j];
												}

												// Lock 상태로 전환
												locked = true;
											}
										}
									}
								}
							}
						}
					}

					// DurationLock 프레임이 지나서 timetorock 을 초기화
					timetorock = 0;
				}
				// DurationLock 프레임이 지나지 않았으면
				else
				{
					// timetorock (프레임 단위) 을 1 증가
					timetorock += 1;
				}

				// 지정된 타겟이 없으면
				if (!Target)
					// Lock 상태가 아님
					locked = false;
			}
		}
	}
}
