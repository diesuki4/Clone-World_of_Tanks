using System.Collections;
using System.Collections.Generic;
using UnityEngine;

public class LSJ_TankCamera : MonoBehaviour
{
    public GameObject Target;  // Ÿ�� ������Ʈ
    public Camera[] Cameras;  // ī�޶� �迭

    public float TurnSpeed = 5; // �̵��ӵ�

    private int indexCamera; // ī�޶� �ε��� ��ȣ
    public Vector3 Offset = new Vector3(0, 0.5f, 0);
    public float Distance = 5; // �Ÿ�
    public float Damping = 0.5f; // ����

    public void SwitchCameras()
    {
        
    }

    // Start is called before the first frame update
    void Start()
    {
        
    }

    // Update is called once per frame
    void Update()
    {
        
    }
}
