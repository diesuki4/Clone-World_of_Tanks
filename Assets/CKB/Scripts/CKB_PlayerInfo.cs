using System.Collections;
using System.Collections.Generic;
using UnityEngine;
using UnityEngine.UI;

public class CKB_PlayerInfo : MonoBehaviour
{
    public Transform target;

    CKB_HPManager ckbHPManager;
    RectTransform rectTransform;
    Slider hpSlider;
    Text hpText;

    void Start()
    {
        ckbHPManager = target.GetComponent<CKB_HPManager>();
        rectTransform = GetComponent<RectTransform>();
        hpSlider = GetComponentInChildren<Slider>();
        hpText = GetComponentInChildren<Text>();
    }

    void Update()
    {
        if (!target.gameObject.activeSelf)
        {
            gameObject.SetActive(false);
            return;
        }

        Vector3 dirCamToTarget = (target.position - Camera.main.transform.position).normalized;

        if (Vector3.Dot(Camera.main.transform.forward, dirCamToTarget) <= 0)
        {
            hpSlider.gameObject.SetActive(false);
        }
        else
        {
            hpSlider.gameObject.SetActive(true);

            rectTransform.anchoredPosition = Camera.main.WorldToScreenPoint(target.position);

            hpSlider.value = ckbHPManager.HP / ckbHPManager.MaxHP;

            hpText.text = ckbHPManager.HP + " / " + ckbHPManager.MaxHP;
        }
    }
}
