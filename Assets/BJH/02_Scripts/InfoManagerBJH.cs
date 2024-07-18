using System.Collections;
using System.Collections.Generic;
using UnityEngine;

public class InfoManagerBJH : MonoBehaviour
{
    public static InfoManagerBJH instance;

    public string nickname;
    public string userName;
    public string token = null;
    public bool isLogin = false;
    public string secretKey;
    public long gameSetNo;
    public List<TestLoginLoginGameSetDTOResponse> testLoginresponse;

    public string greeting;
    public string content;
    public string closing;

    public string crimeScene;
    public string dailySummary;
    public string victim;
    public List<TestGameNpcList> gameNpcList;

    //voteInformation
    public string voteNpcName;
    public string voteResult;
    public int voteNightNumber;

    // check list
    public Dictionary<string, string> npcOxDic = new Dictionary<string, string>(); // 자체 판단용
    public List<CheckLists> checkList = new List<CheckLists>(); // 서버 통신용
    // NPC 체크리스트 정보

    // character custom info
    public string head;
    public string eye;
    public string mouth;
    public string ear;
    public string body;
    public string tail;

    private void Awake()
    {
        if(instance == null)
        {
            instance = this;
        }

        gameNpcList = new List<TestGameNpcList>();
    }

    // check list
    // Dic -> List<CheckList>
    // 데이터 save 통신을 하기 전 해당 메서드를 호출
    public void DicToCheckList()
    {
        foreach(TestGameNpcList gameNpc in gameNpcList)
        {
            CheckLists check = new CheckLists();
            check.npcName = gameNpc.npcName;
            Debug.Log(npcOxDic[gameNpc.npcName]);
            check.mark = npcOxDic[gameNpc.npcName];
            check.checkJob = "null";

            checkList.Add(check);
        }
    }

    public void loginDTO()
    {
        gameSetNo = testLoginresponse[testLoginresponse.Count - 1].gameSetNo;
    }
}

public class CheckList
{
    public string npcName;
    public string mark;
    public string checkJob;
}

public class GameNpcList
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