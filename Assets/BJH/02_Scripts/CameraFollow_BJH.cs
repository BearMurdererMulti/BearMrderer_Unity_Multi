using System.Collections;
using System.Collections.Generic;
using UnityEngine;

public class CameraFollow_BJH : MonoBehaviour
{
    [SerializeField] private Transform player;
    [SerializeField] private Vector3 offset; // 플레이어 - 카메라 사이 offset

    private void Start()
    {

    }
    private void Update()
    {
        transform.position = player.position + offset;
        //transform.LookAt(player);
    }
}
