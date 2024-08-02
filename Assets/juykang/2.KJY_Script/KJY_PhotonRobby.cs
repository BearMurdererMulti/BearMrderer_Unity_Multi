using Photon.Chat;
using Photon.Pun;
using Photon.Realtime;
using System.Collections;
using System.Collections.Generic;
using System.Linq;
using TMPro;
using UnityEngine;
using UnityEngine.UI;

public class KJY_PhotonRobby : MonoBehaviourPunCallbacks
{

    public static KJY_PhotonRobby Instance;

    
    //Input Room Name
    public TMP_InputField inputRoomName;
    //Input Psassword
    public TMP_InputField inputPassword;
    public Image checkImage;
    public bool isPrivate = false;
    private string nowRoomCode;
    int roomindex = 0;
    //방 생성 버튼
    public Button btnCreateRoom;

    //RoomItem Prefab
    public GameObject roomItemFactory;
    //RoomListView -> Content -> RectTransform
    public RectTransform rtContent;

    //비밀번호 설정
    private static readonly string characters = "ABCDEFGHIJKLMNOPQRSTUVWXYZ";

    //방 정보 가지고 있는 Dictionary
    Dictionary<string, RoomInfo> roomCache = new Dictionary<string, RoomInfo>();

    private List<KJY_RoomItem> roomLists = new List<KJY_RoomItem>();

    private bool isPrivateSelectedRoom = false;
    bool value = false;

    private void Awake()
    {
        Instance = this;
    }

    void Start()
    {
        LoginSceneUI.instance.SetUserName();
        //방 생성 비활성화
        btnCreateRoom.interactable = false;
        //inputRoomName 의 내용이 변경될 때 호출되는 함수
        inputRoomName.onValueChanged.AddListener(OnValueChangedRoomName);
    }

    //참여 & 생성 버튼에 관여
    void OnValueChangedRoomName(string room)
    {
        //생성 버튼 활성 / 비활성
        btnCreateRoom.interactable = room.Length > 0;
    }

    public void IsRoomPrivate(bool isPrivates)
    {
        if (isPrivate == true)
        {
            checkImage.enabled = false;
            isPrivate = false;
        }
        else
        {
            checkImage.enabled = true;
            isPrivate = true;
        }
    }

    public void CreateRoom()
    {
        //방 옵션을 설정 (최대 인원)
        RoomOptions option = new RoomOptions();
        //방 목록에 보이게 하냐? 안하냐?
        option.IsVisible = true;
        //방에 참여할 수 있니? 없니?
        option.IsOpen = true;
        option.MaxPlayers = 2;

        //custom 설정
        ExitGames.Client.Photon.Hashtable hash = new ExitGames.Client.Photon.Hashtable();
        hash["room_name"] = inputRoomName.text;
        hash["map_idx"] = roomindex;
        hash["room_master"] = InfoManagerKJY.instance.nickname;
        hash["room_job"] = "탐정";
        hash["room_code"] = "";
        hash["is_private"] = false;
        // 방의 사용자 정의 속성에 방 코드를 추가
        if (isPrivate)
        {
            hash["is_private"] = true;
            hash["room_code"] = GenerateRoomCode();
        }

        //custom 설정을 option 에 셋팅
        option.CustomRoomProperties = hash;

        //custom 정보를 Lobby 에서 사용할 수 있게 설정
        string[] customKeys = { "room_name", "room_master", "map_idx", "room_job", "room_code", "is_private"};
        option.CustomRoomPropertiesForLobby = customKeys;

        InfoManagerKJY.instance.roomIndex = 0;

        roomindex++;
 
        //특정 로비에 방 생성 요청
        //TypedLobby typedLobby = new TypedLobby("Meta Lobby", LobbyType.Default);
        //PhotonNetwork.CreateRoom(inputRoomName.text, option, typedLobby);
        //기본 로비에 방 생성 요청
        PhotonNetwork.CreateRoom(inputRoomName.text, option);
        value = true;
        print("this2");
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
        print("isPrivate");
        if (isPrivate == true)
        {
            //PhotonNetwork.JoinRoom();
            LoginSceneUI.instance.OnsecretKeyG();
        }
        else
        {
            PhotonNetwork.JoinRoom(inputRoomName.text);
        }
    }

    public void JoinPrivateRoom(string password)
    {
        if (nowRoomCode == password)
        {
            PhotonNetwork.JoinRoom(inputRoomName.text);
        }
    }

    // 방 입장 완료시 호출되는 함수
    public override void OnJoinedRoom()
    {
        base.OnJoinedRoom();
        print("방 입장 완료");
        //GameScene 으로 이동
        PhotonNetwork.LoadLevel("02_Room_KJY");
    }

    // 방 입장 실패시 호출되는 함수
    public override void OnJoinRoomFailed(short returnCode, string message)
    {
        base.OnJoinRoomFailed(returnCode, message);
        print("방 입장 실패 : " + message);

        if (returnCode == Photon.Realtime.ErrorCode.GameFull)
        {
            LoginSceneUI.instance.FullRoomUIEvent();
        }
    }

    void RemoveRoomList()
    {
        //rtContent 에 있는 자식 GameObject 를 모두 삭제
        for (int i = 1; i < rtContent.childCount; i++)
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
                    var existingRoom = roomCache[info.Name];
                    //방 인원 바꾸기 수정이 필요
                    roomCache[info.PlayerCount.ToString()] = info;

                    if (info.CustomProperties.ContainsKey("room_job"))
                    {
                        var roomJob = info.CustomProperties["room_job"].ToString();
                        // roomCache의 방 정보 업데이트
                        // 방의 역할 정보 업데이트 로직 추가
                        existingRoom.CustomProperties["room_job"] = roomJob;
                    }

                    if (info.CustomProperties.ContainsKey("room_master"))
                    {
                        var roomMaster = info.CustomProperties["room_master"].ToString();
                        // roomCache의 방 정보 업데이트
                        // 방의 역할 정보 업데이트 로직 추가
                        existingRoom.CustomProperties["room_master"] = roomMaster;
                    }

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
            string roomMaster = (string)(info.CustomProperties["room_master"]);
            string roomMasterJob = (string)(info.CustomProperties["room_job"]);
            int mapIdx = (int)(info.CustomProperties["map_idx"]);
            bool isPrivate = (bool)(info.CustomProperties["is_private"]);
            string roomCode = (string)(info.CustomProperties["room_code"]);

            //만들어진 roomItem 에서 RoomItem 컴포넌트 가져온다.
            KJY_RoomItem roomItem = goRoomItem.GetComponent<KJY_RoomItem>();
            //가져온 컴포넌트가 가지고 있는 SetInfo 함수 실행
            roomItem.SetInfo(roomName, roomMaster, roomMasterJob,info.PlayerCount, info.MaxPlayers, isPrivate, roomCode, mapIdx);
            //RoomItem 이 클릭 되었을 때 호출되는 함수 등록
            roomItem.onChangeRoomName = OnHandleRoom;
            InfoManagerKJY.instance.roomdata = roomItem;

            //roomList.Add(roomItem);
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

        print(roomList.Count);
        //전체 룸리스트UI 삭제
        RemoveRoomList();
        //내가 따로 관리하는 룸리스트 정보 갱신
        UpdateRoomList(roomList);
        //룸리스트 정보를 가지고 UI를 다시 생성
        CreateRoomList();
    }

    public void OnHandleRoom(string roomName, string roomCode, bool Private)
    {
        inputRoomName.text = roomName;
        nowRoomCode = roomCode;
        if (Private)
        {
            isPrivate = true;
        }
        else
        {
            isPrivate = false;
        }
    }

    //방 비밀번호 코드 생성
    public static string GenerateRoomCode(int length = 4)
    {
        char[] code = new char[length];
        System.Random random = new System.Random();

        for (int i = 0; i < length; i++)
        {
            code[i] = characters[random.Next(characters.Length)];
        }

        return new string(code);
    }

    public void OnRoomReset()
    {
        inputRoomName.text = null;
        isPrivate = false;
        checkImage.enabled = false;
    }
}
