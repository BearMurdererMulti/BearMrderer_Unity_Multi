using Photon.Pun;
using System.Collections;
using System.Collections.Generic;
using UnityEngine;

public class KJY_InGamePhoton : MonoBehaviour
{
    [SerializeField] private Transform dollSpawn;
    [SerializeField] private Transform dogSpawn;
    private void Awake()
    {
        if (InfoManagerKJY.instance.role == "Detective")
        {
            PhotonNetwork.Instantiate("Doll", dollSpawn.position, dollSpawn.rotation);
        }
    }
}
