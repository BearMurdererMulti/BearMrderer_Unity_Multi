using System;
using System.Collections;
using System.Collections.Generic;
using TMPro;
using UnityEditor;
using UnityEngine;
using UnityEngine.Networking;



public class EndingcreditManager : MonoBehaviour
{
    [SerializeField] private GameObject bg;
    private Transform letterTrs; // 편지를 띄울 중앙 위치. 직접 assign
    private Transform creditTr;
    [SerializeField] private float speed;
    [SerializeField] private GameObject chiefLetterPrefab; // bg자식으로 instantiate
    [SerializeField] private GameObject basicLetterPrefab; // bg자식으로 instantiate

    [SerializeField] private float durationTime; // bg자식으로 instantiate
    private bool isLetterDone;


    private void Start()
    {
        // 노래 실행
        if(InfoManagerKJY.instance.finalLetterResultCode == "WIN")
        {
            AudioManager.Instnace.PlaySound(BGM_List.Ending_Positive01, 0.15f, 2f);
        }
        else
        {
            AudioManager.Instnace.PlaySound(BGM_List.Ending_Negative03, 0.15f, 2f);
        }

        // 코루틴 실행
        StartCoroutine(CoInstantiateLetter());

        creditTr = bg.GetComponent<EndingcreditUI>().credit.transform; // ending credit의 tr값 받아오기
    }

    #region 통신
 

    private IEnumerator CoInstantiateLetter()
    {
        InfoManagerKJY infoManager = InfoManagerKJY.instance;

        if (infoManager.finalLetterResultCode.CompareTo("WIN") == 0)
        {
            AudioManager.Instnace.PlaySound(BGM_List.Ending_Positive01, 0.3f, 2f);
        }
        else
        {
            AudioManager.Instnace.PlaySound(BGM_List.Ending_Negative03, 0.3f, 2f);
        }

        GameObject chiefLetter = Instantiate(chiefLetterPrefab, letterTrs);
        LetterUI letterUI01 = chiefLetter.GetComponent<LetterUI>();

        letterUI01.sender.text = infoManager.chiefLetter.sender;
        letterUI01.content.text = infoManager.chiefLetter.content;
        letterUI01.receiver.text = infoManager.chiefLetter.receiver;

        yield return new WaitForSeconds(durationTime);
        Destroy(chiefLetter);

        GameObject basicLetter = Instantiate(basicLetterPrefab, letterTrs);
        LetterUI letterUI02 = basicLetter.GetComponent<LetterUI>();
        letterUI02.sender.text = infoManager.murdererLetter.sender;
        letterUI02.content.text = infoManager.murdererLetter.content;
        letterUI02.receiver.text = infoManager.murdererLetter.receiver;

        yield return new WaitForSeconds(durationTime);

        List<SurvivorsLettersLetter> letters = infoManager.finalLetters;
        foreach(SurvivorsLettersLetter letter in letters)
        {
            letterUI02.sender.text = letter.sender;
            letterUI02.content.text = letter.content;
            letterUI02.receiver.text = letter.receiver;
            yield return new WaitForSeconds(durationTime);
        }
        isLetterDone = true;
    }

    #endregion

    void Update()
    {
        if(isLetterDone)
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
