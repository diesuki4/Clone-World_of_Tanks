using UnityEngine;
using System.Collections;

namespace HWRWeaponSystem
{
    public class WeaponSystemManager : MonoBehaviour
    {
        void Awake()
        {
            WeaponSystem.Pool = GameObject.FindObjectOfType<ObjectPoolManager>();
            WeaponSystem.Finder = GameObject.FindObjectOfType<FinderPool>();
        }

        // 소멸 시 초기화
        void OnDestroy()
        {
            if (WeaponSystem.Pool)
                WeaponSystem.Pool.ClearPool();
                
            if (WeaponSystem.Finder)
                WeaponSystem.Finder.ClearTarget();
        }
    }

    public static class WeaponSystem
    {
        // 오브젝트 풀 매니저
        public static ObjectPoolManager Pool;
        // 태그를 이용해 적을 찾기 위한 풀
        public static FinderPool Finder;
    }
}
