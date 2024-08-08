using System.Collections;
using System.Collections.Generic;
using TMPro;
using Unity.VisualScripting;
using UnityEngine;
using UnityEngine.Playables;
using UnityEngine.UI;
using static Photon.Voice.OpusCodec;

public class TextManager : MonoBehaviour
{
    public static TextManager Instance;

    [SerializeField] private GameObject letterBG;
    [SerializeField] private TMP_Text senderText, contentText, receiverText;
    [SerializeField] private TMP_Text pressEnter;

    private string content;

    private bool isDone;

    [SerializeField] private float durationTime;

    [SerializeField] private PlayableDirector timeline;


    private void Awake()
    {
        if(Instance == null)
        {
            Instance = this;
        }    
    }

    // Start is called before the first frame update
    void Start()
    {
        pressEnter.enabled = false;
        letterBG.SetActive(false);

        content = InfoManagerKJY.instance.content;
        senderText.text = InfoManagerKJY.instance.greeting;
        receiverText.text = InfoManagerKJY.instance.closing;


        senderText.text = InfoManagerKJY.instance.greeting;

        SubTimelineEvent();
    }

    void Update()
    {
        if(isDone && Input.GetKeyDown(KeyCode.Return))
        {
            KJY_SceneManager.instance.ChangeScene(SceneName.CharacterCustom_new);
        }

    }

    private void SubTimelineEvent()
    {
        timeline.stopped += DissubTimelineEvent;
        timeline.Play();
    }

    private void DissubTimelineEvent(PlayableDirector timeline)
    {
        timeline.stopped -= DissubTimelineEvent;
        StartCoroutine(CoTyping());
    }

    IEnumerator CoTyping()
    {
        letterBG.SetActive(true);

        yield return null;
        
        for(int i =  0; i < content.Length; i++)
        {
            if(isDone)
            {
                contentText.text = content;
                isDone = false;
                break;
            }
            contentText.text = content.Substring(0, i+1);
            yield return new WaitForSeconds(0.06f);
        }
        isDone = true;
        pressEnter.enabled = true;
        StartCoroutine(Blink());
    }

    IEnumerator Blink()
    {
        while (true)
        {
            // 서서히 사라지기
            yield return StartCoroutine(Fade(1f, 0f, durationTime));

            // 서서히 나타나기
            yield return StartCoroutine(Fade(0f, 1f, durationTime));
        }
    }

    IEnumerator Fade(float startAlpha, float endAlpha, float duration)
    {
        float elapsedTime = 0f;

        while (elapsedTime < duration)
        {
            elapsedTime += Time.deltaTime;
            float alpha = Mathf.Lerp(startAlpha, endAlpha, elapsedTime / duration);
            pressEnter.alpha = alpha;
            yield return null;
        }
        pressEnter.alpha = endAlpha;
    }

}
