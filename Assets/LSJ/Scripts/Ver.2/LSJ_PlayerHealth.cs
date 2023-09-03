using System.Collections;
using System.Collections.Generic;
using UnityEngine;

public class LSJ_PlayerHealth : MonoBehaviour
{
    int hp = 500;

    public int HP
    {
        get
        {
            return hp;
        }
        set
        {
            hp = value;
            if(hp <= 0)
            {
                Destroy(gameObject);
            }
        }
    }

    public static LSJ_PlayerHealth Instance = null;

    private void Awake()
    {
        if(Instance == null)
        {
            Instance = this;
        }
    }

    void Update()
    {
        Debug.Log("hp = 500");    
    }
}
