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

    private string letterContentString = "���̷� ���̷� ���� �����Դϴ� ������!";
    private bool isLetterDone = false;

    private void Start()
    {
        // bgm
        AudioManager.Instnace.PlaySound(SoundList.Ending_Positive01, 1f, 2f);
        creditTr = bg.GetComponent<EndingcreditUI>().credit.transform; // ending credit�� tr�� �޾ƿ���
        creditTr.gameObject.SetActive(false); // ending credit ����

        bg.GetComponent<EndingcreditUI>().letterSender.transform.parent.gameObject.SetActive(true); // ���� �׷� ����
        StartCoroutine(CoWriteLetter());
        
    }
    void Update()
    {
        // �ӽ�
        // ��� �ڵ尡 �߰��Ǹ� ����
        if(!isLetterDone && Input.GetKeyDown(KeyCode.E))
        {
            bg.GetComponent<EndingcreditUI>().letterSender.transform.parent.gameObject.SetActive(false);
            isLetterDone = true;
        }

        if (isLetterDone) // ������ ������ ũ���� ����
        {
            bg.GetComponent<EndingcreditUI>().credit.gameObject.SetActive(true);
            MoveCredit();
            if(Input.GetKeyDown(KeyCode.Escape))
            {
                Debug.Log("ũ������ �����ϰ� ���� �̵��մϴ�.");
            }

            if(bg.GetComponent<EndingcreditUI>().credit.transform.position.y >= 4900)
            {
                Debug.Log("ũ������ �����ϰ� ���� �̵��մϴ�.");
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
