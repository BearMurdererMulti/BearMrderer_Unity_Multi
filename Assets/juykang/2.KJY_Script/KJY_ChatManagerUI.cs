using System.Collections;
using System.Collections.Generic;
using UnityEngine;
using UnityEngine.UI;

public class KJY_ChatManagerUI : MonoBehaviour
{
    [SerializeField] GameObject btn;
    [SerializeField] Button btnInterActive;
    [SerializeField] GameObject inventory;
    private void Start()
    {
        if (InfoManagerKJY.instance.role == "Detective")
        {
            btn.SetActive(true);
            inventory.SetActive(true);
            ChatManager.instance.interactiveBtn = btn;
            UI.instance.talkBtn = btn;
            btnInterActive.onClick.AddListener(() => { ChatManager.instance.StartTalk(); });
        }
        UI.instance.inventoryObject = inventory;
    }
}
