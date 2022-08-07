using System.Collections;
using System.Collections.Generic;
using UnityEngine;

// �÷��̾ �������� 3��Ī ���� : 360�� ȸ��
// �÷��̾ �������� 1��Ī �������� ��ȯ ����
// ���� ���
// ������ �� 6�� : ���Ÿ�(3��Ī ���ͺ�) - �ٰŸ�(���� ��ž��) - ����(1��Ī ������ ��)
// Shift �Ǵ� MouseWheel ���

// ���콺 ��Ŭ���� �� ���¿��� �����̸� ���� ȸ�� ���� 360�� ȸ�� ����
// �� ���, ��ž �� ���� �̵� ����
// ���콺 �ٷ� �⺻�Ÿ� - �߰Ÿ� - ���Ÿ� �̵�
public class LSJ_CamControl : MonoBehaviour
{

    [Header("ī�޶� ��Ʈ��")]
    float x, y;
    public float sensitivity;
    public float distance;
    public Vector2 Xminmax;
    public Transform target;

    [Header("ī�޶� ȸ��")]
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
    private void Start()
    {

    }

    private void LateUpdate()
    {
        CameraControl();

        if (Input.GetButton("Fire2"))
            CamRotate();
    }

    void CameraControl()
    {
        x += Input.GetAxis("Mouse Y") * sensitivity * -1;
        y += Input.GetAxis("Mouse X") * sensitivity;

        x = Mathf.Clamp(x, Xminmax.x, Xminmax.y);

        transform.eulerAngles = new Vector3(x, y + 180, 0);
        transform.position = target.position - transform.forward * distance;
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
