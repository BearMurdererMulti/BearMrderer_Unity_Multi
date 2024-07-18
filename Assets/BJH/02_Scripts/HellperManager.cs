using System.Collections;
using System.Collections.Generic;
using UnityEngine;

public class HellperManager : MonoBehaviour
{
    public static HellperManager _instance;
    public bool isArrive;

    private void Awake()
    {
        if(_instance == null)
        {
            _instance = this;
        }
    }
}
