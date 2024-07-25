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
}
