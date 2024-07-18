using System.Collections;
using System.Collections.Generic;
using TMPro;
using UnityEngine;

public class LoginSceneUI : MonoBehaviour
{
    public static LoginSceneUI instance;

    [Header("Secret key")]
    public GameObject secretKeyG;
    public TMP_InputField secretKeyInput;
    public TMP_Text secretKeyText;
    public GameObject secretkeyFail;

    private void Awake()
    {
        if(secretKeyG != null)
        {
            instance = this;
        }
    }

    public void OnClickSecretKeyConnection()
    {
        string url = "http://ec2-43-201-108-241.ap-northeast-2.compute.amazonaws.com:8081/api/v1/game/key";
        string secretKey = secretKeyText.text;
        SecretKeyConnect secretKeyConnect = new SecretKeyConnect(url, secretKey);
    }

    public void OnsecretKeyG()
    {
        secretKeyG.SetActive(true);
    }

    public void OnClickClose(GameObject go)
    {
        go.SetActive(false);
    }
}
