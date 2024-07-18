using System.Collections;
using System.Collections.Generic;
using System.Text;
using Unity.VisualScripting;
using UnityEngine;
using UnityEngine.Networking;

public enum KJYRequstType
{
    GET,
    POSTSIGN,
    PUT,
    DELETE,
    POST
}

public class HttpManagerKJY : MonoBehaviour
{
    public static HttpManagerKJY instance;

    public string token;

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
       if (InfoManagerKJY.instance.userToken != null)
       {
           token = InfoManagerKJY.instance.userToken;
       }

        UnityWebRequest request = null;

        switch (requester.requestType)
        {
            case RequestType.GET:
                request = UnityWebRequest.Get(requester.url);
                request.SetRequestHeader("Content-Type", "application/json");
                request.SetRequestHeader("Authorization", "Bearer " + token);
                break;
            case RequestType.POST:
                request = UnityWebRequest.PostWwwForm(requester.url, requester.body);

                // body�����͸� ����Ʈ�� ��ȯ
                byte[] jsonToSend = new UTF8Encoding().GetBytes(requester.body);

                request.uploadHandler.Dispose();
                request.uploadHandler = new UploadHandlerRaw(jsonToSend);

                request.SetRequestHeader("Content-Type", "application/json");
                request.SetRequestHeader("Authorization", "Bearer " + token);
                break;
        }

        print("��ٸ��� ��");

        yield return request.SendWebRequest();

        if (request.result == UnityWebRequest.Result.Success)
        {
            print("��û �Ϸ�");
            print(request.downloadHandler.text);
            requester.complete(request.downloadHandler);
        }
        else
        {
            print("��û ����");
            print(request.error);
            //if (FailUI != null)
            //{
            //    StartCoroutine(PopUpFailUI());
            //}
        }
    }
}
