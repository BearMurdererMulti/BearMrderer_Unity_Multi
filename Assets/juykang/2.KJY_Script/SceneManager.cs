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
