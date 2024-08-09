using Photon.Pun;
using Photon.Realtime;
using System.Collections;
using System.Collections.Generic;
using UnityEngine;
using static UnityEngine.ParticleSystem;

public class CameraBack : MonoBehaviourPun
{
    public Transform playerTr;

    [SerializeField]
    private Vector3 camOffset;
    private Vector3 initRot;


    private void Start()
    {
        if (InfoManagerKJY.instance.role == "Detective")
        {
            initRot = transform.eulerAngles;
            playerTr = GameObject.FindWithTag("Detective").transform;
            this.enabled = false;
        }
        else
        {
            initRot = transform.eulerAngles;
            playerTr = GameObject.Find("AssistantCameraSpot").transform;
            this.enabled = false;
        }
    }

    // Update is called once per frame
    void Update()
    {
        if (playerTr != null)
        {
            transform.position = playerTr.position + camOffset;
            transform.rotation = Quaternion.Euler(initRot);
        }
    }
}
