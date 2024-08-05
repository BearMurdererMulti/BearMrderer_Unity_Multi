using Cinemachine;
using System;
using System.Collections;
using System.Collections.Generic;
using TMPro;
using Unity.VisualScripting;
using UnityEngine;
using UnityEngine.Playables;
using UnityEngine.UI;

public class CinemachineManager : MonoBehaviour
{
    [SerializeField] private GameObject doll, dog;
    [SerializeField] private float setSpawnTime;

    [SerializeField] private List<Animator> trafficLightAnimators;
    [SerializeField] private float setTrafficLightTime;

    [SerializeField] private GameObject talkUi;
    [SerializeField] private TMP_Text talkContent;
    [SerializeField] private List<string> talkList;
    private int talkIndex = 1;
    private bool isTalking = false;

    [SerializeField] private PlayableDirector timeline;

    [SerializeField] private CinemachineBlendListCamera blendCamera;

    private bool isHeadmanSpeaking = false;

    private void Start()
    {
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
        Debug.Log("SubTimelineEvent called");
        Debug.Log($"Timeline duration: {timeline.duration} seconds");
        timeline.stopped += DissubTimelineEvent;
        timeline.Play();
        Debug.Log($"Timeline duration: {timeline.duration} seconds");

    }

    private void DissubTimelineEvent(PlayableDirector timeline)
    {
        Debug.Log($"{nameof(DissubTimelineEvent)}을 실행합니다.");
        Debug.Log($"DissubTimelineEvent called. Timeline state: {timeline.state}");
        timeline.stopped -= DissubTimelineEvent;
        ShowUI();
    }

    private void ShowUI()
    {
        talkUi.SetActive(true);
        isHeadmanSpeaking = true;
    }

    private void Update()
    {
        if (isHeadmanSpeaking) // 촌장님 대화 UI 시작
        {
            StartCoroutine(CoStartHeadmanScriptUI());
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


    public void StartTalking()
    {
        isTalking = true;
        talkUi.SetActive(true);
        talkContent.text = talkList[talkIndex];
        talkIndex++;
    }
    private void ShowNextTalkScript()
    {
        
    }

    private IEnumerator CoStartHeadmanScriptUI()
    {
        isHeadmanSpeaking = false; // 업데이트 문에서 한번만 호출되기 위해서 설정한 bool 값
        for (int i = 0; i < talkList.Count; i++)
        {
            if(i == 1) // 두번째 대사부터 카메라가 Blend되면서 촌장으로 이동
            {
                blendCamera.enabled = true;
            }
            talkContent.text = talkList[i];
            Debug.Log(talkContent.text);
            Debug.Log($"i값은 {i}입니다.");
            yield return new WaitForSeconds(5f);
        }
        KJY_SceneManager.instance.ChangeScene(SceneName.GameScene_NPC_Random);
    }

}
