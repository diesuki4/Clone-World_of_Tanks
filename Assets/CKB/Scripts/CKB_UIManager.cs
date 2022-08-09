using System.Collections;
using System.Collections.Generic;
using UnityEngine;
using UnityEngine.UI;

public class CKB_UIManager : MonoBehaviour
{
    public RectTransform rctAllyFireCoolTime;
    public RectTransform rctAllyHP;
    public RectTransform rctAllyScore;
    public RectTransform rctEnemyFireCoolTime;
    public RectTransform rctEnemyHP;
    public RectTransform rctEnemyScore;
    public GameObject[] aiAllies;
    public GameObject[] aiEnemies;

    public RectTransform rctAllyScoreUI;
    public RectTransform rctEnemyScoreUI;
    public Text textAllyScore;
    public Text textEnemyScore;

    public Text textTimer;
    public float gameTimeSec;

    void Start()
    {
        
    }

    void Update()
    {
        AIInfoUpdater();
        GameInfoUpdater();
        TimerUpdater();
    }

    void AIInfoUpdater()
    {
        for (int i = 0; i < rctAllyFireCoolTime.childCount; ++i)
        {
            if (aiAllies[i])
            {
                CKB_HPManager ckbHPManager = aiAllies[i].GetComponent<CKB_HPManager>();
                CKB_Cannon ckbCannon = aiAllies[i].GetComponentInChildren<CKB_Cannon>();
                CKB_Tank ckbTank = aiAllies[i].GetComponentInChildren<CKB_Tank>();
                rctAllyHP.GetChild(i).GetComponent<Slider>().value = ckbHPManager.HP / ckbHPManager.MaxHP;
                rctAllyFireCoolTime.GetChild(i).GetComponent<Slider>().value = (Time.time - ckbCannon.nextFireTime) / ckbCannon.FireRate;
                rctAllyScore.GetChild(i).GetComponent<Text>().text = (ckbTank.killScore == 0 ? "" : ckbTank.killScore.ToString());
            }
            else if (i != 0)
            {
                rctAllyHP.GetChild(i).GetComponent<Slider>().value = 0;
                rctAllyFireCoolTime.GetChild(i).GetComponent<Slider>().value = 0;
            }

            if (aiEnemies[i])
            {
                CKB_HPManager ckbHPManager = aiEnemies[i].GetComponent<CKB_HPManager>();
                CKB_Cannon ckbCannon = aiEnemies[i].GetComponentInChildren<CKB_Cannon>();
                CKB_Tank ckbTank = aiEnemies[i].GetComponentInChildren<CKB_Tank>();
                rctEnemyHP.GetChild(i).GetComponent<Slider>().value = ckbHPManager.HP / ckbHPManager.MaxHP;
                rctEnemyFireCoolTime.GetChild(i).GetComponent<Slider>().value = (Time.time - ckbCannon.nextFireTime) / ckbCannon.FireRate;
                rctEnemyScore.GetChild(i).GetComponent<Text>().text = (ckbTank.killScore == 0 ? "" : ckbTank.killScore.ToString());
            }
            else
            {
                rctEnemyHP.GetChild(i).GetComponent<Slider>().value = 0;
                rctEnemyFireCoolTime.GetChild(i).GetComponent<Slider>().value = 0;
            }
        }
    }

    void GameInfoUpdater()
    {
        int allyScore = 0;
        int enemyScore = 0;

        foreach (RectTransform rct in rctAllyScore)
        {
            string text = rct.GetComponent<Text>().text;
            allyScore += (text == "" ? 0 : int.Parse(text));
        }

        foreach (RectTransform rct in rctEnemyScore)
        {
            string text = rct.GetComponent<Text>().text;
            enemyScore += (text == "" ? 0 : int.Parse(text));
        }

        textAllyScore.text = allyScore.ToString();
        textEnemyScore.text = enemyScore.ToString();

        for (int i = 0; i < rctAllyScoreUI.childCount; ++i)
        {
            if (i < allyScore)
                rctAllyScoreUI.GetChild(i).GetComponent<Image>().enabled = true;
            else
                rctAllyScoreUI.GetChild(i).GetComponent<Image>().enabled = false;

            if (i < 5 - enemyScore)
                rctEnemyScoreUI.GetChild(i).GetComponent<Image>().enabled = true;
            else
                rctEnemyScoreUI.GetChild(i).GetComponent<Image>().enabled = false;
        }
    }

    void TimerUpdater()
    {
        int gameTimeCeil = (int)Mathf.Ceil(gameTimeSec);

        int minutes = gameTimeCeil / 60;
        int sec = gameTimeCeil - minutes * 60;

        textTimer.text = minutes.ToString() + " : " + sec.ToString().PadLeft(2, '0');

        if (0 < gameTimeSec)
            gameTimeSec -= Time.deltaTime;
    }
}
