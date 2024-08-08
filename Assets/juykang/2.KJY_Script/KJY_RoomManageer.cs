using Photon.Pun;
using Photon.Realtime;
using System.Collections;
using System.Collections.Generic;
using TMPro;
using UnityEngine;
using UnityEngine.UI;

public class KJY_RoomManageer : MonoBehaviourPunCallbacks
{
    public static KJY_RoomManageer instance;

    public enum Role
    {
        Detective,
        Assistant
    }

    [Header("UI")]
    public Button changeBtn;

    public Button readyButton;
    public TextMeshProUGUI readyText;
    public TextMeshProUGUI readyButtonText;

    public Image firstProfile;
    public Image secondProfile;

    public TextMeshProUGUI masterText;
    public TextMeshProUGUI participantText;

    public TextMeshProUGUI screteKeyText;

    [Header("information")]
    private KJY_RoomItem roomItem;
    public string role;

    public string participantNickname;
    public string masterNickname;

    public bool isReady = false;

    [SerializeField] string screteKey = "";
    public bool playerReady = false;

    // 방장 역할 설정
    private Role currentMasterRole;

    private void Awake()
    {
        instance = this;
        firstProfile.enabled = false;
        secondProfile.enabled = false;
        masterText.text = string.Empty;
        participantText.text = string.Empty;
        readyButtonText.text = string.Empty;
        screteKeyText.text = string.Empty;
    }

    private void Start()
    {
        if (PhotonNetwork.LocalPlayer.IsMasterClient)
        {
            readyButtonText.text = "Start Game";
            readyButton.onClick.AddListener(OnStartGameButtonClicked);
            UpdateScreteKey();
        }
        else
        {
            readyButtonText.text = "Ready";
            readyText.text = "Not Ready";
            readyButton.onClick.AddListener(OnReadyButtonClicked);

            participantNickname = PhotonNetwork.LocalPlayer.NickName;
            participantText.text = participantNickname;
            changeBtn.interactable = false;
        }
        masterNickname = PhotonNetwork.MasterClient.NickName;
        masterText.text = masterNickname;
        AssignInitialRoles(PhotonNetwork.LocalPlayer);
    }

    public void OnReadyButtonClicked()
    {
        isReady = !isReady;

        if (!PhotonNetwork.IsMasterClient)
        {
            playerReady = isReady;
        }

        photonView.RPC("SetPlayerReadyState", RpcTarget.All, PhotonNetwork.LocalPlayer, isReady);
        photonView.RPC("SetPlayerReadyUI", RpcTarget.All, playerReady);

        Debug.Log($"Setting ready state for {PhotonNetwork.LocalPlayer.NickName} to {isReady}");
    }

    [PunRPC]
    private void SetPlayerReadyState(Player player, bool ready)
    {
        if (PhotonNetwork.IsMasterClient)
        {
            player.SetCustomProperties(new ExitGames.Client.Photon.Hashtable { { "IsReady", true } });
            Debug.Log($"SetPlayerReadyState called for {player.NickName} with ready state: {ready}");
            readyText.text = playerReady ? "Ready" : "Not Ready";
            CheckAllPlayersReady();
        }
        else
        {
            player.SetCustomProperties(new ExitGames.Client.Photon.Hashtable { { "IsReady", ready } });
        }
    }

    [PunRPC]
    private void SetPlayerReadyUI(bool ready)
    {
        foreach (var player in PhotonNetwork.PlayerList)
        {
            readyText.text = ready ? "Ready" : "Not Ready";
        }
    }

    private void OnStartGameButtonClicked()
    {
        if (PhotonNetwork.IsMasterClient)
        {
             CheckAllPlayersReady();
             if (AllPlayersReady())
             {
                   // 시작 가능한 경우 게임 시작
                 photonView.RPC("StartGame", RpcTarget.All);
                  if (PhotonNetwork.IsMasterClient)
                  {
                      saveRole();
                  }
             }
             else
             {
                Debug.Log("모든 플레이어가 준비되지 않았습니다.");
             }
         }
    }

    private bool AllPlayersReady()
    {
        foreach (var player in PhotonNetwork.PlayerList)
        {
            object isReady;
            if (player.CustomProperties.TryGetValue("IsReady", out isReady))
            {
                if (!(bool)isReady)
                {
                    return false;
                }
            }
            else
            {
                return false;
            }
        }
        return true;
    }

    private void CheckAllPlayersReady()
    {
        if (PhotonNetwork.IsMasterClient && AllPlayersReady())
        {
            Debug.Log("모든 플레이어가 준비되었습니다!");
        }
    }

    [PunRPC]
    private void StartGame()
    {
        Role currentRole = (Role)PhotonNetwork.LocalPlayer.CustomProperties["room_job"];
        InfoManagerKJY.instance.role = currentRole.ToString();
        Role oppositeRole = GetOppositeRole(currentRole);
        InfoManagerKJY.instance.roomPartiNickName = participantNickname;
        InfoManagerKJY.instance.roomMasterNickName = masterNickname;
        if (PhotonNetwork.IsMasterClient)
        {
            InfoManagerKJY.instance.roomMasterRole = currentRole.ToString();
            InfoManagerKJY.instance.roomPartiRole = oppositeRole.ToString();
        }
        else
        {
            InfoManagerKJY.instance.roomMasterRole = oppositeRole.ToString();
            InfoManagerKJY.instance.roomPartiRole = currentRole.ToString();
        }

        for (int i = 0; i < PhotonNetwork.PlayerList.Length; i++)
        {
            print(PhotonNetwork.PlayerList[i].NickName + PhotonNetwork.PlayerList[i].CustomProperties["room_job"]);
        }
    }

    public void saveRole()
    {
        //ConnectionKJY.instance.OnClickSendRoomSave(); //이거 나중에 해야함
        ConnectionKJY.instance.RequestGameSet();
    }

    // 방장만 역할을 변경할 수 있도록 제한
    public void OnChangeButtonClicked()
    {
        if (PhotonNetwork.IsMasterClient)
        {
            Role currentRole = (Role)PhotonNetwork.LocalPlayer.CustomProperties["room_job"];
            // 역할 교환을 위한 변수를 초기화
            Role newMasterRole = (currentRole == Role.Assistant) ? Role.Detective : Role.Assistant;

            // 방 정보 갱신
            ExitGames.Client.Photon.Hashtable properties = PhotonNetwork.CurrentRoom.CustomProperties;
            properties["room_job"] = newMasterRole == Role.Assistant ? "보조" : "탐정";
            PhotonNetwork.CurrentRoom.SetCustomProperties(properties);

            foreach (var player in PhotonNetwork.PlayerList)
            {
                // 방장이 플레이어
                if (player.IsMasterClient)
                {
                    // 방장의 역할을 변경
                    PhotonNetwork.LocalPlayer.SetCustomProperties(new ExitGames.Client.Photon.Hashtable { { "room_job", newMasterRole } });

                    // 첫 번째로 발견한 플레이어의 역할을 방장의 현재 역할로 변경
                    if (PhotonNetwork.CountOfPlayersInRooms >= 2)
                    {
                        player.SetCustomProperties(new ExitGames.Client.Photon.Hashtable { { "room_job", currentRole } });
                    }

                    // 변경된 역할을 로컬 변수에 저장
                    currentMasterRole = newMasterRole;
                    // 모든 클라이언트에게 역할 변경을 알림
                    photonView.RPC("UpdateRoleInRoom", RpcTarget.All, PhotonNetwork.LocalPlayer.NickName, newMasterRole);
                    if (PhotonNetwork.CountOfPlayersInRooms >= 2)
                    {
                        photonView.RPC("UpdateRoleInRoom", RpcTarget.All, player.NickName, currentRole);
                    }

                    // 루프를 나와서 다른 플레이어의 역할을 변경하지 않음
                    break;
                }
            }
        }
        else
        {
            changeBtn.interactable = false;
        }
    }

    [PunRPC]
    private void UpdateRoleInRoom(string playerName, Role newRole)
    {
        // 해당 플레이어의 역할을 화면에 업데이트
        foreach (var player in PhotonNetwork.PlayerList)
        {
            if (player.NickName == playerName)
            {
                player.SetCustomProperties(new ExitGames.Client.Photon.Hashtable { { "room_job", newRole } });

                if (newRole == Role.Detective)
                {
                    firstProfile.sprite = Resources.Load<Sprite>("RoomJobProfile/Detective");
                    secondProfile.sprite = Resources.Load<Sprite>("RoomJobProfile/Assistant");
                }
                else
                {
                    firstProfile.sprite = Resources.Load<Sprite>("RoomJobProfile/Assistant");
                    secondProfile.sprite = Resources.Load<Sprite>("RoomJobProfile/Detective");
                }

                break;
            }
            Debug.Log($"{player.NickName} is assigned the role: " + role);
        }
    }

    public override void OnJoinedRoom()
    {
        Debug.Log("룸에 입장했습니다!");
    }

    public override void OnPlayerEnteredRoom(Player newPlayer)
    {
        Debug.Log($"{newPlayer.NickName} 입장했습니다!");

        participantNickname = newPlayer.NickName;
        participantText.text = participantNickname;
        

        secondProfile.enabled = true;
        secondProfile.sprite = Resources.Load<Sprite>("RoomJobProfile/" + GetOppositeRole(currentMasterRole).ToString());

        // 방장 준비 상태 유지
        if (PhotonNetwork.IsMasterClient)
        {
            isReady = true;
            SetPlayerReadyState(PhotonNetwork.LocalPlayer, true);
        }
    }

    public override void OnPlayerLeftRoom(Player otherPlayer)
    {
        base.OnPlayerLeftRoom(otherPlayer);

        secondProfile.enabled = false;
        participantText.text = null;
        participantNickname = null;
        readyText.text = null;

        InfoManagerKJY.instance.roomPartiNickName = null;

        if (otherPlayer.IsMasterClient == false)
        {
          isReady = false;
          SetPlayerReadyState(PhotonNetwork.LocalPlayer, false);
        }
    }

    public override void OnLeftRoom()
    {
        base.OnLeftRoom();

        //InfoManagerKJY.instance.roomdata = null;
        OnClickConnect();
        InfoManagerKJY.instance.roomMasterNickName = null;
        InfoManagerKJY.instance.roomPartiNickName = null;
    }

    public void LeaveRoom()
    {
        PhotonNetwork.LeaveRoom();
    }

    private void AssignInitialRoles(Player player)
    {
        if (player.IsMasterClient)
        {
            // 방장 역할 설정
            currentMasterRole = Role.Detective; // 초기 방장 역할 설정
            player.SetCustomProperties(new ExitGames.Client.Photon.Hashtable { { "room_job", currentMasterRole } });
            Debug.Log($"{player.NickName}는 방장으로서 {currentMasterRole} 역할을 맡습니다.");
            firstProfile.enabled = true;
            firstProfile.sprite = Resources.Load<Sprite>("RoomJobProfile/Detective");
        }
        else
        {
            // 입장 플레이어에게 반대 역할 부여
            print("test" + currentMasterRole);
            Role currentMasterJob = (Role)PhotonNetwork.MasterClient.CustomProperties["room_job"];
            Role oppositeRole = GetOppositeRole(currentMasterJob);
            player.SetCustomProperties(new ExitGames.Client.Photon.Hashtable { { "room_job", oppositeRole } });
            firstProfile.enabled = true;
            firstProfile.sprite = Resources.Load<Sprite>("RoomJobProfile/" + currentMasterJob.ToString());
            secondProfile.enabled = true;
            secondProfile.sprite = Resources.Load<Sprite>("RoomJobProfile/" + oppositeRole.ToString());
            Debug.Log($"{player.NickName}는 입장하여 {oppositeRole} 역할을 맡습니다.");
        }
    }

    private Role GetOppositeRole(Role currentRole)
    {
        return currentRole == Role.Detective ? Role.Assistant : Role.Detective;
    }


    public override void OnMasterClientSwitched(Player newMasterClient)
    {
        // 방장이 변경되었을 때 호출됨
        Debug.Log($"새로운 방장이 되었습니다: {newMasterClient.NickName}");
        readyButtonText.text = "Start Game";
        readyButton.onClick.AddListener(OnStartGameButtonClicked);

        Role currentRole = (Role)PhotonNetwork.LocalPlayer.CustomProperties["room_job"];
        currentMasterRole = currentRole;
        firstProfile.sprite = Resources.Load<Sprite>("RoomJobProfile/"+currentRole.ToString());
        masterText.text = newMasterClient.NickName;

        ExitGames.Client.Photon.Hashtable ht = PhotonNetwork.CurrentRoom.CustomProperties;
        screteKey = (string)ht["room_code"];
        screteKeyText.text = screteKey;
        InfoManagerKJY.instance.roomIndex = 0;
        InfoManagerKJY.instance.roomMasterNickName = PhotonNetwork.LocalPlayer.NickName;


        ExitGames.Client.Photon.Hashtable properties = PhotonNetwork.CurrentRoom.CustomProperties;
        properties["room_master"] = PhotonNetwork.LocalPlayer.NickName;
        PhotonNetwork.CurrentRoom.SetCustomProperties(properties);

        secondProfile.enabled = false;
        participantText.text = null;
        participantNickname = null;
        readyText.text = null;

        InfoManagerKJY.instance.roomPartiNickName = null;
    }

    // 커스텀 프로퍼티로 방장 정보 업데이트
    private void UpdateScreteKey()
    {
        ExitGames.Client.Photon.Hashtable properties = PhotonNetwork.CurrentRoom.CustomProperties;
        screteKey = properties["room_code"].ToString();
        screteKeyText.text = screteKey;
    }

    public void OnClickConnect()
    {
        // 서버 접속 요청
        PhotonNetwork.ConnectUsingSettings();
    }

    public override void OnConnectedToMaster()
    {
        base.OnConnectedToMaster();

        //기본 로비 진입 요청
        PhotonNetwork.JoinLobby();
    }

    public override void OnJoinedLobby()
    {
        base.OnJoinedLobby();

        //로비 씬으로 이동
        string sceneName = SceneName.Lobby_KJY.ToString();
        PhotonNetwork.LoadLevel(sceneName);
    }

}
