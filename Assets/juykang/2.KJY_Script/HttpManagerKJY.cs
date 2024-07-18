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

                // body데이터를 바이트로 변환
                byte[] jsonToSend = new UTF8Encoding().GetBytes(requester.body);

                request.uploadHandler.Dispose();
                request.uploadHandler = new UploadHandlerRaw(jsonToSend);

                request.SetRequestHeader("Content-Type", "application/json");
                request.SetRequestHeader("Authorization", "Bearer " + token);
                break;
        }

        print("기다리는 중");

        yield return request.SendWebRequest();

        if (request.result == UnityWebRequest.Result.Success)
        {
            print("요청 완료");
            print(request.downloadHandler.text);
            requester.complete(request.downloadHandler);
        }
        else
        {
            print("요청 실패");
            print(request.error);
            //if (FailUI != null)
            //{
            //    StartCoroutine(PopUpFailUI());
            //}
        }
    }
}
