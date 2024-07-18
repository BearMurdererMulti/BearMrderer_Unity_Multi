using System.Collections;
using System.Collections.Generic;
using UnityEditor;
using UnityEngine;

public class PlayerMove : MonoBehaviour
{
    [SerializeField]
    private float speed;

    [SerializeField]
    private float rotSpeed;

    private Vector3 dir = Vector3.zero;

    private Rigidbody rigid;

    private void Start()
    {
        rigid = this.GetComponent<Rigidbody>();
    }

    void Update()
    {
        dir.x = Input.GetAxis("Horizontal");
        dir.z = Input.GetAxis("Vertical");
        dir.Normalize();

        if(dir != Vector3.zero)
        {
            rigid.transform.forward = Vector3.Lerp(transform.forward, dir, rotSpeed * Time.deltaTime);

            transform.position += dir * speed * Time.deltaTime;
        }
    }

}
