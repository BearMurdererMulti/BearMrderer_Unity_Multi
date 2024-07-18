using System.Collections;
using System.Collections.Generic;
using System.Text;
using TMPro;
using UnityEngine;
using UnityEngine.Networking;

public class KJY_TestHttpManager : MonoBehaviour
{
    public static KJY_TestHttpManager instance;

    public string token;
    [SerializeField] GameObject FailUI;
    [SerializeField] GameObject RegisterUI;
    [SerializeField] TMP_Text checkText;


    private void Awake()
    {
        instance = this;
    }


    // Request
    public void SendRequest(HttpRequester requester)
    {
        StartCoroutine(SendProcess(requester));
    }

    IEnumerator SendProcess(HttpRequester requester)
    {
        UnityWebRequest request = null;

        if (InfoManagerKJY.instance.userToken != null)
        {
            token = InfoManagerKJY.instance.userToken;
        }

        switch (requester.requestType)
        {
            case RequestType.GET:
                request = UnityWebRequest.Get(requester.url);
                request.SetRequestHeader("Content-Type", "application/json");

                break;
            case RequestType.POST:
                request = UnityWebRequest.PostWwwForm(requester.url, requester.body);

                // body데이터를 바이트로 변환
                byte[] jsonToSend = new UTF8Encoding().GetBytes(requester.body);

                request.uploadHandler.Dispose();
                request.uploadHandler = new UploadHandlerRaw(jsonToSend);

                request.SetRequestHeader("Content-Type", "application/json");
                break;
        }

        yield return request.SendWebRequest();

        if (request.result == UnityWebRequest.Result.Success)
        {
            requester.Complete(request.downloadHandler);
        }
        else
        {
            requester.Complete(request.downloadHandler);
        }
    }

    public void GetCheckMessage(string message)
    {
        checkText.text = message;
    }

    private IEnumerator CheckCoroutine()
    {
        RegisterUI.SetActive(true);
        yield return new WaitForSeconds(2f);
        RegisterUI.SetActive(false);
    }


    IEnumerator Fail()
    {
        FailUI.SetActive(true);
        yield return new WaitForSeconds(2);
        FailUI.SetActive(false);
    }
}
