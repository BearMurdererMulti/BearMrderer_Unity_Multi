using System.Collections;
using System.Collections.Generic;
using UnityEngine;
using UnityEngine.Playables;
using static SceneManager;

public class SceneController_BJH : MonoBehaviour
{
    [SerializeField] private PlayableDirector timeline;
    [SerializeField] private SceneName sceneName;

    private void Start()
    {
        SubTimelineEvent();

        AudioManager.Instnace.PlaySound(BGM_List.Letter_BG, 0.3f, 2f);
}

    private void SubTimelineEvent()
    {
        timeline.stopped += DissubTimelineEvent;
        timeline.Play();
    }

    private void DissubTimelineEvent(PlayableDirector timeline)
    {
        timeline.stopped -= DissubTimelineEvent;
        MoveScene(sceneName);
    }

    public void MoveScene(SceneName sceneName)
    {
        KJY_SceneManager.instance.ChangeScene(sceneName);

    }
}
