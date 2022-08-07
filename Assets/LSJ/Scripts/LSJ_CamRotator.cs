using System.Collections;
using System.Collections.Generic;
using UnityEngine;

public class LSJ_CamRotator : MonoBehaviour
{
    // ���콺 ��Ŭ���� ���¿��� ���콺 �������� �����̸� 
    // ���� ȸ�� ���� �÷��̾ �߽����� ī�޶� 360�� ȸ�� ����


    // ���콺 ��Ŭ���� �� ���¿��� �����̸� ���� ȸ�� ���� 360�� ȸ�� ����
    // �� ���, ��ž �� ���� �̵� ����
    // ���콺 �ٷ� �⺻�Ÿ� - �߰Ÿ� - ���Ÿ� �̵�

    // �ʿ� �Ӽ�: ���콺 ������, ȸ�� �ӵ�, �÷��̾� Ÿ�� ������

    [SerializeField]
    private float mouseSensitivity = 3f;

    private float rotationY;
    private float rotationX;

    [SerializeField]
    private Transform basicTarget;

    [SerializeField]
    private Transform middleTarget;

    [SerializeField]
    private Transform RemoteTarget;

    [SerializeField]
    private float distanceFromTarget = 5.0f;

    private Vector3 currentRotation;
    private Vector3 smoothVelocity = Vector3.zero;

    [SerializeField]
    private float smoothTime = 0.2f;

    [SerializeField]
    private Vector2 rotationXMinMax = new Vector3(-30f, 30f);
    // Start is called before the first frame update
    void Start()
    {

    }

    // Update is called once per frame
    void Update()
    {
        // ���콺 ��Ŭ�� ���¿��� ���� & ���콺 ���� ���� �Ÿ� ����
        if (Input.GetButton("Fire2"))
            CamRotate();
    }

    void CamRotate()
    {
        float mouseX = Input.GetAxis("Mouse X") * mouseSensitivity;
        float mouseY = Input.GetAxis("Mouse Y") * mouseSensitivity;

        rotationY += mouseX;
        rotationX += mouseY;

        rotationX = Mathf.Clamp(rotationX, rotationXMinMax.x, rotationXMinMax.y);

        Vector3 nextRotation = new Vector3(rotationX, rotationY);

        currentRotation = Vector3.SmoothDamp(currentRotation, nextRotation, ref smoothVelocity, smoothTime);
        transform.localEulerAngles = currentRotation;

        transform.position = basicTarget.position - transform.forward * distanceFromTarget;
    }
}
