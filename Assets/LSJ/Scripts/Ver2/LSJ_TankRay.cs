using System.Collections;
using System.Collections.Generic;
using UnityEngine;
using UnityEngine.UI;

public class LSJ_TankRay : MonoBehaviour
{
    public GameObject TankAimUI;
    float distance = 10f;
    Vector3 currentPos;

    // Start is called before the first frame update
    void Start()
    {
        
    }

    // Update is called once per frame
    void FixedUpdate()
    {
        Ray ray = new Ray(transform.position, transform.forward);
        Debug.DrawRay(ray.origin, ray.direction * 100.0f, Color.red);
        RaycastHit hitInfo;

        if (Physics.Raycast(ray, out hitInfo, 10000f))
        {
        }
        else
        {
            currentPos = ray.GetPoint(distance);
        }

        Vector2 raycurrent = Camera.main.WorldToScreenPoint(currentPos); 
        TankAimUI.transform.position = raycurrent;
    }
}
