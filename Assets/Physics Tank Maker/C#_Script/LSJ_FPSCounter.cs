using System.Collections;
using UnityEngine;
using UnityEngine.UI;


[RequireComponent(typeof(Text))]
public class LSJ_FPSCounter : MonoBehaviour
{
    public Text thisText;

    const string textFormat = "{0} FPS";
    int ignoredFrames;
    float previousTime;
    float deltaTime;


    void Start()
    {
        if (thisText == null)
        {
            thisText = GetComponent<Text>();
        }
    }


    void Update()
    {
        ignoredFrames++;

        deltaTime = Time.realtimeSinceStartup - previousTime;
        if (deltaTime < 0.5f)
        {
            return;
        }

        thisText.text = string.Format(textFormat, (int)(ignoredFrames / deltaTime));
        ignoredFrames = 0;
        previousTime = Time.realtimeSinceStartup;
    }

}
