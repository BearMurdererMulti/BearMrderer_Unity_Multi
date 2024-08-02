using System.Collections;
using System.Collections.Generic;
using UnityEngine;

// enum ������ �¿� _audioSources�� assign ���ּ���.
public enum SoundList
{
    BG
}

public class AudioManager : MonoBehaviour
{
    private static AudioManager _instance;
    [SerializeField] private List<AudioClip> _ausioClips = new List<AudioClip>(); // ����� �ҽ� ���
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
