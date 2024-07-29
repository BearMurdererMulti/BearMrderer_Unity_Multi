using Photon.Pun;
using System.Collections;
using System.Collections.Generic;
using UnityEngine;
using UnityEngine.UI;

public class KJY_RoomManageer : MonoBehaviourPunCallbacks
{
    public enum Role
    {
        Detective,
        assistant
    }

    public Button changeBtn;
    public Image masterProfile;
    public Image notMasterProfile;

    // 방장만 역할을 변경할 수 있도록 제한
    public void SetRole(Role newRole)
    {
        if (PhotonNetwork.IsMasterClient)
        {
            ExitGames.Client.Photon.Hashtable hash = new ExitGames.Client.Photon.Hashtable();
            hash["roles"] = newRole;

            PhotonNetwork.CurrentRoom.SetCustomProperties(hash);

            photonView.RPC("UpdateRoleInRoom", RpcTarget.All, newRole);
        }
        else
        {
            changeBtn.interactable = false;
        }
    }

    [PunRPC]
    private void UpdateRoleInRoom(Role newRole)
    {
        InfoManagerKJY.instance.role = newRole.ToString();
        if (newRole == Role.Detective)
        {
            masterProfile.sprite = Resources.Load<Sprite>("NPC/npc01");
            notMasterProfile.sprite = Resources.Load<Sprite>("NPC/npc02");
        }
        else
        {
            masterProfile.sprite = Resources.Load<Sprite>("NPC/npc02");
            notMasterProfile.sprite = Resources.Load<Sprite>("NPC/npc01");
        }
    }
}
