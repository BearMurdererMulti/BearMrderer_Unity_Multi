using Photon.Pun;
using System.Collections;
using System.Collections.Generic;
using UnityEngine;

public class KJY_LobbyConnection : MonoBehaviourPunCallbacks
{
    public static KJY_LobbyConnection instance;

    private void Awake()
    {
        instance = this;
    }

    public void OnClickConnect()
    {
        // 서버 접속 요청
        PhotonNetwork.ConnectUsingSettings();
    }

    public override void OnConnectedToMaster()
    {
        base.OnConnectedToMaster();

        //닉네임 설정
        PhotonNetwork.NickName = InfoManagerKJY.instance.nickname;
        //특정 Lobby 정보 셋팅
        //TypedLobby typedLobby = new TypedLobby("Meta Lobby", LobbyType.Default);
        //PhotonNetwork.JoinLobby(typedLobby);

        //기본 로비 진입 요청
        PhotonNetwork.JoinLobby();
    }

    public override void OnJoinedLobby()
    {
        base.OnJoinedLobby();

        //로비 씬으로 이동
        PhotonNetwork.LoadLevel("01_Lobby_KJY");
    }
}
