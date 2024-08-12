using JetBrains.Annotations;
using System.Collections;
using System.Collections.Generic;
using TMPro;
using TMPro.Examples;
using UnityEngine;
using UnityEngine.UI;

public class AutoLogin : MonoBehaviour
{
    [SerializeField] private TMP_Text id, pw;
    [SerializeField] private Button loginButton;

    // Update is called once per frame
    void Update()
    {
#if UNITY_EDITOR
        // F1 키를 눌렀는지 확인
        //if (Input.GetKeyDown(KeyCode.F1))
        //{
        //    AutoFillAndLogin();
        //}
        //if(Input.GetKeyDown(KeyCode.F2))
        //{
        //    TryLogin();
        //}
#endif
    }

    private void AutoFillAndLogin()
    {
        string s = "aaaa";
        id.text = s;
        pw.text = s;

        Debug.Log(id.text + pw.text);
    }

    private void TryLogin()
    {
        loginButton.GetComponent<Button>().onClick.Invoke();

    }
}
