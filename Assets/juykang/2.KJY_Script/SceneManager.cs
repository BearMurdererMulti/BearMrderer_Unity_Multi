using System.Collections;
using System.Collections.Generic;
using Unity.VisualScripting;
using UnityEngine;

public class SceneManager : MonoBehaviour
{
    //로그인 -> 씬이동을 해 -> (인트로) -> GameManager
    //유저 토큰을 DontDestroy -> 유저
    //게임 매니저를 로그인
    public static SceneManager instance;

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

    //[YarnCommand("SceneChange")]
    public void ChangeScene(int num)
    {
        // 이동할 신 마다 번호 지정하고 해당 UI 마다 Event Trigger 로 추가하기
        // 마을 : 0
        // 광장 : 1
        UnityEngine.SceneManagement.SceneManager.LoadScene(num);
    }


    public void ChangeMainScene()
    {
        UnityEngine.SceneManagement.SceneManager.LoadScene(3);
    }

    public void ChangeWinScene()
    {
        UnityEngine.SceneManagement.SceneManager.LoadScene(3);
    }

    public void ChangeLoseScene()
    {
        UnityEngine.SceneManagement.SceneManager.LoadScene(4);
    }
}
