using Photon.Pun;
using System.Collections;
using System.Collections.Generic;
using TMPro;
using Unity.VisualScripting;
using UnityEngine;
using UnityEngine.Playables;
using UnityEngine.UI;
using static Photon.Voice.OpusCodec;

public class ReceiveLetterUI : MonoBehaviourPunCallbacks
{
    [Header("UI")]
    [SerializeField] private GameObject letterBG;
    [SerializeField] private TMP_Text senderText, contentText, receiverText;
    [SerializeField] private TMP_Text pressEnterMasterText01, pressEnterMasterText02;
    [SerializeField] private TMP_Text pressEnterClientText01, pressEnterClientText02;
    
    private TMP_Text targetText;
    private string content;
    private bool isDone;

    [SerializeField] private float durationTime;

    [SerializeField] private PlayableDirector timeline;

    private bool isReadyClient;

    private Coroutine _coroutine;

    // Start is called before the first frame update
    void Start()
    {
        // �⺻ �ؽ�Ʈ ����
        pressEnterMasterText01.enabled = false;
        pressEnterMasterText02.enabled = false;
        pressEnterClientText01.enabled = false;
        pressEnterClientText02.enabled = false;

        // ���� ��� ����
        letterBG.SetActive(false);

        // ��� �� �޾ƿ���
        content = InfoManagerKJY.instance.content;
        receiverText.text = InfoManagerKJY.instance.closing;
        senderText.text = InfoManagerKJY.instance.greeting;

        // Ÿ�Ӷ��� ������ �̺�Ʈ ���
        SubTimelineEvent();
    }

    void Update()
    {
        // ������ �� ��
        if(PhotonNetwork.IsMasterClient)
        {
            if(isReadyClient)
            {
                pressEnterMasterText01.enabled = false;
                pressEnterMasterText02.enabled = true;
            }
            if (isDone && Input.GetKeyDown(KeyCode.Return) && isReadyClient)
            {
                photonView.RPC("NextScene", RpcTarget.All);
            }
        }
        // Ŭ���̾�Ʈ �� ��
        else
        {
            if (isDone && Input.GetKeyDown(KeyCode.Return))
            {
                pressEnterClientText01.enabled = false;
                pressEnterClientText02.enabled = true;
                photonView.RPC("SendRead", RpcTarget.All);
            }
        }

        // ������ ��� �ۼ����� �ʾ��� �� ���͸� ������
        // Ÿ����ȿ�� �ʱ�ȭ
        if(!isDone && Input.GetKeyDown(KeyCode.Return))
        {
            StopCoroutine(_coroutine);
            contentText.text = content;
            if (PhotonNetwork.IsMasterClient)
            {
                pressEnterMasterText01.enabled = true;
            }
            else
            {
                pressEnterClientText01.enabled = true;
            }
            isDone = true;
        }

    }

    [PunRPC]
    private void SendRead()
    {
        isReadyClient = true;
    }

    [PunRPC]
    private void NextScene()
    {
        KJY_SceneManager.instance.ChangeScene(SceneName.CharacterCustom_new);
    }

    private void SubTimelineEvent()
    {
        timeline.stopped += DissubTimelineEvent;
        timeline.Play();
    }

    private void DissubTimelineEvent(PlayableDirector timeline)
    {
        timeline.stopped -= DissubTimelineEvent;
        _coroutine = StartCoroutine(CoTyping());
    }

    IEnumerator CoTyping()
    {
        letterBG.SetActive(true);

        yield return null;
        
        for(int i =  0; i < content.Length; i++)
        {
            if(isDone)
            {
                contentText.text = content;
                isDone = false;
                break;
            }
            contentText.text = content.Substring(0, i+1);
            yield return new WaitForSeconds(0.06f);
        }
        isDone = true;

        if (PhotonNetwork.IsMasterClient)
        {
            pressEnterMasterText01.enabled = true;
        }
        else
        {
            pressEnterClientText01.enabled = true;
        }
        yield return null;
    }
}
