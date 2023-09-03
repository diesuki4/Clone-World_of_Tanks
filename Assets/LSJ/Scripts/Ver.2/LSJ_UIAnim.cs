using System.Collections;
using System.Collections.Generic;
using UnityEngine;
using DG.Tweening;

public class LSJ_UIAnim : MonoBehaviour
{
    public RectTransform AttackSuccess;
    public RectTransform RecordAttackDamage;
    public RectTransform DamagedFromEnemy;
    public RectTransform RecordDamagedFromEnemy;


    public static LSJ_UIAnim Instance;


    Vector3 originPos1;
    Vector3 originPos2;
    Vector3 originPos3;
    Vector3 originPos4;

    private void Awake()
    {

        if (Instance == null)
            Instance = this;
    }

    void Start()
    {
        originPos1 = AttackSuccess.position;
        originPos2 = RecordAttackDamage.position;
        originPos3 = DamagedFromEnemy.position;
        originPos4 = RecordDamagedFromEnemy.position;
    }

    public enum PanelType
    {
        Left,
        Right
    }
    public PanelType panelType;

    public void ShowLog(PanelType panelType)
    {
        RectTransform panel1 = null;
        RectTransform panel2 = null;

        switch (panelType)
        {
            case PanelType.Left:
                panel1 = AttackSuccess;
                panel2 = RecordAttackDamage;
                StartCoroutine(ShowLogMesssage_1(panel1, panel2));
                break;
            case PanelType.Right:
                panel1 = DamagedFromEnemy;
                panel2 = RecordDamagedFromEnemy;

                StartCoroutine(ShowLogMesssage_2(panel1, panel2));
                break;
        }

    }

    // Update is called once per frame
    void Update()
    {

    }

    IEnumerator ShowLogMesssage_1(RectTransform panel1, RectTransform panel2)
    {
        panel1.DOMove(originPos1 + Vector3.right * 250, 1f).SetEase(Ease.InOutSine);
        panel2.DOMove(originPos2 + Vector3.right * 250, 1f).SetEase(Ease.InOutSine);
        yield return new WaitForSeconds(2f);

        panel1.DOMove(originPos1, 1f).SetEase(Ease.InOutSine);
        panel2.DOMove(originPos2, 1f).SetEase(Ease.InOutSine);
    }

    IEnumerator ShowLogMesssage_2(RectTransform panel1, RectTransform panel2)
    {
        panel1.DOMove(originPos3 + Vector3.left * 350, 1f).SetEase(Ease.InOutSine);
        panel2.DOMove(originPos4 + Vector3.left * 350, 1f).SetEase(Ease.InOutSine);
        yield return new WaitForSeconds(2f);

        panel1.DOMove(originPos3, 1f).SetEase(Ease.InOutSine);
        panel2.DOMove(originPos4, 1f).SetEase(Ease.InOutSine);
    }
}
