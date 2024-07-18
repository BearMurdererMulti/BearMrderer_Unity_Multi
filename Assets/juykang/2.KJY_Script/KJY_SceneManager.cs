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
        // �̵��� �� ���� ��ȣ �����ϰ� �ش� UI ���� Event Trigger �� �߰��ϱ�
        // ����ȯ �߰� �� ����
        // �α��� : 0
        // ĳ���� Ŀ���� : 1
        // ���ΰ��� : 2
        // �й�� : 3
        // �¸��� : 4
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
