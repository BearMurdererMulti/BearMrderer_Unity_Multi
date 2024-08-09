using System.Collections;
using System.Collections.Generic;
using UnityEngine;

public class CameraFollow_BJH : MonoBehaviour
{
    [SerializeField] private Transform player;
    [SerializeField] private Vector3 offset; // �÷��̾� - ī�޶� ���� offset
    [SerializeField] private float rotX;

    private void Start()
    {

    }
    private void Update()
    {
        // ī�޶� ��ġ�� �÷��̾� ��ġ�� �������� ������� ����
        Vector3 desiredPosition = player.position + offset;

        // ī�޶� ���ϴ� ��ġ�� �̵�
        transform.position = desiredPosition;

        // ī�޶��� ȸ���� ���� (ȸ������ �ʵ��� ����)
        transform.rotation = Quaternion.Euler(rotX, 0, 0); // ���ϴ� ȸ�� ������ ���� (ž�ٿ� ��)
    }
}
