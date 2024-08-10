using Photon.Pun;
using System;
using System.Collections;
using System.Collections.Generic;
using UnityEngine;
using UnityEngine.Networking;

#region Request and Responze

[Serializable]
public class EndingLetterRequest
{
    public long gameSetNo;
}

[Serializable]
public class EndingLetterResponse
{
    public string resultCode;
    public EndingLetterMessage message;
}

[Serializable]
public class EndingLetterMessage
{
    public string result;
    public ChiefLetter chiefLetter;
    public MurdererLetter murdererLetter;
    public List<SurvivorsLetters> survivorsLetters;
}

[Serializable]
public class ChiefLetter
{
    public string receiver;
    public string content;
    public string sender;
}

[Serializable]
public class MurdererLetter
{
    public string receiver;
    public string content;
    public string sender;
}

[Serializable]
public class SurvivorsLetters
{
    public string name;
    public SurvivorsLettersLetter letter;
}

[Serializable]
public class SurvivorsLettersLetter
{
    public string receiver;
    public string content;
    public string sender;
}

#endregion

public class EndingLetterConnection : MonoBehaviourPunCallbacks
{
    // 인게임 씬부터 부착해서 사용
    private static EndingLetterConnection _instance;

    public static EndingLetterConnection Instance
    {
        get { return _instance; }
    }

    private void Awake()
    {
        if( _instance == null )
        {
            _instance = this;
            DontDestroyOnLoad(gameObject);
        }
        else
        {
            Destroy(gameObject);
        }
    }


    public void LetterConnection(long gameSetNo)
    {
        EndingLetterRequest request = new EndingLetterRequest();
        request.gameSetNo = gameSetNo;
        string createdJson = JsonUtility.ToJson(request);

        HttpRequester requester = new HttpRequester();
        requester.requestType = RequestType.POST;
        requester.url = "http://ec2-15-165-15-244.ap-northeast-2.compute.amazonaws.com:8081/api/v1/game/ending-letter";
        requester.body = createdJson;
        requester.complete = OnGetComplete;
        requester.failed = OnGetFailed;

        HttpManagerKJY.instance.SendRequest(requester);
    }
    public void OnGetComplete(DownloadHandler handler)
    {
        EndingLetterResponse response = new EndingLetterResponse();
        response = JsonUtility.FromJson<EndingLetterResponse>(handler.text);

        InfoManagerKJY.instance.finalLetterResultCode = response.resultCode;

        ChiefLetter chiefLetter = new ChiefLetter();
        chiefLetter.sender = response.message.chiefLetter.sender;
        chiefLetter.content = response.message.chiefLetter.content;
        chiefLetter.receiver = response.message.chiefLetter.receiver;
        InfoManagerKJY.instance.chiefLetter = chiefLetter;

        MurdererLetter murdererLetter = new MurdererLetter();
        murdererLetter.sender = response.message.murdererLetter.sender;
        murdererLetter.content = response.message.murdererLetter.content;
        murdererLetter.receiver = response.message.murdererLetter.receiver;
        InfoManagerKJY.instance.murdererLetter = murdererLetter;

        List<SurvivorsLetters> survivorsLetters = response.message.survivorsLetters;
        foreach(SurvivorsLetters survivorLetter in survivorsLetters)
        {
            SurvivorsLettersLetter letter = new SurvivorsLettersLetter();
            letter.sender = survivorLetter.letter.sender;
            letter.content = survivorLetter.letter.content;
            letter.receiver = survivorLetter.letter.receiver;

            InfoManagerKJY.instance.finalLetters.Add(letter);
        }
        var sendLetters = InfoManagerKJY.instance.finalLetters;
        PhotonView pv = gameObject.GetComponent<PhotonView>();
        pv.RPC("UpdateEndingLetterInfo", RpcTarget.All, chiefLetter, murdererLetter, sendLetters);

    }

    public void OnGetFailed(DownloadHandler handler)
    {
        Debug.Log("실패");
    }

    [PunRPC]
    public void UpdateEndingLetterInfo(ChiefLetter chiefLetter, MurdererLetter murdererLetter, List<SurvivorsLettersLetter> finalLetters)
    {
        // 방장 외 다른 플레이어의 letter info를 업데이트 하는 메서드
        if(!PhotonNetwork.IsMasterClient)
        {
            InfoManagerKJY.instance.chiefLetter = chiefLetter;
            InfoManagerKJY.instance.murdererLetter = murdererLetter;
            InfoManagerKJY.instance.finalLetters = finalLetters;
        }
    }

}
