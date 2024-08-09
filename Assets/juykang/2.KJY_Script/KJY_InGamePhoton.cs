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
        // �⺻������ �����տ� �����ϴ� ī�޶� ���� �������ּ���. -����ȯ-
        if (InfoManagerKJY.instance.role == "Detective")
        {
            GameObject doll = PhotonNetwork.Instantiate("Doll", dollSpawn.position, dollSpawn.rotation);
            GameObject camera = doll.transform.Find("DollMainCamera").gameObject;
            camera.SetActive(true);
        }
        else
        {
            GameObject dog = PhotonNetwork.Instantiate("Dog", dogSpawn.position, dogSpawn.rotation);
            GameObject camera = dog.transform.Find("DogMainCamera").gameObject;
            camera.SetActive(true);
        }
    }
}
