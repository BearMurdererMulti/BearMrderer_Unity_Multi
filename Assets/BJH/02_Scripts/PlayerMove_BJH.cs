using Photon.Pun;
using System.Collections;
using System.Collections.Generic;
using UnityEditor;
using UnityEngine;
using UnityEngine.EventSystems;

public class PlayerMove_BJH : MonoBehaviourPunCallbacks
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

        // 스페이스는 구르기
        if(Input.GetKeyDown(KeyCode.Space))
        {
            photonView.RPC("PlaySpecialAnimation", RpcTarget.All, "isRoll");
        }
        // 돌기, 바운스, 두려움, 앉기, 선택된
        if(Input.GetKeyDown(KeyCode.Alpha1))
        {
            photonView.RPC("PlaySpecialAnimation", RpcTarget.All, "isSpin");
        }
        if(Input.GetKeyDown(KeyCode.Alpha2))
        {
            photonView.RPC("PlaySpecialAnimation", RpcTarget.All, "isBounce");
        }
        if (Input.GetKeyDown(KeyCode.Alpha3))
        {
            photonView.RPC("PlaySpecialAnimation", RpcTarget.All, "isFear");
        }
        if (Input.GetKeyDown(KeyCode.Alpha4))
        {
            photonView.RPC("PlaySpecialAnimation", RpcTarget.All, "isSit");
        }
        if (Input.GetKeyDown(KeyCode.Alpha5))
        {
            photonView.RPC("PlaySpecialAnimation", RpcTarget.All, "isClicked");
        }
    }

    [PunRPC]
    private void PlaySpecialAnimation(string triggerName)
    {
        isWalk = false;
        animator.SetTrigger(triggerName);
        isWalk = true;
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


}
