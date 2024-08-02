using System.Collections;
using System.Collections.Generic;
using UnityEngine;

// enum 순서에 맞에 _audioSources에 assign 해주세요.
public enum SoundList
{
    BG
}

public class AudioManager : MonoBehaviour
{
    private static AudioManager _instance;
    [SerializeField] private List<AudioClip> _ausioClips = new List<AudioClip>(); // 오디오 소스 담기
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

    public void PlaySound(SoundList soundList)
    {
        index = ((int)soundList);
        _audioSource.clip = _ausioClips[index];
        _audioSource.Play();
    }

    public void StopSound()
    {
        _audioSource.Stop();
    }

}
