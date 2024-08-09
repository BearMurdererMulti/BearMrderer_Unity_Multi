using Photon.Pun;
using System.Collections;
using System.Collections.Generic;
using UnityEngine;
using UnityEngine.Rendering.PostProcessing;

public class KJY_InGamePhoton : MonoBehaviour
{
    [SerializeField] private Transform dollSpawn;
    [SerializeField] private Transform dogSpawn;
    private void Awake()
    {
        // 기본적으로 프리팹에 존재하는 카메라를 끄고 시작해주세요. -변지환-
        // 기본적으로 프리팹에 존재하는 canvas는 끄고 시작해주세요. -변지환-
        if (InfoManagerKJY.instance.role == "Detective")
        {
            GameObject doll = PhotonNetwork.Instantiate("Doll", dollSpawn.position, dollSpawn.rotation);
            GameObject camera = doll.transform.Find("DollMainCamera").gameObject;
            camera.SetActive(true);
            GameObject canvas = doll.transform.Find("DollCanvas").gameObject;
            canvas.SetActive(true);
        }
        else
        {
            GameObject dog = PhotonNetwork.Instantiate("Dog", dogSpawn.position, dogSpawn.rotation);
            GameObject camera = dog.transform.Find("DogMainCamera").gameObject;
            camera.SetActive(true);
            GameObject canvas = dog.transform.Find("DogCanvas").gameObject;
            canvas.SetActive(true);
        }
    }
}
