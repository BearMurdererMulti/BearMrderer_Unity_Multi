using System.Collections;
using System.Collections.Generic;
using UnityEditor;
using UnityEngine;
using UnityEngine.EventSystems;

public class PlayerMove_BJH : MonoBehaviour
{
    [SerializeField] private float walkSpeed;
    [SerializeField] private float runSpeed;

    [SerializeField] private float rotSpeed;

    private Rigidbody rigid;
    private bool isGround;
    private float jumpForce;
    [SerializeField] private LayerMask groundLayer;
    private Vector3 dir = Vector3.zero;
    private Animator animator;

    private bool isWalk;

    private void Start()
    {
        rigid = this.GetComponent<Rigidbody>();
        animator = gameObject.GetComponent<Animator>();
    }

    void Update()
    {
        if (Input.GetAxis("Horizontal") != 0 || Input.GetAxis("Vertical") != 0)
        {
            isWalk = true;
        }
        else
        {
            isWalk = false;
        }

        if (isWalk)
        {
            if(Input.GetKey(KeyCode.RightShift))
            {
                Move(runSpeed, 1.0f);
            }
            else
            {
                Move(walkSpeed, 0.5f);
            }

            Rotate();
        }
        else
        {
            animator.SetFloat("MoveSpeed", 0.0f);
        }

        if(Input.GetKeyDown(KeyCode.Space))
        {
            Jump();
        }

        if(Input.GetKeyDown(KeyCode.Alpha1))
        {
            isWalk = false;
            PlaySpecialAnimation("isSpin");
            isWalk = true;
        }
        if(Input.GetKeyDown(KeyCode.Alpha2))
        {
            isWalk = false;
            PlaySpecialAnimation("isRoll");
            isWalk = true;
        }
    }

    private void Move(float speed, float animationSpeed)
    {
        float horizontal = Input.GetAxis("Horizontal");
        float vertical = Input.GetAxis("Vertical");

        Vector3 dir = new Vector3(horizontal, 0, vertical).normalized;

        if(dir.magnitude >= 0.1f) // 벡터의 크기
        {
            transform.position += dir * speed * Time.deltaTime;
            animator.SetFloat("MoveSpeed", animationSpeed);
            //rigid.MovePosition(transform.position + dir * speed * Time.deltaTime);
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
            transform.rotation = Quaternion.Slerp(transform.rotation, targetRotation, Time.deltaTime * rotSpeed);
        }
    }

    // 미적용
    private void Jump()
    {
        isGround = Physics.CheckSphere(transform.position, 0.1f, groundLayer); // 바닥 판별

        if (isGround && Input.GetButtonDown("Jump"))
        {
            rigid.AddForce(Vector3.up * jumpForce, ForceMode.Impulse); // 물체에 순간적인 힘을 적용
        }
    }

    private void PlaySpecialAnimation(string triggerName)
    {
        animator.SetTrigger(triggerName);
    }

}
