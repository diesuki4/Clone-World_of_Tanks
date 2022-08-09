using System.Collections;
using System.Collections.Generic;
using UnityEngine;
using UnityEngine.UI;

// ���� ����
// ���콺 ������ ��ġ�� ���� ��ũ ������ �¿� ȸ��(360��)�ϰ� �ʹ�
// �ʿ� �Ӽ� : ���콺 ������ ��ġ, ȸ�� �ӵ�, ���� �Ա� ���ذ�

// mousePosition ��ġ ���� firePos���� LookAt
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
