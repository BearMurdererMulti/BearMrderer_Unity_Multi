using System.Collections;
using System.Collections.Generic;
using UnityEngine;

public class CameraBack : MonoBehaviour
{
    public Transform playerTr;

    [SerializeField]
    private Vector3 camOffset;
    private Vector3 initRot;

    private void Start()
    {
        initRot = transform.eulerAngles;
        playerTr = GameObject.FindWithTag("Player").transform;
        this.enabled = false;
    }
    // Update is called once per frame
    void Update()
    {
        transform.position = playerTr.position + camOffset;
        transform.rotation = Quaternion.Euler(initRot);
    }
}
