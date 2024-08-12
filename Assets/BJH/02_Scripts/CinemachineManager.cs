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
        // ĳ���� ���� ��
        // Ŀ���ҿ� �°� ����
        this.doll = Instantiate(dollPrefab);
        var custom = doll.GetComponent<Doll>();
        var info = InfoManagerKJY.instance;
        Debug.Log(info.customDictionary["ears"]); // ��
        Debug.Log(info.CustomMaterialsAndMesh.bodyMaterials[info.customDictionary["body"]]); // ��

        // �� �� ����
        custom.bodyRenderer.material = info.CustomMaterialsAndMesh.bodyMaterials[info.customDictionary["body"]];
        custom.earsRenderer.material = info.CustomMaterialsAndMesh.bodyMaterials[info.customDictionary["body"]];
        custom.tailRenderer.material = info.CustomMaterialsAndMesh.bodyMaterials[info.customDictionary["body"]];

        Material[] eyesMaterialArray = custom.eyesRenderer.materials;
        eyesMaterialArray[0] = info.CustomMaterialsAndMesh.eyesMaterials[info.customDictionary["eyes"]];
        eyesMaterialArray[1] = info.CustomMaterialsAndMesh.eyesMaterials[info.customDictionary["eyes"]];
        custom.eyesRenderer.materials = eyesMaterialArray;

        Debug.Log("�Ƴ� " + info.CustomMaterialsAndMesh.eyesMaterials[info.customDictionary["eyes"]]);
        Debug.Log(custom.eyesRenderer.materials[1]);


        custom.earsRenderer.sharedMesh = info.CustomMaterialsAndMesh.earsMesh[info.customDictionary["ears"]];
        custom.mouthRenderer.material = info.CustomMaterialsAndMesh.mouthMaterials[info.customDictionary["mouth"]];
        custom.tailRenderer.sharedMesh = info.CustomMaterialsAndMesh.tailMesh[info.customDictionary["tail"]];
        doll.transform.position = new Vector3(-24.4f, 1.6f, -24.7f);
        doll.transform.eulerAngles = new Vector3(0f, 0f, 0.5f);
        doll.SetActive(false);

        // ���� ����
        AudioManager.Instnace.PlaySound(BGM_List.Play_BG, 0.3f, 2f);

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
        isActiveEnter = false;
        StartCoroutine(CoSpeakHeadmanScripts());
        //photonView.RPC("SetTargetScript", RpcTarget.AllBuffered);
    }

    private void Update()
    {
        // ��ȭ�� ����Ǵ� �߿�
        // ���� ���� + ���͸� ������ (���� ��ũ��Ʈ�� ã��(�޼���) Ÿ���� ȿ���� ���(�ڷ�ƾ))
        if(talkUi.activeSelf)
        {
            if(PhotonNetwork.IsMasterClient && isActiveEnter && Input.GetKeyDown(KeyCode.Return)) // Ŭ���̾�Ʈ�� ���� ġ�� ����!!
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
        // �ι�° ������ ī�޶� Blend�Ǹ鼭 �������� �̵�
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
