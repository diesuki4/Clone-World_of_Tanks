using UnityEngine;
using System.Collections;
using System.Collections.Generic;

namespace HWRWeaponSystem
{
    // 오브젝트 풀 매니저
    public class ObjectPoolManager : MonoBehaviour
    {
        // 오브젝트를 저장하는 리스트
        List<ObjectPool> usedObject;

        void Start()
        {
            usedObject = new List<ObjectPool>();
        }

        // 풀 초기화
        public void ClearPool()
        {
            usedObject.Clear();
            usedObject = new List<ObjectPool>(1);
        }

        // 오브젝트 생성
        public GameObject Instantiate(GameObject obj, Vector3 position, Quaternion rotation, float lifeTime)
        {
            // 풀이 null 이면 아무 것도 안 함
            if (usedObject == null)
                return null;

            // 풀을 돌면서 사용할 수 있는 오브젝트가 있는지 탐색
            foreach (ObjectPool bu in usedObject)
            {
                if (bu != null)
                {
                    // 프리팹 종류가 같고
                    if (bu.Prefab.gameObject == obj.gameObject)
                    {
                        // 비활성화 상태인 오브젝트가 있으면
                        if (bu.Active == false)
                        {
                            // 그 오브젝트를 초기화하고
                            bu.OnSpawn(position, obj.transform.localScale, rotation, obj, lifeTime);
                            // 반환한다.
                            return bu.gameObject;
                        }
                    }
                }
            }

            // 풀에 사용할 수 있는 오브젝트가 없으면
            // 오브젝트를 새로 생성하고
            GameObject newobj = GameObject.Instantiate(obj.gameObject, position, obj.transform.rotation);
            ObjectPool newpoolobj = newobj.GetComponent<ObjectPool>();

            if (newpoolobj != null)
            {
                // 초기화하고
                newpoolobj.OnSpawn(position, newobj.transform.localScale, rotation, obj, lifeTime);
                // 풀에 추가하고
                usedObject.Add(newpoolobj);
            }

            // 반환한다.
            return newobj;
        }

        // 오브젝트 생성 (오버라이딩)
        public GameObject Instantiate(GameObject obj, Vector3 position, Quaternion rotation)
        {
            // 풀이 null 이면 아무 것도 안 함
            if (usedObject == null)
                return null;

            // 풀을 돌면서 사용할 수 있는 오브젝트가 있는지 탐색
            foreach (ObjectPool bu in usedObject)
            {
                if (bu != null)
                {
                    // 프리팹 종류가 같고
                    if (bu.Prefab.gameObject == obj.gameObject)
                    {
                        // 비활성화 상태인 오브젝트가 있으면
                        if (bu.Active == false)
                        {
                            // 그 오브젝트를 초기화하고
                            bu.OnSpawn(position, obj.transform.localScale, rotation, obj, -1);
                            // 반환한다.
                            return bu.gameObject;
                        }
                    }
                }
            }

            // 풀에 사용할 수 있는 오브젝트가 없으면
            // 오브젝트를 새로 생성하고
            GameObject newobj = GameObject.Instantiate(obj.gameObject, position, obj.transform.rotation);
            ObjectPool newpoolobj = newobj.GetComponent<ObjectPool>();

            if (newpoolobj != null)
            {
                // 초기화하고
                newpoolobj.OnSpawn(position, newobj.transform.localScale, rotation, obj, -1);
                // 풀에 추가하고
                usedObject.Add(newpoolobj);
            }

            // 반환한다.
            return newobj;
        }
    }
}
