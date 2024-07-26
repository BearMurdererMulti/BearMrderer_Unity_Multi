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

    // ���õǸ� selector �Ѱ� ����
    // �׷��� �ٸ� ������Ʈ�� ���õȴٸ�?
    [SerializeField] private GameObject selectedWeaponSelector;
    public void OnClickSelector(GameObject selector)
    {
        // �̹� ���õ� �����Ͱ� �ִٸ�?
        if (selectedWeaponSelector != null)
        {
            // �̹� ���õ� �����Ͱ� �ڱ� �ڽ��̶��?
            if (selectedWeaponSelector.name == selector.name)
            {
                selectedWeaponSelector.GetComponent<Image>().enabled = false;
                selectedWeaponSelector = null;
                return;
            }
            else
            {
                // ���õ� ������ ��Ȱ��ȭ
                selectedWeaponSelector.GetComponent<Image>().enabled = false;
            }
        }
        selector.GetComponent<Image>().enabled = true;
        selectedWeaponSelector = selector;
    }
}
