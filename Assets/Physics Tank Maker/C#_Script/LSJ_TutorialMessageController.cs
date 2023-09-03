using System.Collections;
using System.Collections.Generic;
using UnityEngine;


[System.Serializable]
public class Tutorial_Messages
{
    public GameObject[] targetObjects;
}


public class LSJ_TutorialMessageController : MonoBehaviour
{


    [SerializeField] Tutorial_Messages[] tutorialMessagesGroups = default;


    void Start()
    {
        for (int i = 0; i < tutorialMessagesGroups.Length; i++)
        {
            if (i == LSJ_GeneralSettings.Input_Type)
            {
                continue;
            }

            for (int j = 0; j < tutorialMessagesGroups[i].targetObjects.Length; j++)
            {
                var targetObject = tutorialMessagesGroups[i].targetObjects[j];
                if (targetObject)
                {
                    Destroy(targetObject);
                }
            }
        }

        Destroy(gameObject);
    }


}
