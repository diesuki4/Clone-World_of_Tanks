using System.Collections;
using System.Collections.Generic;
using UnityEngine;
using UnityEngine.UI;

// ���� �ٸ� ������ �̹����� Ŭ����  �� ����
// �� �̹����� ����� stats �гΰ� model�� Ȱ��ȭ�ǰ� �ʹ�
// Ȱ��ȭ �� ������ ���� ������Ʈ ���� ������ ��Ȱ��ȭ ���ش�.
public class LSJ_LobbyUISelection : MonoBehaviour
{
    public GameObject Tank1;
    public GameObject panel1;
    public GameObject Tank2;
    public GameObject panel2;
    public GameObject Tank3;
    public GameObject panel3;
    public GameObject Tank4;
    public GameObject panel4;
    // Start is called before the first frame update
    void Start()
    {
        
    }

    // Update is called once per frame
    void Update()
    {
        
    }

    public void OnClickStatsFirst()
    {
        Tank1.SetActive(true);
        panel1.SetActive(true);
        Tank2.SetActive(false);
        panel2.SetActive(false);
        Tank3.SetActive(false);
        panel3.SetActive(false);
        Tank4.SetActive(false);
        panel4.SetActive(false);
    }
    public void OnClickStatsSecond()
    {
        Tank1.SetActive(false);
        panel1.SetActive(false);
        Tank2.SetActive(true);
        panel2.SetActive(true);
        Tank3.SetActive(false);
        panel3.SetActive(false);
        Tank4.SetActive(false);
        panel4.SetActive(false);
    }
    public void OnClickStatsThird()
    {
        Tank1.SetActive(false);
        panel1.SetActive(false);
        Tank2.SetActive(false);
        panel2.SetActive(false);
        Tank3.SetActive(true);
        panel3.SetActive(true);
        Tank4.SetActive(false);
        panel4.SetActive(false);
    }
    public void OnClickStatsForth()
    {
        Tank1.SetActive(false);
        panel1.SetActive(false);
        Tank2.SetActive(false);
        panel2.SetActive(false);
        Tank3.SetActive(false);
        panel3.SetActive(false);
        Tank4.SetActive(true);
        panel4.SetActive(true);
    }
}
