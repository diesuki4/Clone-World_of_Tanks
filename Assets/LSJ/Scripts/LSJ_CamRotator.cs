using System.Collections;
using System.Collections.Generic;
using UnityEngine;

public class LSJ_CamRotator : MonoBehaviour
{
    // 마우스 우클릭한 상태에서 마우스 포지션을 움직이면 
    // 주포 회전 없이 플레이어를 중심으로 카메라 360도 회전 가능


    // 마우스 우클릭을 한 상태에서 움직이면 주포 회전 없이 360도 회전 가능
    // 이 경우, 포탑 및 주포 이동 정지
    // 마우스 휠로 기본거리 - 중거리 - 원거리 이동

    // 필요 속성: 마우스 포지션, 회전 속도, 플레이어 타겟 포지션

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
        // 마우스 우클릭 상태에서 실행 & 마우스 휠을 통한 거리 조정
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
