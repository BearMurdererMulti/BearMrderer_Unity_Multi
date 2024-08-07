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
        // 음악 실행
        AudioManager.Instnace.PlaySound(SoundList.Play_BG, 0.3f, 2f);

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
        photonView.RPC("SetTargetScript", RpcTarget.AllBuffered);
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
        doll = Resources.Load<GameObject>("CustomDoll");
        Instantiate(doll);
        doll.transform.position = new Vector3(-24.4f, 1.6f, -24.7f);
        doll.transform.eulerAngles = new Vector3(0, 0, 0.5f);

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
            yield return new WaitForSeconds(0.1f);
        }
        scriptIndex++;
        isActiveEnter = true;
    }
}
