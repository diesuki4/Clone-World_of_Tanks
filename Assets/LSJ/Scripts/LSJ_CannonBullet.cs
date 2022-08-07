using System.Collections;
using System.Collections.Generic;
using UnityEngine;

public class LSJ_CannonBullet : MonoBehaviour
{
    public float speed = 60000.0f;
    public GameObject expEffect;
    private SphereCollider collider;
    private Rigidbody rigidbody;

    // Start is called before the first frame update
    void Start()
    {
        collider = GetComponent<SphereCollider>();
        rigidbody = GetComponent<Rigidbody>();

        GetComponent<Rigidbody>().AddForce(transform.forward * speed);
        StartCoroutine(this.FireCannon(3.0f));
    }

    private void OnTriggerEnter(Collider other)
    {
        StartCoroutine(this.FireCannon(0.0f));
    }

    IEnumerator FireCannon(float tm)
    {
        yield return new WaitForSeconds(tm);
        collider.enabled = false;
        rigidbody.isKinematic = true;
        GameObject obj = (GameObject)Instantiate(expEffect, transform.position, Quaternion.identity);
        Destroy(obj, 1.0f);
        Destroy(this.gameObject, 1.0f);
    }

    // Update is called once per frame
    void Update()
    {
        
    }
}
