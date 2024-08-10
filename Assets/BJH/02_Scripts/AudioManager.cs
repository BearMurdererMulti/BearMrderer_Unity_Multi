using Photon.Pun;
using System.Collections;
using System.Collections.Generic;
using UnityEngine;

// enum ������ �¿� _audioSources�� assign ���ּ���.
public enum SoundList
{
    LoginBG,
    BG,
    Ending_Positive01,
    Ending_Positive02,
    Ending_Negative03,
    Play_BG
}

public class AudioManager : MonoBehaviourPunCallbacks
{
    private static AudioManager _instance;
    [SerializeField] private List<AudioClip> _audioClips = new List<AudioClip>(); // ����� Ŭ���� ���
    [SerializeField] private AudioSource _audioSource; // ����� �ҽ�
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

    // duration time�� ���̵��ΰ� ���̵� �ƿ���
    public void PlaySound(SoundList soundList, float volume, float durationTime)
    {
        StartCoroutine(CoPlayMusicWithFadeIn(soundList, volume, durationTime));
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

    IEnumerator CoPlayMusicWithFadeIn(SoundList soundList, float volume, float durationTime)
    {
        index = ((int)soundList);
        _audioSource.clip = _audioClips[index];
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

}
