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

    // 선택되면 selector 색을 흰색 -> 노란색으로 변경
    [SerializeField] private GameObject selectedWeaponSelector;
    public void OnClickSelector(GameObject selector)
    {
        // 이미 선택된 셀렉터가 있다면?
        if (selectedWeaponSelector != null)
        {
            // 이미 선택된 셀렉터가 자기 자신이라면?
            if (selectedWeaponSelector.name == selector.name)
            {
                selectedWeaponSelector.GetComponent<Image>().color = Color.white; // 선택 취소
                selectedWeaponSelector = null;
                return;
            }
            else
            {
                // 선택된 셀렉터 비활성화
                selectedWeaponSelector.GetComponent<Image>().color= Color.white; // 선택 취소
            }
        }
        selector.GetComponent<Image>().color = Color.yellow; // 선택
        selectedWeaponSelector = selector;
    }
}
