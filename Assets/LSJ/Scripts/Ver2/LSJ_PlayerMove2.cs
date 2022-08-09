using System.Collections;
using System.Collections.Generic;
using UnityEngine;

// 기본 이동 조작 : 전후좌우, 360도, 플레이어 앞방향 기준
// 순항 조작 모드 : 전진 순항/후진 순항, 속도(느림, 중간, 최고)
// 급정지 : Space 입력 시 급정지(핸드브레이크)

// 360도 이동 WASD(전후좌우)
// R 키 : 전차 전진 순항모드
// F 키 : 전차 후진 순항모드

// WASD 이동 키를 입력 시 순항모드는 바로 해제

// 기본 이동 / 전후진 순항 모드 

public class LSJ_PlayerMove2 : MonoBehaviour
{
    public float moveSpeed = 30f; // 이동 속도
    public float rotSpeed = 50f; // 회전 속도

    private Rigidbody rbody;
    private Transform tr;


    // Start is called before the first frame update
    void Start()
    {
        rbody = GetComponent<Rigidbody>();
        tr = GetComponent<Transform>();
        rbody.centerOfMass = new Vector3(0.0f, -0.5f, 0.0f);
    }

    // Update is called once per frame
    private void FixedUpdate()
    {
        TankMove();
    }

    private void Update()
    {
        CruiseForward();
        CruiseBackward();
    }

    public void TankMove()
    {
        float h = Input.GetAxis("Horizontal");
        float v = Input.GetAxis("Vertical");

        tr.Translate(Vector3.forward * moveSpeed * v * Time.deltaTime);
        tr.Rotate(Vector3.up * rotSpeed * h * Time.deltaTime);

    }

    // 전진 순항 모드
    private bool isForwardMoving = false;
    private bool isBackMoving = false;
    private float CruiseForwardSpeed = 15f;
    private float CruiseBackSpeed = 15f;

    public void CruiseForward()
    {
        if (isForwardMoving)
        {
            LSJ_MoveAnimation.Instance.MoveAnim2();
            transform.position += transform.forward * CruiseForwardSpeed * Time.deltaTime;
        }
        if (isForwardMoving == false)
        {
            if (Input.GetKeyDown(KeyCode.R))
            {
                isForwardMoving = true;
            }
        }
        if (isForwardMoving == true)
        {
            if (Input.GetKeyDown(KeyCode.Space))
            {
                isForwardMoving = false;
            }
        }
    }

    // 후진 순항 모드
    public void CruiseBackward()
    {
        if (isBackMoving)
        {
            LSJ_MoveAnimation.Instance.MoveAnim2();
            transform.position += -transform.forward * CruiseForwardSpeed * Time.deltaTime;
        }
        if (isBackMoving == false)
        {
            if (Input.GetKeyDown(KeyCode.F))
            {
                isBackMoving = true;
            }
        }
        if (isBackMoving == true)
        {
            if (Input.GetKeyDown(KeyCode.Space))
            {
                isBackMoving = false;
            }
        }
    }
}