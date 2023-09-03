using System.Collections;
using System.Collections.Generic;
using UnityEngine;

public class CKB_GameManager_old : MonoBehaviour
{
    public static CKB_GameManager_old Instance;

    void Awake()
    {
        if (Instance == null)
            Instance = this;
    }

    public float gameTimeSec;
    public float[] fieldTimeSec;
    public float fieldShrinkTime;
    public Transform fileds;
    [HideInInspector]
    public int currentField;

    float t;

    void Start()
    {
        currentField = 0;

        foreach (Transform field in fileds)
            field.gameObject.SetActive(false);
    }

    void Update()
    {
        float remainTime = gameTimeSec - t;

        if (currentField < fileds.childCount && remainTime <= fieldTimeSec[currentField])
        {
            fileds.GetChild(currentField).gameObject.SetActive(true);

            if (1 <= currentField)
                fileds.GetChild(currentField - 1).gameObject.SetActive(false);

            if (currentField < 2)
                StartCoroutine(IEShrinkField());

            ++currentField;
        }

        t += Time.deltaTime;
    }

    IEnumerator IEShrinkField()
    {
        float t = 0;

        Transform field = fileds.GetChild(currentField);
        Transform nextField = fileds.GetChild(currentField + 1);

        Vector3 o_localScale = field.localScale;

        while ((t += Time.deltaTime) < fieldShrinkTime)
        {
            field.localScale = Vector3.Lerp(o_localScale, nextField.transform.localScale, t / fieldShrinkTime);

            yield return null;
        }
    }
}
