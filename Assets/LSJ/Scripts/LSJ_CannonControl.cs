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

    public float minimumY = -60f;
    public float maximumY = 60f;

    float my;
    float angle;

    float mindeltax;
    float maxdeltax;

    // private Quaternion currRot = Quaternion.identity;
    // float rotationY = 0f;
    // Start is called before the first frame update

    private void Awake()
    {
        tr = GetComponent<Transform>();
        //currRot = tr.localRotation;
    }
    void Start()
    {
        tr.rotation = Quaternion.Euler(0, 0, 0);
        
        // my = -transform.eulerAngles.x;
        // angle = -transform.eulerAngles.x;
    }

    // Update is called once per frame
    void Update()
    {
        //Vector3 dir = Input.mousePosition - transform.position;
        //dir.x = 0f;
        //var nextRot = Quaternion.LookRotation(dir);
        //transform.rotation = Quaternion.Slerp(transform.rotation, nextRot, Time.deltaTime * rotSpeed);

        // mindeltax += -30f * Time.deltaTime;
        // maxdeltax += 5f * Time.deltaTime;

        // float v = Input.GetAxis("Mouse Y");
        // angle += v * rotSpeed * Time.deltaTime;
        // angle = Mathf.Clamp(angle, -10, 30);
        // transform.eulerAngles = new Vector3(-angle, 0, 0);

        // float v = Input.GetAxis("Mouse Y");
        // my += v * rotSpeed * Time.deltaTime;
        // my = Mathf.Clamp(my, -30, 30);
        // transform.eulerAngles = new Vector3(-my, 0, 0);

        /*if (tr.rotation == Quaternion.Euler(mindeltax, 0, 0))
            tr.rotation = Quaternion.Euler(-30, 0, 0);
        if (tr.rotation == Quaternion.Euler(maxdeltax, 0, 0))
            tr.rotation = Quaternion.Euler(5, 0, 0);*/

        // rotationY += Input.GetAxis("Mouse Y") * sensitivityY;
        // rotationY = Mathf.Clamp(rotationY, minimumY, maximumY);

        // tr.Rotate(0, 0, rotationY);

        float angle = -Input.GetAxis("Mouse ScrollWheel") * Time.deltaTime * rotSpeed;
        angle = Mathf.Clamp(angle, minimumY, maximumY);
        //tr.localRotation = Quaternion.Slerp(tr.localRotation, currRot, Time.deltaTime * 3.0f);
        tr.Rotate(angle, 0, 0);
        /*if (tr.rotation.x > -15)
            tr.rotation = Quaternion.Euler(-15, 0, 0);
        if (tr.rotation.x < 0)
            tr.rotation = Quaternion.Euler(-0, 0, 0);*/
    }
}
