using Photon.Pun;
using System.Collections;
using System.Collections.Generic;
using UnityEngine;

// enum 순서에 맞에 _audioSources에 assign 해주세요.
public enum BGM_List
{
    LoginBG,
    Letter_BG, // 편지씬 슬픈 배경음악
    Ending_Positive01,
    Ending_Positive02,
    Ending_Negative03,
    Play_BG,
    InterrogationRoom_BG
}

public enum SoundEffect_List
{
    Bow_SoundEffect,
    Pop_SoundEffect,
    Hearbeat_SoundEffect
}

public class AudioManager : MonoBehaviourPunCallbacks
{
    private static AudioManager _instance;
    [SerializeField] private List<AudioClip> _BgmClips = new List<AudioClip>(); // 배경음 클립들 담기
    [SerializeField] private List<AudioClip> _audioEffectClips = new List<AudioClip>(); // 효과음 클립들 담기

    [SerializeField] private AudioSource _audioSource; // 오디오 소스
    [SerializeField] private AudioSource _effectSource; // 효과음 소스

    private int index;

    public static AudioManager Instnace
    {
        get { return _instance; }
    }

    private void Awake()
    {
        if(_instance == null)
        {
            _instance = this;
            DontDestroyOnLoad(gameObject);
            return;
        }
        else
        {
            Destroy(gameObject);
        }
    }

    private void Start()
    {
        _audioSource = GetComponent<AudioSource>();
    }

    private void Update()
    {
        if(_audioSource == null)
        {
            _audioSource = GetComponent<AudioSource>();
        }
    }

    // duration time은 페이드인과 페이드 아웃용
    public void PlaySound(BGM_List soundList, float volume, float durationTime)
    {
        StartCoroutine(CoPlayMusicWithFadeIn(soundList, volume, durationTime));
    }

    // duration time은 페이드인과 페이드 아웃용
    public void PlayEffect(SoundEffect_List effectList, float volume, float durationTime)
    {
        StartCoroutine(CoPlayAudioEffectWithFadeIn(effectList, volume, durationTime));
    }

    [PunRPC]
    public void StopSoundPun()
    {
        _audioSource.Stop();
    }

    public void StopSound()
    {
        _audioSource.Stop();
    }

    [PunRPC]
    public void StopEffectSoundPun()
    {
        _effectSource.Stop();
    }

    public void StopEffectSound()
    {
        _effectSource.Stop();
    }

    IEnumerator CoPlayMusicWithFadeIn(BGM_List soundList, float volume, float durationTime)
    {
        index = ((int)soundList);
        _audioSource.clip = _BgmClips[index];
        _audioSource.volume = 0f;
        _audioSource.Play();

        float startVolume = 0f;
        float targetVolume = volume;
        float currentTime = 0f;

        while(currentTime < durationTime)
        {
            currentTime += Time.deltaTime;
            _audioSource.volume = Mathf.Lerp(startVolume, targetVolume, currentTime / durationTime);
            yield return null;
        }

        _audioSource.volume = targetVolume;
    }

    IEnumerator CoPlayAudioEffectWithFadeIn(SoundEffect_List soundList, float volume, float durationTime)
    {
        index = ((int)soundList);
        _effectSource.clip = _audioEffectClips[index];
        _effectSource.volume = 0f;
        _effectSource.Play();

        float startVolume = 0f;
        float targetVolume = volume;
        float currentTime = 0f;

        while (currentTime < durationTime)
        {
            currentTime += Time.deltaTime;
            _effectSource.volume = Mathf.Lerp(startVolume, targetVolume, currentTime / durationTime);
            yield return null;
        }

        _effectSource.volume = targetVolume;
    }

}
