using System.Collections;
using System.Collections.Generic;
using UnityEngine;
using UnityEngine.UI;

public class LSJ_TankRay : MonoBehaviour
{
    public Image TankAimUI;
    float distance = 100000f;

    // Start is called before the first frame update
    void Start()
    {
        
    }

    // Update is called once per frame
    void Update()
    {
        Ray ray = new Ray(transform.position, transform.forward);
        Debug.DrawRay(ray.origin, ray.direction * 100.0f, Color.red);
        RaycastHit hitInfo;

        if (Physics.Raycast(ray, out hitInfo, 10000f))
        {
        }
        else
        {
            Vector3 currentPos = ray.GetPoint(distance);
        }
    }
}
