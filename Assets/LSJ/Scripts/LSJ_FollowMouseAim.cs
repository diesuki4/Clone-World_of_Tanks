using System.Collections;
using System.Collections.Generic;
using UnityEngine;

public class LSJ_FollowMouseAim : MonoBehaviour
{
    Vector3 pos;
    public float speed = 1f;
    // Start is called before the first frame update
    void Start()
    {
        
    }

    // Update is called once per frame
    void Update()
    {
        pos = Input.mousePosition;
        pos.z = speed;
        transform.position = Camera.main.ScreenToWorldPoint(pos);
    }
}
