using System.Collections;
using System.Collections.Generic;
using UnityEngine;

public class LSJ_CamRotator : MonoBehaviour
{
    // ���콺 ��Ŭ���� ���¿��� ���콺 �������� �����̸� 
    // ���� ȸ�� ���� �÷��̾ �߽����� ī�޶� 360�� ȸ�� ����
    // �ʿ� �Ӽ�: ���콺 ������, ȸ�� �ӵ�, �÷��̾� Ÿ�� ������

    [SerializeField]
    private float mouseSensitivity = 3.0f;

    private float rotationX;
    private float rotationY;

    [SerializeField]
    private Transform target;

    [SerializeField]
    private float distanceFromTarget = 3.0f;

    private Vector3 currentRotation;
    private Vector3 smoothVelocity = Vector3.zero;

    [SerializeField]
    private float smoothTime = 3.0f;

    // Start is called before the first frame update
    void Start()
    {
        
    }

    // Update is called once per frame
    void Update()
    {
        if(Input.GetButton("Fire2"))
        {
            float mouseX = Input.GetAxis("Mouse X") * mouseSensitivity;
            float mouseY = Input.GetAxis("Mouse Y") * mouseSensitivity;

            rotationY += mouseX;
            rotationX += mouseY;

            rotationX = Mathf.Clamp(rotationX, -20f, 20f);

            Vector3 nextRotation = new Vector3(rotationX, rotationY);
            currentRotation = Vector3.SmoothDamp(currentRotation, nextRotation, ref smoothVelocity, smoothTime);
            transform.localEulerAngles = currentRotation;

            transform.position = target.position - transform.forward * distanceFromTarget;
        }
    }
}
