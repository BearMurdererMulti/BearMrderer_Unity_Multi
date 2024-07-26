using System.Collections;
using System.Collections.Generic;
using UnityEngine;
using UnityEngine.UI;


public class Button_BJH : Button
{
    public void OnclickClose(GameObject go)
    {
        go.SetActive(!go.activeSelf);

        if(UI.instance.dicChatHistoryState.ContainsKey(go.name))
        {
            UI.instance.dicChatHistoryState[go.name] = go.activeSelf;
        }
    }

    public void OnClickCloseAndOpen(GameObject go)
    {
        go.SetActive(!go.activeSelf);
    }

    public void OnClickNextScene()
    {
        UnityEngine.SceneManagement.SceneManager.LoadScene(2);
    }

    // 선택되면 selector 켜고 끄기
    // 그런데 다른 오브젝트가 선택된다면?
    [SerializeField] private GameObject selectedWeaponSelector;
    public void OnClickSelector(GameObject selector)
    {
        // 이미 선택된 셀렉터가 있다면?
        if (selectedWeaponSelector != null)
        {
            // 이미 선택된 셀렉터가 자기 자신이라면?
            if (selectedWeaponSelector.name == selector.name)
            {
                selectedWeaponSelector.GetComponent<Image>().enabled = false;
                selectedWeaponSelector = null;
                return;
            }
            else
            {
                // 선택된 셀렉터 비활성화
                selectedWeaponSelector.GetComponent<Image>().enabled = false;
            }
        }
        selector.GetComponent<Image>().enabled = true;
        selectedWeaponSelector = selector;
    }
}
