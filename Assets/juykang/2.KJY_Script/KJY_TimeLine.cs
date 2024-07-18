using System.Collections;
using System.Collections.Generic;
using UnityEngine;
using UnityEngine.Playables;

public class KJY_TimeLine : MonoBehaviour
{
    [SerializeField] PlayableDirector director;

    private void Start()
    {
        // Ÿ�Ӷ����� ���� �� ������ �޼ҵ� ����
        Invoke("OnTimelineFinished", (float)director.duration);
    }

    private void OnTimelineFinished()
    {
        KJY_SceneManager.instance.ChangeScene(0);
        Debug.Log("�ε� �Ϸ�");
    }
}
