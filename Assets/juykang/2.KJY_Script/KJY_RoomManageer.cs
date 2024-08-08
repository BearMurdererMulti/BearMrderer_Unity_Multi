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

    // ���� ���� ����
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
                   // ���� ������ ��� ���� ����
                 photonView.RPC("StartGame", RpcTarget.All);
                  if (PhotonNetwork.IsMasterClient)
                  {
                      saveRole();
                  }
             }
             else
             {
                Debug.Log("��� �÷��̾ �غ���� �ʾҽ��ϴ�.");
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
            Debug.Log("��� �÷��̾ �غ�Ǿ����ϴ�!");
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
        //ConnectionKJY.instance.OnClickSendRoomSave(); //�̰� ���߿� �ؾ���
        ConnectionKJY.instance.RequestGameSet();
    }

    // ���常 ������ ������ �� �ֵ��� ����
    public void OnChangeButtonClicked()
    {
        if (PhotonNetwork.IsMasterClient)
        {
            Role currentRole = (Role)PhotonNetwork.LocalPlayer.CustomProperties["room_job"];
            // ���� ��ȯ�� ���� ������ �ʱ�ȭ
            Role newMasterRole = (currentRole == Role.Assistant) ? Role.Detective : Role.Assistant;

            // �� ���� ����
            ExitGames.Client.Photon.Hashtable properties = PhotonNetwork.CurrentRoom.CustomProperties;
            properties["room_job"] = newMasterRole == Role.Assistant ? "����" : "Ž��";
            PhotonNetwork.CurrentRoom.SetCustomProperties(properties);

            foreach (var player in PhotonNetwork.PlayerList)
            {
                // ������ �÷��̾�
                if (player.IsMasterClient)
                {
                    // ������ ������ ����
                    PhotonNetwork.LocalPlayer.SetCustomProperties(new ExitGames.Client.Photon.Hashtable { { "room_job", newMasterRole } });

                    // ù ��°�� �߰��� �÷��̾��� ������ ������ ���� ���ҷ� ����
                    if (PhotonNetwork.CountOfPlayersInRooms >= 2)
                    {
                        player.SetCustomProperties(new ExitGames.Client.Photon.Hashtable { { "room_job", currentRole } });
                    }

                    // ����� ������ ���� ������ ����
                    currentMasterRole = newMasterRole;
                    // ��� Ŭ���̾�Ʈ���� ���� ������ �˸�
                    photonView.RPC("UpdateRoleInRoom", RpcTarget.All, PhotonNetwork.LocalPlayer.NickName, newMasterRole);
                    if (PhotonNetwork.CountOfPlayersInRooms >= 2)
                    {
                        photonView.RPC("UpdateRoleInRoom", RpcTarget.All, player.NickName, currentRole);
                    }

                    // ������ ���ͼ� �ٸ� �÷��̾��� ������ �������� ����
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
        // �ش� �÷��̾��� ������ ȭ�鿡 ������Ʈ
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
        Debug.Log("�뿡 �����߽��ϴ�!");
    }

    public override void OnPlayerEnteredRoom(Player newPlayer)
    {
        Debug.Log($"{newPlayer.NickName} �����߽��ϴ�!");

        participantNickname = newPlayer.NickName;
        participantText.text = participantNickname;
        

        secondProfile.enabled = true;
        secondProfile.sprite = Resources.Load<Sprite>("RoomJobProfile/" + GetOppositeRole(currentMasterRole).ToString());

        // ���� �غ� ���� ����
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
            // ���� ���� ����
            currentMasterRole = Role.Detective; // �ʱ� ���� ���� ����
            player.SetCustomProperties(new ExitGames.Client.Photon.Hashtable { { "room_job", currentMasterRole } });
            Debug.Log($"{player.NickName}�� �������μ� {currentMasterRole} ������ �ý��ϴ�.");
            firstProfile.enabled = true;
            firstProfile.sprite = Resources.Load<Sprite>("RoomJobProfile/Detective");
        }
        else
        {
            // ���� �÷��̾�� �ݴ� ���� �ο�
            print("test" + currentMasterRole);
            Role currentMasterJob = (Role)PhotonNetwork.MasterClient.CustomProperties["room_job"];
            Role oppositeRole = GetOppositeRole(currentMasterJob);
            player.SetCustomProperties(new ExitGames.Client.Photon.Hashtable { { "room_job", oppositeRole } });
            firstProfile.enabled = true;
            firstProfile.sprite = Resources.Load<Sprite>("RoomJobProfile/" + currentMasterJob.ToString());
            secondProfile.enabled = true;
            secondProfile.sprite = Resources.Load<Sprite>("RoomJobProfile/" + oppositeRole.ToString());
            Debug.Log($"{player.NickName}�� �����Ͽ� {oppositeRole} ������ �ý��ϴ�.");
        }
    }

    private Role GetOppositeRole(Role currentRole)
    {
        return currentRole == Role.Detective ? Role.Assistant : Role.Detective;
    }


    public override void OnMasterClientSwitched(Player newMasterClient)
    {
        // ������ ����Ǿ��� �� ȣ���
        Debug.Log($"���ο� ������ �Ǿ����ϴ�: {newMasterClient.NickName}");
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

    // Ŀ���� ������Ƽ�� ���� ���� ������Ʈ
    private void UpdateScreteKey()
    {
        ExitGames.Client.Photon.Hashtable properties = PhotonNetwork.CurrentRoom.CustomProperties;
        screteKey = properties["room_code"].ToString();
        screteKeyText.text = screteKey;
    }

    public void OnClickConnect()
    {
        // ���� ���� ��û
        PhotonNetwork.ConnectUsingSettings();
    }

    public override void OnConnectedToMaster()
    {
        base.OnConnectedToMaster();

        //�⺻ �κ� ���� ��û
        PhotonNetwork.JoinLobby();
    }

    public override void OnJoinedLobby()
    {
        base.OnJoinedLobby();

        //�κ� ������ �̵�
        string sceneName = SceneName.Lobby_KJY.ToString();
        PhotonNetwork.LoadLevel(sceneName);
    }

}
