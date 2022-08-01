using System.Collections;
using System.Collections.Generic;
using UnityEngine;

// ���� ���� ����
// ���콺 ��ġ�� ���� ���� ���� �̵�(x��ǥ �̵� ���� : 50��?)
// �ʿ�Ӽ� : ���콺 ������ ��ġ, ȸ�� �ӵ�
public class LSJ_CannonControl : MonoBehaviour
{
    private Transform tr;
    public float rotSpeed = 100.0f;

    public float minimumY = -20f;
    public float maximumY = 0f;

    // float rotationY = 0f;
    // Start is called before the first frame update
    void Start()
    {
        tr = GetComponent<Transform>();
        tr.rotation = Quaternion.Euler(0, 0, 0);
    }

    // Update is called once per frame
    void Update()
    {
        float angle = -Input.GetAxis("Mouse Y") * Time.deltaTime * rotSpeed;
        angle = Mathf.Clamp(angle, minimumY, maximumY); // clamp ���� �� �ȸ���??
        tr.Rotate(angle, 0, 0);
        // rotationY += Input.GetAxis("Mouse Y") * sensitivityY;
        // rotationY = Mathf.Clamp(rotationY, minimumY, maximumY);

        // tr.Rotate(0, 0, rotationY);
    }
}
