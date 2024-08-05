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

    [SerializeField] private PlayableDirector timeline;

    [SerializeField] private CinemachineBlendListCamera blendCamera;

    private bool isHeadmanSpeaking = false;

    private void Start()
    {
        // ���� ����
        AudioManager.Instnace.PlaySound(SoundList.Play_BG, 0.3f, 2f);

        // Ÿ�Ӷ��ο��� Ÿ�̹� �°� ����, ������ �� �͵��� �ڷ�ƾ���� ����
        StartCoroutine(CoSpawnDollAndDog());
        StartCoroutine(CoTurnOffTrafficLight());

        // ���� ī�޶� ����
        blendCamera.enabled = false;

        // �̺�Ʈ ����
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
        isHeadmanSpeaking = true;
    }

    private void Update()
    {
        if (isHeadmanSpeaking) // ����� ��ȭ UI ����
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


    private IEnumerator CoStartHeadmanScriptUI()
    {
        isHeadmanSpeaking = false; // ������Ʈ ������ �ѹ��� ȣ��Ǳ� ���ؼ� ������ bool ��
        for (int i = 0; i < talkList.Count; i++)
        {
            if(i == 1) // �ι�° ������ ī�޶� Blend�Ǹ鼭 �������� �̵�
            {
                blendCamera.enabled = true;
            }
            talkContent.text = talkList[i];
            yield return new WaitForSeconds(5f);
        }
        KJY_SceneManager.instance.ChangeScene(SceneName.GameScene_NPC_Random);
    }

}
