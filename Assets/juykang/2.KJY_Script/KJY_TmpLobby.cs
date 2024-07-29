using Photon.Pun;
using System.Collections;
using System.Collections.Generic;
using UnityEngine;

public class KJY_TmpLobby : MonoBehaviourPunCallbacks
{

    public void OnClickConnect()
    {
        // ���� ���� ��û
        PhotonNetwork.ConnectUsingSettings();
    }

    public override void OnConnectedToMaster()
    {
        base.OnConnectedToMaster();

        //�г��� ����
        PhotonNetwork.NickName = "��������";
        //Ư�� Lobby ���� ����
        //TypedLobby typedLobby = new TypedLobby("Meta Lobby", LobbyType.Default);
        //PhotonNetwork.JoinLobby(typedLobby);

        //�⺻ �κ� ���� ��û
        PhotonNetwork.JoinLobby();
    }

    public override void OnJoinedLobby()
    {
        base.OnJoinedLobby();

        //�κ� ������ �̵�
        PhotonNetwork.LoadLevel("01_Lobby_KJY");
    }
}
