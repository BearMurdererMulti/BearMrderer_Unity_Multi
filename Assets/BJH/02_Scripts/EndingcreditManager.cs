using System;
using System.Collections;
using System.Collections.Generic;
using TMPro;
using UnityEditor;
using UnityEngine;
using UnityEngine.Networking;

#region Request and Responze

[Serializable]
public class EndingLetterRequest
{
    public long gameSetNo;
}

[Serializable]
public class EndingLetterResponse
{
    public string resultCode;
    public EndingLetterMessage message;
}

[Serializable]
public class EndingLetterMessage
{
    public string result;
    public ChiefLetter chiefLetter;
    public MurdererLetter murdererLetter;
    public List<SurvivorsLetters> survivorsLetters;
}

[Serializable]
public class ChiefLetter
{
    public string receiver;
    public string content;
    public string sender;
}

[Serializable]
public class MurdererLetter
{
    public string receiver;
    public string content;
    public string sender;
}

[Serializable]
public class SurvivorsLetters
{
    public string name;
    public SurvivorsLettersLetter letter;
}

[Serializable]
public class SurvivorsLettersLetter
{
    public string receiver;
    public string content;
    public string sender;
}

#endregion

public class EndingcreditManager : MonoBehaviour
{
    [SerializeField] private GameObject bg;
    private Transform[] letterTrs;
    private Transform creditTr;
    [SerializeField] private float speed;
    [SerializeField] private GameObject letterPrefab; // bg�ڽ����� instantiate

    private string letterContentString = "���̷� ���̷� ���� �����Դϴ� ������!";
    //private bool isLetterDone = false;

    // �ӽ�
    private long gameSetNo = 32;

    private void Start()
    {
        // bgm
        AudioManager.Instnace.PlaySound(BGM_List.Ending_Positive01, 0.15f, 2f);

        // ũ���� �ö󰡱�
        bg.GetComponent<EndingcreditUI>().credit.gameObject.SetActive(true);
        creditTr = bg.GetComponent<EndingcreditUI>().credit.transform; // ending credit�� tr�� �޾ƿ���

        // ���
        LetterConnection(gameSetNo);


        letterTrs = new Transform[2];
        letterTrs[0] = bg.GetComponent<EndingcreditUI>().letterTr01;
        letterTrs[1] = bg.GetComponent<EndingcreditUI>().letterTr02;

        //bg.GetComponent<EndingcreditUI>().letterSender.transform.parent.gameObject.SetActive(true); // ���� �׷� ����
        //StartCoroutine(CoWriteLetter());

    }

    #region ���
    private void LetterConnection(long gameSetNo)
    {
        EndingLetterRequest request = new EndingLetterRequest();
        request.gameSetNo = gameSetNo;
        string createdJson = JsonUtility.ToJson(request);

        HttpRequester requester = new HttpRequester();
        requester.requestType = RequestType.POST;
        requester.url = "http://ec2-15-165-15-244.ap-northeast-2.compute.amazonaws.com:8081/api/v1/game/ending-letter";
        requester.body = createdJson;
        requester.complete = OnGetComplete;
        requester.failed = OnGetFailed;

        HttpManagerKJY.instance.SendRequest(requester);
    }
    private void OnGetComplete(DownloadHandler handler)
    {
        Debug.Log("���� ����� �Ϸ� �Ǿ����ϴ�.");

        EndingLetterResponse response = new EndingLetterResponse();
        response = JsonUtility.FromJson<EndingLetterResponse>(handler.text);

        Debug.Log(response.ToString());

        Debug.Log("��Ƴ��� ����� ���� ��¼����¼��");

        StartCoroutine(CoInstantiateLetter(response));
    }

    private void OnGetFailed(DownloadHandler handler)
    {
        Debug.Log("����");
    }


    private IEnumerator CoInstantiateLetter(EndingLetterResponse response)
    {
        if (response.resultCode.CompareTo("WIN") == 0)
        {
            AudioManager.Instnace.PlaySound(BGM_List.Ending_Positive01, 0.3f, 2f);
        }
        else
        {
            AudioManager.Instnace.PlaySound(BGM_List.Ending_Negative03, 0.3f, 2f);

        }

        GameObject chiefLetterGo01 = Instantiate(letterPrefab, letterTrs[0]);
        LetterUI letter01 = chiefLetterGo01.GetComponent<LetterUI>();

        letter01.sender.text = response.message.chiefLetter.sender;
        letter01.content.text = response.message.chiefLetter.content;
        letter01.receiver.text = response.message.chiefLetter.receiver;
        Debug.Log($"����� ���� ���� �Ϸ� {chiefLetterGo01}, {letter01}");

        yield return new WaitForSeconds(5f);
        Destroy(chiefLetterGo01);


        GameObject chiefLetterGo02 = Instantiate(letterPrefab, letterTrs[1]);
        LetterUI letter02 = chiefLetterGo02.GetComponent<LetterUI>();

        letter02.sender.text = response.message.murdererLetter.sender;
        letter02.content.text = response.message.murdererLetter.content;
        letter02.receiver.text = response.message.murdererLetter.receiver;
        Debug.Log($" ���� ���� �Ϸ� {chiefLetterGo02}, {letter02}");

        yield return new WaitForSeconds(5f);
        Destroy(chiefLetterGo02);

        int index = 0;
        if (response.message.survivorsLetters.Count > 0)
        {
            foreach (var survivorLetter in response.message.survivorsLetters)
            {
                if(index >= 2)
                {
                    index = 0;
                }

                Debug.Log(survivorLetter.letter.content);

                GameObject chiefLetterGo03 = Instantiate(letterPrefab, letterTrs[index]);
                index++;
                LetterUI letter03 = chiefLetterGo03.GetComponent<LetterUI>();
                letter03.sender.text = survivorLetter.letter.sender;
                letter03.content.text = survivorLetter.letter.content;
                letter03.receiver.text = survivorLetter.letter.receiver;
                yield return new WaitForSeconds(5f);
                Destroy(chiefLetterGo03);
            }
        }
    }

    #endregion

    void Update()
    {
        if(creditTr.gameObject.activeSelf)
        {
            // ũ���� ���� �̵�
            MoveCredit();

            if (Input.GetKeyDown(KeyCode.Escape))
            {
                Debug.Log("ũ������ �����ϰ� ���� �̵��մϴ�.");
            }

            if (bg.GetComponent<EndingcreditUI>().credit.transform.position.y >= 4900)
            {
                Debug.Log("ũ������ �����ϰ� ���� �̵��մϴ�.");
            }
        }

        // �ӽ�
        // ��� �ڵ尡 �߰��Ǹ� ����
        //if(!isLetterDone && Input.GetKeyDown(KeyCode.E))
        //{
        //    bg.GetComponent<EndingcreditUI>().letterSender.transform.parent.gameObject.SetActive(false);
        //    isLetterDone = true;
        //}

        //if (isLetterDone) // ������ ������ ũ���� ����
        //{
        //    bg.GetComponent<EndingcreditUI>().credit.gameObject.SetActive(true);
        //    MoveCredit();
        //    if(Input.GetKeyDown(KeyCode.Escape))
        //    {
        //        Debug.Log("ũ������ �����ϰ� ���� �̵��մϴ�.");
        //    }

        //    if(bg.GetComponent<EndingcreditUI>().credit.transform.position.y >= 4900)
        //    {
        //        Debug.Log("ũ������ �����ϰ� ���� �̵��մϴ�.");
        //    }
        //}
    }

    //IEnumerator CoWriteLetter()
    //{
    //    TMP_Text letterContent = bg.GetComponent<EndingcreditUI>().letterContent;
    //    letterContent.text = "";

    //    foreach(char s in letterContentString)
    //    {
    //        letterContent.text += s;
    //        yield return new WaitForSeconds(0.1f);
    //    }
    //}

    private void MoveCredit()
    {
        Vector3 position = creditTr.position;
        position.y += speed * Time.deltaTime;
        creditTr.position = position;
    }
}
