using Cinemachine;
using Photon.Pun;
using System;
using System.Collections;
using System.Collections.Generic;
using TMPro;
using Unity.VisualScripting;
using UnityEngine;
using UnityEngine.EventSystems;
using UnityEngine.Playables;
using UnityEngine.UI;

public class CinemachineManager : MonoBehaviourPunCallbacks
{
    [SerializeField] GameObject dollPrefab;
    [SerializeField] public GameObject doll, dog; 
    [SerializeField] private float setSpawnTime;

    [SerializeField] private List<Animator> trafficLightAnimators;
    [SerializeField] private float setTrafficLightTime;

    [SerializeField] private GameObject talkUi;
    [SerializeField] private TMP_Text talkContent;
    [SerializeField] private List<string> talkList;

    [SerializeField] private PlayableDirector timeline;

    [SerializeField] private CinemachineBlendListCamera blendCamera;

    private bool isActiveEnter = false;
    private int scriptIndex = 0;


    private void Start()
    {
        // 캐릭터 스폰 후
        // 커스텀에 맞게 적용
        this.doll = Instantiate(dollPrefab);
        var custom = doll.GetComponent<Doll>();
        var info = InfoManagerKJY.instance;
        Debug.Log(info.customDictionary["ears"]); // 뜸
        Debug.Log(info.CustomMaterialsAndMesh.bodyMaterials[info.customDictionary["body"]]); // 뜸

        // 몸 색 설정
        custom.bodyRenderer.material = info.CustomMaterialsAndMesh.bodyMaterials[info.customDictionary["body"]];
        custom.earsRenderer.material = info.CustomMaterialsAndMesh.bodyMaterials[info.customDictionary["body"]];
        custom.tailRenderer.material = info.CustomMaterialsAndMesh.bodyMaterials[info.customDictionary["body"]];

        Material[] eyesMaterialArray = custom.eyesRenderer.materials;
        eyesMaterialArray[0] = info.CustomMaterialsAndMesh.eyesMaterials[info.customDictionary["eyes"]];
        eyesMaterialArray[1] = info.CustomMaterialsAndMesh.eyesMaterials[info.customDictionary["eyes"]];
        custom.eyesRenderer.materials = eyesMaterialArray;

        Debug.Log("됐냐 " + info.CustomMaterialsAndMesh.eyesMaterials[info.customDictionary["eyes"]]);
        Debug.Log(custom.eyesRenderer.materials[1]);


        custom.earsRenderer.sharedMesh = info.CustomMaterialsAndMesh.earsMesh[info.customDictionary["ears"]];
        custom.mouthRenderer.material = info.CustomMaterialsAndMesh.mouthMaterials[info.customDictionary["mouth"]];
        custom.tailRenderer.sharedMesh = info.CustomMaterialsAndMesh.tailMesh[info.customDictionary["tail"]];
        doll.transform.position = new Vector3(-24.4f, 1.6f, -24.7f);
        doll.transform.eulerAngles = new Vector3(0f, 0f, 0.5f);
        doll.SetActive(false);

        // 음악 실행
        AudioManager.Instnace.PlaySound(BGM_List.Play_BG, 0.3f, 2f);

        // 타임라인에서 타이밍 맞게 시작, 끝내야 할 것들을 코루틴으로 구현
        StartCoroutine(CoSpawnDollAndDog());
        StartCoroutine(CoTurnOffTrafficLight());

        // 블랜드 카메라 끄기
        blendCamera.enabled = false;

        // 이벤트 구독
        SubTimelineEvent();
    }

    private void SubTimelineEvent()
    {
        timeline.stopped += DissubTimelineEvent;
        timeline.Play();
    }

    private void DissubTimelineEvent(PlayableDirector timeline)
    {
        timeline.stopped -= DissubTimelineEvent;
        ShowUI();
    }

    private void ShowUI()
    {
        talkUi.SetActive(true);
        isActiveEnter = false;
        StartCoroutine(CoSpeakHeadmanScripts());
        //photonView.RPC("SetTargetScript", RpcTarget.AllBuffered);
    }

    private void Update()
    {
        // 대화가 진행되는 중에
        // 엔터 가능 + 엔터를 누르면 (다음 스크립트를 찾아(메서드) 타이핑 효과로 출력(코루틴))
        if(talkUi.activeSelf)
        {
            if(PhotonNetwork.IsMasterClient && isActiveEnter && Input.GetKeyDown(KeyCode.Return)) // 클라이언트만 엔터 치기 가능!!
            {
                photonView.RPC("SetTargetScript", RpcTarget.AllBuffered);
            }
        }
    }


    IEnumerator CoSpawnDollAndDog()
    {
        yield return new WaitForSeconds(setSpawnTime);
        doll.SetActive(true);
        dog.SetActive(true);
    }

    IEnumerator CoTurnOffTrafficLight()
    {
        yield return new WaitForSeconds(setTrafficLightTime);
        foreach (Animator animator in trafficLightAnimators)
        {
            animator.SetBool("isExit", true);
        }
    }

    [PunRPC]
    private void SetTargetScript()
    {
        isActiveEnter = false;
        // 두번째 대사부터 카메라가 Blend되면서 촌장으로 이동
        if (scriptIndex == 1)
        {
            blendCamera.enabled = true;
        }

        if(scriptIndex >= talkList.Count)
        {
            PhotonConnection.Instance.InGameResponsePhoton();
            return;
        }

        StartCoroutine(CoSpeakHeadmanScripts());
    }


    private IEnumerator CoSpeakHeadmanScripts()
    {
        talkContent.text = "";

        string script = talkList[scriptIndex];
        foreach (char c in script)
        {
            talkContent.text += c;
            yield return new WaitForSeconds(0.03f);
        }
        scriptIndex++;
        isActiveEnter = true;
    }
}
