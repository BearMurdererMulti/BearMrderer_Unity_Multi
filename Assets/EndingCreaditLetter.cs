using Photon.Pun;
using System.Collections;
using System.Collections.Generic;
using UnityEngine;
using UnityEngine.Networking;

public class EndingCreaditLetter : MonoBehaviourPunCallbacks
{

    // �ΰ��� ������ �����ؼ� ���
    private static EndingCreaditLetter _instance;

    public static EndingCreaditLetter Instance
    {
        get { return _instance; }
    }

    private void Awake()
    {
        if (_instance == null)
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
        foreach (SurvivorsLetters survivorLetter in survivorsLetters)
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
        Debug.Log("����");
    }

    [PunRPC]
    public void UpdateEndingLetterInfo(ChiefLetter chiefLetter, MurdererLetter murdererLetter, List<SurvivorsLettersLetter> finalLetters)
    {
        // ���� �� �ٸ� �÷��̾��� letter info�� ������Ʈ �ϴ� �޼���
        if (!PhotonNetwork.IsMasterClient)
        {
            InfoManagerKJY.instance.chiefLetter = chiefLetter;
            InfoManagerKJY.instance.murdererLetter = murdererLetter;
            InfoManagerKJY.instance.finalLetters = finalLetters;
        }
    }

}
