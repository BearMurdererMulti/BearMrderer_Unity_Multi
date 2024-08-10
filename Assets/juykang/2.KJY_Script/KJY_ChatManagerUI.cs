using System.Collections;
using System.Collections.Generic;
using UnityEngine;
using UnityEngine.UI;

public class KJY_ChatManagerUI : MonoBehaviour
{
    [SerializeField] GameObject btn;
    [SerializeField] Button btnInterActive;
    [SerializeField] Button btnInterInactive;
    [SerializeField] GameObject inventory;
    [SerializeField] GameObject keyWord;
    private void Start()
    {
        if (InfoManagerKJY.instance.role == "Detective")
        {
            btn.SetActive(true);
            inventory.SetActive(true);
            ChatManager.instance.interactiveBtn = btn;
            ChatManager.instance.KeyWord = keyWord;
            UI.instance.talkBtn = btn;

            btnInterActive.onClick.RemoveAllListeners();
            btnInterInactive.onClick.RemoveAllListeners();

            btnInterActive.onClick.AddListener(() => { ChatManager.instance.StartKeyWordCanVas(); });
            btnInterInactive.onClick.AddListener(() => { ChatManager.instance.StartTalk(); });
        }
        else
        {
            btn.SetActive(false);
        }
        UI.instance.inventoryObject = inventory;
    }
}
