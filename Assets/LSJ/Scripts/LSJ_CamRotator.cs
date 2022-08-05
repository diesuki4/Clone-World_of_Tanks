using System.Collections;
using System.Collections.Generic;
using UnityEngine;

public class LSJ_CamRotator : MonoBehaviour
{
    // 마우스 우클릭한 상태에서 마우스 포지션을 움직이면 
    // 주포 회전 없이 플레이어를 중심으로 카메라 360도 회전 가능
    // 필요 속성: 마우스 포지션, 회전 속도, 플레이어 타겟 포지션

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
