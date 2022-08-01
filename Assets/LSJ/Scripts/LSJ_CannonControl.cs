using System.Collections;
using System.Collections.Generic;
using UnityEngine;

// 주포 상하 조작
// 마우스 위치에 따라 주포 상하 이동(x좌표 이동 범위 : 50도?)
// 필요속성 : 마우스 포인터 위치, 회전 속도
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
        angle = Mathf.Clamp(angle, minimumY, maximumY); // clamp 값이 왜 안먹지??
        tr.Rotate(angle, 0, 0);
        // rotationY += Input.GetAxis("Mouse Y") * sensitivityY;
        // rotationY = Mathf.Clamp(rotationY, minimumY, maximumY);

        // tr.Rotate(0, 0, rotationY);
    }
}
