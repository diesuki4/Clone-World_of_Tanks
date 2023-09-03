using System.Collections;
using System.Collections.Generic;
using UnityEngine;
using UnityEngine.UI;
using UnityEngine.SceneManagement;

public class CKB_AsyncScene : MonoBehaviour
{
    public float[] timeCheckPoints;
    public float totalDuration;
    public int nextSceneNo;

    Slider slider;
    float currentTime;
    int currentPoint;

    IEnumerator Start()
    {
        slider = GetComponent<Slider>();

        slider.value = 0;

        currentPoint = 0;

        while (currentTime < totalDuration)
        {
            currentTime += Time.deltaTime;

            if (timeCheckPoints[currentPoint] <= currentTime)
                slider.value = timeCheckPoints[currentPoint++] / totalDuration;

            yield return null;
        }

        yield return new WaitForSeconds(0.7f);

        SceneManager.LoadScene(nextSceneNo);
    }
}
