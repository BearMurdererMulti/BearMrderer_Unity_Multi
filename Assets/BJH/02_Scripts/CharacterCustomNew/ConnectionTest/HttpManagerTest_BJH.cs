using System.Collections;
using System.Net.Http;
using System.Text;
using UnityEngine;
using UnityEngine.Networking;



public class HttpManagerTest_BJH : MonoBehaviour
{
    public static HttpManagerTest_BJH instance;

    public string token = "eyJhbGciOiJIUzI1NiJ9.eyJzdWIiOiJ0ZXN0Iiwicm9sZXMiOltdLCJpYXQiOjE3MjE5MDk4NTUsImV4cCI6MTcyMTkxMzQ1NX0.LF1nr6y2Qb3xlYDcQNoNz7XrGJZVbcOPIaDfXUsceBE";

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

        Debug.Log("기다리는 중");

        yield return request.SendWebRequest();

        if (request.result == UnityWebRequest.Result.Success)
        {
            Debug.Log("요청 완료");
            requester.complete(request.downloadHandler);
        }
        else
        {
            Debug.Log("요청 실패");
            Debug.Log(request.error);
        }
    }
}
