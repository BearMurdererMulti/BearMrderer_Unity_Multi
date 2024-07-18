using System.Collections;
using System.Collections.Generic;
using UnityEngine;


public class Button_BJH : MonoBehaviour
{
    public void OnclickClose(GameObject go)
    {
        go.SetActive(!go.activeSelf);

        if(UI.instance.dicChatHistoryState.ContainsKey(go.name))
        {
            UI.instance.dicChatHistoryState[go.name] = go.activeSelf;
        }
    }

    public void OnClickNextScene()
    {
        UnityEngine.SceneManagement.SceneManager.LoadScene(2);
    }
}
