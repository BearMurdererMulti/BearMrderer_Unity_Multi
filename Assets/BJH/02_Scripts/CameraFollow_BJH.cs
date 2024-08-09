using System.Collections;
using System.Collections.Generic;
using UnityEngine;

public class CameraFollow_BJH : MonoBehaviour
{
    [SerializeField] private Transform player;
    [SerializeField] private Vector3 offset; // 플레이어 - 카메라 사이 offset
    [SerializeField] private float rotX;

    private void Start()
    {

    }
    private void Update()
    {
        // 카메라 위치를 플레이어 위치와 오프셋을 기반으로 설정
        Vector3 desiredPosition = player.position + offset;

        // 카메라를 원하는 위치로 이동
        transform.position = desiredPosition;

        // 카메라의 회전은 유지 (회전하지 않도록 설정)
        transform.rotation = Quaternion.Euler(rotX, 0, 0); // 원하는 회전 각도로 설정 (탑다운 뷰)
    }
}
