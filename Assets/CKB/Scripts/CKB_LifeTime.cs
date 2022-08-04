using System.Collections;
using System.Collections.Generic;
using UnityEngine;

// Lifetime 뒤에 오브젝트 Destroy
public class CKB_LifeTime : MonoBehaviour
{
    [Header("수명")]
	public float Lifetime = 3;

    void Start()
    {
        Destroy(gameObject, Lifetime);
    }

    void Update() { }
}
