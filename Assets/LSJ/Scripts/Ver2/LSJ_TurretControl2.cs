using System.Collections;
using System.Collections.Generic;
using UnityEngine;
using UnityEngine.UI;

// 주포 조작
// 마우스 포인터 위치에 따라 탱크 포신을 좌우 회전(360도)하고 싶다
// 필요 속성 : 마우스 포인터 위치, 회전 속도, 포신 입구 기준값

// mousePosition 위치 값을 firePos값이 LookAt
public class LSJ_TurretControl2 : MonoBehaviour
{
    private Transform tr;
    private RaycastHit hit;
    public float rotSpeed = 3f;
    // Start is called before the first frame update
    void Start()
    {
        tr = GetComponent<Transform>();
    }

    // Update is called once per frame
    void Update()
    {
        MouseRay();
    }

    void MouseRay()
    {
        Ray ray = Camera.main.ScreenPointToRay(Input.mousePosition);
        Debug.DrawRay(ray.origin, ray.direction * 100.0f, Color.green);

        if (Physics.Raycast(ray, out hit, Mathf.Infinity))
        {
            Vector3 relative = tr.InverseTransformPoint(hit.point);
            float angle = Mathf.Atan2(relative.x, relative.z) * Mathf.Rad2Deg;
            tr.Rotate(0, angle * Time.deltaTime * rotSpeed, 0);
        }
    }
}
