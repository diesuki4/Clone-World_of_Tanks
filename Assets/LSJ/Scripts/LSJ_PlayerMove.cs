using System.Collections;
using System.Collections.Generic;
using UnityEngine;

// 기본 이동 조작 : 전후좌우, 360도, 플레이어 앞방향 기준
// 순항 조작 모드 : 전진 순항/후진 순항, 속도(느림, 중간, 최고)
// 급정지 : Space 입력 시 급정지(핸드브레이크)

// 360도 이동 WASD(전후좌우)
public class LSJ_PlayerMove : MonoBehaviour
{
    public float moveSpeed = 3f;
    Vector3 dir;
    // Start is called before the first frame update
    void Start()
    {
        
    }

    // Update is called once per frame
    void Update()
    {
        float h = Input.GetAxis("Horizontal");
        float v = Input.GetAxis("Vertical");

        dir = new Vector3(h, 0, v);
        dir.Normalize();

        transform.Translate(dir * moveSpeed * Time.deltaTime);
    }
}
