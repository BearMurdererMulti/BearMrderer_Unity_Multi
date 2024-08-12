using System.Collections;
using System.Collections.Generic;
using UnityEngine;

public class KJY_AudioManager : MonoBehaviour
{
    public static KJY_AudioManager instance;
    [SerializeField] AudioSource audioSource;

    private void Awake()
    {
        instance = this; 
    }

    public void StartAudio(float volume, AudioClip sound)
    {
        audioSource.volume = volume;
        audioSource.PlayOneShot(sound);
    }

    
}
