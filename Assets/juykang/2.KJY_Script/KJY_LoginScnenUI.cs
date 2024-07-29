using System.Collections;
using System.Collections.Generic;
using UnityEngine;

public class KJY_LoginScnenUI : MonoBehaviour
{
    public GameObject LoginUI;
    public GameObject MainUI;

    // Start is called before the first frame update
    void Start()
    {
        if (InfoManagerKJY.instance.nickname == "")
        {
           LoginUI.SetActive(true);
           MainUI.SetActive(false);
        }
        else
        {
            LoginUI.SetActive(false);
            MainUI.SetActive(true);
        }
    }

    // Update is called once per frame
    void Update()
    {
        
    }
}
