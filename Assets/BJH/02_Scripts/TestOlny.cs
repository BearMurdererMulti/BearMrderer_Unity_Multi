using System;
using System.Collections;
using System.Collections.Generic;
using Unity.VisualScripting;
using UnityEngine;
using UnityEngine.Networking;

[System.Serializable]
public class TestLoginRequest
{
    public string account;
    public string password;
}

[System.Serializable]
public class TestLoginResponse
{
    public string resultCode;
    public TestLoginMessageResponse message;
}

[System.Serializable]
public class TestLoginMessageResponse
{
    public long memberNo;
    public string account;
    public string nickname;
    public string name;
    public string email;
    public List<TestLoginMessageRolsesResponse> roles;
    public string token;
    public List<TestLoginLoginGameSetDTOResponse> loginGameSetDTO;
}


[System.Serializable]
public class TestLoginMessageRolsesResponse
{
    public string name;
}

[System.Serializable]
public class TestLoginLoginGameSetDTOResponse
{
    public long gameSetNo;
    public DateTime createdAt;
    public DateTime modifiedAt;
    public int gameDay; // n일차
    public string gameStatus; // 게임 진행 여부
    public string gameResult; // 게임 완료 여부(유저 승패 정보)
}


// id : test01
// pw : test01
// nickname, username : 테스트
// email : test01@gmail.com
public class TestTryLogin
{
    TestLoginRequest request = new TestLoginRequest();

    string url;
    string secretKey;

    // 생성자
    public TestTryLogin(string account, string password, string secretKey, string url) 
    {
        request.account = account;
        request.password = password;
        this.url = url;
        this.secretKey = secretKey;

        string json = JsonUtility.ToJson(request);
        TestTryLoginConnection(json);
    }

    public void TestTryLoginConnection(string json)
    {
        HttpRequester requester = new HttpRequester();
        requester.Settting(RequestType.POST, url);
        requester.body = json;
        requester.complete = Complete;
        Debug.Log(requester.body);
        HttpManagerBJH.instance.SendRequest(requester);
    }

    public void Complete(DownloadHandler result)
    {
        TestLoginResponse response = new TestLoginResponse();
        response = JsonUtility.FromJson<TestLoginResponse>(result.text);
        
        if(response.resultCode == "SUCCESS")
        {
            InfoManagerKJY info = InfoManagerKJY.instance;
            info.token = response.message.token;
            info.nickname = response.message.nickname;
            info.userName = response.message.name;
            info.secretKey = secretKey;

            int idx = response.message.loginGameSetDTO.Count;
            info.gameSetNo = response.message.loginGameSetDTO[idx-1].gameSetNo;
            InfoManagerKJY.instance.isLogin = false;
            //InfoManagerKJY.instance.testLoginresponse = response.message.loginGameSetDTO;
            InfoManagerKJY.instance.loginDTO();
        }
        else
        {
            Debug.Log($"로그인 통신에 실패했습니다. resultCode == {response.resultCode}입니다.");
        }
    }
}

[System.Serializable]
public class TestGameStarResponse
{ 
    public string resultCode;
    public TestGameStartResponsetMessage message;
}

[System.Serializable]
public class TestGameStartResponsetMessage
{
    public long gameSetNo;
    public List<TestGameStartResponseMessagegameNpcList> gameNpcList;
}

[System.Serializable]
public class TestGameStartResponseMessagegameNpcList
{
    public long gameNpcNo;
    public string npcName;
    public string npcJob;
    public string npcPersonality;
    public string npcFeature;
    public string npcStatus;
    public float npcDeathLocationX;
    public float npcDeathLocationY;
    public float npcDeathLocationZ;
    public long npcDeathNightNumber;
}

[System.Serializable]
public class TestGameStart
{
    string url;
    // 생성자
    public TestGameStart(string url)
    {
        this.url = url;

        HttpRequester requester = new HttpRequester();
        requester.Settting(RequestType.POST, url);
        //requester.body = "";
        requester.complete = Complete;

        HttpManagerBJH.instance.SendRequest(requester);
        
    }

    private void Complete(DownloadHandler result)
    {
        TestGameStarResponse response = new TestGameStarResponse();
        response = JsonUtility.FromJson<TestGameStarResponse>(result.text);

        if(response.resultCode == "SUCCESS")
        {
            InfoManagerKJY.instance.gameSetNo = response.message.gameSetNo;
        }
        else
        {
            Debug.Log($"game start 통신에 실패했습니다. resultCode == {response.resultCode}입니다.");
        }
    }
}




[System.Serializable]
public class TestIntroScenarioRequest
{
    public TestIntroRequest introRequest;
    public TestMakeScenarioRequest makeScenarioRequest;
}

[System.Serializable]
public class TestIntroRequest
{
    public long gameSetNo;
    public string secretKey;
    public List<string> characters;
}

[System.Serializable]
public class TestMakeScenarioRequest
{
    public long gameSetNo;
    public string secretKey;
}

[System.Serializable]
public class TestIntroScenarioResponse
{
    public string resultCode;
    public TestIntroScenarioMessage message;
}

[System.Serializable]
public class TestIntroScenarioMessage
{
    public TestIntroAnswer introAnswer;
    public TestMessageScenarioResponse scenarioResponse;
}

[System.Serializable]
public class TestIntroAnswer
{
    public string greeting;
    public string content;
    public string closing;
}

[System.Serializable]
public class TestMessageScenarioResponse
{
    public string crimeScene;
    public string dailySummary;
    public string victim;
    public List<TestGameNpcList> gameNpcList;
}

[System.Serializable]
public class TestGameNpcList
{
    public long gameNpcNo;
    public string npcName;
    public string npcJob;
    public string npcPersonality;
    public string npcFeature;
    public string npcStatus;
    public string deathLocation; // 임시 : 서버에서 수정하면 통신 테스트
    public int npcDeathNightNumber;
}

public class TestIntroScenario : MonoBehaviour
{
    string url;
    // 생성자
    public TestIntroScenario(string url)
    {
        this.url = url;

        TestIntroScenarioRequest request = new TestIntroScenarioRequest();
        request.introRequest = new TestIntroRequest();
        request.makeScenarioRequest = new TestMakeScenarioRequest();

        request.introRequest.gameSetNo = InfoManagerKJY.instance.gameSetNo;
        request.introRequest.secretKey = InfoManagerKJY.instance.secretKey;
        request.introRequest.characters = new List<string>(); // 임시 : 뭐지?
        request.makeScenarioRequest.gameSetNo = InfoManagerKJY.instance.gameSetNo;
        request.makeScenarioRequest.secretKey = InfoManagerKJY.instance.secretKey;
        // 질문 : 왜 game set no, secert key를 두 번이나 받아가지?

        string json = JsonUtility.ToJson(request);

        HttpRequester requester = new HttpRequester();
        requester.Settting(RequestType.POST, url);
        requester.body = json;
        requester.complete = Complete;

        print(requester);
        HttpManagerBJH.instance.SendRequest(requester);

    }

    private void Complete(DownloadHandler result)
    {
        TestIntroScenarioResponse response = new TestIntroScenarioResponse();
        response = JsonUtility.FromJson<TestIntroScenarioResponse>(result.text);

        if (response.resultCode == "SUCCESS")
        {
            InfoManagerKJY instance = InfoManagerKJY.instance;
            instance.greeting = response.message.introAnswer.greeting;
            instance.content = response.message.introAnswer.content;
            instance.closing = response.message.introAnswer.closing;

            instance.crimeScene = response.message.scenarioResponse.crimeScene;
            instance.dailySummary = response.message.scenarioResponse.dailySummary;
            instance.victim = response.message.scenarioResponse.victim;

            //instance.gameNpcList = response.message.scenarioResponse.gameNpcList;

            Debug.Log($"intro scenario 통신을 성공했습니다. resultCode == {response.resultCode}입니다.");

            // npc list가 업데이트되면, npc dic 업데이트하기
            //foreach (TestGameNpcList npc in instance.gameNpcList)
            //{
            //    instance.npcOxDic.Add(npc.npcName, null);
            //}

        }
        else
        {
            Debug.Log($"intro scenario 통신에 실패했습니다. resultCode == {response.resultCode}입니다.");
        }
    }
}

// game load
[System.Serializable]
public class GameLoadRequest
{
    public string gameSetNo;
}

[System.Serializable]
public class GameLoadResponse
{
    public string resultCode;
    public GameLoadMessage message;
}

[System.Serializable]
public class GameLoadMessage
{
    public GameLoadGameSet gameSet;
    public string deadNpc;
    public string deadPlace;
    public List<GameLoadCheckList> checkList;
    public List<Alibi> alibi;
    public List<GameLoadScenario> scenario;
}


[System.Serializable]
public class GameLoadGameSet
{
    public long gameSetNo;
    public DateTime createdAt;
    public DateTime modifiedAt;
    public int gameDay;
    public string gameStatus;
    public string gameResult;
}


[System.Serializable]
public class GameLoadCheckList
{
    public long userChecklistNo;
    public string mark;
    public string checkJob;
    public string npcName;
}

[System.Serializable]
public class Alibi
{
    public string name;
    public string alibi;
    public long gameNpcNo;
}

[System.Serializable]
public class GameLoadScenario
{
    public string crimeScene;
    public string dailySummary;
    public string victim;
    public List<GameLoaGameNpcListt> gameNpcList;
}

[System.Serializable]
public class GameLoaGameNpcListt
{
    public long gameNpcNo;
    public string name;
    public string npcJob;
    public string npcPersonality;
    public string npcFeature;
    public string npcState;
    public string deathLocation;
    public long npcDeathNightNumber;
}

// game load
public class ConnectGameLoad
{
    string url;
    string gameSetNo;

    public ConnectGameLoad(string url) 
    {
        //this.url = $"{url}?gameSetNo={InfoManagerBJH.instance.gameSetNo}";
        gameSetNo = InfoManagerKJY.instance.gameSetNo.ToString();
        Debug.Log(gameSetNo);
        this.url = "http://ec2-43-201-108-241.ap-northeast-2.compute.amazonaws.com:8081/api/v1/game/load?gameSetNo=" + gameSetNo;

        Debug.Log(this.url);

        GameLoadRequest request = new GameLoadRequest();
        //request.gameSetNo = InfoManagerBJH.instance.gameSetNo;
        request.gameSetNo = gameSetNo;

        string json = JsonUtility.ToJson(request);

        HttpRequester requester = new HttpRequester();
        requester.Settting(RequestType.GET, url);
        requester.body = null;
        requester.complete = Complete;

        HttpManagerBJH.instance.SendRequest(requester);
    }

    public void Complete(DownloadHandler result)
    {
        GameLoadResponse response = new GameLoadResponse();
        response = JsonUtility.FromJson<GameLoadResponse>(result.text);

        if(response.resultCode == "SUCCESS")
        {
            Debug.Log($"game load 통신이 완료되었습니다.");
        }
    }
}


public class TestOlny : MonoBehaviour
{
    //private void Start()
    //{
    //    // 게임이 시작되면 자동으로 로그인하여 토큰을 가져옴
    //    string loginUrl = "http://ec2-43-201-108-241.ap-northeast-2.compute.amazonaws.com:8081/api/v1/members/sign-in";
    //    TestTryLogin login = new TestTryLogin("test01", "test01", "mafia", loginUrl);

    //    string gameStartUrl = "http://ec2-43-201-108-241.ap-northeast-2.compute.amazonaws.com:8081/api/v1/game/start";
    //    TestGameStart gameStart = new TestGameStart(gameStartUrl);

    //    string introScenarioUrl = "http://ec2-43-201-108-241.ap-northeast-2.compute.amazonaws.com:8081/api/v1/scenario/intro-scenario";
    //    TestIntroScenario testIntroScenario = new TestIntroScenario(introScenarioUrl);
    //}

    private void Update()
    {
        if(Input.GetKeyDown(KeyCode.Alpha1))
        {
            // 게임이 시작되면 자동으로 로그인하여 토큰을 가져옴
            InfoManagerKJY.instance.isLogin = true;
            string loginUrl = "http://ec2-43-201-108-241.ap-northeast-2.compute.amazonaws.com:8081/api/v1/members/sign-in";
            TestTryLogin login = new TestTryLogin("test01", "test01", "mafia", loginUrl);
        }

        if (Input.GetKeyDown(KeyCode.Alpha2))
        {
            //게임 새로 만듦
            string gameStartUrl = "http://ec2-43-201-108-241.ap-northeast-2.compute.amazonaws.com:8081/api/v1/game/start";
            TestGameStart gameStart = new TestGameStart(gameStartUrl);

            // 기존 게임 로드
            //string gameStartUrl = "http://ec2-43-201-108-241.ap-northeast-2.compute.amazonaws.com:8081/api/v1/game/load";
            //ConnectGameLoad gameLoad = new ConnectGameLoad(gameStartUrl);

            //ConnectionKJY.instance.RequestLoad();
        }


        if (Input.GetKeyDown(KeyCode.Alpha3))
        {
            string introScenarioUrl = "http://ec2-43-201-108-241.ap-northeast-2.compute.amazonaws.com:8081/api/v1/scenario/intro-scenario";
            TestIntroScenario testIntroScenario = new TestIntroScenario(introScenarioUrl);
        }
    }
}
