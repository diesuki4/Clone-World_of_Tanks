using System;
using System.Collections;
using System.Collections.Generic;
using UnityEngine;

public class CKB_OutlineManager : MonoBehaviour
{
    public bool debugMode;
    public float radius;
    public float deltaRadius;
    public float deltaAngle;
    public LayerMask detectLayers;
    public GameObject[] arrAI;

    void Start() { }

    void Update()
    {
        float currentRadius = 0;
        List<Ray> rays = new List<Ray>();

        while (currentRadius <= radius)
        {
            float currentAngle = 0;

            while (currentAngle < 360)
            {
                Vector3 deltaVec = Quaternion.Euler(0, 0, currentAngle) * Vector3.right;
                Vector3 newCameraPos = Camera.main.transform.position + Camera.main.transform.TransformDirection(deltaVec) * currentRadius;
                
                Ray ray = new Ray(newCameraPos, Camera.main.transform.forward);
                rays.Add(ray);

                if (debugMode)
                    Debug.DrawLine(newCameraPos, newCameraPos + ray.direction * 500, Color.red, Time.deltaTime);

                currentAngle += deltaAngle;

                if (currentRadius == 0)
                    break;
            }

            currentRadius += deltaRadius;
            print(currentRadius);
        }

        List<GameObject> objDetected = new List<GameObject>();

        foreach (Ray ray in rays)
        {
            foreach (RaycastHit hitInfo in Physics.RaycastAll(ray, float.MaxValue, detectLayers))
            {
                GameObject hitObj = hitInfo.transform.gameObject;
                Outline outline = null;

                if (objDetected.Contains(hitObj))
                    continue;

                if ((outline = hitObj.GetComponent<Outline>()) != null)
                {
                    outline.enabled = true;
                    objDetected.Add(hitObj);
                }
            }
        }

        foreach (GameObject AI in arrAI)
            if (!objDetected.Contains(AI))
                AI.GetComponent<Outline>().enabled = false;
    }
/*
    void Update()
    {
        Vector3 cameraPos2D = Camera.main.WorldToScreenPoint(Camera.main.transform.position);
        Vector3 newCameraPos2D = cameraPos2D + deltaVec * radius;
        Vector3 newCameraPos3D = Camera.main.ScreenToWorldPoint(newCameraPos2D + Vector3.forward);
        Vector3 newCameraPos3D = Camera.main.transform.position + Camera.main.transform.TransformDirection(deltaVec) * radius;

        deltaVec = Quaternion.Euler(0, 0, deltaAngle) * deltaVec;

        Ray ray = new Ray(newCameraPos3D, Camera.main.transform.forward);
        Debug.DrawLine(newCameraPos3D, newCameraPos3D + ray.direction * 500, Color.red, 1);

        List<GameObject> objDetected = new List<GameObject>();

        foreach (RaycastHit hitInfo in Physics.RaycastAll(ray, float.MaxValue, detectLayers))
        {
            GameObject hitObj = hitInfo.transform.gameObject;
            Outline outline = null;

            if ((outline = hitObj.GetComponent<Outline>()) != null)
            {
                outline.enabled = true;
                objDetected.Add(hitObj);
            }
        }
        foreach (GameObject AI in arrAI)
            if (!objDetected.Contains(AI))
                AI.GetComponent<Outline>().enabled = false;
    }*/
}
