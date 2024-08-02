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

    private string screteKey = "";

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

            masterNickname = PhotonNetwork.LocalPlayer.NickName;
            masterText.text = masterNickname;

        }
        else
        {
            readyButtonText.text = "Ready";
            readyButton.onClick.AddListener(OnReadyButtonClicked);

            masterNickname = PhotonNetwork.MasterClient.NickName;
            participantNickname = PhotonNetwork.LocalPlayer.NickName;
            participantText.text = participantNickname;
        }
        AssignInitialRoles();
    }

    public void OnReadyButtonClicked()
    {
        isReady = !isReady;

        photonView.RPC("SetPlayerReadyState", RpcTarget.MasterClient, PhotonNetwork.LocalPlayer, isReady);

        Debug.Log($"Setting ready state for {PhotonNetwork.LocalPlayer.NickName} to {isReady}");

        readyButtonText.text = isReady ? "Unready" : "Ready";
    }

    [PunRPC]
    public void SetPlayerReadyState(Player player, bool ready)
    {
        if (PhotonNetwork.IsMasterClient)
        {
            player.SetCustomProperties(new ExitGames.Client.Photon.Hashtable { { "IsReady", true } });
            Debug.Log($"SetPlayerReadyState called for {player.NickName} with ready state: {ready}");
            readyText.text = ready ? "" : "Ready";
            CheckAllPlayersReady();
        }
        else
        {
            readyText.text = ready ? "" : "Ready";
            Debug.LogWarning("SetPlayerReadyState was called, but this client is not the MasterClient.");
        }
    }

    private void OnStartGameButtonClicked()
    {
    //    if (PhotonNetwork.IsMasterClient)
    //    {
    //         CheckAllPlayersReady();
    //         if (AllPlayersReady())
    //         {
     //              // 시작 가능한 경우 게임 시작
                   photonView.RPC("StartGame", RpcTarget.All);
        //         }
        //         else
        // {
        // Debug.Log("모든 플레이어가 준비되지 않았습니다.");
        // }
        // }
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
        Debug.Log("게임 시작!");
        // 여기에서 실제 게임 시작 로직을 추가합니다.
        //Role currentRole = (Role)PhotonNetwork.LocalPlayer.CustomProperties["room_job"];
        //Role oppositeRole = GetOppositeRole(currentRole);
        //if (PhotonNetwork.IsMasterClient)
        //{
        //    InfoManagerKJY.instance.roomMasterRole = currentRole.ToString();
        //    InfoManagerKJY.instance.roomPartiRole = oppositeRole.ToString();
        //}
        //else
        //{
        //    InfoManagerKJY.instance.roomMasterRole = oppositeRole.ToString();
        //    InfoManagerKJY.instance.roomPartiRole = currentRole.ToString();
        //}

        //for (int i = 0; i < PhotonNetwork.PlayerList.Length; i++)
        //{
        //    print(PhotonNetwork.PlayerList[i].NickName + PhotonNetwork.PlayerList[i].CustomProperties["room_job"]);
        //}

        saveRole();
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
                    role = newMasterRole.ToString();

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


        AssignRolesToPlayer(PhotonNetwork.LocalPlayer);
    }

    public override void OnPlayerEnteredRoom(Player newPlayer)
    {
        Debug.Log($"{newPlayer.NickName} 입장했습니다!");

        participantNickname = newPlayer.NickName;
        participantText.text = participantNickname;

        // 방장 준비 상태 유지
        if (PhotonNetwork.IsMasterClient)
        {
            isReady = true;
            SetPlayerReadyState(PhotonNetwork.LocalPlayer, true);
            AssignRolesToPlayer(newPlayer);
        }
    }

    public override void OnPlayerLeftRoom(Player otherPlayer)
    {
        base.OnPlayerLeftRoom(otherPlayer);

        secondProfile.enabled = false;
        participantText.text = null;
        participantNickname = null;
        readyText.text = string.Empty;
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
        InfoManagerKJY.instance.roomMasterNickName = null;
        InfoManagerKJY.instance.roomPartiNickName = null;

        PhotonNetwork.LoadLevel(SceneName.Lobby_KJY.ToString());
    }

    public void LeaveRoom()
    {
        PhotonNetwork.LeaveRoom();
    }

    private void AssignInitialRoles()
    {
        foreach (var player in PhotonNetwork.PlayerList)
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
                Role oppositeRole = GetOppositeRole(currentMasterRole);
                player.SetCustomProperties(new ExitGames.Client.Photon.Hashtable { { "room_job", oppositeRole } });
                Debug.Log($"{player.NickName}는 입장하여 {oppositeRole} 역할을 맡습니다.");
                secondProfile.enabled = true;
                secondProfile.sprite = Resources.Load<Sprite>("RoomJobProfile/" + oppositeRole.ToString());
            }
        }
    }

    private void AssignRolesToPlayer(Player player)
    {
        if (player.IsMasterClient)
        {
            // 방장은 이미 역할이 부여되어 있으므로 할당 필요 없음
            Debug.Log($"{player.NickName}는 이미 방장으로 {currentMasterRole} 역할을 맡고 있습니다.");
            return;
        }

        // 반대 역할 부여
        //player.SetCustomProperties(new ExitGames.Client.Photon.Hashtable { { "Role", oppositeRole } });
        Role oppositeRole = GetOppositeRole(currentMasterRole);
        secondProfile.enabled = true;
        secondProfile.sprite = Resources.Load<Sprite>("RoomJobProfile/"+oppositeRole.ToString());
        //Debug.Log($"{player.NickName}는 {oppositeRole} 역할을 부여받았습니다.");
    }

    private Role GetOppositeRole(Role currentRole)
    {
        return currentRole == Role.Detective ? Role.Assistant : Role.Detective;
    }


    public override void OnMasterClientSwitched(Player newMasterClient)
    {
        // 방장이 변경되었을 때 호출됨
        Debug.Log($"새로운 방장이 되었습니다: {newMasterClient.NickName}");
        
        Role currentRole = (Role)PhotonNetwork.LocalPlayer.CustomProperties["room_job"];
        firstProfile.sprite = Resources.Load<Sprite>("RoomJobProfile/"+currentRole.ToString());
        masterText.text = newMasterClient.NickName;

        ExitGames.Client.Photon.Hashtable ht = PhotonNetwork.CurrentRoom.CustomProperties;
        screteKey = (string)ht["screteKey"];
        screteKeyText.text = screteKey;
        InfoManagerKJY.instance.roomIndex = (int)ht["roomIndex"];

        InfoManagerKJY.instance.roomMasterNickName = PhotonNetwork.LocalPlayer.NickName;


        ExitGames.Client.Photon.Hashtable properties = PhotonNetwork.CurrentRoom.CustomProperties;
        properties["room_master"] = PhotonNetwork.LocalPlayer.NickName;
        PhotonNetwork.CurrentRoom.SetCustomProperties(properties);

        if (PhotonNetwork.LocalPlayer.IsMasterClient)
        {
            // 새로운 방장으로서 역할 재할당
            AssignRolesToPlayer(PhotonNetwork.LocalPlayer);

        }
    }

    public void ChangeMasterRole()
    {
        if (PhotonNetwork.IsMasterClient)
        {
            // 방장만 역할을 변경할 수 있도록 허용
            currentMasterRole = GetOppositeRole(currentMasterRole);
            PhotonNetwork.LocalPlayer.SetCustomProperties(new ExitGames.Client.Photon.Hashtable { { "room_job", currentMasterRole } });
            Debug.Log($"방장의 역할이 {currentMasterRole}로 변경되었습니다.");

            readyButtonText.text = "Start Game";
            readyButton.onClick.AddListener(OnStartGameButtonClicked);

            // 모든 플레이어의 역할을 재할당
            foreach (var player in PhotonNetwork.PlayerList)
            {
                if (!player.IsMasterClient)
                {
                    AssignRolesToPlayer(player);
                }
            }
        }
    }

    // 커스텀 프로퍼티로 방장 정보 업데이트
    private void UpdateScreteKey()
    {
        RoomOptions option = new RoomOptions();
        screteKey = InfoManagerKJY.instance.secretKey;
        ExitGames.Client.Photon.Hashtable hash = new ExitGames.Client.Photon.Hashtable();

        hash["screteKey"] = screteKey;
        hash["roomIndex"] = InfoManagerKJY.instance.roomIndex;

        option.CustomRoomProperties = hash;
    }


}
