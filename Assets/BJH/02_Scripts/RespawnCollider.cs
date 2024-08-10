using System.Collections;
using System.Collections.Generic;
using UnityEngine;

public class RespawnCollider : MonoBehaviour
{
    private void OnTriggerEnter(Collider other)
    {
        if(other.gameObject.CompareTag("User") || other.gameObject.CompareTag("Detective"))
        {
            Debug.Log("세상 밖입니다. 리스폰 합니다.");
            var respawnPoint = new Vector3(-23.3f, 2.2f, -12.4f);
            other.transform.position = respawnPoint;
        }
    }
}
