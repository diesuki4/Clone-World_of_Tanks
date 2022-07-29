using System.Collections;
using System.Collections.Generic;
using UnityEngine;

// �⺻ �̵� ���� : �����¿�, 360��, �÷��̾� �չ��� ����
// ���� ���� ��� : ���� ����/���� ����, �ӵ�(����, �߰�, �ְ�)
// ������ : Space �Է� �� ������(�ڵ�극��ũ)

// 360�� �̵� WASD(�����¿�)
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
