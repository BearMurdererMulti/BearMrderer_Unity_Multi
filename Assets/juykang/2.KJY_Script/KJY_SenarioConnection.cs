using System.Collections;
using System.Collections.Generic;
using UnityEngine;
using UnityEngine.Networking;
using static System.Net.WebRequestMethods;

public class KJY_SenarioConnection : MonoBehaviour
{
    public static KJY_SenarioConnection instance;

    private void Awake()
    {
        instance = this;
    }

    public interface ConnectionStratege
    {
        public void CreateJson();
        public void OnGetRequest(string jsonData);
        public void Complete(DownloadHandler result);
    }

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
        public SenarioMessage message;
    }

    [System.Serializable]
    public class SenarioMessage
    {
        public string crimeScene;
        public string dailySummary;
        public string victim;
        public List<GameNpcList> gameNpcList;
    }

    [System.Serializable]
    public class GameNpcList
    {
        public long gameNpcNo;
        public string npcName;
        public string npcJob;
        public string npcPersonality;
        public string npcFeature;
        public string npcStatus;
        public string deathLocation;
        public long npcDeathNightNumber;
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

            //InfoManagerBJH.instance.victim = reponse.message.victim;
            //InfoManagerBJH.instance.crimeScene = reponse.message.crimeScene;
            //InfoManagerBJH.instance.dailySummary = reponse.message.dailySummary;

            DayAndNIghtManager.instance.DieResidentSet();
        }
    }

    public void ReqeustScenario()
    {
        SenarioSetting str = new SenarioSetting();
        str.url = "http://ec2-43-201-108-241.ap-northeast-2.compute.amazonaws.com:8081/api/v1/scenario/save";
        str.gameSetNo = InfoManagerKJY.instance.gameSetNo;
        //str.gameSetNo = InfoManagerBJH.instance.gameSetNo;
        str.secretKey = "mafia";
        //Select = false;
        TrySenarioSetting senario = new TrySenarioSetting(str);
    }
}
