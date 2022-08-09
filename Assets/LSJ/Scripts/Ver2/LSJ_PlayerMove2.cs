using System.Collections;
using System.Collections.Generic;
using UnityEngine;

// �⺻ �̵� ���� : �����¿�, 360��, �÷��̾� �չ��� ����
// ���� ���� ��� : ���� ����/���� ����, �ӵ�(����, �߰�, �ְ�)
// ������ : Space �Է� �� ������(�ڵ�극��ũ)

// 360�� �̵� WASD(�����¿�)
// R Ű : ���� ���� ���׸��
// F Ű : ���� ���� ���׸��

// WASD �̵� Ű�� �Է� �� ���׸��� �ٷ� ����

// �⺻ �̵� / ������ ���� ��� 

public class LSJ_PlayerMove2 : MonoBehaviour
{
    public float moveSpeed = 30f; // �̵� �ӵ�
    public float rotSpeed = 50f; // ȸ�� �ӵ�

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

    // ���� ���� ���
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

    // ���� ���� ���
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