using System.Collections;
using System.Collections.Generic;
using UnityEngine;
using UnityEngine.UI;

// �÷��̾ �������� 3��Ī ���� : 360�� ȸ��
// �÷��̾ �������� 1��Ī �������� ��ȯ ����
// ���� ���
// ������ �� 6�� : ���Ÿ�(3��Ī ���ͺ�) - �ٰŸ�(���� ��ž��) - ����(1��Ī ������ ��)
// Shift �Ǵ� MouseWheel ���

// ���콺 ��Ŭ���� �� ���¿��� �����̸� ���� ȸ�� ���� 360�� ȸ�� ����
// �� ���, ��ž �� ���� �̵� ����
// ���콺 �ٷ� �⺻�Ÿ� - �߰Ÿ� - ���Ÿ� �̵�

// FOV 40 70 100
public class LSJ_CamControl : MonoBehaviour
{

    [Header("ī�޶� ��Ʈ��")]
    float x, y;
    public float sensitivity;
    public float distance;
    public Vector2 Xminmax;
    public Transform target;
    public GameObject zoomcam;
    public GameObject zoomPanel;
    public GameObject cursorUI;

    public float FOVspeed = 100.0f;
    // public Transform[] ShiftCamPos;

    [Header("ī�޶� ȸ��")]
    [SerializeField]
    private float mouseSensitivity = 3f;

    private float rotationY;
    private float rotationX;

    [SerializeField]
    private Transform basicTarget;

    // [SerializeField]
    // private Transform middleTarget;

    // [SerializeField]
    // private Transform RemoteTarget;

    [SerializeField]
    private float distanceFromTarget = 5.0f;

    private Vector3 currentRotation;
    private Vector3 smoothVelocity = Vector3.zero;

    [SerializeField]
    private float smoothTime = 0.2f;

    [SerializeField]
    private Vector2 rotationXMinMax = new Vector3(-30f, 30f);

    [Header("ī�޶� ���Ÿ� ȸ��")]
    [SerializeField]
    private float r_mouseSensitivity = 3f;

    private float r_rotationY;
    private float r_rotationX;

    [SerializeField]
    private Transform r_basicTarget;

    [SerializeField]
    private float r_distanceFromTarget = 5.0f;

    private Vector3 r_currentRotation;
    private Vector3 r_smoothVelocity = Vector3.zero;

    [SerializeField]
    private float r_smoothTime = 0.2f;

    [SerializeField]
    private Vector2 r_rotationXMinMax = new Vector3(-30f, 30f);

    [SerializeField]
    private float r_FOVspeed = 30f;
    private void Start()
    {
        Camera.main.fieldOfView = 40.0f;
    }

    private void LateUpdate()
    {
        CameraControl();
        ZoomMode();
        FOVmove();
        RemoteControl();
    }

    // �⺻ 
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

    private bool isZooming;
    void ZoomMode()
    {
        if (Input.GetKeyDown(KeyCode.LeftShift))
            isZooming = !isZooming;

        if(isZooming)
        {
            zoomcam.SetActive(true);
            zoomPanel.SetActive(true);
            cursorUI.SetActive(false);
        }
        else
        {
            zoomcam.SetActive(false);
            zoomPanel.SetActive(false);
            cursorUI.SetActive(true);
        }
    }

    void FOVmove()
    {
        float scroll = -Input.GetAxis("Mouse ScrollWheel") * FOVspeed;

        if (Camera.main.fieldOfView <= 40.0f && scroll < 0)
            Camera.main.fieldOfView = 40.0f;
        else if (Camera.main.fieldOfView >= 100.0f && scroll > 0)
            Camera.main.fieldOfView = 100.0f;
        else
            Camera.main.fieldOfView += scroll;
    }
    void RemoteControl()
    {
        if (Input.GetButton("Fire2"))
        {
            float mouseX = Input.GetAxis("Mouse X") * r_mouseSensitivity;
            float mouseY = Input.GetAxis("Mouse Y") * r_mouseSensitivity;

            r_rotationY += mouseX;
            r_rotationX += mouseY;

            r_rotationX = Mathf.Clamp(r_rotationX, r_rotationXMinMax.x, r_rotationXMinMax.y);

            Vector3 nextRotation = new Vector3(r_rotationX, r_rotationY);

            r_currentRotation = Vector3.SmoothDamp(r_currentRotation, nextRotation, ref r_smoothVelocity, r_smoothTime);
            transform.localEulerAngles = r_currentRotation;

            transform.position = r_basicTarget.position - transform.forward * r_distanceFromTarget;

            float scroll = -Input.GetAxis("Mouse ScrollWheel") * FOVspeed;

            if (Camera.main.fieldOfView <= 40.0f && scroll < 0)
                Camera.main.fieldOfView = 40.0f;
            else if (Camera.main.fieldOfView >= 100.0f && scroll > 0)
                Camera.main.fieldOfView = 100.0f;
            else
                Camera.main.fieldOfView += scroll;
        }
    }
}
