using JetBrains.Annotations;
using System;
using System.Collections;
using System.Collections.Generic;
using System.Threading;
using TMPro;
using UnityEngine;
using UnityEngine.Networking;
using Newtonsoft.Json;
using static System.Net.WebRequestMethods;
using Newtonsoft.Json.Linq;

[System.Serializable]
public class ChatRequest
{
    public long gameSetNo;
    public string secretKey;
    public string receiver;
    public string chatContent;
    public int chatDay;
}

[System.Serializable]
public class ChatResponse
{
    public string resultCode;
    public ChatResponseMessage message;
}

[System.Serializable]
public class ChatResponseMessage
{
    public string sender;
    public string chatContent;
}


// chat list

[System.Serializable]
public class ChatListRequest
{
    public string nickName;
    public string aiNpcName;
    public long gameSetNo;
}

[System.Serializable]
public class ChatListResponse
{
    public string resultCode;
    public List<ChatListContent> message;
}

[System.Serializable]
public class ChatListContent
{
    public string sender;
    public string receiver;
    public string chatContent;
    public int chatDay;
}


public struct StrTryChat
{
    public string url;
    public long gameSetNo;
    public string secertKey;
    public string chatContent, receiver;
    public int chatDay;
    public string sender;
}

public interface ConnectionStratege
{
    public void CreateJson();
    public void OnGetRequest(string jsonData);
    public void Complete(DownloadHandler result);
}

public class ChatConnection : MonoBehaviour, ConnectionStratege
{
    string url;
    long gameSetNo;
    string secretKey;
    string chatContent, receiver;
    int chatDay;

    public ChatConnection(StrTryChat str)
    {
        this.url = str.url;
        this.gameSetNo = str.gameSetNo;
        this.secretKey = str.secertKey;
        this.chatContent = str.chatContent;
        this.receiver = str.receiver;
        this.chatDay = str.chatDay;
        CreateJson();
    }
    public void CreateJson()
    {
        ChatRequest request = new ChatRequest();
        request.gameSetNo = this.gameSetNo;
        request.secretKey = this.secretKey;
        request.chatContent = this.chatContent;
        request.receiver = this.receiver;
        request.chatDay = this.chatDay;

        string jsonData = JsonUtility.ToJson(request);
        OnGetRequest(jsonData);
    }

    public void OnGetRequest(string jsonData)
    {
        HttpRequester requester = new HttpRequester();

        requester.Settting(RequestType.POST, this.url);
        requester.body = jsonData;
        requester.complete = Complete;

        HttpManagerBJH.instance.SendRequest(requester);
    }

    public void Complete(DownloadHandler result)
    {
        ChatResponse response = new ChatResponse();
        response = JsonUtility.FromJson<ChatResponse>(result.text); 
        if (response.resultCode == "SUCCESS")
        {
            ChatManager.instance.talkingName.text = ChatManager.instance.npcdata.npcName;
            ChatManager.instance.dialog.text = response.message.chatContent;
            //ChatManager.instance.EmitChat();

            ChatManager.instance.content = response.message.chatContent;
            ChatManager.instance.isConnection = true;
            ChatManager.instance.npctalk = true;
        }
    }
}

// connection chat list 

public class ShowChatList : ConnectionStratege
{
    string url, userName, aiNpcName;
    long gameSetNo;

    public ShowChatList(string url, string userName, string aiNpcName, long gameSetNo)
    {
        this.url = $"{url}?nickName=\"{InfoManagerKJY.instance.nickname}\"&aiNpcName=\"{aiNpcName}\"&gameSetNo={gameSetNo}";
        Debug.Log("chat list 요청하는 url : " + this.url);
        this.userName = userName;
        this.aiNpcName = aiNpcName;
        this.gameSetNo = gameSetNo;

        CreateJson();
    }

    public void CreateJson()
    {
        ChatListRequest request = new ChatListRequest();
        request.nickName = userName;
        request.aiNpcName = aiNpcName;
        request.gameSetNo = gameSetNo;
        Debug.Log($"chat list를 호출 할 game set no : {gameSetNo}");

        string json = JsonUtility.ToJson(request);

        OnGetRequest(json);
    }

    public void OnGetRequest(string jsonData)
    {
        HttpRequester requester = new HttpRequester();

        requester.Settting(RequestType.GET, this.url);
        requester.body = jsonData;
        requester.complete = Complete;

        HttpManagerBJH.instance.SendRequest(requester);
    }

    public string result;
    public ChatListResponse response;

    public void Complete(DownloadHandler result)
    {
        ChatListResponse response = new ChatListResponse();
        response = JsonUtility.FromJson<ChatListResponse>(result.text);

        if (response.resultCode == "SUCCESS")
        {
            Debug.Log("채팅리스트 통신 요청을 완료했습니다.");
            CheckListInteraction.instance.chatListContent = response.message;
            this.result = result.text;
            this.response = response;

            CheckListInteraction.instance.isInstantiate = true;
            CheckListInteraction.instance.isUpdate = true; // true가 되면 chet list를 받아 instantiate해줌
        }
    }
}















[System.Serializable]
public class CheckListRequest
{
    public long gameSetNo;
    public List<CheckListInfo> checkList;
}

[System.Serializable]
public class CheckListInfo
{
    public string npcName;
    public string mark;
    public string checkJob;
}

[System.Serializable]
public class CheckListReponse
{
    public string mark;
    public string checkJob;
}

public class CheckListConnection : ConnectionStratege
{
    public string url;
    public long gameSetNo; // slot number
    public List<CheckListInfo> checkList;

    public CheckListConnection(string url, long gameSetNo, List<CheckListInfo> list) 
    {
        this.url = url;
        this.gameSetNo = gameSetNo;
        this.checkList = list;

        CreateJson();
    }

    public void CreateJson()
    {
        CheckListRequest request = new CheckListRequest();
        request.gameSetNo = this.gameSetNo;
        request.checkList = this.checkList;
        string json = JsonUtility.ToJson(request);

        OnGetRequest(json);
    }

    public void OnGetRequest(string jsonData)
    {
        HttpRequester requester = new HttpRequester();

        requester.Settting(RequestType.POST, this.url);
        requester.body = jsonData;
        requester.complete = Complete;

        HttpManagerBJH.instance.SendRequest(requester);

    }
    public void Complete(DownloadHandler result)
    {
        CheckListReponse response = new CheckListReponse();
        response = JsonUtility.FromJson<CheckListReponse>(result.text);
        
        Console.WriteLine(response.mark + "    " + response.checkJob);

    }
}



[System.Serializable]
public class IntroRequest
{
    public long gameSetNo;
    public string secretKey;
}

[System.Serializable]
public class IntroResponse
{
    public string resultCode;
    public IntroMessage message;
}

[System.Serializable]
public class IntroMessage
{
    public string greeting;
    public string content;
    public string closing;
}

public class IntroConnection : MonoBehaviour, ConnectionStratege
{
    long gameSetNo;
    string secretKey;
    string url;

    public IntroConnection(long gameSetNo, string url)
    {
        this.gameSetNo = gameSetNo;
        this.secretKey = "mafia";
        this.url = url;

        CreateJson();
    }
    public void CreateJson()
    {
        IntroRequest request = new IntroRequest();
        request.gameSetNo = gameSetNo;
        request.secretKey = secretKey;

        string json = JsonUtility.ToJson(request);
        OnGetRequest(json);
    }

    public void OnGetRequest(string jsonData)
    {
        HttpRequester requester = new HttpRequester();

        requester.Settting(RequestType.POST, this.url);
        requester.body = jsonData;
        requester.complete = Complete;

        HttpManagerBJH.instance.SendRequest(requester);
    }

    public void Complete(DownloadHandler result)
    {
        IntroResponse response = new IntroResponse();
        response = JsonUtility.FromJson<IntroResponse>(result.text);

        if (response.resultCode == "SUCCESS")
        {

            InfoManagerKJY.instance.greeting = response.message.greeting;
            InfoManagerKJY.instance.content = response.message.content;
            InfoManagerKJY.instance.closing = response.message.closing;

            UnityEngine.SceneManagement.SceneManager.LoadScene(1);
        }
    }

}



#region Secret key
public class SecretKeyRequest
{
    public string secretKey;
}

public class SecretKeyConnect
{
    public SecretKeyConnect(string url, string secretKey)
    {
        SecretKeyRequest request = new SecretKeyRequest();
        request.secretKey = secretKey;
        InfoManagerKJY.instance.secretKey = secretKey;


        string json = JsonUtility.ToJson(request);

        HttpRequester requester = new HttpRequester();
        requester.Settting(RequestType.POST, url);
        requester.body = json;
        Debug.Log(requester.body.ToString());   
        requester.complete = Complete;

        HttpManagerBJH.instance.SendRequest(requester);
    }

    private void Complete(DownloadHandler result)
    {
        JObject jObject = JObject.Parse(result.text);
        
        // true라면?
        if (jObject["message"]["valid"].ToString() == "True" || (jObject["message"]["valid"].ToString() == "true"))
        {
            Debug.Log("시크릿키 인증이 완료되었습니다.");
            LoginSceneUI.instance.secretKeyG.SetActive(false);

            //KJYKJYKJY
            //KJY_SceneManager.instance.ChangeScene(1);
        }
        else
        {
            Debug.Log("시크릿키 인증이 실패했습니다.");
            Debug.Log(jObject["message"]["valid"].ToString());

            LoginSceneUI.instance.secretkeyFail.SetActive(true);
            LoginSceneUI.instance.secretKeyInput.text = "";
        }
    }
}
#endregion







public class Connection_BJH : MonoBehaviour
{
    public static Connection_BJH instance;

    private void Awake()
    {
        if(instance == null)
        {
            instance = this;
            DontDestroyOnLoad(gameObject);
            return;
        }
        Destroy(gameObject);
    }

    // 시작 하자마자 인트로 텍스트 받아오기
    private void Start()
    {

    }
}







//public class ChatHttpTestBJH : MonoBehaviour
//{
//    public TMP_Text npcName;
//    public TMP_Text content;
//    public void TryChat(string text)
//    {
//        string url = "https://efad-115-136-106-231.ngrok-free.app/api/v1/chat/send";
//        CreateJson(url, text);
//    }









//    public void CreateJson(string url, string text)
//    {
//        string u = url;
//        ChatRequest chatRequest = new ChatRequest();
//        chatRequest.sender = "지환이";
//        chatRequest.chatContent = text;
//        chatRequest.receiver = "김쿵야";
//        chatRequest.chatDay = 1;

//        string createdJsonData = JsonUtility.ToJson(chatRequest);

//        OnGetRequest(RequestType.POST, u, createdJsonData);
//    }

//    public void CreateJson(string url, string userName, string aiNpcName)
//    {
//        string u = url;

//        ChatListRequest request = new ChatListRequest();
//        request.userName = userName;
//        request.aiNpcName = aiNpcName;

//        string createdJsonData = JsonUtility.ToJson(request);
//        print(createdJsonData);

//        OnGetRequest(RequestType.POST, u, createdJsonData);
//    }





//    public void OnGetRequest(RequestType type, string url, string s)
//    {
//        HttpRequester requester = new HttpRequester();

//        requester.Settting(type, url);
//        requester.body = s;
//        requester.complete = Complete;

//        HttpManagerBJH.instance.SendRequest(requester);
//    }

//    public void Complete(DownloadHandler result)
//    {
//        ChatResponse chatResponse = new ChatResponse();
//        chatResponse = JsonUtility.FromJson<ChatResponse>(result.text);
//        print(chatResponse.sender + "    " + chatResponse.chatContent);

//        if (chatResponse.chatContent != null)
//        {
//            print("complete함수 if문 실행");
//            npcName.text = chatResponse.sender;
//            StartCoroutine(Typing(chatResponse.chatContent));
//            //text.text = chatResponse.chatContent;
//        }
//    }

//    IEnumerator Typing(string t)
//    {
//        yield return new WaitForSeconds(0.1f);

//        for (int i = 0; i < t.Length; i++)
//        {
//            content.text = t.Substring(0, i);
//            yield return new WaitForSeconds(0.01f);
//        }
//    }

//}