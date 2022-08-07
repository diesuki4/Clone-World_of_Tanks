using System.Collections;
using System.Collections.Generic;
using UnityEngine;

public class LSJ_TankCamera : MonoBehaviour
{
    public GameObject Target;  // 타겟 오브젝트
    public Camera[] Cameras;  // 카메라 배열

    public float TurnSpeed = 5; // 이동속도

    private int indexCamera; // 카메라 인덱스 번호
    public Vector3 Offset = new Vector3(0, 0.5f, 0);
    public float Distance = 5; // 거리
    public float Damping = 0.5f; // 제동

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
