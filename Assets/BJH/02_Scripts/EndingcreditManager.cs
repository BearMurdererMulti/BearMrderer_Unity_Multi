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
    [SerializeField] private GameObject letterPrefab; // bg자식으로 instantiate

    private string letterContentString = "하이루 하이루 편지 내용입니다 유후훗!";
    //private bool isLetterDone = false;

    // 임시
    private long gameSetNo = 32;

    private void Start()
    {
        // bgm
        AudioManager.Instnace.PlaySound(BGM_List.Ending_Positive01, 0.15f, 2f);

        // 크레딧 올라가기
        bg.GetComponent<EndingcreditUI>().credit.gameObject.SetActive(true);
        creditTr = bg.GetComponent<EndingcreditUI>().credit.transform; // ending credit의 tr값 받아오기

        // 통신
        LetterConnection(gameSetNo);


        letterTrs = new Transform[2];
        letterTrs[0] = bg.GetComponent<EndingcreditUI>().letterTr01;
        letterTrs[1] = bg.GetComponent<EndingcreditUI>().letterTr02;

        //bg.GetComponent<EndingcreditUI>().letterSender.transform.parent.gameObject.SetActive(true); // 편지 그룹 실행
        //StartCoroutine(CoWriteLetter());

    }

    #region 통신
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
        Debug.Log("편지 통신이 완료 되었습니다.");

        EndingLetterResponse response = new EndingLetterResponse();
        response = JsonUtility.FromJson<EndingLetterResponse>(handler.text);

        Debug.Log(response.ToString());

        Debug.Log("살아남은 사람들 편지 어쩌고저쩌고");

        StartCoroutine(CoInstantiateLetter(response));
    }

    private void OnGetFailed(DownloadHandler handler)
    {
        Debug.Log("실패");
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
        Debug.Log($"촌장님 편지 생성 완료 {chiefLetterGo01}, {letter01}");

        yield return new WaitForSeconds(5f);
        Destroy(chiefLetterGo01);


        GameObject chiefLetterGo02 = Instantiate(letterPrefab, letterTrs[1]);
        LetterUI letter02 = chiefLetterGo02.GetComponent<LetterUI>();

        letter02.sender.text = response.message.murdererLetter.sender;
        letter02.content.text = response.message.murdererLetter.content;
        letter02.receiver.text = response.message.murdererLetter.receiver;
        Debug.Log($" 편지 생성 완료 {chiefLetterGo02}, {letter02}");

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
            // 크레딧 위로 이동
            MoveCredit();

            if (Input.GetKeyDown(KeyCode.Escape))
            {
                Debug.Log("크레딧을 종료하고 씬을 이동합니다.");
            }

            if (bg.GetComponent<EndingcreditUI>().credit.transform.position.y >= 4900)
            {
                Debug.Log("크레딧을 종료하고 씬을 이동합니다.");
            }
        }

        // 임시
        // 통신 코드가 추가되면 수정
        //if(!isLetterDone && Input.GetKeyDown(KeyCode.E))
        //{
        //    bg.GetComponent<EndingcreditUI>().letterSender.transform.parent.gameObject.SetActive(false);
        //    isLetterDone = true;
        //}

        //if (isLetterDone) // 편지가 끝나면 크레딧 시작
        //{
        //    bg.GetComponent<EndingcreditUI>().credit.gameObject.SetActive(true);
        //    MoveCredit();
        //    if(Input.GetKeyDown(KeyCode.Escape))
        //    {
        //        Debug.Log("크레딧을 종료하고 씬을 이동합니다.");
        //    }

        //    if(bg.GetComponent<EndingcreditUI>().credit.transform.position.y >= 4900)
        //    {
        //        Debug.Log("크레딧을 종료하고 씬을 이동합니다.");
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
