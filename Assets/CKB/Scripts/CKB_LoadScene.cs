using System.Collections;
using System.Collections.Generic;
using UnityEngine;
using UnityEngine.SceneManagement;

public class CKB_LoadScene : MonoBehaviour
{
    public void LoadScene(int sceneNo)
    {
        SceneManager.LoadScene(sceneNo);
    }
}
