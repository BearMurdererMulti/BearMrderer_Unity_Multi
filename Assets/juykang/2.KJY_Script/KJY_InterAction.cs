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
            print(other.gameObject.GetComponent<NpcData>().npcName);
            button.interactable = true;
            if (ChatManager.instance.talk == false)
            {
                ChatManager.instance.nowNpc = other.gameObject;
            }
            ChatManager.instance.npcdata = other.GetComponent<NpcData>();
            other.gameObject.GetComponent<NPC>().isWalking = false;
        }
    }

    private void OnTriggerExit(UnityEngine.Collider other)
    {
        if (other.gameObject.CompareTag("Npc"))
        {
            button.interactable = false;
            ChatManager.instance.npcdata = null;
            other.gameObject.GetComponent<NPC>().isWalking = true;
        }
    }

    public void ButtonInterAction()
    {
        button.interactable = false;
    }
}
