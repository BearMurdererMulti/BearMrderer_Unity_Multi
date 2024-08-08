using Photon.Pun;
using System.Collections;
using System.Collections.Generic;
using UnityEngine;
using UnityEngine.UI;

public class CharacterCustomManager : MonoBehaviour
{
   [SerializeField] private List<Button> bodyButtons = new List<Button>();

    [SerializeField] private CharacterCustom_new characterCustom;

    // 플레이어 회전
    [SerializeField] private Transform playerTransform; // 캐릭터 Transform
    private float rotationDuration; // 회전하는 시간
    private Vector3 targetRotationEuler; // 목표 회전값

    [SerializeField] private GameObject doll, dog;
    [SerializeField] private GameObject dollCanvas, dogCanvas;

    private void Awake()
    {
        doll.SetActive(false);
        dog.SetActive(false);
        dollCanvas.SetActive(false);
        dogCanvas.SetActive(false);
    }

    private void Start()
    {
        // 탐정, 강아지별로 다른 캔버스 활성화
        if(PhotonNetwork.IsMasterClient)
        {
            doll.SetActive(true);
            dollCanvas.SetActive(true);
        }
        else
        {
            dog.SetActive(true);
            dogCanvas.SetActive(true);
        }
    }


    public void OnClickCustomButton(string keyName)
    {
        int buttonNumber = int.Parse(UnityEngine.EventSystems.EventSystem.current.currentSelectedGameObject.name);
        characterCustom.SetCustomDictionary(keyName, buttonNumber);
    }

    public void OnClickCategory(int index)
    {
        // 꼬리 인덱스는 캐릭터 뒤로 돌리기
        if(index == 4)
        {
            rotationDuration = 1f;
            targetRotationEuler = new Vector3(0, 350, 0);
            StartCoroutine(CoRotateCharater(targetRotationEuler, rotationDuration));
        }
        else
        {
            rotationDuration = 1f;
            targetRotationEuler = new Vector3(0, 150, 0);
            StartCoroutine(CoRotateCharater(targetRotationEuler, rotationDuration));
        }
        characterCustom.ActiveCategoryContent(index);
    }

    IEnumerator CoRotateCharater(Vector3 targetRotation, float duration)
    {
        Quaternion initialRotation = playerTransform.rotation; //초기 회전값
        Quaternion finalRotation = Quaternion.Euler(targetRotation); // 목표 회전값

        float time = 0f;

        while(time < duration)
        {
            time += Time.deltaTime;

            playerTransform.rotation = Quaternion.Slerp(initialRotation, finalRotation, time/duration);

            // 한 프레임 대기
            yield return null;
        }

        playerTransform.rotation = finalRotation;
    }

}
