using Photon.Voice.PUN;
using System.Collections;
using System.Collections.Generic;
using UnityEngine;
using UnityEngine.UI;

public class DogCanvas : MonoBehaviour
{
    private PhotonVoiceView photonVoiceView;
    [SerializeField] private Image isVoiceImage;

    private void Awake()
    {
        this.photonVoiceView = gameObject.GetComponentInParent<PhotonVoiceView>();
    }

    private void Start()
    {
        this.isVoiceImage.enabled = false;
    }

    private void Update()
    {
        this.isVoiceImage.enabled = photonVoiceView.IsSpeaking;
        Debug.Log(photonVoiceView.IsSpeaking);
    }
}
