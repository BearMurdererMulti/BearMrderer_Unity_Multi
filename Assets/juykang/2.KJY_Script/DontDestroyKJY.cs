using System.Collections;
using System.Collections.Generic;
using UnityEngine;

public class DontDestroyKJY : MonoBehaviour
{
    public static DontDestroyKJY instance;
    void Awake()
    {
        if (instance != null)
        {
            Destroy(gameObject);
            return;
        }
        instance = this;
        DontDestroyOnLoad(gameObject);

    }
}
