using Photon.Pun;
using System;
using System.Collections;
using System.Collections.Generic;
using System.Linq;
using TMPro;
using UnityEngine;
using UnityEngine.Networking;
using UnityEngine.UI;
using static ConnectionKJY;
using static KJY_SenarioConnection;

#region Save


[System.Serializable]
public class SaveRequest
{
    public long gameSetNo;
    public int gameDay;
    public string voteNpcName;
    public string voteResult;
    public int voteNightNumber;
    public List<CheckLists> checkList;
    public List<NpcCustomInfos> npcCustomInfos;
}

[System.Serializable]
public class SaveSetting
{
    public long gameSetNo;
    public int gameDay;
    public string voteNpcName;
    public string voteResult;
    public int voteNightNumber;
    public List<CheckLists> checkList;
    public string url;

    // 변지환이 추가함
    //public Custom userCustom;
    public List<NpcCustomInfos> npcCustomInfos;
}

//public class Custom
//{
//    public string head;
//    public string eye;
//    public string mouth;
//    public string ear;
//    public string body;
//    public string tail;
//}

[System.Serializable]
public class CheckLists
{
    public long userChecklistNo;
    public string npcName;
    public string mark;
    public string checkJob;
}

[System.Serializable]
public class NpcCustomInfos
{
    public string npcName;
    public int mouth;
    public int ear;
    public int body;
    public int tail;
}

[System.Serializable]
public class SaveResponse
{
    public string resultCode;
    public SaveMessage message;
}

[System.Serializable]
public class SaveMessage
{
    DateTime saveTime;
    int saveDay;
    bool saved;
}

public class TrySaveSetting : ConnectionStratege
{
    public long gameSetNo;
    public int gameDay;
    public string voteNpcName;
    public string voteResult;
    public int voteNightNumber;
    public List<CheckLists> checkList;
    public List<NpcCustomInfos> npcCustomInfos;
    public string url;


    public TrySaveSetting(SaveSetting str)
    {
        this.url = str.url;
        this.gameSetNo = str.gameSetNo;
        this.gameDay = str.gameDay;
        this.voteNpcName = str.voteNpcName;
        this.voteResult = str.voteResult;
        this.voteNightNumber = str.voteNightNumber;
        this.checkList = str.checkList;
        this.npcCustomInfos = str.npcCustomInfos;

        CreateJson();
    }

    public void CreateJson()
    {
        SaveRequest request = new SaveRequest();
        request.gameSetNo = this.gameSetNo;
        request.gameDay = this.gameDay;
        request.voteNpcName = this.voteNpcName;
        request.voteResult = this.voteResult;
        request.voteNightNumber = this.voteNightNumber;
        request.checkList = this.checkList;
        request.npcCustomInfos = this.npcCustomInfos;

        string jsonData = JsonUtility.ToJson(request);
        OnGetRequest(jsonData);
    }
    public void OnGetRequest(string jsonData)
    {
        HttpRequester request = new HttpRequester();

        request.Settting(RequestType.POST, this.url);
        request.body = jsonData;
        request.complete = Complete;

        //KJY_TestHttpManager.instance.SendRequest(request);
        HttpManagerKJY.instance.SendRequest(request);
    }

    public void Complete(DownloadHandler result)
    {
        SaveResponse reponse = new SaveResponse();
        reponse = JsonUtility.FromJson<SaveResponse>(result.text);
        Debug.Log("success");
    }
}

public class RequestSave
{
    public void Request()
    {
        SaveSetting str = new SaveSetting();
        str.url = "http://ec2-43-201-108-241.ap-northeast-2.compute.amazonaws.com:8081/api/v1/game/save";
        //str.gameSetNo = InfoManagerBJH.instance.gameSetNo;
        //str.gameDay = UI.instance.dayInt;
        //str.voteNpcName = InfoManagerBJH.instance.voteNpcName;
        //str.voteNightNumber = InfoManagerBJH.instance.voteNightNumber;
        //str.voteResult = InfoManagerBJH.instance.voteResult;
        //InfoManagerBJH.instance.DicToCheckList();
        //str.checkList = InfoManagerBJH.instance.checkList;
        str.gameSetNo = InfoManagerKJY.instance.gameSetNo;
        str.gameDay = UI.instance.dayInt;
        str.voteNpcName = InfoManagerKJY.instance.voteNpcName;
        str.voteNightNumber = InfoManagerKJY.instance.voteNightNumber;
        str.voteResult = InfoManagerKJY.instance.voteResult;
        str.npcCustomInfos = InfoManagerKJY.instance.npcCustomLists;

        // 체크리스트 dic을 list로 변환하는 메서드
        InfoManagerKJY.instance.DicToCheckList();

        str.checkList = InfoManagerKJY.instance.checkList;
        TrySaveSetting senario = new TrySaveSetting(str);
        Debug.Log("In Save Request");
    }
}
#endregion

#region Register
[System.Serializable]
public class RegisterRequest
{
    public string account;
    public string password;
    public string nickname;
}

[System.Serializable]
public class RegisterMessage
{
    public string errorCode;
    public string message;
}

public struct StrTryRegister
{
    public string url;
    public string account, password, nickname;
}

[System.Serializable]
public class RegisterResponse
{
    public string resultCode;
    public string userName;
}

public class TryResgister : ConnectionStratege
{
    string url;
    string account, password, nickname, name, email;
    ConnectionKJY kjy = GameObject.FindAnyObjectByType<ConnectionKJY>();

    public TryResgister(StrTryRegister str)
    {
        this.url = str.url;
        this.account = str.account;
        this.password = str.password;
        this.nickname = str.nickname;
        CreateJson();
    }

    public void CreateJson()
    {
        RegisterRequest request = new RegisterRequest();
        request.account = this.account;
        request.password = this.password;
        request.nickname = this.nickname;

        string jsonData = JsonUtility.ToJson(request);
        OnGetRequest(jsonData);
    }
    public void OnGetRequest(string jsonData)
    {
        HttpRequester request = new HttpRequester();

        request.Settting(RequestType.POST, this.url);
        request.body = jsonData;
        request.complete = Complete;

        KJY_TestHttpManager.instance.SendRequest(request);
    }
    public void Complete(DownloadHandler result)
    {
        RegisterResponse reponse = new RegisterResponse();
        reponse = JsonUtility.FromJson<RegisterResponse>(result.text);
        if (reponse.resultCode == "SUCCESS")
        {
            InfoManagerKJY.instance.playerName = reponse.userName;
            ConnectionKJY.instance.RegisterPopUP();
        }
        else
        {
            kjy.GetCheckMessage("회원가입에 실패했습니다.");
            kjy.Check();
        }
    }
}
#endregion


#region LoginSetting
[System.Serializable]
public class LoginRequest
{
    public string account;
    public string password;
}

public struct StrTryLogin
{
    public string url;
    public string account, password;
}

[System.Serializable]
public class LoginResponse
{
    public string resultCode;
    public LoginMessage message;
}

[System.Serializable]
public class LoginMessage
{
    public long memberNo;
    public string account;
    public string nickname;
    public string name;
    public string email;
    public List<RoleList> role;
    public string token;
    public List<LoginGameSetDTO> loginGameSetDTO;
}

[System.Serializable]
public class RoleList
{
    public string name;
}

[System.Serializable]
public class LoginGameSetDTO
{
    public long gameSetNo;
    public DateTime createdAt;
    public DateTime modifiedAt;
    public int gameDay;
    public string gmaeStatus;
    public string gameResult;
    public List<Custom> custom;
}

[System.Serializable]
public class Custom
{
    public int eyes;
    public int mouth;
    public int ears;
    public int body;
    public int tail;
}

public class TryLogin : ConnectionStratege
{
    string url;
    string account, password;
    ConnectionKJY kjy = GameObject.FindAnyObjectByType<ConnectionKJY>();
    
    public TryLogin(StrTryLogin str)
    {
        this.url = str.url;
        this.account = str.account;
        this.password = str.password;
        CreateJson();
    }
    public void CreateJson()
    {
        LoginRequest request = new LoginRequest();
        request.account = this.account;
        request.password = this.password;

        string jsonData = JsonUtility.ToJson(request);
        OnGetRequest(jsonData);
    }
    public void OnGetRequest(string jsonData)
    {
        HttpRequester request = new HttpRequester();

        request.Settting(RequestType.POST, this.url);
        request.body = jsonData;
        request.complete = Complete;

        KJY_TestHttpManager.instance.SendRequest(request);
    }
    public void Complete(DownloadHandler result)
    {
        LoginResponse reponse = new LoginResponse();
        reponse = JsonUtility.FromJson<LoginResponse>(result.text);

        Debug.Log(reponse.resultCode);

        if (reponse.resultCode == "SUCCESS")
        {
            InfoManagerKJY.instance.userToken = reponse.message.token;
            InfoManagerKJY.instance.playerName = reponse.message.name;
            InfoManagerKJY.instance.nickname = reponse.message.nickname;

            KJY_LobbyConnection.instance.OnClickConnect();

        }
        else
        {
            kjy.GetCheckMessage("로그인을 실패했습니다.\n 아이디나 비밀번호를 다시 확인해주세요");
            kjy.Check();
        }
    }
}
#endregion

#region RoomSaveSetting
[System.Serializable]
public class RoomSaveRequest
{
    public int roomNumber;
    public string creatorNickname;
    public string participantNickname;
}

public struct StrRoomSave
{
    public string url;
    public string creatorNickname, participantNickname;
    public int roomNumber;
}

[System.Serializable]
public class RoomSaveResponse
{
    public string resultCode;
    public RoomSaveMessage message;
}

[System.Serializable]
public class RoomSaveMessage
{
    public DateTime saveTime;
    public bool saved;
}

public class TryRoomSave : ConnectionStratege
{
    string url;
    public int roomNumber;
    public string creatorNickname, participantNickname;

    public TryRoomSave(StrRoomSave str)
    {
        this.url = str.url;
        this.roomNumber = str.roomNumber;
        this.creatorNickname = str.creatorNickname;
        this.participantNickname = str.participantNickname;
        CreateJson();
    }
    public void CreateJson()
    {
        RoomSaveRequest request = new RoomSaveRequest();
        request.roomNumber = this.roomNumber;
        request.creatorNickname = this.creatorNickname;
        request.participantNickname = this.participantNickname;

        string jsonData = JsonUtility.ToJson(request);
        OnGetRequest(jsonData);
    }
    public void OnGetRequest(string jsonData)
    {
        HttpRequester request = new HttpRequester();

        request.Settting(RequestType.POST, this.url);
        request.body = jsonData;
        request.complete = Complete;

        HttpManagerKJY.instance.SendRequest(request);
    }
    public void Complete(DownloadHandler result)
    {
        RoomSaveResponse reponse = new RoomSaveResponse();
        reponse = JsonUtility.FromJson<RoomSaveResponse>(result.text);

        Debug.Log(reponse.resultCode);

        if (reponse.resultCode == "SUCCESS")
        {
            ConnectionKJY.instance.RequestGameSet();
        }
        else
        {

        }
    }
}

#endregion

#region AccountCheck
[System.Serializable]
public class AccountCheckRequest
{
    public string account;
}

public struct StrTryAccountCheck
{
    public string url;
    public string account;
}

[System.Serializable]
public class AccountCheckResponse
{
    public string resultCode;
    public AccountCheckMessage message;
}

[System.Serializable]
public class AccountCheckMessage
{
    public string message;
    public string answer;
}

public class TryAccountCheck : ConnectionStratege
{
    string url;
    string account;
    ConnectionKJY kjy = GameObject.FindAnyObjectByType<ConnectionKJY>();

    public TryAccountCheck(StrTryAccountCheck str)
    {
        this.url = str.url;
        this.account = str.account;
        CreateJson();
    }
    public void CreateJson()
    {
        AccountCheckRequest request = new AccountCheckRequest();
        request.account = this.account;

        string jsonData = JsonUtility.ToJson(request);
        OnGetRequest(jsonData);
    }
    public void OnGetRequest(string jsonData)
    {
        HttpRequester request = new HttpRequester();

        request.Settting(RequestType.POST, this.url);
        request.body = jsonData;
        request.complete = Complete;

        KJY_TestHttpManager.instance.SendRequest(request);
    }
    public void Complete(DownloadHandler result)
    {
        AccountCheckResponse response = new AccountCheckResponse();
        response = JsonUtility.FromJson<AccountCheckResponse>(result.text);

        kjy.accountCheck = true;
        if (response.resultCode == "SUCCESS")
        {
            kjy.GetCheckMessage(response.message.answer);
            kjy.Check();
        }
        if (response.resultCode == null)
        {
            kjy.GetCheckMessage(response.message.message);
            kjy.Check();
            kjy.ResetAccount();
        }
    }
}
#endregion

#region NickNameCheck
[System.Serializable]
public class NickNameCheckRequest
{
    public string nickname;
}

public struct StrTryNickNameCheck
{
    public string url;
    public string nickname;
}

[System.Serializable]
public class NickNameCheckResponse
{
    public string resultCode;
    public NickNameCheckMessage message;
}

[System.Serializable]
public class NickNameCheckMessage
{
    public string message;
    public string answer;
}

public class TryNickNameCheck : ConnectionStratege
{
    string url;
    string nickname;
    ConnectionKJY kjy = GameObject.FindAnyObjectByType<ConnectionKJY>();

    public TryNickNameCheck(StrTryNickNameCheck str)
    {
        this.url = str.url;
        this.nickname = str.nickname;
        CreateJson();
    }
    public void CreateJson()
    {
        NickNameCheckRequest request = new NickNameCheckRequest();
        request.nickname = this.nickname;

        string jsonData = JsonUtility.ToJson(request);
        OnGetRequest(jsonData);
    }
    public void OnGetRequest(string jsonData)
    {
        HttpRequester request = new HttpRequester();

        request.Settting(RequestType.POST, this.url);
        request.body = jsonData;
        request.complete = Complete;

        KJY_TestHttpManager.instance.SendRequest(request);
    }
    public void Complete(DownloadHandler result)
    {
        NickNameCheckResponse response = new NickNameCheckResponse();
        response = JsonUtility.FromJson<NickNameCheckResponse>(result.text);

        kjy.nickNameCheck = true;
        if (response.resultCode == "SUCCESS")
        {
            kjy.GetCheckMessage(response.message.answer);
            kjy.Check();
        }
        else
        {
            kjy.GetCheckMessage(response.message.message);
            kjy.Check();
            kjy.ResetNickName();
        }
    }
}
#endregion

#region GameStartSetting
[System.Serializable]
public class gameSetingResopnse
{
    public string resultCode;
    public message message;
}

[System.Serializable]
public class message
{
    public string playerName;
    public string playerNickName;
    public int gameStatus;
    public long gameSetNo;
    public List<GameNpcSetting> gameNpcList;
}

[System.Serializable]
public class GameNpcSetting
{
    public long gameNpcNo;

    public string npcName;
    public string npcJob;
    public string npcPersonality;
    public string npcFeature;
    public string npcStatus;
    public string deathLocation;
    public long npcDeathNightNumber;

    public Material npcMaterial;
    public Material npcMouth;
    public Mesh earCollider;
    public Mesh tailCollider;
}
#endregion

#region SenarioSetting
[System.Serializable]
public class SenarioRequst
{
    public long gameSetNo;
    public string secretKey;
}

[System.Serializable]
public class SenarioSetting
{
    public string url;
    public long gameSetNo;
    public string secretKey;
}

[System.Serializable]
public class SenarioResponse
{
    public string resultCode;
    public ScenarioMessage message;
}

[System.Serializable]
public class ScenarioMessage
{
    public string crimeScene;
    public string dailySummary;
    public string victim;
    public List<GameNpcSetting> gameNpcSetting;
}

public class TrySenarioSetting : ConnectionStratege
{
    string url;
    long gameSetNo;
    string secretKey;

    public TrySenarioSetting(SenarioSetting str)
    {
        this.url = str.url;
        this.gameSetNo = str.gameSetNo;
        this.secretKey = str.secretKey;
        CreateJson();
    }
    public void CreateJson()
    {
        SenarioRequst request = new SenarioRequst();
        request.gameSetNo = this.gameSetNo;
        request.secretKey = this.secretKey;

        string jsonData = JsonUtility.ToJson(request);
        OnGetRequest(jsonData);
    }
    public void OnGetRequest(string jsonData)
    {
        HttpRequester request = new HttpRequester();

        request.Settting(RequestType.POST, this.url);
        request.body = jsonData;
        request.complete = Complete;

        //KJY_TestHttpManager.instance.SendRequest(request);
        HttpManagerKJY.instance.SendRequest(request);
    }
    public void Complete(DownloadHandler result)
    {

        SenarioResponse reponse = new SenarioResponse();
        reponse = JsonUtility.FromJson<SenarioResponse>(result.text);

        InfoManagerKJY.instance.victim = reponse.message.victim;
        InfoManagerKJY.instance.crimeScene = reponse.message.crimeScene;
        InfoManagerKJY.instance.dailySummary = reponse.message.dailySummary;

        //KJYKJYKJY
        //KJY_SceneManager.instance.ChangeMainScene();
    }
}
#endregion

#region Intro-ScenarioSetting

[System.Serializable]
public class IntroScenarioRequest
{
    public IntroRequests introRequest;
    public MakeScenarioRequest makeScenarioRequest;
}

[System.Serializable]
public class IntroScenarioSetting
{
    public IntroRequests introRequest;
    public MakeScenarioRequest makeScenarioRequest;
    public string url;
}

[System.Serializable]
public class IntroRequests
{
    public long gameSetNo;
    public string secretKey;
    public List<string> characters;
}

[System.Serializable]
public class MakeScenarioRequest
{
    public long gameSetNo;
    public string secretKey;
}

[System.Serializable]
public class IntroScenarioResponse
{
    public string resultCode;
    public IntroScenarioMessage message;
}

[System.Serializable]
public class IntroScenarioMessage
{
    public IntroAnswer introAnswer;
    public MessageScenarioResponse scenarioResponse;
}

[System.Serializable]
public class IntroAnswer
{
    public string greeting;
    public string content;
    public string closing;
}

[System.Serializable]
public class MessageScenarioResponse
{
    public string crimeScene;
    public string dailySummary;
    public string victim;
    public List<GameNpcSetting> gameNpcList;
}

[System.Serializable]
public class TryIntroScenarioSetting : ConnectionStratege
{
    string url;
    IntroRequests introRequest;
    MakeScenarioRequest makeScenarioRequest;

    public TryIntroScenarioSetting(IntroScenarioSetting str)
    {
        this.url = str.url;
        this.introRequest = str.introRequest;
        this.makeScenarioRequest = str.makeScenarioRequest;
        CreateJson();
    }
    public void CreateJson()
    {
        IntroScenarioRequest request = new IntroScenarioRequest();
        request.introRequest = this.introRequest;
        request.makeScenarioRequest = this.makeScenarioRequest;

        string jsonData = JsonUtility.ToJson(request);
        OnGetRequest(jsonData);
    }
    public void OnGetRequest(string jsonData)
    {
        HttpRequester request = new HttpRequester();

        request.Settting(RequestType.POST, this.url);
        request.body = jsonData;
        request.complete = Complete;

        HttpManagerKJY.instance.SendRequest(request);
    }
    public void Complete(DownloadHandler result)
    {

        IntroScenarioResponse reponse = new IntroScenarioResponse();
        reponse = JsonUtility.FromJson<IntroScenarioResponse>(result.text);


        if(reponse.resultCode == "SUCCESS")
        {
            InfoManagerKJY.instance.ScenarioOfIntroScenarSetting(reponse.message.scenarioResponse);
            InfoManagerKJY.instance.IntroOfIntroScenarioSetting(reponse.message.introAnswer);



            foreach (GameNpcSetting npc in reponse.message.scenarioResponse.gameNpcList)
            {
                InfoManagerKJY.instance.npcOxDic.Add(npc.npcName.ToString(), null);
            }
        }
        string sceneName = SceneName.Chinemachine_01.ToString();
        PhotonNetwork.LoadLevel(sceneName);
        //KJYKJYKJY
        //KJY_SceneManager.instance.ChangeScene(2);
    }
}

#endregion

#region FinalSetting
[System.Serializable]
public class FinalRequest
{
    public long gameSetNo;
    public string secretKey;
}

[System.Serializable]
public class FinalSetting
{
    public long gameSetNo;
    public string secretKey;
    public string url;
}

[System.Serializable]
public class FinalResponse
{
    public string resultCode;
    public FinalMessage message;
}

[System.Serializable]
public class FinalMessage
{
    public string finalWords;
}

public class TryFinalSetting : ConnectionStratege
{
    long gameSetNo;
    string url;
    string secretKey;

    public TryFinalSetting(FinalSetting str)
    {
        this.url = str.url;
        this.gameSetNo = str.gameSetNo;
        this.secretKey = str.secretKey;

        CreateJson();
    }
    public void CreateJson()
    {
        FinalRequest request = new FinalRequest();
        request.gameSetNo = this.gameSetNo;
        request.secretKey = this.secretKey;

        string jsonData = JsonUtility.ToJson(request);
        OnGetRequest(jsonData);
    }
    public void OnGetRequest(string jsonData)
    {
        HttpRequester request = new HttpRequester();

        request.Settting(RequestType.POST, this.url);
        request.body = jsonData;
        request.complete = Complete;

        //KJY_TestHttpManager.instance.SendRequest(request);
        HttpManagerKJY.instance.SendRequest(request);
    }
    public void Complete(DownloadHandler result)
    {

        FinalResponse reponse = new FinalResponse();
        reponse = JsonUtility.FromJson<FinalResponse>(result.text);

        InfoManagerKJY.instance.FinalWordConnect(reponse.message.finalWords); 
    }
}
#endregion

#region GameEndSetting

[System.Serializable]
public class GameEndRequest
{
    public long gameSetNo;
    public string secretKey;
    public string resultMessage;
}

[System.Serializable]
public class GameEndSetting
{
    public long gameSetNo;
    public string secretKey;
    public string resultMessage;
    public string url;
}

[System.Serializable]
public class GameEndResponse
{
    public string resultCode;
    public GameEndMessage message;
}

[System.Serializable]
public class GameEndMessage
{
    public string resultMessage;
}

public class TryGameEndSetting : ConnectionStratege
{
    long gameSetNo;
    string url;
    string secretKey;
    string resultMessage;

    public TryGameEndSetting(GameEndSetting str)
    {
        this.url = str.url;
        this.gameSetNo = str.gameSetNo;
        this.secretKey = str.secretKey;
        this.resultMessage = str.resultMessage;

        CreateJson();
    }
    public void CreateJson()
    {
        GameEndRequest request = new GameEndRequest();
        request.gameSetNo = this.gameSetNo;
        request.secretKey = this.secretKey;
        request.resultMessage = this.resultMessage;

        string jsonData = JsonUtility.ToJson(request);
        OnGetRequest(jsonData);
    }
    public void OnGetRequest(string jsonData)
    {
        HttpRequester request = new HttpRequester();

        request.Settting(RequestType.POST, this.url);
        request.body = jsonData;
        request.complete = Complete;

        //KJY_TestHttpManager.instance.SendRequest(request);
        HttpManagerKJY.instance.SendRequest(request);
    }
    public void Complete(DownloadHandler result)
    {

        GameEndResponse reponse = new GameEndResponse();
        reponse = JsonUtility.FromJson<GameEndResponse>(result.text);

        //KJYKJYKJY
        // KJY_SceneManager.instance.ChangeLoseScene();
    }
}
#endregion

#region Load
[System.Serializable]
public class LoadRequest
{
    public string gameSetNo;
}

[System.Serializable]
public class LoadSetting
{
    public string gameSetNo;
    public string url;
}

[System.Serializable]
public class LoadResponse
{
    public string resultCode;
    public LoadMessage message;
}


[System.Serializable]
public class LoadMessage
{
    public LoadGameSet gameSet;
    public string deadNpc;
    public string deadPlace;
    public List<CheckLists> checkList;
    public LoadGameNpcSetting scenario;
    //public List<LoadGameNpcSetting> gameNpcSetting;
}

[System.Serializable]
public class LoadGameSet
{
    public long gameSetNo;
    public DateTime createdAt;
    public DateTime modifiedAt;
    public int gameDay;
    public string gameStatus;
    public string gameResult;
    public Custom custom;
}

[System.Serializable]
public class LoadCheckList
{
    public long userChecklistNo;
    public string mark;
    public string checkJob;
    public string npcName;
}

[System.Serializable]
public class LoadGameNpcSetting
{
    public string crimeScene;
    public string dailySummary;
    public string victim;
    public List<GameNpcSetting> gameNpcList;
}

[System.Serializable]
public class TryLoadSetting : ConnectionStratege
{
    string gameSetNo;
    string url;

    public TryLoadSetting(LoadSetting str)
    {
        this.gameSetNo = str.gameSetNo;
        this.url = str.url;
        Debug.Log(str.url);
        CreateJson();
    }

    public void CreateJson()
    {
        LoadRequest request = new LoadRequest();
        request.gameSetNo = this.gameSetNo;

        string jsonData = JsonUtility.ToJson(request);
        OnGetRequest(jsonData);
    }
    public void OnGetRequest(string jsonData)
    {
        HttpRequester request = new HttpRequester();

        request.Settting(RequestType.GET, this.url);
        request.body = jsonData;
        request.complete = Complete;

        HttpManagerKJY.instance.SendRequest(request);
    }
    public void Complete(DownloadHandler result)
    {
        LoadResponse response = new LoadResponse();
        response = JsonUtility.FromJson<LoadResponse>(result.text);

        InfoManagerKJY.instance.LoadSetting(response.message);

        // 변지환이 추가함
        if (response.message.gameSet.custom != null)
        {
            InfoManagerKJY.instance.body = response.message.gameSet.custom.body;
            InfoManagerKJY.instance.tail = response.message.gameSet.custom.tail;
            InfoManagerKJY.instance.eye = response.message.gameSet.custom.eyes;
            InfoManagerKJY.instance.mouth = response.message.gameSet.custom.mouth;
            InfoManagerKJY.instance.ear = response.message.gameSet.custom.ears;
        }

        //KJYKJYKJY
        //KJY_SceneManager.instance.ChangeMainScene();
    }
}
#endregion

#region Question3Setting
[System.Serializable]
public class QuestionRequest
{
    public long gameSetNo;
    public string npcName;
    public string keyWord;
    public string keyWordType;
}

public class QuestionSetting
{
    public long gameSetNo;
    public string npcName;
    public string keyWord;
    public string keyWordType;

    public string url;
}

[System.Serializable]
public class QuestionResponse
{
    public string resultCode;
    public QuestionMessage message;
}

[System.Serializable]
public class QuestionMessage
{
    public List<Questions> questions;
}

[System.Serializable]
public class Questions
{
    public long number;
    public string question;
}

[System.Serializable]
public class TryQeustionSetting : ConnectionStratege
{
    long gameSetNo;
    string npcName;
    string keyWord;
    string keyWordType;
    string url;

    public TryQeustionSetting(QuestionSetting str)
    {
        this.gameSetNo = str.gameSetNo;
        this.npcName = str.npcName;
        this.keyWord = str.keyWord;
        this.keyWordType = str.keyWordType;

        this.url = str.url;
        CreateJson();
    }

    public void CreateJson()
    {
        QuestionRequest request = new QuestionRequest();
        request.gameSetNo = this.gameSetNo;
        request.npcName = this.npcName;
        request.keyWord = this.keyWord;
        request.keyWordType = this.keyWordType;


        string jsonData = JsonUtility.ToJson(request);
        OnGetRequest(jsonData);
    }
    public void OnGetRequest(string jsonData)
    {
        HttpRequester request = new HttpRequester();

        request.Settting(RequestType.POST, this.url);
        request.body = jsonData;
        request.complete = Complete;

        HttpManagerKJY.instance.SendRequest(request);
    }
    public void Complete(DownloadHandler result)
    {
        QuestionResponse response = new QuestionResponse();
        response = JsonUtility.FromJson<QuestionResponse>(result.text);

        if (response.resultCode == "SUCCESS")
        {
            Debug.Log(response.message.questions[0].question);
            ChatManager.instance.ChatButtonList(response.message.questions);
        }

    }
}
#endregion


#region QuestionAnswerSetting
[System.Serializable]
public class QuestionAnswerRequest
{
    public long gameSetNo;
    public string npcName;
    public int questionIndex;
    public string keyWord;
    public string keyWordType;
}

public class QuestionAnswerSetting
{
    public long gameSetNo;
    public string npcName;
    public int questionIndex;
    public string keyWord;
    public string keyWordType;

    public string url;
}

[System.Serializable]
public class QuestionAnswerResponse
{
    public string resultCode;

    public QuestionAnswerMessage message;
}

[System.Serializable]
public class QuestionAnswerMessage
{
    public string response;
}

[System.Serializable]
public class TryQeustionAnswerSetting : ConnectionStratege
{
    long gameSetNo;
    string npcName;
    int questionIndex;
    string keyWord;
    string keyWordType;
    string url;

    public TryQeustionAnswerSetting(QuestionAnswerSetting str)
    {
        this.gameSetNo = str.gameSetNo;
        this.npcName = str.npcName;
        this.questionIndex = str.questionIndex;
        this.keyWord = str.keyWord;
        this.keyWordType = str.keyWordType;

        this.url = str.url;
        CreateJson();
    }

    public void CreateJson()
    {
        QuestionAnswerRequest request = new QuestionAnswerRequest();
        request.gameSetNo = this.gameSetNo;
        request.npcName = this.npcName;
        request.questionIndex = this.questionIndex;
        request.keyWord = this.keyWord;
        request.keyWordType = this.keyWordType;


        string jsonData = JsonUtility.ToJson(request);
        OnGetRequest(jsonData);
    }
    public void OnGetRequest(string jsonData)
    {
        HttpRequester request = new HttpRequester();

        request.Settting(RequestType.POST, this.url);
        request.body = jsonData;
        request.complete = Complete;

        HttpManagerKJY.instance.SendRequest(request);
    }
    public void Complete(DownloadHandler result)
    {
        QuestionAnswerResponse response = new QuestionAnswerResponse();
        response = JsonUtility.FromJson<QuestionAnswerResponse>(result.text);

        if (response.resultCode == "SUCCESS")
        {
            ChatManager.instance.talkingName.text = ChatManager.instance.npcdata.npcName;
            ChatManager.instance.npcText.text = response.message.response;
            ChatManager.instance.isConnection = true;
            ChatManager.instance.npctalk = true;
        }

    }
}
#endregion

public class ConnectionKJY : MonoBehaviour
{
    public static ConnectionKJY instance;
    [SerializeField] TMP_Text account;
    [SerializeField] TMP_Text password;
    [SerializeField] TMP_Text nickname;
    [SerializeField] TMP_Text playerName;
    [SerializeField] TMP_Text email;

    [SerializeField] TMP_InputField account1;
    [SerializeField] TMP_InputField password1;
    [SerializeField] TMP_InputField nickname1;
    [SerializeField] TMP_InputField playerName1;
    [SerializeField] TMP_InputField email1;

    [SerializeField] TMP_Text log_id;
    [SerializeField] TMP_Text log_password;

    public GameObject RegisterPopUp;
    public GameObject loginUI;
    public GameObject mainUI;
    public GameObject loadingPopup;
    [SerializeField] GameObject FailUI;
    [SerializeField] GameObject RegisterUI;
    [SerializeField] TMP_Text checkText;

    public GameObject LobbyLoadingUI;

    public bool accountCheck;
    public bool nickNameCheck;
    public bool emailCheck;

    private void Awake()
    {
        instance = this;
        accountCheck = false;
        nickNameCheck = false;
        emailCheck = false;
    }

    public void RegisterPopUP()
    {
        StartCoroutine(RegisterPopUI());
    }

    IEnumerator RegisterPopUI()
    {
        RegisterPopUp.SetActive(true);
        yield return new WaitForSeconds(2f);
        RegisterPopUp.SetActive(false);
    }

    public void ResetInputField()
    {
        account1.text = string.Empty;
        password1.text = string.Empty;
        nickname1.text = string.Empty;
        playerName1.text = string.Empty;
        email1.text = string.Empty;
    }

    public interface ConnectionStratege
    {
        public void CreateJson();
        public void OnGetRequest(string jsonData);
        public void Complete(DownloadHandler result);
    }

    public void RequestAccoutCheck()
    {
        StrTryAccountCheck str;
        str.url = "http://ec2-15-165-15-244.ap-northeast-2.compute.amazonaws.com:8081/api/v1/members/check-account";
        str.account = account.text;

        TryAccountCheck tryAccountCheck = new TryAccountCheck(str);
    }

    public void RequestNickNameCheck()
    {
        StrTryNickNameCheck str;
        str.url = "http://ec2-15-165-15-244.ap-northeast-2.compute.amazonaws.com:8081/api/v1/members/check-nickname";
        str.nickname = nickname.text;

        TryNickNameCheck tryAccountCheck = new TryNickNameCheck(str);
    }

    #region Register

    public void OnclickSend() //요거 내껄로
    {
        
        StrTryRegister str;
        str.url = "http://ec2-15-165-15-244.ap-northeast-2.compute.amazonaws.com:8081/api/v1/members/register";
        str.account = account.text;
        str.password = password.text;
        str.nickname = nickname.text;

        if (nickNameCheck == true && accountCheck == true)
        {
            TryResgister tryResgister = new TryResgister(str);
        }
        else
        {
            if (accountCheck == false)
            {
                GetCheckMessage("아이디 중복 체크를 하지 않았습니다.");
                Check();
            }
            else
            {
                GetCheckMessage("닉네임 중복 체크를 하지 않았습니다.");
                Check();
            }
        }
    }
    #endregion

    #region Login

    public void OnclickSendLogin() //요거 내껄로
    {
        StrTryLogin str;
        str.url = "http://ec2-15-165-15-244.ap-northeast-2.compute.amazonaws.com:8081/api/v1/members/sign-in";
        str.account = log_id.text;
        str.password = log_password.text;

        if (str.account.Length < 4 || str.password.Length < 2)
        {
            GetCheckMessage("아이디나 비밀번호가 잘못됐습니다.");
            Check();
        }
        else
        {
            LobbyLoadingUI.SetActive(true);
            TryLogin trylogin = new TryLogin(str);
        }

    }
    #endregion

    #region
    public void OnClickSendRoomSave()
    {
        StrRoomSave str;
        str.url = "http://ec2-15-165-15-244.ap-northeast-2.compute.amazonaws.com:8081/api/v1/user/room/save";
        str.roomNumber = 0; // 바꿔야함
        str.creatorNickname = KJY_RoomManageer.instance.masterNickname;
        //str.participantNickname = KJY_RoomManageer.instance.participantNickname;
        str.participantNickname = "Test";

        print (str.roomNumber);
        print(str.creatorNickname);   
        print(str.participantNickname);

        TryRoomSave roomSave = new TryRoomSave(str);
    }
    #endregion

    #region GameSetting

    public void RequestGameSet()
    {
        HttpRequester res = new HttpRequester();
        res.Settting(RequestType.POST, "http://ec2-15-165-15-244.ap-northeast-2.compute.amazonaws.com:8081/api/v1/game/start");
        res.complete = rgComplete;


        HttpManagerKJY.instance.SendRequest(res);
    }

    public void rgComplete(DownloadHandler result)
    {
        gameSetingResopnse gsr = new gameSetingResopnse();
        gsr = JsonUtility.FromJson<gameSetingResopnse>(result.text); //담겨있는 상태
        if (gsr.resultCode == "SUCCESS")
        {
            InfoManagerKJY.instance.Setting(gsr);

            RequestIntroScenarioSetting();

            //KJY_SceneManager.instance.ChangeScene(1);//임시추가
        }
    }
    #endregion

    #region Scenario

    public void ReqeustScenario()
    {
        loadingPopup.SetActive(true);
        SenarioSetting str = new SenarioSetting();
        str.url = "http://ec2-15-165-15-244.ap-northeast-2.compute.amazonaws.com:8081/api/v1/scenario/save";
        str.gameSetNo = InfoManagerKJY.instance.gameSetNo;
        str.secretKey = "mafia";

        TrySenarioSetting senario = new TrySenarioSetting(str);
    }
    #endregion

    #region TMPIntro
    //[System.Serializable]
    //public class IntroRequest
    //{
    //    public long gameSetNo;
    //    public string secretKey;
    //}

    //[System.Serializable]
    //public class IntroSetting
    //{
    //    public long gameSetNo;
    //    public string secretKey;
    //    public string url;
    //}

    //[System.Serializable]
    //public class IntroResponse
    //{
    //    public string resultCode;
    //    public IntroMessage message;
    //}

    //[System.Serializable]
    //public class IntroMessage
    //{
    //    public string greeting;
    //    public string content;
    //    public string closing;
    //}

    //public class TryIntroSetting : ConnectionStratege
    //{
    //    string url;
    //    string secretKey;
    //    long gameSetNo;

    //    public TryIntroSetting(IntroSetting str)
    //    {
    //        this.url = str.url;
    //        this.gameSetNo = str.gameSetNo;
    //        this.secretKey = str.secretKey;
          
    //        CreateJson();
    //    }
    //    public void CreateJson()
    //    {
    //        IntroRequest request = new IntroRequest();
    //        request.gameSetNo = this.gameSetNo;
    //        request.secretKey = this.secretKey;

    //        string jsonData = JsonUtility.ToJson(request);
    //        OnGetRequest(jsonData);
    //    }
    //    public void OnGetRequest(string jsonData)
    //    {
    //        HttpRequester request = new HttpRequester();

    //        request.Settting(RequestType.POST, this.url);
    //        request.body = jsonData;
    //        request.complete = Complete;

    //        //KJY_TestHttpManager.instance.SendRequest(request);
    //        HttpManagerKJY.instance.SendRequest(request);
    //    }
    //    public void Complete(DownloadHandler result)
    //    {

    //        IntroResponse reponse = new IntroResponse();
    //        reponse = JsonUtility.FromJson<IntroResponse>(result.text);

    //            InfoManagerKJY.instance.greeting = reponse.message.greeting;
    //            InfoManagerKJY.instance.content = reponse.message.content;
    //            InfoManagerKJY.instance.closing = reponse.message.closing;

    //            UnityEngine.SceneManagement.SceneManager.LoadScene(1);
    //    }
    //}


    //public void RequestIntro()
    //{
    //    IntroSetting str = new IntroSetting();
    //    str.url = "http://ec2-43-201-108-241.ap-northeast-2.compute.amazonaws.com:8081/api/v1/scenario/intro";
    //    str.gameSetNo = InfoManagerKJY.instance.gameSetNo;
    //    str.secretKey = "mafia";

    //    TryIntroSetting senario = new TryIntroSetting(str);
    //}

    #endregion


    #region FinalConnection

    public void RequestFinal()
    {
        FinalSetting str = new FinalSetting();
        str.url = "http://ec2-43-201-108-241.ap-northeast-2.compute.amazonaws.com:8081/api/v1/scenario/final-words";
        //str.gameSet = InfoManagerKJY.instance.gameSetNo;
        str.gameSetNo = InfoManagerKJY.instance.gameSetNo;
        str.secretKey = "mafia";

        TryFinalSetting final = new TryFinalSetting(str);
    }

    #endregion


    #region GameLodad
    public void RequestLoad()
    {
        LoadSetting str = new LoadSetting();
        str.gameSetNo = InfoManagerKJY.instance.gameSetNo.ToString();
        str.url = "http://ec2-43-201-108-241.ap-northeast-2.compute.amazonaws.com:8081/api/v1/game/load?gameSetNo=" + str.gameSetNo;

        TryLoadSetting load = new TryLoadSetting(str);
    }
    #endregion


    #region GameEnd
    public void RequestGameEnd()
    {
        GameEndSetting str = new GameEndSetting();
        str.url = "http://ec2-15-165-15-244.ap-northeast-2.compute.amazonaws.com:8081/api/v1/game/end";
        str.gameSetNo = InfoManagerKJY.instance.gameSetNo;
        str.secretKey = "mafia";
        str.resultMessage = "FAILURE";

        TryGameEndSetting gameEnd = new TryGameEndSetting(str);
    }
    #endregion

    #region IntroScenario
    public void RequestIntroScenarioSetting()
    {
        IntroScenarioSetting str = new IntroScenarioSetting();
        str.introRequest = new IntroRequests();
        str.makeScenarioRequest = new MakeScenarioRequest();

        str.url = "http://ec2-15-165-15-244.ap-northeast-2.compute.amazonaws.com:8081/api/v1/scenario/intro-scenario";

        str.introRequest.gameSetNo = InfoManagerKJY.instance.gameSetNo;
        str.introRequest.secretKey = "mafia";
        str.introRequest.characters = new List<string>();
        str.makeScenarioRequest.gameSetNo = InfoManagerKJY.instance.gameSetNo;
        str.makeScenarioRequest.secretKey = "mafia";

        TryIntroScenarioSetting introScenario = new TryIntroScenarioSetting(str);
    }

    #endregion

    #region NpcCustom
    //[System.Serializable]
    //public class NpcCustomRequest
    //{
    //    public long gameSetNo;
    //    public List<NpcCustomInfos> npcCustomInfos;
    //}

    //[System.Serializable]
    //public class NpcCustomInfos
    //{
    //    public string npcName;
    //    public int mouth;
    //    public int ear;
    //    public int body;
    //    public int tail;
    //}

    //[System.Serializable]
    //public class NpcCustomSetting
    //{
    //    public string url;
    //    public long gameSetNo;
    //    public List<NpcCustomInfos> npcCustomInfos;
    //}

    //[System.Serializable]
    //public class NpcCustomResponse
    //{
    //    public string resultCode;
    //    public NpcCustomMessage message;
    //}

    //[System.Serializable]
    //public class NpcCustomMessage
    //{
    //    public DateTime saveTime;
    //    public bool saved;
    //}

    //public class TryNpcCustomSaveSetting : ConnectionStratege
    //{
    //    public string url;
    //    public long gameSetNo;
    //    public List<NpcCustomInfos> npcCustomInfos;

    //    public TryNpcCustomSaveSetting(NpcCustomSetting str)
    //    {
    //        this.url = str.url;
    //        this.gameSetNo = str.gameSetNo;
    //        this.npcCustomInfos = str.npcCustomInfos;

    //        CreateJson();
    //    }

    //    public void CreateJson()
    //    {
    //        NpcCustomRequest request = new NpcCustomRequest();
    //        request.gameSetNo = this.gameSetNo;
    //        request.npcCustomInfos = this.npcCustomInfos; 

    //        string jsonData = JsonUtility.ToJson(request);
    //        OnGetRequest(jsonData);
    //    }

    //    public void OnGetRequest(string jsonData)
    //    {
    //        HttpRequester request = new HttpRequester();

    //        request.Settting(RequestType.POST, this.url);
    //        request.body = jsonData;
    //        request.complete = Complete;

    //        HttpManagerKJY.instance.SendRequest(request);
    //    }
    //    public void Complete(DownloadHandler result)
    //    {
    //        NpcCustomResponse response = new NpcCustomResponse();
    //        response = JsonUtility.FromJson<NpcCustomResponse>(result.text);
    //    }
    //}

    //public class NpcCustomRequestSave
    //{
    //    public void Request()
    //    {
    //        NpcCustomSetting str = new NpcCustomSetting();
    //        str.url = "http://ec2-43-201-108-241.ap-northeast-2.compute.amazonaws.com:8081/api/v1/npc/custom/save";
    //        str.gameSetNo = InfoManagerKJY.instance.gameSetNo;
    //        str.npcCustomInfos = InfoManagerKJY.instance.npcCustomLists;
    //        Debug.Log(str.npcCustomInfos[2].npcName);


    //        TryNpcCustomSaveSetting setting = new TryNpcCustomSaveSetting(str);
    //    }
    //}
    #endregion


    #region question3 Generate

    public void Request_Question(string npc_name, string keyword)
    {
        QuestionSetting str = new QuestionSetting();
        str.url = "http://ec2-15-165-15-244.ap-northeast-2.compute.amazonaws.com:8081/api/v1/question/create";

        str.gameSetNo = InfoManagerKJY.instance.gameSetNo;
        str.npcName = npc_name;
        str.keyWord = keyword;
        str.keyWordType = "weapon";

        print(str.gameSetNo);
        print(str.npcName);
        print(str.keyWord);
        print(str.keyWordType);

        TryQeustionSetting question = new TryQeustionSetting(str);
    }
    #endregion

    #region question Answer
    public void RequestAnswer(int number, string npc_name, string keyword)
    {
        QuestionAnswerSetting str = new QuestionAnswerSetting();
        str.url = "http://ec2-15-165-15-244.ap-northeast-2.compute.amazonaws.com:8081/api/v1/question/answer";

        str.gameSetNo = InfoManagerKJY.instance.gameSetNo;
        str.npcName = npc_name;
        str.questionIndex = number + 1;
        str.keyWord = keyword;
        str.keyWordType = "weapon";


        print(str.gameSetNo);
        print(str.npcName);
        print(str.questionIndex); 
        print(str.keyWord);


        TryQeustionAnswerSetting answer = new TryQeustionAnswerSetting(str);
    }
    #endregion

    public void GetCheckMessage(string message)
    {
        checkText.text = message;
    }

    public void Check()
    {
        StartCoroutine(CheckCoroutine());
    }

    private IEnumerator CheckCoroutine()
    {
        RegisterUI.SetActive(true);
        yield return new WaitForSeconds(2f);
        RegisterUI.SetActive(false);
    }

    public void ResetAccount()
    {
        account1.text = string.Empty;
    }

    public void ResetNickName()
    {
        nickname1.text = string.Empty;
    }

    public void ResetEmail() 
    { 
        email1.text = string.Empty;
    }
}
