using System.Collections;
using System.Collections.Generic;
using UnityEngine;

public class CKB_FixMarker : MonoBehaviour
{
    public Transform[] tanks;

    void Update()
    {
        for (int i = 0; i < transform.childCount; ++i)
        {
            Transform tr = transform.GetChild(i);
            Vector3 pos = tr.position;

            if (tanks[i].gameObject.activeSelf)
                tr.position = Vector3.Scale(new Vector3(1, 0, 1), tanks[i].position) + Vector3.up * tr.position.y;
            else
                tr.gameObject.SetActive(false);
        }
    }
}
