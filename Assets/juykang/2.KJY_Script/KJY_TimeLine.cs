using System.Collections;
using System.Collections.Generic;
using UnityEngine;
using UnityEngine.Playables;

public class KJY_TimeLine : MonoBehaviour
{
    [SerializeField] PlayableDirector director;

    private void Start()
    {
        // 타임라인이 끝난 후 실행할 메소드 예약
        Invoke("OnTimelineFinished", (float)director.duration);
    }

    private void OnTimelineFinished()
    {
        KJY_SceneManager.instance.ChangeScene(0);
        Debug.Log("로드 완료");
    }
}
