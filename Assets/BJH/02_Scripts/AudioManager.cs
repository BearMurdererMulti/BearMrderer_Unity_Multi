using System.Collections;
using System.Collections.Generic;
using UnityEngine;

// enum 순서에 맞에 _audioSources에 assign 해주세요.
public enum SoundList
{
    BG,
    Ending_Positive01,
    Ending_Positive02,
    Ending_Negative03,
    Play_BG
}

public class AudioManager : MonoBehaviour
{
    private static AudioManager _instance;
    [SerializeField] private List<AudioClip> _audioClips = new List<AudioClip>(); // 오디오 소스 담기
    [SerializeField] private AudioSource _audioSource;
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

    public void PlaySound(SoundList soundList, float volume, float durationTime)
    {
        StartCoroutine(CoPlayMusicWithFadeIn(soundList, volume, durationTime));
    }

    public void StopSound()
    {
        _audioSource.Stop();
    }

    IEnumerator CoPlayMusicWithFadeIn(SoundList soundList, float volume, float durationTime)
    {
        index = ((int)soundList);
        _audioSource.clip = _audioClips[index];
        _audioSource.volume = 0f;
        _audioSource.Play();

        float startVolume = 0f;
        float targetVolume = 1f;
        float currentTime = 0f;

        while(currentTime < durationTime)
        {
            currentTime += Time.deltaTime;
            _audioSource.volume = Mathf.Lerp(startVolume, targetVolume, currentTime / durationTime);
            yield return null;
        }

        _audioSource.volume = targetVolume;
    }

}
