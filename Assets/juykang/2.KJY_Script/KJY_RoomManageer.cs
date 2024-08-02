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
     //              // ���� ������ ��� ���� ����
                   photonView.RPC("StartGame", RpcTarget.All);
        //         }
        //         else
        // {
        // Debug.Log("��� �÷��̾ �غ���� �ʾҽ��ϴ�.");
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
            Debug.Log("��� �÷��̾ �غ�Ǿ����ϴ�!");
        }
    }

    [PunRPC]
    private void StartGame()
    {
        Debug.Log("���� ����!");
        // ���⿡�� ���� ���� ���� ������ �߰��մϴ�.
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
                    role = newMasterRole.ToString();

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


        AssignRolesToPlayer(PhotonNetwork.LocalPlayer);
    }

    public override void OnPlayerEnteredRoom(Player newPlayer)
    {
        Debug.Log($"{newPlayer.NickName} �����߽��ϴ�!");

        participantNickname = newPlayer.NickName;
        participantText.text = participantNickname;

        // ���� �غ� ���� ����
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
                Role oppositeRole = GetOppositeRole(currentMasterRole);
                player.SetCustomProperties(new ExitGames.Client.Photon.Hashtable { { "room_job", oppositeRole } });
                Debug.Log($"{player.NickName}�� �����Ͽ� {oppositeRole} ������ �ý��ϴ�.");
                secondProfile.enabled = true;
                secondProfile.sprite = Resources.Load<Sprite>("RoomJobProfile/" + oppositeRole.ToString());
            }
        }
    }

    private void AssignRolesToPlayer(Player player)
    {
        if (player.IsMasterClient)
        {
            // ������ �̹� ������ �ο��Ǿ� �����Ƿ� �Ҵ� �ʿ� ����
            Debug.Log($"{player.NickName}�� �̹� �������� {currentMasterRole} ������ �ð� �ֽ��ϴ�.");
            return;
        }

        // �ݴ� ���� �ο�
        //player.SetCustomProperties(new ExitGames.Client.Photon.Hashtable { { "Role", oppositeRole } });
        Role oppositeRole = GetOppositeRole(currentMasterRole);
        secondProfile.enabled = true;
        secondProfile.sprite = Resources.Load<Sprite>("RoomJobProfile/"+oppositeRole.ToString());
        //Debug.Log($"{player.NickName}�� {oppositeRole} ������ �ο��޾ҽ��ϴ�.");
    }

    private Role GetOppositeRole(Role currentRole)
    {
        return currentRole == Role.Detective ? Role.Assistant : Role.Detective;
    }


    public override void OnMasterClientSwitched(Player newMasterClient)
    {
        // ������ ����Ǿ��� �� ȣ���
        Debug.Log($"���ο� ������ �Ǿ����ϴ�: {newMasterClient.NickName}");
        
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
            // ���ο� �������μ� ���� ���Ҵ�
            AssignRolesToPlayer(PhotonNetwork.LocalPlayer);

        }
    }

    public void ChangeMasterRole()
    {
        if (PhotonNetwork.IsMasterClient)
        {
            // ���常 ������ ������ �� �ֵ��� ���
            currentMasterRole = GetOppositeRole(currentMasterRole);
            PhotonNetwork.LocalPlayer.SetCustomProperties(new ExitGames.Client.Photon.Hashtable { { "room_job", currentMasterRole } });
            Debug.Log($"������ ������ {currentMasterRole}�� ����Ǿ����ϴ�.");

            readyButtonText.text = "Start Game";
            readyButton.onClick.AddListener(OnStartGameButtonClicked);

            // ��� �÷��̾��� ������ ���Ҵ�
            foreach (var player in PhotonNetwork.PlayerList)
            {
                if (!player.IsMasterClient)
                {
                    AssignRolesToPlayer(player);
                }
            }
        }
    }

    // Ŀ���� ������Ƽ�� ���� ���� ������Ʈ
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
