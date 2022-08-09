using System.Collections;
using System.Collections.Generic;
using UnityEngine;

public class LSJ_TankShoot : MonoBehaviour
{
    public GameObject cannon = null;
    public Transform firePos;

    // Start is called before the first frame update
    void Start()
    {
        cannon = (GameObject)Resources.Load("Bullet");
    }

    // Update is called once per frame
    void Update()
    {
        Fire();
    }

    void Fire()
    {
        if(Input.GetMouseButtonDown(0))
            Instantiate(cannon, firePos.position, firePos.rotation);
    }
}
