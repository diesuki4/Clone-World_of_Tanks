using UnityEngine;
using System.Collections;

namespace HWRWeaponSystem
{
	public class ObjectPool : MonoBehaviour
	{
		[Header("비활성화까지 대기 시간")]
		public float LifeTime;
		// 활성화 여부
		[HideInInspector]
		public bool Active;
		// 프리팹 종류
		[HideInInspector]
		public GameObject Prefab;

		// 초기 위치
		Vector3 positionTemp;
		// 초기 회전값
		Quaternion rotationTemp;
		// 초기 Scale
		Vector3 scaleTemp;
		Rigidbody rigidBody;
		LineRenderer lineRenderer;
		TrailRenderer trailRenderer;
		float trailTemp;
	
		void Awake()
		{
			// 초깃값 백업
			scaleTemp = transform.localScale;
			positionTemp = transform.position;
			rotationTemp = transform.rotation;
			rigidBody = GetComponent<Rigidbody>();
			lineRenderer = GetComponent<LineRenderer>();
			trailRenderer = GetComponent<TrailRenderer>();

			if (trailRenderer)
				trailTemp = trailRenderer.time;
		}

		void Start() { }
	
		// 활성화 되면
		void OnEnable()
		{
			// LifeTime 뒤에 소멸
			if (LifeTime > 0) 
				StartCoroutine(setDestroying(LifeTime));
		}

		// 오브젝트 초기화 루틴
		public virtual void OnSpawn(Vector3 position, Vector3 scale, Quaternion rotation, GameObject prefab, float lifeTime)
		{
			// 수명을 설정한다.
			if (lifeTime != -1)
				LifeTime = lifeTime;
		
			// 보이게 설정한다.
			if (GetComponent<Renderer>())
				GetComponent<Renderer>().enabled = true;

			// 초깃값 설정
			Prefab = prefab;
			transform.position = position;
			transform.rotation = rotation;
			transform.localScale = scale;
			scaleTemp = transform.localScale;
			positionTemp = transform.position;
			rotationTemp = transform.rotation;
		
			if (rigidBody)
			{
				rigidBody.velocity = Vector3.zero;
				rigidBody.angularVelocity = Vector3.zero;
			}

			if (lineRenderer)
			{
				lineRenderer.SetPosition(0, transform.position);
				lineRenderer.SetPosition(1, transform.position);
			}

			if (GetComponent<ParticleSystem>())
				GetComponent<ParticleSystem>().Play();
				
			if (trailRenderer)
				trailRenderer.time = trailTemp;	
				
			// 활성화 상태로 전환
			Active = true;
		
			// 겡미 오브젝트 활성화
			gameObject.SetActive(true);
		
			// LifeTime 뒤에 소멸하도록 지정
			if (LifeTime > 0)
				StartCoroutine(setDestroying(LifeTime));
				
			StartCoroutine(resetTrail());
		}
	
		IEnumerator resetTrail()
		{
			if (trailRenderer)
			{
				trailRenderer.time = -trailRenderer.time;
				yield return new WaitForSeconds(0.01f);
				trailRenderer.time = trailTemp;
			}
		}

		// time 뒤에 소멸
		public void Destroying(float time)
		{
			SetDestroy(time);
		}

		public void SetDestroy(float time)
		{
			StartCoroutine(setDestroying(time));
		}

		public IEnumerator setDestroying(float time)
		{
			yield return new WaitForSeconds(time);
			OnDestroyed();
		}

		public virtual void OnDestroyed()
		{
			Destroying();
		}

		// 소멸 시 동작
		public void Destroying()
		{
			// 보이지 않게 처리한다.
			if (GetComponent<Renderer>())
				GetComponent<Renderer>().enabled = false;
		
			// 초기 위치로 복구
			transform.localScale = scaleTemp;
			transform.position = positionTemp;
			transform.rotation = rotationTemp;

			if (rigidBody)
			{
				rigidBody.velocity = Vector3.zero;
				rigidBody.angularVelocity = Vector3.zero;
			}
			
			if (lineRenderer)
			{
				lineRenderer.SetPosition(0, transform.position);
				lineRenderer.SetPosition(1, transform.position);
			}

			if (GetComponent<ParticleSystem>())
				GetComponent<ParticleSystem>().Stop();
			
			if (trailRenderer)
			{
				trailRenderer.time = 0;	
#if UNITY_5
				trailRenderer.Clear();
#endif
			}
		
			// 삭제하지 않고 풀에 비활성화 상태로 남겨둔다.
			gameObject.SetActive(false);
			Active = false;
		}
	}
}
