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

    private void Awake()
    {
        Instance = this;
    }

    private void Start()
    {
        ui = UI.instance; 
        gameManager = GameManager_KJY.instance;
        citizenManager = KJY_CitizenManager.Instance;
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
            string sceneName = SceneName.GameScene_NPC_Random.ToString();
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

}
