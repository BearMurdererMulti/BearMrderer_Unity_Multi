using Photon.Pun;
using System.Collections;
using System.Collections.Generic;
using UnityEngine;

public class CameraTopDown : MonoBehaviour
{
    [SerializeField] Transform player;
    [SerializeField] float smoothing = 0.2f;
    [SerializeField] float height;
    [SerializeField] float width;
    [SerializeField] Vector3 minCameraBoundary;
    [SerializeField] Vector3 maxCameraBoundary;
    public Quaternion first;
    //private void FixedUpdated()
    //{
    //    //Vector3 targetPos = new Vector3(player.position.x, height, player.position.z);

    //    //targetPos.x = Mathf.Clamp(targetPos.x, minCameraBoundary.x, maxCameraBoundary.x);
    //    //targetPos.z = Mathf.Clamp(targetPos.z, minCameraBoundary.z, maxCameraBoundary.z);
    //    Vector3 targetPos = player.position;
    //    targetPos.y = 8;
    //    targetPos.z += 8;
    //    transform.position = Vector3.Lerp(transform.position, targetPos, smoothing);
    //}

    private void Start()
    {
        player = GameObject.FindWithTag("Detective").transform;
        first = transform.rotation;
    }

    private void LateUpdate()
    {
       Vector3 targetPos = GameObject.FindWithTag("Detective").transform.position;
       targetPos.y += height;
       targetPos.z += width;
       transform.position = Vector3.Lerp(transform.position, targetPos, smoothing);
    }
}
