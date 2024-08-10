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
        // 타임라인이 끝난 후 실행할 메소드 예약
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
