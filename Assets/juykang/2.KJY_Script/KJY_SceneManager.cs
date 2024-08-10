using System.Collections;
using System.Collections.Generic;
using UnityEngine;

public enum SceneName
{
    LoginScene_KJY,
    Lobby_KJY,
    Room_KJY,
    Chinemachine_01,
    PostmanLoadingScene,
    Cinemachine_ReceiveLetter,
    Cinemachine02,
    CharacterCustom_new,
    Cinemachine03,
    //GameScene_NPC_Random_BJH,
    GameScene_NPC_Random,
    GameScene_NPC_Random3,
    KJY_Test_Ending_Success,
    KJY_Test_Ending_Fail,
    EndingCredit,
}

public class KJY_SceneManager : MonoBehaviour
{
    public static KJY_SceneManager instance;

    public void Awake()
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

    public void ChangeScene(SceneName scene)
    {
        // Load the scene by its name
        UnityEngine.SceneManagement.SceneManager.LoadScene(scene.ToString());
    }
}
