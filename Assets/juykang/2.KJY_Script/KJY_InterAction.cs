using System.Collections;
using System.Collections.Generic;
using UnityEngine;
using UnityEngine.UI;

public class KJY_InterAction : MonoBehaviour
{
    [SerializeField] private Button button;
    public bool isOccupy;

    private void Start()
    {
        StartCoroutine(FindButtonCoroutine("Talk_Btn"));
    }

    private IEnumerator FindButtonCoroutine(string buttonName)
    {
        GameObject buttonObject = null;

        // 버튼이 씬에 나타날 때까지 기다림
        while (buttonObject == null)
        {
            buttonObject = GameObject.Find(buttonName);
            yield return null; // 다음 프레임까지 대기
        }

        // 버튼의 컴포넌트를 가져옴
        button = buttonObject.GetComponent<Button>();

        if (button != null)
        {
            button.interactable = false;
        }
    }

    private void OnTriggerStay(UnityEngine.Collider other)
    {
        if (other.gameObject.CompareTag("Npc") && ChatManager.instance.talk == false)
        {
            if (other.GetComponent<NpcData>().status == "ALIVE" && InfoManagerKJY.instance.role == "Detective")
            {
                button.interactable = true;
                ChatManager.instance.nowNpc = other.gameObject;
                ChatManager.instance.npcdata = other.GetComponent<NpcData>();
                other.gameObject.GetComponent<Collider_BJH>().enabled = false;
                other.gameObject.GetComponent<NPC>().isWalking = false;
            }
        }
    }

    private void OnTriggerExit(UnityEngine.Collider other)
    {
        if (other.gameObject.CompareTag("Npc") && ChatManager.instance.talk == false && InfoManagerKJY.instance.role == "Detective")
        {
            button.interactable = false;
            ChatManager.instance.npcdata = null;
            other.gameObject.GetComponent<NPC>().isWalking = true;
            other.gameObject.GetComponent<Collider_BJH>().enabled = true;
        }
    }

    public void ButtonInterAction()
    {
        button.interactable = false;
    }
}
