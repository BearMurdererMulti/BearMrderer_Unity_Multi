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
        button = GameObject.Find("Talk_Btn").GetComponent<Button>();
        button.interactable = false;
    }

    private void OnTriggerStay(UnityEngine.Collider other)
    {
        if (other.gameObject.CompareTag("Npc") && ChatManager.instance.talk == false)
        {
            if (other.GetComponent<NpcData>().status == "ALIVE")
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
        if (other.gameObject.CompareTag("Npc") && ChatManager.instance.talk == false)
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
