using System.Collections;
using System.Collections.Generic;
using UnityEngine;
using UnityEngine.UI;
using UnityEngine.SceneManagement;

public class CKB_GameManager : MonoBehaviour
{
    public static CKB_GameManager Instance;

    void Awake()
    {
        if (Instance == null)
            Instance = this;
    }

    public enum State
    {
        Idle = 1,
        InGame = 2,
        AllyWin = 4,
        EnemyWin = 8,
        Draw = 16,
    }
    public State state;

    public float gameTimeSec;
    public Transform MGField;
    public float[] fieldAlertTimeSec;
    public float[] fieldAppearTimeSec;
    public Transform[] fieldSizes;
    public float fieldShrinkTimeSec;
    [HideInInspector]
    public int currentAlert;
    [HideInInspector]
    public int currentField;

    public Image fadeOutImage;

    float remain_t;
    bool isEnd = false;

    IEnumerator Start()
    {
        currentAlert = currentField = 0;

        remain_t = gameTimeSec;

        MGField.gameObject.SetActive(false);

        float duration = 4;

        CKB_UIManager.Instance.ShowAlertPanel(CKB_UIManager.PanelType.Mission,
                                            "임무 : 제한 시간 내 적군 기지를 점령하거나 모든 적 전차를 격파하십시오", duration);

        yield return new WaitForSeconds(duration + 0.5f);

        CKB_UIManager.Instance.ShowAlertPanel(CKB_UIManager.PanelType.InfoMGF,
            @"주의 : 일정 시간 내 자기장이 생성됩니다
          자기장 영역에서 벗어나면 일정량의 체력이 감소합니다
          자기장 내부로 이동하십시오", duration);

        yield return new WaitForSeconds(duration + 0.5f);

        CKB_UIManager.Instance.ShowAlertPanel(CKB_UIManager.PanelType.InfoArtyCannon,
            @"주의 : 일정 시간을 넘어 이동하지 않으면 적 자주포의 타겟이 됩니다
          위치를 노출시키지 않도록 주의하세요", duration);
    }

    void Update()
    {
        TimeScaler();

        UpdateAlert();
        UpdateMGFieldSize();
        CheckEnd();

        remain_t = Mathf.Clamp(remain_t - Time.deltaTime, 0, gameTimeSec);
    }

    void TimeScaler()
    {
        float timeScale = Time.timeScale;

        if (Input.GetKeyDown(KeyCode.Alpha1))
            timeScale = 1;
        else if (Input.GetKeyDown(KeyCode.Alpha2))
            timeScale = 2;
        else if (Input.GetKeyDown(KeyCode.Alpha3))
            timeScale = 3;
        else if (Input.GetKeyDown(KeyCode.Alpha4))
            timeScale = 4;
        else if (Input.GetKeyDown(KeyCode.Alpha5))
            timeScale = 12;

        Time.timeScale = timeScale;
    }

    void UpdateAlert()
    {
        if (currentAlert < fieldSizes.Length - 1 && remain_t <= fieldAlertTimeSec[currentAlert])
            ShowFieldAlert(++currentAlert);
    }

    void ShowFieldAlert(int fieldNum)
    {
        string message;
        float duration = 4;

        if (fieldNum == 1)
            message = "경고 : 자기장이 곧 생성됩니다";
        else
            message = "경고 : 자기장이 곧 축소됩니다";

        CKB_UIManager.Instance.ShowAlertPanel(CKB_UIManager.PanelType.Warning, message, duration);
    }

    void UpdateMGFieldSize()
    {
        MGField.Rotate(0, 0, -0.05f);

        if (currentField < fieldSizes.Length - 1 && remain_t <= fieldAppearTimeSec[currentField])
            StartCoroutine(IEMGFieldSize(++currentField));
    }

    IEnumerator IEMGFieldSize(int destFieldNum)
    {
        float t = 0;

        Vector3 o_localScale = MGField.localScale;

        while (t < fieldShrinkTimeSec)
        {
            MGField.localScale = Vector3.Lerp(o_localScale, fieldSizes[destFieldNum].localScale, t / fieldShrinkTimeSec);

            t += Time.deltaTime;

            yield return null;
        }

        MGField.localScale = fieldSizes[destFieldNum].localScale;

        MGField.gameObject.SetActive(true);
    }

    void CheckEnd()
    {
        if (isEnd)
            return;

        if (remain_t <= 0 || CKB_UIManager.Instance.allyScore == 7 || CKB_UIManager.Instance.enemyScore == 7)
            GameEnd();
    }

    public void GameEnd()
    {
        int allyScore = CKB_UIManager.Instance.allyScore;
        int enemyScore = CKB_UIManager.Instance.enemyScore;

        state = (allyScore == enemyScore) ? State.Draw : (allyScore > enemyScore) ? State.AllyWin : State.EnemyWin;

        CKB_UIManager.Instance.ShowGameResultUI(state);

        Time.timeScale = 1;

        isEnd = true;

        StartCoroutine(EndFadeOut(3));
    }

    IEnumerator EndFadeOut(float duration)
    {
        float t = 0;
        Color color = fadeOutImage.color;

        yield return new WaitForSeconds(3.0f);

        while (t < duration)
        {
            color.a = t / duration;
            fadeOutImage.color = color;

            t += Time.deltaTime;

            yield return null;
        }

        color.a = 1;
        fadeOutImage.color = color;

        SceneManager.LoadScene(4);
    }
}
