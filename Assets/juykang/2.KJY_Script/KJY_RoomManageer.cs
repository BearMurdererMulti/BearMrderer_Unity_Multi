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

    public Button changeBtn;

    public Image firstProfile;
    public Image secondProfile;

    public string role;

    public Button readyButton;
    public TextMeshProUGUI readyButtonText;

    public string participantNickname;
    public string masterNickname;

    public bool isReady = false;

    // ���� ���� ����
    private Role currentMasterRole = Role.Detective;

    private void Awake()
    {
        instance = this;
        firstProfile.enabled = false;
        secondProfile.enabled = false;
    }

    private void Start()
    {
        if (PhotonNetwork.LocalPlayer.IsMasterClient)
        {
            readyButtonText.text = "Start Game";
            readyButton.onClick.AddListener(OnStartGameButtonClicked);
            masterNickname = PhotonNetwork.MasterClient.NickName;
        }
        else
        {
            readyButtonText.text = "Ready";
            readyButton.onClick.AddListener(OnReadyButtonClicked);
        }
        AssignInitialRoles();
    }

    public void OnReadyButtonClicked()
    {
        isReady = !isReady;
        print(PhotonNetwork.LocalPlayer);

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
            CheckAllPlayersReady();
        }
        else
        {
            Debug.LogWarning("SetPlayerReadyState was called, but this client is not the MasterClient.");
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
        // ���⿡�� ���� ���� ���� ������ �߰��մϴ�.
        Debug.Log("���� ����!");
        saveRole();
    }

    public void saveRole()
    {
        InfoManagerKJY.instance.role = role;
        ConnectionKJY.instance.OnClickSendRoomSave();
    }

    // ���常 ������ ������ �� �ֵ��� ����
    public void OnChangeButtonClicked()
    {
        if (PhotonNetwork.IsMasterClient)
        {
            Role currentRole = (Role)PhotonNetwork.LocalPlayer.CustomProperties["Role"];
            print(currentRole.ToString());

            // ���� ��ȯ�� ���� ������ �ʱ�ȭ
            Role newMasterRole = (currentRole == Role.Assistant) ? Role.Detective : Role.Assistant;

            foreach (var player in PhotonNetwork.PlayerList)
            {
                // ������ �÷��̾�
                if (player.IsMasterClient)
                {
                    // ������ ������ ����
                    PhotonNetwork.LocalPlayer.SetCustomProperties(new ExitGames.Client.Photon.Hashtable { { "Role", newMasterRole } });

                    // ù ��°�� �߰��� �÷��̾��� ������ ������ ���� ���ҷ� ����
                    if (PhotonNetwork.CountOfPlayersInRooms >= 2)
                    {
                        player.SetCustomProperties(new ExitGames.Client.Photon.Hashtable { { "Role", currentRole } });
                    }

                    // ���ҿ� ���� UI ������Ʈ
                    if (newMasterRole == Role.Detective)
                    {
                        firstProfile.sprite = Resources.Load<Sprite>("RoomJobProfile/Detective");
                        secondProfile.sprite = Resources.Load<Sprite>("RoomJobProfile/Assistant");
                    }
                    else
                    {
                        firstProfile.sprite = Resources.Load<Sprite>("RoomJobProfile/Assistant");
                        secondProfile.sprite = Resources.Load<Sprite>("RoomJobProfile/Detective");
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
                player.SetCustomProperties(new ExitGames.Client.Photon.Hashtable { { "Role", newRole } });
                
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

        // ���� �غ� ���� ����
        if (PhotonNetwork.IsMasterClient)
        {
            isReady = true;
            SetPlayerReadyState(PhotonNetwork.LocalPlayer, true);
            AssignRolesToPlayer(newPlayer);
        }
    }

    private void AssignInitialRoles()
    {
        foreach (var player in PhotonNetwork.PlayerList)
        {
            if (player.IsMasterClient)
            {
                // ���� ���� ����
                currentMasterRole = Role.Detective; // �ʱ� ���� ���� ����
                player.SetCustomProperties(new ExitGames.Client.Photon.Hashtable { { "Role", currentMasterRole } });
                Debug.Log($"{player.NickName}�� �������μ� {currentMasterRole} ������ �ý��ϴ�.");
                firstProfile.enabled = true;
                firstProfile.sprite = Resources.Load<Sprite>("RoomJobProfile/Detective");
            }
            else
            {
                // ���� �÷��̾�� �ݴ� ���� �ο�
                Role oppositeRole = GetOppositeRole(currentMasterRole);
                player.SetCustomProperties(new ExitGames.Client.Photon.Hashtable { { "Role", oppositeRole } });
                Debug.Log($"{player.NickName}�� �����Ͽ� {oppositeRole} ������ �ý��ϴ�.");
                secondProfile.enabled = true;
                secondProfile.sprite = Resources.Load<Sprite>("RoomJobProfile/{currentRole}");
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
        Role oppositeRole = GetOppositeRole(currentMasterRole);
        player.SetCustomProperties(new ExitGames.Client.Photon.Hashtable { { "Role", oppositeRole } });
        Debug.Log($"{player.NickName}�� {oppositeRole} ������ �ο��޾ҽ��ϴ�.");
    }

    private Role GetOppositeRole(Role currentRole)
    {
        return currentRole == Role.Detective ? Role.Assistant : Role.Detective;
    }


    public override void OnMasterClientSwitched(Player newMasterClient)
    {
        // ������ ����Ǿ��� �� ȣ���
        Debug.Log($"���ο� ������ �Ǿ����ϴ�: {newMasterClient.NickName}");
        secondProfile.enabled = false;

        Role currentRole = (Role)PhotonNetwork.LocalPlayer.CustomProperties["Role"];
        firstProfile.sprite = Resources.Load<Sprite>("RoomJobProfile/{currentRole}");

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
            PhotonNetwork.LocalPlayer.SetCustomProperties(new ExitGames.Client.Photon.Hashtable { { "Role", currentMasterRole } });
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

}
