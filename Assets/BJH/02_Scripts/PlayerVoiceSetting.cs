using Photon.Pun;
using Photon.Voice.Unity;
using System.Collections;
using System.Collections.Generic;
using UnityEngine;

public class PlayerVoiceSetting : MonoBehaviour
{
    [SerializeField] private Recorder recorder;

    private void Start()
    {
        if(recorder == null)
        {
            recorder = GameObject.Find("VoiceManager").GetComponent<Recorder>();
        }

        // 마스터 클라이언트가 아니면 뮤트
        if(!PhotonNetwork.IsMasterClient)
        {
            MuteRecorder();
        }
    }

    private void MuteRecorder()
    {
        recorder.TransmitEnabled = false;
    }
}
