using System.Collections;
using System.Collections.Generic;
using UnityEngine;
using UnityEngine.UI;
using DG.Tweening;

// UIManager 에 부착
public class CKB_UIManager : MonoBehaviour
{
    public static CKB_UIManager Instance;

    void Awake()
    {
        if (Instance == null)
            Instance = this;
    }

    public float floatingHPMaxY;
    public float floatingHPMinY;
    public float floatingHPLerfDistance;

    public RectTransform rctAllyFloatingHP;

    // Canvas/Ally Fire CoolTime
    // 7개의 자식 오브젝트가 플레이어 팀 발사 쿨타임 슬라이더 
    public RectTransform rctAllyFireCoolTime;

    // Canvas/Ally HP
    // 7개의 자식 오브젝트가 플레이어 팀 HP 슬라이더 
    public RectTransform rctAllyHP;

    // Canvas/HP Score
    // 7개의 자식 오브젝트가 플레이어 팀 킬 수 텍스트
    public RectTransform rctAllyScore;

    public RectTransform rctEnemyFloatingHP;

    // Canvas/Enemy Fire CoolTime
    // 7개의 자식 오브젝트가 에너미 팀 발사 쿨타임 슬라이더 
    public RectTransform rctEnemyFireCoolTime;

    // Canvas/Enemy HP
    // 7개의 자식 오브젝트가 에너미 팀 HP 슬라이더 
    public RectTransform rctEnemyHP;

    // Canvas/Enemy Score
    // 7개의 자식 오브젝트가 에너미 팀 킬 수 텍스트
    public RectTransform rctEnemyScore;

    // AI Tanks/Player 에 존재
    // 플레이어 팀 탱크 1부터 ~ 7까지 지정 (0번째는 플레이어이므로 None)
    public GameObject[] allies;

    // AI Tanks/Enemy 에 존재
    // 에너미 팀 탱크 1부터 ~ 7까지 지정
    public GameObject[] enemies;

    // Canvas/Game Info/Score UI Ally
    // 7개의 자식 오브젝트가 중앙 상단의 플레이어 팀
    // 남은 탱크 수에 해당하는 마름모 이미지
    public RectTransform rctAllyScoreUI;

    // Canvas/Game Info/Score UI Enemy
    // 7개의 자식 오브젝트가 중앙 상단의 에너미 팀
    // 남은 탱크 수에 해당하는 마름모 이미지
    public RectTransform rctEnemyScoreUI;

    // Canvas/Game Info/Total Score Ally/Ally Score
    // 중앙 상단의 플레이어 팀 총 킬 수 텍스트
    public Text textAllyScore;

    // Canvas/Game Info/Total Score Enemy/Enemy Score
    // 중앙 상단의 에너미 팀 총 킬 수 텍스트
    public Text textEnemyScore;

    // Canvas/Game Info/Timer/Time
    // 중앙 상단 현재 남은 시간 표시 텍스트
    public Text textTimer;

    // UI_Settings/Scaler
    // 자식 중 알림창 4개가 사용된다.
    public RectTransform alertPanels;

    // Canvas/Game Result
    public Text textGameResult;

    // GameManager 에서 사용되므로 public 으로 지정 
    // 플레이어 팀 총 킬 수
    [HideInInspector]
    public int allyScore = 0;
    // 에너미 팀 총 킬 수
    [HideInInspector]
    public int enemyScore = 0;

    bool isEnd = false;

    void Start() { }

    void Update()
    {
        // 화면 사이드에 위치한 HP 슬라이더, HP 텍스트, 발사 쿨타임 등의 값을 갱신한다.
        AIInfoUpdater();
        // 중앙 상단의 팀별 총 킬 수
        // 중앙 상단의 팀별 남은 탱크 수에 해당하는 마름모 이미지
        // 갱신
        GameInfoUpdater();
        // 중앙 상단의 현재 남은 시간 텍스트 갱신
        TimerUpdater();
    }

    // 화면 사이드에 위치한 HP 슬라이더, HP 텍스트, 발사 쿨타임 등의 값을 갱신한다.
    void AIInfoUpdater()
    {
        // 등록된 탱크의 개수만큼 반복 (플레이어/에너미 둘 다 7)
        for (int i = 0; i < rctAllyFireCoolTime.childCount; ++i)
        {
            // None 이 아니면
            // 플레이어는 None 으로 지정되어 있다.
            // 탱크가 죽었을 경우에도 None 으로 처리된다.
            if (allies[i].activeSelf)
            {
                CKB_HPManager ckbHPManager = allies[i].GetComponent<CKB_HPManager>();
                CKB_Tank ckbTank = allies[i].GetComponent<CKB_Tank>();
                CKB_TankAI ckbTankAI = allies[i].GetComponent<CKB_TankAI>();
                float fireCoolTime = 1;

                Vector3 dirCamToTarget = (allies[i].transform.position - Camera.main.transform.position).normalized;

                if (Vector3.Dot(Camera.main.transform.forward, dirCamToTarget) <= 0)
                {
                    rctAllyFloatingHP.GetChild(i).gameObject.SetActive(false);
                }
                else
                {
                    Vector3 localScale = Vector3.one * Mathf.Lerp(1, 0.7f, Vector3.Distance(allies[0].transform.position, allies[i].transform.position) / 400);
                    rctAllyFloatingHP.GetChild(i).GetComponent<RectTransform>().localScale = localScale;

                    rctAllyFloatingHP.GetChild(i).gameObject.SetActive(true);

                    rctAllyFloatingHP.GetChild(i).GetComponent<RectTransform>().anchoredPosition = Camera.main.WorldToScreenPoint(allies[i].transform.position);

                    rctAllyFloatingHP.GetChild(i).GetComponentInChildren<Slider>().GetComponent<RectTransform>().anchoredPosition
                        = Vector2.up * Mathf.Lerp(floatingHPMaxY, floatingHPMinY, Vector3.Distance(allies[0].transform.position, allies[i].transform.position) / floatingHPLerfDistance);

                    rctAllyFloatingHP.GetChild(i).GetComponentInChildren<Slider>().value = ckbHPManager.HP / ckbHPManager.MaxHP;

                    rctAllyFloatingHP.GetChild(i).GetComponentInChildren<Text>().text = ckbHPManager.HP + " / " + ckbHPManager.MaxHP;
                }

                // 일반 탱크는 CKB_Cannon 을 갖는다.
                if (ckbTankAI)
                    // 발사 쿨타임 슬라이더에 적용할 값
                    // 0 부터 증가 중인 쿨타임 / 총 발사 쿨타임
                    fireCoolTime = (ckbTankAI.FiringDurationMax - Mathf.Clamp(ckbTankAI.aiFireTime, 0, ckbTankAI.FiringDurationMax)) / ckbTankAI.FiringDurationMax;

                // HP 슬라이더에 적용 = 현재 HP / 총 HP
                rctAllyHP.GetChild(i).GetComponent<Slider>().value = ckbHPManager.HP / ckbHPManager.MaxHP;
                // HP 텍스트에 적용 = 현재 HP
                rctAllyHP.GetChild(i).GetComponentInChildren<Text>().text = ckbHPManager.HP.ToString();
                // 발사 쿨타임 슬라이더에 적용 = 위에서 계산한 fireCoolTime
                rctAllyFireCoolTime.GetChild(i).GetComponent<Slider>().value = fireCoolTime;
                // 탱크 당 현재 킬 수 텍스트 = 0 이면 표시 안 함, 1 이상이면 표시
                rctAllyScore.GetChild(i).GetComponent<Text>().text = (ckbTank.killScore == 0 ? "" : ckbTank.killScore.ToString());
            }
            // 탱크가 죽었을 때, 플레이어가 아니면
            else
            {
                rctAllyFloatingHP.GetChild(i).gameObject.SetActive(false);
                // HP 슬라이더의 값 = 0
                rctAllyHP.GetChild(i).GetComponent<Slider>().value = 0;
                rctAllyHP.GetChild(i).GetComponentInChildren<Text>().text = "0";
                // 발사 쿨타임의 값 = 0
                rctAllyFireCoolTime.GetChild(i).GetComponent<Slider>().value = 0;
            }

            // None 이 아니면
            // 탱크가 죽었을 경우에 None 으로 처리된다.
            if (enemies[i].activeSelf)
            {
                CKB_HPManager ckbHPManager = enemies[i].GetComponent<CKB_HPManager>();
                CKB_Tank ckbTank = enemies[i].GetComponent<CKB_Tank>();
                CKB_TankAI ckbTankAI = enemies[i].GetComponent<CKB_TankAI>();

                Vector3 dirCamToTarget = (enemies[i].transform.position - Camera.main.transform.position).normalized;

                if (Vector3.Dot(Camera.main.transform.forward, dirCamToTarget) <= 0)
                {
                    rctEnemyFloatingHP.GetChild(i).gameObject.SetActive(false);
                }
                else
                {
                    Vector3 localScale = Vector3.one * Mathf.Lerp(1, 0.7f, Vector3.Distance(allies[0].transform.position, enemies[i].transform.position) / 400);
                    rctEnemyFloatingHP.GetChild(i).GetComponent<RectTransform>().localScale = localScale;

                    rctEnemyFloatingHP.GetChild(i).gameObject.SetActive(true);

                    rctEnemyFloatingHP.GetChild(i).GetComponent<RectTransform>().anchoredPosition = Camera.main.WorldToScreenPoint(enemies[i].transform.position);

                    rctEnemyFloatingHP.GetChild(i).GetComponentInChildren<Slider>().GetComponent<RectTransform>().anchoredPosition
                        = Vector2.up * Mathf.Lerp(floatingHPMaxY, floatingHPMinY, Vector3.Distance(allies[0].transform.position, enemies[i].transform.position) / floatingHPLerfDistance);

                    rctEnemyFloatingHP.GetChild(i).GetComponentInChildren<Slider>().value = ckbHPManager.HP / ckbHPManager.MaxHP;

                    rctEnemyFloatingHP.GetChild(i).GetComponentInChildren<Text>().text = ckbHPManager.HP + " / " + ckbHPManager.MaxHP;
                }

                // HP 슬라이더에 적용 = 현재 HP / 총 HP
                rctEnemyHP.GetChild(i).GetComponent<Slider>().value = ckbHPManager.HP / ckbHPManager.MaxHP;
                // HP 텍스트에 적용 = 현재 HP
                rctEnemyHP.GetChild(i).GetComponentInChildren<Text>().text = ckbHPManager.HP.ToString();
                // 발사 쿨타임 슬라이더에 적용 = 위에서 계산한 fireCoolTime
                rctEnemyFireCoolTime.GetChild(i).GetComponent<Slider>().value = (ckbTankAI.FiringDurationMax - Mathf.Clamp(ckbTankAI.aiFireTime, 0, ckbTankAI.FiringDurationMax)) / ckbTankAI.FiringDurationMax;
                // 탱크 당 현재 킬 수 텍스트 = 0 이면 표시 안 함, 1 이상이면 표시
                rctEnemyScore.GetChild(i).GetComponent<Text>().text = (ckbTank.killScore == 0 ? "" : ckbTank.killScore.ToString());
            }
            // 탱크가 죽었으면
            else
            {
                rctEnemyFloatingHP.GetChild(i).gameObject.SetActive(false);
                // HP 슬라이더의 값 = 0
                rctEnemyHP.GetChild(i).GetComponent<Slider>().value = 0;
                rctEnemyHP.GetChild(i).GetComponentInChildren<Text>().text = "0";
                // 발사 쿨타임의 값 = 0
                rctEnemyFireCoolTime.GetChild(i).GetComponent<Slider>().value = 0;
            }
        }
    }

    // 중앙 상단의 팀별 총 킬 수
    // 중앙 상단의 팀별 남은 탱크 수에 해당하는 마름모 이미지
    // 갱신
    void GameInfoUpdater()
    {
        int _allyScore = 0;
        // UI 에 있는 텍스트 값을 이용해 플레이어 팀 총 킬 수를 구한다.
        // 탱크가 죽었을 경우 컴포넌트를 가져올 수 없기 때문이다.
        foreach (GameObject enemyTank in enemies)
            // 플레이어 탱크별 텍스트에 있는 킬 수를 가져온다.
            _allyScore += enemyTank.activeSelf ? 0 : 1;

        allyScore = _allyScore;

        int _enemyScore = 0;
        // 에너미 팀 총 킬수 구하기
        foreach (GameObject allyTank in allies)
            // 에너미 탱크별 텍스트에 있는 킬 수를 가져온다.
            _enemyScore += allyTank.activeSelf ? 0 : 1;

        enemyScore = _enemyScore;

        // 중앙 상단의 플레이어 팀 텍스트에 적용한다.
        textAllyScore.text = allyScore.ToString();
        // 중앙 상단의 에너미 팀 텍스트에 적용한다.
        textEnemyScore.text = enemyScore.ToString();

        // 등록된 탱크의 개수만큼 반복 (플레이어/에너미 둘 다 7)
        // 중앙 상단의 팀별 남은 탱크 수에 해당하는 마름모 이미지
        // 를 갱신하는 부분
        for (int i = 0; i < rctAllyScoreUI.childCount; ++i)
        {
            // 총 킬 수보다 큰 마름모 이미지는 켜고
            if (i < enemyScore)
                rctAllyScoreUI.GetChild(i).GetComponent<Image>().enabled = true;
            // 총 킬 수보다 작거나 같은 마름모 이미지는 끈다.
            else
                rctAllyScoreUI.GetChild(i).GetComponent<Image>().enabled = false;

            // 방향이 반대이므로 7 - 총 킬수로 계산
            // 7 - 총 킬 수보다 큰 마름모 이미지는 켜고
            if (i < 7 - allyScore)
                rctEnemyScoreUI.GetChild(i).GetComponent<Image>().enabled = true;
            // 그것보다 작거나 같은 마름모 이미지는 끈다.
            else
                rctEnemyScoreUI.GetChild(i).GetComponent<Image>().enabled = false;
        }
    }

    float t;

    // 중앙 상단의 현재 남은 시간 텍스트 갱신
    void TimerUpdater()
    {
        if (isEnd)
            return;

        // 현재 남은 시간 (초)
        float gameTimeSec = CKB_GameManager.Instance.gameTimeSec - t;

        // 1초를 미루기 위해 올림
        // 정수로 변환하여 소숫점 제거
        int gameTimeCeil = (int)Mathf.Ceil(gameTimeSec);

        // 분
        int minutes = gameTimeCeil / 60;
        // 분을 제외한 초
        int sec = gameTimeCeil - minutes * 60;

        // 텍스트 갱신
        textTimer.text = minutes.ToString() + " : " + sec.ToString().PadLeft(2, '0');

        // 총 게임 시간이 되기 전까지 시간이 흐른다.
        if (t < CKB_GameManager.Instance.gameTimeSec)
            t += Time.deltaTime;
    }

    public enum PanelType
    {
        Mission,    // 가장 위의 알림창
        InfoMGF,   // 두번째 알림창
        InfoArtyCannon,   // 세번째 알림창
        Warning        // 가장 아래 알림창
    }
    [HideInInspector]
    public PanelType panelType;

    public void ShowAlertPanel(PanelType panelType, string message, float duration)
    {
        GameObject panel = null;

        switch (panelType)
        {
            case PanelType.Mission : panel = alertPanels.GetChild(0).gameObject;
                break;
            case PanelType.InfoMGF : panel = alertPanels.GetChild(1).gameObject;
                break;
            case PanelType.InfoArtyCannon : panel = alertPanels.GetChild(2).gameObject;
                break;
            case PanelType.Warning : panel = alertPanels.GetChild(3).gameObject;
                break;
        }

        StartCoroutine(IEShowAlertPanel(panel, message, duration));
    }

    IEnumerator IEShowAlertPanel(GameObject panel, string message, float duration)
    {
        RectTransform rectPanel = panel.GetComponent<RectTransform>();
        Vector3 o_position = rectPanel.position;

        rectPanel.GetComponentInChildren<Text>().text = message;

        rectPanel.position = o_position - Vector3.right * 2000;
        panel.SetActive(true);
        rectPanel.DOMove(rectPanel.position + Vector3.right * 2000, duration / 4).SetEase(Ease.InOutSine);
        yield return new WaitForSeconds(duration);

        rectPanel.DOMove(rectPanel.position + Vector3.right * 2000, duration / 4).SetEase(Ease.InOutSine);
        yield return new WaitForSeconds(duration);
        panel.SetActive(false);
        rectPanel.position = o_position - Vector3.right * 2000; 
    }

    public void ShowGameResultUI(CKB_GameManager.State state)
    {
        string text = "";
        Color color = Color.black;

        isEnd = true;

        switch (state)
        {
            case CKB_GameManager.State.AllyWin :
                text = "승리";
                color = new Color(92, 202, 92, 255) / 255;
                break;
            case CKB_GameManager.State.EnemyWin :
                text = "패배";
                color = new Color(209, 56, 56, 255) / 255;
                break;
            case CKB_GameManager.State.Draw :
                text = "무승부";
                color = Color.black;
                break;
        }

        textGameResult.text = text;
        textGameResult.color = color;

        textGameResult.gameObject.SetActive(true);
    }
}
