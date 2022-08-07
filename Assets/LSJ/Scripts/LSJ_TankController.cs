using System.Collections;
using System.Collections.Generic;
using UnityEngine;

public class LSJ_TankController : MonoBehaviour
{
    public Transform[] WheelMeshes;
    public WheelCollider[] WheelColls;
    Vector3 pos;
    Vector3 rotation;
    Quaternion quat;
    public float Force;
    public float RotSpeed;

    // Start is called before the first frame update
    private void Start()
    {
        rotation = transform.eulerAngles;
    }

    // Update is called once per frame
    private void Update()
    {
        transform.eulerAngles = rotation;

        for(int i = 0; i < WheelColls.Length; i++)
        {
            WheelColls[i].GetWorldPose(out pos, out quat);
            //WheelMeshes[i].position = pos;
            //WheelMeshes[i].rotation = quat;
        }

        foreach(var wheelcols in WheelColls)
        {
            wheelcols.motorTorque = Input.GetAxis("Vertical") * Force * Time.deltaTime * -1;
        }

        rotation.y += Input.GetAxis("Horizontal") * RotSpeed;
    }
}
