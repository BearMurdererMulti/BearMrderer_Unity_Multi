using System.Collections;
using System.Collections.Generic;
using UnityEngine;

public class KJY_SceneManager : MonoBehaviour
{
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

    public void ChangeScene(int num)
    {
        // 이동할 신 마다 번호 지정하고 해당 UI 마다 Event Trigger 로 추가하기
        // 변지환 추가 및 수정
        // 로그인 : 0
        // 캐릭터 커스텀 : 1
        // 메인게임 : 2
        // 패배씬 : 3
        // 승리씬 : 4
        UnityEngine.SceneManagement.SceneManager.LoadScene(num);
    }


    // LoaingPopup
    public GameObject loadingPopup;
    public void ChangeIntroScene()
    {
        loadingPopup.SetActive(true);
        ConnectionKJY.instance.RequestIntroScenarioSetting();
        //IntroConnection intro = new IntroConnection(InfoManagerKJY.instance.gameSetNo, "http://ec2-43-201-108-241.ap-northeast-2.compute.amazonaws.com:8081/api/v1/scenario/intro");
    }

    public void ChangeFirstScene()
    {
        UnityEngine.SceneManagement.SceneManager.LoadScene(0);
    }

    public void ChangeMainScene()
    {
        UnityEngine.SceneManagement.SceneManager.LoadScene(3);
    }

    public void ChangeWinScene()
    {
        UnityEngine.SceneManagement.SceneManager.LoadScene(4);
    }

    public void ChangeLoseScene()
    {
        int number = InfoManagerKJY.instance.murderObjectNumber;
        UnityEngine.SceneManagement.SceneManager.LoadScene(5 + number);
    }
}
