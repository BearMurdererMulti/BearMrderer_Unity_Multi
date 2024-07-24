using Photon.Pun;
using System.Collections;
using System.Collections.Generic;
using UnityEngine;

public class KJY_PhotonRoom : MonoBehaviourPunCallbacks
{
    private const string PlayerReadyProperty = "IsReady";

    // Call this function to toggle the player's ready state
    public void ToggleReady()
    {
        bool isReady = GetIsReady();
        SetIsReady(!isReady);
    }

    // Check if the player is ready
    private bool GetIsReady()
    {
        if (PhotonNetwork.LocalPlayer.CustomProperties.TryGetValue(PlayerReadyProperty, out object isReady))
        {
            return (bool)isReady;
        }
        return false;
    }

    // Set the player's ready state
    private void SetIsReady(bool ready)
    {
        Hashtable properties = new Hashtable
        {
            { PlayerReadyProperty, ready }
        };
        //PhotonNetwork.LocalPlayer.SetCustomProperties(properties);

        Debug.Log($"Player {PhotonNetwork.LocalPlayer.NickName} is now " + (ready ? "Ready" : "Not Ready"));
    }
}
