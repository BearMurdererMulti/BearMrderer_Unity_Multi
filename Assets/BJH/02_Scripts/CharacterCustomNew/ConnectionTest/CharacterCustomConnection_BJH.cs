using System;
using System.Collections;
using System.Collections.Generic;
using Unity.VisualScripting;
using UnityEngine;
using UnityEngine.Networking;

[Serializable]
public class UserCustomSaveRequest
{
    public int eyes;
    public int mouth;
    public int body;
    public int tail;
    public long gameSetNo;
}

[Serializable]
public class UserCustomSaveResponse
{
    public string resultCode;
    public UserCustomSaveResponseMessage message;
}

[Serializable]
public class UserCustomSaveResponseMessage
{
    public DateTime saveTime; // 저장된 날짜
    public bool saved; // 저장 여부
}

public class CharacterCustomConnection_BJH : MonoBehaviour
{
    Dictionary<string, int> dic = new Dictionary<string, int>();
    public CharacterCustomConnection_BJH(Dictionary<string, int> dic)
    {
        this.dic = dic;

        CreateJson();
    }

    private void CreateJson()
    {
        UserCustomSaveRequest request = new UserCustomSaveRequest();

        request.eyes = dic["eyes"];
        request.mouth = dic["mouth"];
        request.body = dic["body"];
        request.tail = dic["tail"];
        request.gameSetNo = InfoManagerKJY.instance.gameSetNo;

        string createdJson = JsonUtility.ToJson(request);

        OnGetRequest(createdJson);
    }

    private void OnGetRequest(string createdJson)
    {
        HttpRequester requester = new HttpRequester();
        requester.requestType = RequestType.POST;
        requester.url = "http://ec2-15-165-15-244.ap-northeast-2.compute.amazonaws.com:8081/api/v1/custom/save";
        requester.body = createdJson;
        requester.complete = OnGetComplete;
        requester.failed = OnGetFailed;

        HttpManagerKJY.instance.SendRequest(requester);
    }

    private void OnGetComplete(DownloadHandler handler)
    {
        Debug.Log("성공");

        UserCustomSaveResponse response = new UserCustomSaveResponse();
        response = JsonUtility.FromJson<UserCustomSaveResponse>(handler.text);

        Debug.Log(response.resultCode);
        Debug.Log(response.message.saveTime);
        Debug.Log(response.message.saved);

    }

    private void OnGetFailed(DownloadHandler handler)
    {
        Debug.Log("실패");
    }
}
