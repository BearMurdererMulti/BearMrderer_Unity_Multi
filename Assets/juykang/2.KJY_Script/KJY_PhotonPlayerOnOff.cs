using System.Collections;
using System.Collections.Generic;
using UnityEngine;

public class KJY_PhotonPlayerOnOff : MonoBehaviour
{
    [SerializeField]private GameObject detective;
    [SerializeField]private GameObject assistant;

    private void Awake()
    {
        detective = transform.GetChild(0).gameObject;
        assistant = transform.GetChild(1).gameObject;
        if (InfoManagerKJY.instance.role == "Detective")
        {
            detective.SetActive(true);
        }
        else
        {
            assistant.SetActive(true);
        }
    }
}
