using System.Collections;
using System.Collections.Generic;
using UnityEngine;

// 플레이어를 기준으로 3인칭 시점 : 360도 회전
// 플레이어를 기준으로 1인칭 시점으로 전환 가능
// 조준 모드
// 시점은 총 6개 : 원거리(3인칭 쿼터뷰) - 근거리(주포 포탑뷰) - 조준(1인칭 조준점 뷰)
// Shift 또는 MouseWheel 사용

// 마우스 우클릭을 한 상태에서 움직이면 주포 회전 없이 360도 회전 가능
// 이 경우, 포탑 및 주포 이동 정지
// 마우스 휠로 기본거리 - 중거리 - 원거리 이동
public class LSJ_CamControl : MonoBehaviour
{

    [Header("카메라 컨트롤")]
    float x, y;
    public float sensitivity;
    public float distance;
    public Vector2 Xminmax;
    public Transform target;

    [Header("카메라 회전")]
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
