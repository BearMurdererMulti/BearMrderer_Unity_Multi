using Photon.Pun;
using Photon.Realtime;
using System.Collections;
using System.Collections.Generic;
using UnityEngine;

public class KJY_RoomStart : MonoBehaviourPunCallbacks
{

    public void CreateRoom()
    {
        //�� �ɼ��� ���� (�ִ� �ο�)
        RoomOptions option = new RoomOptions();
        //�� ��Ͽ� ���̰� �ϳ�? ���ϳ�?
        option.IsVisible = false;
        //�濡 ������ �� �ִ�? ����?
        option.IsOpen = false;
        option.MaxPlayers = 2;

        //custom ����
        ExitGames.Client.Photon.Hashtable hash = new ExitGames.Client.Photon.Hashtable();
        hash["room_job"] = "Ž��";



        //custom ������ option �� ����
        option.CustomRoomProperties = hash;


        //Ư�� �κ� �� ���� ��û
        //TypedLobby typedLobby = new TypedLobby("Meta Lobby", LobbyType.Default);
        //PhotonNetwork.CreateRoom(inputRoomName.text, option, typedLobby);
        //�⺻ �κ� �� ���� ��û
        PhotonNetwork.CreateRoom(InfoManagerKJY.instance.nickname, option);
    }

    //�� ���� �Ϸ�� ȣ�� �Ǵ� �Լ�
    public override void OnCreatedRoom()
    {
        base.OnCreatedRoom();
        print("�� ���� �Ϸ�");
    }

    //�� ���� ���н� ȣ�� �Ǵ� �Լ�
    public override void OnCreateRoomFailed(short returnCode, string message)
    {
        base.OnCreateRoomFailed(returnCode, message);
        print("�� ���� ���� : " + message);
    }

    // �� ���� �Ϸ�� ȣ��Ǵ� �Լ�
    public override void OnJoinedRoom()
    {
        base.OnJoinedRoom();
        print("�� ���� �Ϸ�");
        print("�� ���� ���� : ");
        //GameScene ���� �̵�
        ConnectionKJY.instance.RequestIntroScenarioSetting();
        //PhotonNetwork.LoadLevel("02_Room_KJY");
    }
}
