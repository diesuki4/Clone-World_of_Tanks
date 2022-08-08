using System.Collections;
using System.Collections.Generic;
using UnityEngine;

public class LSJ_Aim : MonoBehaviour
{
    public float rotation_speed = 0.5f;
    private Camera cam;

    // Start is called before the first frame update
    void Start()
    {
        cam = Camera.main;
        Cursor.lockState = CursorLockMode.Locked;
    }
    // Update is called once per frame
    void Update()
    {

        Ray ray = cam.ScreenPointToRay(Input.mousePosition);
        Quaternion look_rotation = Quaternion.LookRotation(ray.direction);

        Quaternion current_rotation = transform.rotation;

        //Smooth Look
        float angular_difference = Quaternion.Angle(current_rotation, look_rotation);
        if (angular_difference > 0)
        {
            transform.rotation = Quaternion.Slerp(current_rotation, look_rotation, (rotation_speed * 90 * Time.deltaTime) / angular_difference);

        }
        else
        {
            transform.rotation = look_rotation;
        }
    }
}
