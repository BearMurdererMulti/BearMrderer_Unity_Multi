using Photon.Pun;
using System.Collections;
using System.Collections.Generic;
using UnityEngine;

public class PhotonConnection : MonoBehaviourPunCallbacks
{
    public static PhotonConnection Instance;

    private void Awake()
    {
        Instance = this;
    }

    #region IntroScenarioResponse
    public void IntroScenarioPhoton(IntroScenarioResponse response)
    {
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
            string sceneName = SceneName.Chinemachine_01.ToString();
            PhotonNetwork.LoadLevel(sceneName);
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

        string sceneName = SceneName.GameScene_NPC_Random.ToString();
        PhotonNetwork.LoadLevel(sceneName);
    }

    #endregion

}
