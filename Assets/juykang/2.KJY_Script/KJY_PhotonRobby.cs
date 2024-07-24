using Photon.Pun;
using Photon.Realtime;
using System.Collections;
using System.Collections.Generic;
using UnityEngine;
using UnityEngine.UI;

public class KJY_PhotonRobby : MonoBehaviourPunCallbacks
{
    //Input Room Name
    public InputField inputRoomName;
    //Input Max Player
    public InputField inputMaxPlayer;
    //Input Password
    public InputField inputPassword;

    //방 참여 버튼
    public Button btnJoinRoom;
    //방 생성 버튼
    public Button btnCreateRoom;

    //RoomItem Prefab
    public GameObject roomItemFactory;
    //RoomListView -> Content -> RectTransform
    public RectTransform rtContent;

    //방 정보 가지고 있는 Dictionary
    Dictionary<string, RoomInfo> roomCache = new Dictionary<string, RoomInfo>();

    void Start()
    {
        //방 참여, 생성 비활성화
        btnJoinRoom.interactable = btnCreateRoom.interactable = false;
        //inputRoomName 의 내용이 변경될 때 호출되는 함수
        inputRoomName.onValueChanged.AddListener(OnValueChangedRoomName);
        //inputMaxPlayer 의 내용이 변경될 때 호출되는 함수
        inputMaxPlayer.onValueChanged.AddListener(OnValueChangedMaxPlayer);
    }

    //참여 & 생성 버튼에 관여
    void OnValueChangedRoomName(string room)
    {
        //참여 버튼 활성/ 비활성
        btnJoinRoom.interactable = room.Length > 0;
        //생성 버튼 활성 / 비활성
        btnCreateRoom.interactable = room.Length > 0 && inputMaxPlayer.text.Length > 0;
    }

    //생성 버튼에 관여
    void OnValueChangedMaxPlayer(string max)
    {
        btnCreateRoom.interactable = max.Length > 0 && inputRoomName.text.Length > 0;
    }

    public void CreateRoom()
    {
        //방 옵션을 설정 (최대 인원)
        RoomOptions option = new RoomOptions();
        option.MaxPlayers = int.Parse(inputMaxPlayer.text);
        //방 목록에 보이게 하냐? 안하냐?
        option.IsVisible = true;
        //방에 참여할 수 있니? 없니?
        option.IsOpen = true;

        //custom 설정
        ExitGames.Client.Photon.Hashtable hash = new ExitGames.Client.Photon.Hashtable();
        hash["room_name"] = inputRoomName.text;
        hash["map_idx"] = 2;

        //custom 설정을 option 에 셋팅
        option.CustomRoomProperties = hash;

        //custom 정보를 Lobby 에서 사용할 수 있게 설정
        string[] customKeys = { "room_name", "map_idx"};
        option.CustomRoomPropertiesForLobby = customKeys;

        //특정 로비에 방 생성 요청
        //TypedLobby typedLobby = new TypedLobby("Meta Lobby", LobbyType.Default);
        //PhotonNetwork.CreateRoom(inputRoomName.text, option, typedLobby);

        //기본 로비에 방 생성 요청
        PhotonNetwork.CreateRoom(inputRoomName.text + inputPassword.text, option);
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

    public void JoinRoom()
    {
        //방 입장 요청
        PhotonNetwork.JoinRoom(inputRoomName.text + inputPassword.text);
    }

    // 방 입장 완료시 호출되는 함수
    public override void OnJoinedRoom()
    {
        base.OnJoinedRoom();
        print("방 입장 완료");

        //GameScene 으로 이동
        PhotonNetwork.LoadLevel("Room_KJY");
    }

    // 방 입장 실패시 호출되는 함수
    public override void OnJoinRoomFailed(short returnCode, string message)
    {
        base.OnJoinRoomFailed(returnCode, message);
        print("방 입장 실패 : " + message);
    }

    void RemoveRoomList()
    {
        //rtContent 에 있는 자식 GameObject 를 모두 삭제
        for (int i = 0; i < rtContent.childCount; i++)
        {
            Destroy(rtContent.GetChild(i).gameObject);
        }


        //foreach (Transform tr in rtContent)
        //{
        //    Destroy(tr.gameObject);
        //}
    }

    void UpdateRoomList(List<RoomInfo> roomList)
    {
        foreach (RoomInfo info in roomList)
        {
            //roomCache 에 info 의 방이름 으로 되어있는 Key 값 존재하니?
            if (roomCache.ContainsKey(info.Name))
            {
                //삭제 해야하니?
                if (info.RemovedFromList)
                {
                    roomCache.Remove(info.Name);
                }
                //수정
                else
                {
                    roomCache[info.Name] = info;
                }
            }
            else
            {
                //추가
                roomCache[info.Name] = info;
            }
        }
    }

    void CreateRoomList()
    {
        foreach (RoomInfo info in roomCache.Values)
        {
            //roomItem prefab 을 이용해서 roomItem 을 만든다. 
            GameObject goRoomItem = Instantiate(roomItemFactory, rtContent);
            ////만들어진 roomItem 의 부모를 scrollView -> Content 의 transform 으로 한다.
            //goRoomItem.transform.parent = rtContent;

            //custom 정보 뽑아오자.
            string roomName = (string)(info.CustomProperties["room_name"]);
            int mapIdx = (int)(info.CustomProperties["map_idx"]);

            //만들어진 roomItem 에서 RoomItem 컴포넌트 가져온다.
            KJY_RoomItem roomItem = goRoomItem.GetComponent<KJY_RoomItem>();
            //가져온 컴포넌트가 가지고 있는 SetInfo 함수 실행
            roomItem.SetInfo(roomName, InfoManagerKJY.instance.nickname ,info.PlayerCount, info.MaxPlayers);
            //RoomItem 이 클릭 되었을 때 호출되는 함수 등록
            roomItem.onChangeRoomName = OnChangeRoomNameField;

            //람다식 lamda 
            //roomItem.onChangeRoomName = (string roomName) => {
            //    inputRoomName.text = roomName;
            //};
        }
    }

    // 누군가 방을 만들거나 수정했을때 호출되는 함수
    public override void OnRoomListUpdate(List<RoomInfo> roomList)
    {
        base.OnRoomListUpdate(roomList);

        //전체 룸리스트UI 삭제
        RemoveRoomList();
        //내가 따로 관리하는 룸리스트 정보 갱신
        UpdateRoomList(roomList);
        //룸리스트 정보를 가지고 UI 를 다시 생성
        CreateRoomList();
    }

    public void OnChangeRoomNameField(string roomName)
    {
        inputRoomName.text = roomName;
    }
}
