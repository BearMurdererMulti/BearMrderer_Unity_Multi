using System.Collections;
using System.Collections.Generic;
using TMPro;
using UnityEditor;
using UnityEngine;

public class EndingcreditManager : MonoBehaviour
{
    [SerializeField] private GameObject bg;
    private Transform creditTr;
    [SerializeField] private float speed;

    private string letterContentString = "하이루 하이루 편지 내용입니다 유후훗!";
    private bool isLetterDone = false;

    private void Start()
    {
        // bgm
        AudioManager.Instnace.PlaySound(SoundList.Ending_Positive01, 1f, 2f);
        creditTr = bg.GetComponent<EndingcreditUI>().credit.transform; // ending credit의 tr값 받아오기
        creditTr.gameObject.SetActive(false); // ending credit 끄기

        bg.GetComponent<EndingcreditUI>().letterSender.transform.parent.gameObject.SetActive(true); // 편지 그룹 실행
        StartCoroutine(CoWriteLetter());
        
    }
    void Update()
    {
        // 임시
        // 통신 코드가 추가되면 수정
        if(!isLetterDone && Input.GetKeyDown(KeyCode.E))
        {
            bg.GetComponent<EndingcreditUI>().letterSender.transform.parent.gameObject.SetActive(false);
            isLetterDone = true;
        }

        if (isLetterDone) // 편지가 끝나면 크레딧 시작
        {
            bg.GetComponent<EndingcreditUI>().credit.gameObject.SetActive(true);
            MoveCredit();
            if(Input.GetKeyDown(KeyCode.Escape))
            {
                Debug.Log("크레딧을 종료하고 씬을 이동합니다.");
            }

            if(bg.GetComponent<EndingcreditUI>().credit.transform.position.y >= 4900)
            {
                Debug.Log("크레딧을 종료하고 씬을 이동합니다.");
            }
        }
    }

    IEnumerator CoWriteLetter()
    {
        TMP_Text letterContent = bg.GetComponent<EndingcreditUI>().letterContent;
        letterContent.text = "";

        foreach(char s in letterContentString)
        {
            letterContent.text += s;
            yield return new WaitForSeconds(0.1f);
        }
    }

    private void MoveCredit()
    {
        Vector3 position = creditTr.position;
        position.y += speed * Time.deltaTime;
        creditTr.position = position;
    }
}
