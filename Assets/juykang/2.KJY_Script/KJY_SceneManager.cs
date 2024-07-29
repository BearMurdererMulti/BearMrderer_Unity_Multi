using System.Collections;
using System.Collections.Generic;
using UnityEngine;

public class KJY_SceneManager : MonoBehaviour
{
    public enum SceneName
    {
        LeaderRoomScene,
        PostmanLoadingScene,
        UserRoomScene,
    }

    public static KJY_SceneManager instance;

    private void Awake()
    {
        if (instance == null)
        {
            instance = this;
            DontDestroyOnLoad(instance);
        }
        else
        {
            Destroy(this);
        }
    }

    // LoaingPopup
    public GameObject loadingPopup;
    public void ChangeIntroScene()
    {
        loadingPopup.SetActive(true);
        ConnectionKJY.instance.RequestIntroScenarioSetting();
    }

    public void ChangeScene(SceneName scene)
    {
        // Load the scene by its name
        UnityEngine.SceneManagement.SceneManager.LoadScene(scene.ToString());
    }
}
