using Photon.Pun;
using System.Collections;
using System.Collections.Generic;
using UnityEngine;

public class KJY_PhotonConnection : MonoBehaviourPunCallbacks
{
    public static KJY_PhotonConnection Instance;

    private void Awake()
    {
        Instance = this;
    }

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
}
