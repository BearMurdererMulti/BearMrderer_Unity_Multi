using System;
using System.Collections;
using System.Collections.Generic;
using Unity.VisualScripting;
using UnityEngine;

public class SceneManager : MonoBehaviour
{
    //�α��� -> ���̵��� �� -> (��Ʈ��) -> GameManager
    //���� ��ū�� DontDestroy -> ����
    //���� �Ŵ����� �α���
    public static SceneManager instance;

    public enum SceneName
    {
        LeaderRoomScene,
        PostmanLoadingScene,
    }

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
        // �̵��� �� ���� ��ȣ �����ϰ� �ش� UI ���� Event Trigger �� �߰��ϱ�
        // ���� : 0
        // ���� : 1
        UnityEngine.SceneManagement.SceneManager.LoadScene(num);
    }

    public void ChangeScene(SceneName scene)
    {
        string sceneName = scene.ToString();

        UnityEngine.SceneManagement.SceneManager.LoadScene(sceneName);
    }
}
