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
    public GameObject crosshair;

    public float FOVspeed = 10.0f;
    // public Transform[] ShiftCamPos;

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
        ZoomIn();
        ZoomOut();
        FOVmove();
        //Shiftmove();
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

    void ZoomIn()
    {
        if(Input.GetButtonDown("Fire2"))
        {
            zoomcam.SetActive(true);
            crosshair.SetActive(true);
        }
    }

    // �߻� �� �˹�
    void ZoomOut() 
    {
        if(Input.GetButtonUp("Fire2"))
        {
            zoomcam.SetActive(false);
            crosshair.SetActive(false);
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

        // ���� ���� ������ ���� ĳ���͸� �ٶ󺻴�
        /* if (cameraTarget && thisCamera.fieldOfView <= 30.0f)
        {
            transform.rotation = Quaternion.Slerp(transform.rotation, Quaternion.LookRotation(cameraTarget.position - transform.position), 0.15f);
        } */
        // ���� �̰� �ۿ����� ������ ī�޶� �������� �ǵ��ư�
        /* {
            transform.rotation = Quaternion.Slerp(transform, rotationX, Quaternion.LookRotation(worldDefaultForward), 0.15f);
        }*/
    }

    /* void Shiftmove()
    {
        if(Input.GetKey(KeyCode.LeftShift))
        {
            for (int i = 1; i < ShiftCamPos.Length; i++)
            {
                Camera.main.transform.position = ShiftCamPos[i].position;
                
            }
        }
    }*/
}
