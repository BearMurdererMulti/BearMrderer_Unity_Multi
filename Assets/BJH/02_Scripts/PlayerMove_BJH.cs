using System.Collections;
using System.Collections.Generic;
using UnityEditor;
using UnityEngine;
using UnityEngine.EventSystems;

public class PlayerMove_BJH : MonoBehaviour
{
    [SerializeField] private float speed;
    [SerializeField] private float rotSpeed;

    private Rigidbody rigid;
    private bool isGround;
    private float jumpForce;
    [SerializeField] private LayerMask groundLayer;
    private Vector3 dir = Vector3.zero;

    private void Start()
    {
        rigid = this.GetComponent<Rigidbody>();
    }

    void Update()
    {
        Move();
        Rotate();
        Jump();
    }

    private void Move()
    {
        float horizontal = Input.GetAxis("Horizontal");
        float vertical = Input.GetAxis("Vertical");

        Vector3 dir = new Vector3(horizontal, 0, vertical).normalized;

        if(dir.magnitude >= 0.1f) // 벡터의 크기
        {
            // 이동 = 현재 위치 + (방향 * 속도 * Time.DeltaTime)
            // 현재 위치 += 방향 * 속도 * Time.DeltaTime
            rigid.MovePosition(transform.position + dir * speed * Time.deltaTime);
        }
    }

    private void Rotate()
    {
        float horizontal = Input.GetAxis("Horizontal");
        float vertical = Input.GetAxis("Vertical");

        Vector3 dir = new Vector3(horizontal, 0, vertical).normalized;

        if (dir.magnitude >= 0.1f)
        {
            Quaternion targetRotation = Quaternion.LookRotation(dir); // 목표 회전값 계산
            transform.rotation = Quaternion.Slerp(transform.rotation, targetRotation, Time.deltaTime * 10f);
        }
    }

    private void Jump()
    {
        isGround = Physics.CheckSphere(transform.position, 0.1f, groundLayer); // 바닥 판별

        if (isGround && Input.GetButtonDown("Jump"))
        {
            rigid.AddForce(Vector3.up * jumpForce, ForceMode.Impulse); // 물체에 순간적인 힘을 적용
        }
    }

    

}
