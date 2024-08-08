using System;
using System.Collections;
using System.Collections.Generic;
using Unity.VisualScripting;
using UnityEngine;
using UnityEngine.UIElements;
using static ConnectionKJY;

public class InfoManagerKJY : MonoBehaviour
{
    public static InfoManagerKJY instance;

    void Awake()
    {
        if (instance != null)
        {
            Destroy(gameObject);
            return;
        }
        instance = this;
        DontDestroyOnLoad(gameObject);
        
    }

    //userInformation
    public string userToken;
    public string playerName;
    public List<LoginGameSetDTO> loginGameSetDTO = new List<LoginGameSetDTO>();

    //npcInformaion
    public List<GameNpcSetting> gameNpcList;

    //gameInformation
    public long gameSetNo;
    public string gameStatus;
    public DateTime createdAt;
    public DateTime modifiedAt;
    public int gameDay;
    public string gameResult;
    public string deadNpc;
    public string deadPlace;

    //senario
    public string victim;
    public string crimeScene;
    public string dailySummary;

    //voteInformation
    public string voteNpcName;
    public string voteResult;
    public int voteNightNumber;
    public string voteNpcObjectName;

    // intro
    public string greeting;
    public string content;
    public string closing;

    //checklist
    // check list
    public Dictionary<string, string> npcOxDic = new Dictionary<string, string>(); // 자체 판단용
    public List<CheckLists> checkList = new List<CheckLists>(); // 서버 통신용
    

    //saveInformation
    public DateTime saveTime;
    public int saveDay;
    public bool saved;

    //finalWord
    public string finalWorld;

    // login
    public bool isLogin = false;
    public List<LoginGameSetDTO> testLoginresponse;

    public string nickname = null;
    public string token;
    public string userName;
    public string secretKey;

    // chat list
    public int murderObjectNumber;

    // 변지환이 추가함
    [Header("캐릭터 커스텀")]
    public int head;
    public int eye;
    public int mouth;
    public int ear;
    public int body;
    public int tail;

    [Header("캐릭터 커스텀 머터리얼, 메쉬")]
    public CustomMaterialsAndMesh CustomMaterialsAndMesh;
    public Dictionary<string, int> customDictionary = new Dictionary<string, int>();

    //NpcCustomList
    public List<NpcCustomInfos> npcCustomLists = new List<NpcCustomInfos>();

    public string role;

    //내방 데이터
    public KJY_RoomItem roomdata;
    public int roomIndex;
    public string roomMasterNickName;
    public string roomPartiNickName;
    public string roomMasterRole;
    public string roomPartiRole;

    public void loginDTO()
    {
        gameSetNo = testLoginresponse[testLoginresponse.Count - 1].gameSetNo;
    }

    public struct SetNpcList
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

        public long npcDeathNightNumer;
        public long npcToken;

        public Material npcMaterial;
        public Material npcMouth;
        public Mesh earCollider;
        public Mesh tailCollider;
    }
    //public struct CheckList
    //{
    //    public string npcName;
    //    public string mark;
    //    public string checkJob;
    //}

    // check list
    // Dic -> List<CheckList>
    // 데이터 save 통신을 하기 전 해당 메서드를 호출
    public void DicToCheckList()
    {
        foreach (GameNpcSetting gameNpc in gameNpcList)
        {
            CheckLists check = new CheckLists();
            check.npcName = gameNpc.npcName;
            Debug.Log(npcOxDic[gameNpc.npcName]);
            check.mark = npcOxDic[gameNpc.npcName];
            check.checkJob = "null";

            checkList.Add(check);
        }
    }

    public void Setting(gameSetingResopnse setting)
    {
        StartCoroutine(SettingCoroutine(setting));
    }

    IEnumerator SettingCoroutine(gameSetingResopnse setting)
    {
        gameSetNo = setting.message.gameSetNo;
        gameStatus = setting.message.gameStatus.ToString();
        yield return new WaitForSeconds(1f);
    }

    public void ScenarioOfIntroScenarSetting(MessageScenarioResponse response)
    {
        crimeScene = response.crimeScene;
        dailySummary = response.method;
        victim = response.victim;
        gameNpcList = response.gameNpcList;
        //gameNpcList = SuffleList(gameNpcList);
    }

    public void IntroOfIntroScenarioSetting(IntroAnswer response)
    {
        greeting = response.greeting;
        content = response.content;
        closing = response.closing;
    }

    public void Senario(SenarioResponse senario)
    {
        victim = senario.message.victim;
        crimeScene = senario.message.crimeScene;
        dailySummary = senario.message.dailySummary;
    }

    public List<T> SuffleList<T>(List<T> array)
    {
        int random1, random2;
        T tmp;

        for (int i = 0; i < gameNpcList.Count; i++) 
        {
            random1 = UnityEngine.Random.Range(0, gameNpcList.Count);
            random2 = UnityEngine.Random.Range(0, gameNpcList.Count);

            tmp = array[random1];
            array[random1] = array[random2];
            array[random2] = tmp;
        }

        return array;
    }

    public void FinalWordConnect(string word)
    {
        
        FinalDialog finalDialog = GameObject.FindFirstObjectByType<FinalDialog>();

        finalDialog.FinalTexts(word);
    }

    public void SetSlot()
    {
        KJY_SlotManager.Instance.SetDTOList();
    }

    public void LoadSetting(LoadMessage response)
    {
        gameSetNo = response.gameSet.gameSetNo;
        createdAt = response.gameSet.createdAt;
        modifiedAt = response.gameSet.modifiedAt;
        gameDay = response.gameSet.gameDay;
        gameStatus = response.gameSet.gameStatus;
        gameResult = response.gameSet.gameResult;
        deadNpc = response.deadNpc;
        deadPlace = response.deadPlace;
        checkList = response.checkList;
        victim = response.scenario.victim;
        crimeScene = response.scenario.crimeScene;
        dailySummary = response.scenario.dailySummary;
        gameNpcList = response.scenario.gameNpcList;
        //gameNpcList = response.gameNpcSetting;
        //foreach (GameNpcSetting npc in response.gameNpcSetting)
        //{
        //    InfoManagerKJY.instance.npcOxDic.Add(npc.npcName.ToString(), null);
        //}
       // KJY_SceneManager.instance.ChangeMainScene();
    }
}
