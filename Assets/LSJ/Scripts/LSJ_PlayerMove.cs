using System.Collections;
using System.Collections.Generic;
using UnityEngine;

// 기본 이동 조작 : 전후좌우, 360도, 플레이어 앞방향 기준
// 순항 조작 모드 : 전진 순항/후진 순항, 속도(느림, 중간, 최고)
// 급정지 : Space 입력 시 급정지(핸드브레이크)

// 360도 이동 WASD(전후좌우)
// R 키 : 전차 전진 순항모드(1회 입력 - 2회 입력 - 3회 입력 순으로 빨라짐)
// F 키 : 전차 후진 순항모드(1회 입력 - 2회 입력 - 3회 입력 순으로 느려짐)

// WASD 이동 키를 입력 시 순항모드는 바로 해제

public class LSJ_PlayerMove : MonoBehaviour
{
    public float moveSpeed = 30f; // 이동 속도
    public float rotSpeed = 50f; // 회전 속도

    private Rigidbody rbody;
    private Transform tr;

    public bool isMove;

    // Start is called before the first frame update
    void Start()
    {
        rbody = GetComponent<Rigidbody>();
        tr = GetComponent<Transform>();
        rbody.centerOfMass= new Vector3(0.0f, -0.5f, 0.0f);
        isMove = false;
    }

    // Update is called once per frame
    void Update()
    {
         TankMove();   
    }

    public void TankMove()
    {
        float h = Input.GetAxis("Horizontal");
        float v = Input.GetAxis("Vertical");

        tr.Translate(Vector3.forward * moveSpeed * v * Time.deltaTime);
        tr.Rotate(Vector3.up * rotSpeed * h * Time.deltaTime);

        isMove = true;
    }

    // 전진 순항 모드
    public void CruiseForward()
    {
        if(Input.GetKeyDown(KeyCode.R))
        {
            // 느림

            if(Input.GetKeyDown(KeyCode.R))
            {
                // 중간

                if(Input.GetKeyDown(KeyCode.R))
                {
                    // 최고

                }
            }
        }
    }

    // 후진 순항 모드
    public void CruiseBackward()
    {
        if (Input.GetKeyDown(KeyCode.F))
        {
            // 느림

            if (Input.GetKeyDown(KeyCode.F))
            {
                // 중간

                if (Input.GetKeyDown(KeyCode.F))
                {
                    // 최고

                }
            }
        }
    }

    public void UrgentBreak()
    {
        if(Input.GetKeyDown(KeyCode.Space))
        {

        }
    }
}
