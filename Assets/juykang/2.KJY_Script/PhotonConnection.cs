using Photon.Pun;
using System.Collections;
using System.Collections.Generic;
using UnityEngine;
using static KJY_SenarioConnection;

public class PhotonConnection : MonoBehaviourPunCallbacks
{
    public static PhotonConnection Instance;
    [SerializeField] private UI ui;
    [SerializeField] private GameManager_KJY gameManager;
    [SerializeField] private SceneName targetSceneName;
    [SerializeField] private KJY_CitizenManager citizenManager;
    [SerializeField] private ChatManager chatManager;
    private bool isTimerRunning = false;

    private void Awake()
    {
        Instance = this;
    }

    private void Start()
    {
        ui = UI.instance; 
        gameManager = GameManager_KJY.instance;
        citizenManager = KJY_CitizenManager.Instance;
        chatManager = ChatManager.instance;
    }

    #region IntroScenarioResponse
    public void IntroScenarioPhoton(IntroScenarioResponse response)
    {
        List<GameNpcSetting> list = response.message.firstScenarioResponse.gameNpcList;
        response.message.firstScenarioResponse.gameNpcList = InfoManagerKJY.instance.SuffleList(list);
        string jsonResponse = JsonUtility.ToJson(response);
        photonView.RPC("UpdateScenario", RpcTarget.All, jsonResponse);
    }

    [PunRPC]
    private void UpdateScenario(string jsonResponse)
    {
        IntroScenarioResponse response = JsonUtility.FromJson<IntroScenarioResponse>(jsonResponse);

        if (response != null)
        {
            
            InfoManagerKJY.instance.ScenarioOfIntroScenarSetting(response.message.firstScenarioResponse);
            InfoManagerKJY.instance.IntroOfIntroScenarioSetting(response.message.introAnswer);
            foreach (GameNpcSetting npc in response.message.firstScenarioResponse.gameNpcList)
            {
                InfoManagerKJY.instance.npcOxDic.Add(npc.npcName.ToString(), null);
            }
            //string sceneName = SceneName.GameScene_NPC_Random_BJH.ToString();
            string sceneName = SceneName.GameScene_NPC_Random3.ToString();
            //string sceneName = SceneName.Chinemachine_01.ToString();
            PhotonNetwork.LoadLevel(sceneName);
            //PhotonNetwork.LoadLevel($"{targetSceneName}");
        }
    }
    #endregion

    #region CharacterCustom
    public void UserCustomSaveResponsePhoton(UserCustomSaveResponse response)
    {
        string jsonResponse = JsonUtility.ToJson(response);
        photonView.RPC("UpdateUserCustomSave", RpcTarget.All, jsonResponse);
    }

    [PunRPC]
    private void UpdateUserCustomSave(string jsonResponse)
    {
        UserCustomSaveResponse response = JsonUtility.FromJson<UserCustomSaveResponse>(jsonResponse);

        string sceneName = SceneName.Cinemachine03.ToString();
        PhotonNetwork.LoadLevel(sceneName);
    }
    #endregion

    #region InGame
    public void InGameResponsePhoton()
    {
        //photonView.RPC("GoInGame", RpcTarget.All);
        string sceneName = SceneName.GameScene_NPC_Random.ToString();
        PhotonNetwork.LoadLevel(sceneName);
    }

    [PunRPC]
    private void GoInGame()
    {
        string sceneName = SceneName.GameScene_NPC_Random.ToString();
        PhotonNetwork.LoadLevel(sceneName);
    }
    #endregion

    #region SetNpc
    #endregion

    #region MinusLife

    public void UpdateMinusLife()
    {
        PhotonView photonView = PhotonView.Get(ui);

        photonView.RPC("MinusLife", RpcTarget.All);
    }
    #endregion


    #region MorningAndNight

    public void UpdateDayAndNight(bool value)
    {
        PhotonView photonView = PhotonView.Get(ui);

        photonView.RPC("DayAndNight", RpcTarget.All, value);
    }

    #endregion

    #region SelectNpc
    public void UpdateChooseNpc(int viewID)
    {
        PhotonView photonView = PhotonView.Get(gameManager);

        photonView.RPC("ChooseNpc", RpcTarget.All, viewID);
    }
    #endregion

    #region
    public void UpdateCitizenCall()
    {
        PhotonView photonView = PhotonView.Get(citizenManager);

        photonView.RPC("CallCitizen", RpcTarget.All);
    }
    #endregion

    #region
    public void UpdateGoInterrRoom()
    {
        PhotonView photonView = PhotonView.Get(gameManager);
        photonView.RPC("GointerrogationRoom", RpcTarget.All);
    }
    #endregion

    #region MainCamera
    public void CameraOnOff(bool value)
    {
        PhotonView photonView = PhotonView.Get(gameManager);

        photonView.RPC("UpdateMainCamera", RpcTarget.All, value);
    }
    #endregion


    #region
    public void StartTimer()
    {
        UI.instance.timerText.enabled = true;
        float startTime = (float)PhotonNetwork.Time;
        photonView.RPC("StartTimerRPC", RpcTarget.All, startTime, 120);
    }

    [PunRPC]
    private void StartTimerRPC(float startTimestamp, int duration)
    {
        if (!isTimerRunning)
        {
            StartCoroutine(TimerCoroutine(startTimestamp, (float)duration));
        }
    }

    private IEnumerator TimerCoroutine(float startTimestamp, float duration)
    {
        isTimerRunning = true;
        float endTime = startTimestamp + duration;

        while (isTimerRunning)
        {
            float timeRemaining = endTime - (float)PhotonNetwork.Time;

            if (timeRemaining > 0)
            {
                UpdateTimerDisplay(timeRemaining);
            }
            else
            {
                UpdateTimerDisplay(0);
                UI.instance.timerText.enabled = false;
                ChatManager.instance.isTimerover = true;
                isTimerRunning = false;
            }

            yield return null; // 다음 프레임까지 대기
        }
    }

    private void UpdateTimerDisplay(float timeRemaining)
    {
        int minutes = Mathf.FloorToInt(timeRemaining / 60);
        int seconds = Mathf.FloorToInt(timeRemaining % 60);
        UI.instance.timerText.text = $"{minutes:00}:{seconds:00}";
    }
    #endregion

    #region interrogationUpdate

    public void Updateinterrogation(string message)
    {
        print("message" + message);
        photonView.RPC("UpdateText", RpcTarget.All, message);
    }

    [PunRPC]
    private void UpdateText(string message)
    {
        ChatManager.instance.dialog.text = message;
        ChatManager.instance.talkButton.SetActive(true);
    }

    #endregion

    #region
    public void VictoryGo()
    {
        photonView.RPC("GoVictory", RpcTarget.All);
    }

    [PunRPC]
    private void GoVictory()
    {
        string sceneName = SceneName.KJY_Test_Ending_Success.ToString();
        PhotonNetwork.LoadLevel(sceneName);
    }

    #endregion


    #region
    public void FailGo()
    {
        photonView.RPC("GoFail", RpcTarget.All);
    }

    [PunRPC]
    private void GoFail()
    {
        string sceneName = SceneName.KJY_Test_Ending_Fail.ToString();
        PhotonNetwork.LoadLevel(sceneName);
    }
    #endregion

    #region
    public void UpdateCancle()
    {
        PhotonView photonView = PhotonView.Get(gameManager);
        photonView.RPC("CancleSeletNpc", RpcTarget.All);
    }
    #endregion

    #region
    public void UpdateAfterSkip()
    {
        PhotonView photonView = PhotonView.Get(gameManager);
        photonView.RPC("AfterSkip", RpcTarget.All);
    }
    #endregion

    #region
    public void UpdateSelect()
    {
        PhotonView photonView = PhotonView.Get(gameManager);
        photonView.RPC("SelectNpc", RpcTarget.All);
    }
    #endregion
}
