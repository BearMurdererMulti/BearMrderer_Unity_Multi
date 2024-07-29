using Photon.Pun;
using Photon.Realtime;
using System.Collections;
using System.Collections.Generic;
using UnityEngine;

public class KJY_RoomStart : MonoBehaviourPunCallbacks
{

    public void CreateRoom()
    {
        //방 옵션을 설정 (최대 인원)
        RoomOptions option = new RoomOptions();
        //방 목록에 보이게 하냐? 안하냐?
        option.IsVisible = false;
        //방에 참여할 수 있니? 없니?
        option.IsOpen = false;
        option.MaxPlayers = 2;

        //custom 설정
        ExitGames.Client.Photon.Hashtable hash = new ExitGames.Client.Photon.Hashtable();
        hash["room_job"] = "탐정";



        //custom 설정을 option 에 셋팅
        option.CustomRoomProperties = hash;


        //특정 로비에 방 생성 요청
        //TypedLobby typedLobby = new TypedLobby("Meta Lobby", LobbyType.Default);
        //PhotonNetwork.CreateRoom(inputRoomName.text, option, typedLobby);
        //기본 로비에 방 생성 요청
        PhotonNetwork.CreateRoom(InfoManagerKJY.instance.nickname, option);
    }

    //방 생성 완료시 호출 되는 함수
    public override void OnCreatedRoom()
    {
        base.OnCreatedRoom();
        print("방 생성 완료");
    }

    //방 생성 실패시 호출 되는 함수
    public override void OnCreateRoomFailed(short returnCode, string message)
    {
        base.OnCreateRoomFailed(returnCode, message);
        print("방 생성 실패 : " + message);
    }

    // 방 입장 완료시 호출되는 함수
    public override void OnJoinedRoom()
    {
        base.OnJoinedRoom();
        print("방 입장 완료");
        print("방 입장 실패 : ");
        //GameScene 으로 이동
        ConnectionKJY.instance.RequestIntroScenarioSetting();
        //PhotonNetwork.LoadLevel("02_Room_KJY");
    }
}
