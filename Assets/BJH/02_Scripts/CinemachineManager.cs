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
    private int talkIndex = 0;
    private bool isTalking = false;

    [SerializeField] private PlayableDirector timeline;

    private void Start()
    {
        // 타임라인에서 타이밍 맞게 시작, 끝내야 할 것들을 코루틴으로 구현
        StartCoroutine(CoSpawnDollAndDog());
        StartCoroutine(CoTurnOffTrafficLight());

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
    }

    private void Update()
    {
        if(talkUi.activeSelf && Input.GetKeyDown(KeyCode.KeypadEnter) && Input.GetKeyDown(KeyCode.K))
        {
            ShowNextTalkScript();
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
        foreach(Animator animator in trafficLightAnimators)
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
        talkContent.text = talkList[talkIndex];
        talkIndex++;
    }
}
