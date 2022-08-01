using System.Collections;
using System.Collections.Generic;
using UnityEngine;

// �⺻ �̵� ���� : �����¿�, 360��, �÷��̾� �չ��� ����
// ���� ���� ��� : ���� ����/���� ����, �ӵ�(����, �߰�, �ְ�)
// ������ : Space �Է� �� ������(�ڵ�극��ũ)

// 360�� �̵� WASD(�����¿�)
// R Ű : ���� ���� ���׸��(1ȸ �Է� - 2ȸ �Է� - 3ȸ �Է� ������ ������)
// F Ű : ���� ���� ���׸��(1ȸ �Է� - 2ȸ �Է� - 3ȸ �Է� ������ ������)

// WASD �̵� Ű�� �Է� �� ���׸��� �ٷ� ����

public class LSJ_PlayerMove : MonoBehaviour
{
    public float moveSpeed = 30f; // �̵� �ӵ�
    public float rotSpeed = 50f; // ȸ�� �ӵ�

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

    // ���� ���� ���
    public void CruiseForward()
    {
        if(Input.GetKeyDown(KeyCode.R))
        {
            // ����

            if(Input.GetKeyDown(KeyCode.R))
            {
                // �߰�

                if(Input.GetKeyDown(KeyCode.R))
                {
                    // �ְ�

                }
            }
        }
    }

    // ���� ���� ���
    public void CruiseBackward()
    {
        if (Input.GetKeyDown(KeyCode.F))
        {
            // ����

            if (Input.GetKeyDown(KeyCode.F))
            {
                // �߰�

                if (Input.GetKeyDown(KeyCode.F))
                {
                    // �ְ�

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
