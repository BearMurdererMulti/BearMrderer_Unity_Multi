using Photon.Pun;
using System.Collections;
using System.Collections.Generic;
using UnityEngine;
using UnityEngine.Playables;

public class KJY_TimeLine : MonoBehaviourPun
{
    [SerializeField] PlayableDirector director;

    private void Start()
    {
        // Ÿ�Ӷ����� ���� �� ������ �޼ҵ� ����
        Invoke("OnTimelineFinished", (float)director.duration);
    }

    private void OnTimelineFinished()
    {
        photonView.RPC("GoEndingCredit", RpcTarget.All);
    }

    [PunRPC]
    private void GoEndingCredit()
    {
        string sceneName = SceneName.EndingCredit.ToString();
        PhotonNetwork.LoadLevel(sceneName);
    }
}
