using Photon.Realtime;
using System.Collections;
using System.Collections.Generic;
using UnityEngine;
using static UnityEngine.ParticleSystem;

public class CameraBack : MonoBehaviour
{
    public Transform playerTr;

    [SerializeField]
    private Vector3 camOffset;
    private Vector3 initRot;

    private void Start()
    {
        if (InfoManagerKJY.instance.role == "Assistant")
        {
            initRot = transform.eulerAngles;
            playerTr = GameObject.FindWithTag("User").transform;
            this.enabled = false;
        }
        else
        {
            initRot = transform.eulerAngles;
            playerTr = GameObject.FindWithTag("Detective").transform;
            this.enabled = false;
        }
    }
    // Update is called once per frame
    void Update()
    {
        if (InfoManagerKJY.instance.role == "Detective")
        {
            transform.position = playerTr.position + camOffset;
            transform.rotation = Quaternion.Euler(initRot);
        }
    }
}
