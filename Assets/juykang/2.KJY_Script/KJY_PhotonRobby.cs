using Photon.Pun;
using Photon.Realtime;
using System.Collections;
using System.Collections.Generic;
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

    //�� ���� ��ư
    public Button btnCreateRoom;

    //RoomItem Prefab
    public GameObject roomItemFactory;
    //RoomListView -> Content -> RectTransform
    public RectTransform rtContent;

    //��й�ȣ ����
    private static readonly string characters = "ABCDEFGHIJKLMNOPQRSTUVWXYZ";

    //�� ���� ������ �ִ� Dictionary
    Dictionary<string, RoomInfo> roomCache = new Dictionary<string, RoomInfo>();

    private void Awake()
    {
        Instance = this;
    }

    void Start()
    {
        //�� ���� ��Ȱ��ȭ
        btnCreateRoom.interactable = false;
        //inputRoomName �� ������ ����� �� ȣ��Ǵ� �Լ�
        inputRoomName.onValueChanged.AddListener(OnValueChangedRoomName);
    }

    //���� & ���� ��ư�� ����
    void OnValueChangedRoomName(string room)
    {
        //���� ��ư Ȱ�� / ��Ȱ��
        btnCreateRoom.interactable = room.Length > 0;
    }

    public void IsRoomPrivate(bool isPrivates)
    {
        if (isPrivate == true)
        {
            print("bbbs");
            checkImage.enabled = false;
            isPrivate = false;
        }
        else
        {
            print("aaa");
            checkImage.enabled = true;
            isPrivate = true;
        }
    }

    public void CreateRoom()
    {
        //�� �ɼ��� ���� (�ִ� �ο�)
        RoomOptions option = new RoomOptions();
        //�� ��Ͽ� ���̰� �ϳ�? ���ϳ�?
        option.IsVisible = true;
        //�濡 ������ �� �ִ�? ����?
        option.IsOpen = true;
        option.MaxPlayers = 2;

        //custom ����
        ExitGames.Client.Photon.Hashtable hash = new ExitGames.Client.Photon.Hashtable();
        hash["room_name"] = inputRoomName.text;
        hash["map_idx"] = 2;
        hash["room_master"] = InfoManagerKJY.instance.nickname;
        hash["room_job"] = "Ž��";
        hash["room_code"] = "";
        hash["is_private"] = false;
        // ���� ����� ���� �Ӽ��� �� �ڵ带 �߰�
        if (isPrivate)
        {
            hash["is_private"] = true;
            hash["room_code"] = GenerateRoomCode();
        }

        //custom ������ option �� ����
        option.CustomRoomProperties = hash;

        //custom ������ Lobby ���� ����� �� �ְ� ����
        string[] customKeys = { "room_name", "room_master", "map_idx", "room_job", "room_code", "is_private"};
        option.CustomRoomPropertiesForLobby = customKeys;

        //Ư�� �κ� �� ���� ��û
        //TypedLobby typedLobby = new TypedLobby("Meta Lobby", LobbyType.Default);
        //PhotonNetwork.CreateRoom(inputRoomName.text, option, typedLobby);
        print("bssbs");
        //�⺻ �κ� �� ���� ��û
        PhotonNetwork.CreateRoom(inputRoomName.text, option);
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

    public void JoinRoomNotPrivate()
    {

        //�� ���� ��û
        PhotonNetwork.JoinRoom(inputRoomName.text);
    }

    public void JoinRoomPrivateSetting(string roomCode)
    {
        LoginSceneUI.instance.OnsecretKeyG();
        //�� ���� ��û
        nowRoomCode = roomCode;
    }

    public void JoinRoomPrivate()
    {
        if (LoginSceneUI.instance.secretKeyInput.text == nowRoomCode)
        {
            //�� ���� ��û
            PhotonNetwork.JoinRoom(inputRoomName.text);
            nowRoomCode = null;
        }
    }

    // �� ���� �Ϸ�� ȣ��Ǵ� �Լ�
    public override void OnJoinedRoom()
    {
        base.OnJoinedRoom();
        print("�� ���� �Ϸ�");
        print("�� ���� ���� : ");
        //GameScene ���� �̵�
        PhotonNetwork.LoadLevel("02_Room_KJY");
    }

    // �� ���� ���н� ȣ��Ǵ� �Լ�
    public override void OnJoinRoomFailed(short returnCode, string message)
    {
        base.OnJoinRoomFailed(returnCode, message);
        print("�� ���� ���� : ");
        print("�� ���� ���� : " + message);
    }

    void RemoveRoomList()
    {
        //rtContent �� �ִ� �ڽ� GameObject �� ��� ����
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
            //roomCache �� info �� ���̸� ���� �Ǿ��ִ� Key �� �����ϴ�?
            if (roomCache.ContainsKey(info.Name))
            {
                //���� �ؾ��ϴ�?
                if (info.RemovedFromList)
                {
                    roomCache.Remove(info.Name);
                }
                //����
                else
                {
                    roomCache[info.Name] = info;
                }
            }
            else
            {
                //�߰�
                roomCache[info.Name] = info;
            }
        }
    }

    void CreateRoomList()
    {
        foreach (RoomInfo info in roomCache.Values)
        {
            //roomItem prefab �� �̿��ؼ� roomItem �� �����. 
            GameObject goRoomItem = Instantiate(roomItemFactory, rtContent);
            ////������� roomItem �� �θ� scrollView -> Content �� transform ���� �Ѵ�.
            //goRoomItem.transform.parent = rtContent;

            //custom ���� �̾ƿ���.
            string roomName = (string)(info.CustomProperties["room_name"]);
            string roomMaster = (string)(info.CustomProperties["room_master"]);
            string roomMasterJob = (string)(info.CustomProperties["room_job"]);
            int mapIdx = (int)(info.CustomProperties["map_idx"]);
            bool isPrivate = (bool)(info.CustomProperties["is_pirvate"]);
            string roomCode = (string)(info.CustomProperties["room_code"]);

            //������� roomItem ���� RoomItem ������Ʈ �����´�.
            KJY_RoomItem roomItem = goRoomItem.GetComponent<KJY_RoomItem>();
            //������ ������Ʈ�� ������ �ִ� SetInfo �Լ� ����
            roomItem.SetInfo(roomName, roomMaster, roomMasterJob,info.PlayerCount, info.MaxPlayers, isPrivate, roomCode);
            //RoomItem �� Ŭ�� �Ǿ��� �� ȣ��Ǵ� �Լ� ���
            roomItem.onChangeRoomName = OnChangeRoomNameField;

            //���ٽ� lamda 
            //roomItem.onChangeRoomName = (string roomName) => {
            //    inputRoomName.text = roomName;
            //};
        }
    }

    // ������ ���� ����ų� ���������� ȣ��Ǵ� �Լ�
    public override void OnRoomListUpdate(List<RoomInfo> roomList)
    {
        base.OnRoomListUpdate(roomList);

        //��ü �븮��ƮUI ����
        RemoveRoomList();
        //���� ���� �����ϴ� �븮��Ʈ ���� ����
        UpdateRoomList(roomList);
        //�븮��Ʈ ������ ������ UI �� �ٽ� ����
        CreateRoomList();
    }

    public void OnChangeRoomNameField(string roomName)
    {
        inputRoomName.text = roomName;
    }

    //�� ��й�ȣ �ڵ� ����
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
