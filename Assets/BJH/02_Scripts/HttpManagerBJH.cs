using System.Collections;
using System.Net.Http;
using System.Text;
using UnityEngine;
using UnityEngine.Networking;

public enum RequestType
{
    GET,
    POST,
    PUT,
    DELETE
}

public class HttpManagerBJH : MonoBehaviour
{
    public static HttpManagerBJH instance;

    public string token = null;

    private void Awake()
    {
        if(instance == null)
        {
            instance = this;
            return;
        }
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

        if (InfoManagerKJY.instance.userToken != null)
        {
            token = InfoManagerKJY.instance.userToken;
        }

        UnityWebRequest request = null;

        switch (requester.requestType)
        {
            case RequestType.GET:
                token = InfoManagerKJY.instance.userToken;

                request = UnityWebRequest.Get(requester.url);
                request.SetRequestHeader("Content-Type", "application/json");
                request.SetRequestHeader("Authorization", "Bearer " + token);

                //if (!InfoManagerBJH.instance.isLogin)
                //{
                //    token = InfoManagerKJY.instance.userToken;
                //    request.SetRequestHeader("Authorization", "Bearer " + token);
                //    Debug.Log("토큰을 담아서 보냅니다.");
                //    Debug.Log(token);
                //}

                break;

            case RequestType.POST:
                request = UnityWebRequest.PostWwwForm(requester.url, requester.body);

                // body데이터를 바이트로 변환
                byte[] jsonToSend = new UTF8Encoding().GetBytes(requester.body);

                request.uploadHandler.Dispose();
                request.uploadHandler = new UploadHandlerRaw(jsonToSend);

                request.SetRequestHeader("Content-Type", "application/json");
                request.SetRequestHeader("Authorization", "Bearer " + token);
                //if(!InfoManagerBJH.instance.isLogin)
                //{
                //    token = InfoManagerKJY.instance.userToken;
                //    request.SetRequestHeader("Authorization", "Bearer " + token);
                //    Debug.Log("토큰을 담아서 보냅니다.");
                //    Debug.Log(token);
                //}
                break;
        }

        print("기다리는 중");

        yield return request.SendWebRequest();

        if(request.result == UnityWebRequest.Result.Success)
        {
            print("요청 완료");
            print(request.downloadHandler.text);
            requester.Complete(request.downloadHandler);
        }
        else
        {
            print("요청 실패");
            print(request.downloadHandler.text);
            print(request.error);
        }
    }


}
