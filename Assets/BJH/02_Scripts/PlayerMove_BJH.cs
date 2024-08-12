using Photon.Pun;
using System.Collections;
using System.Collections.Generic;
using UnityEditor;
using UnityEngine;
using UnityEngine.EventSystems;

public class PlayerMove_BJH : MonoBehaviourPunCallbacks, IPunObservable
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

    private float h, v;

    //서버에서 넘어오는 위치값
    Vector3 receivePos;
    //서버에서 넘어오는 회전값
    Quaternion receiveRot = Quaternion.identity;

    private PhotonView _audioManagerPhotonView;

    private void Start()
    {
        rigid = this.GetComponent<Rigidbody>();
        animator = gameObject.GetComponent<Animator>();

        _audioManagerPhotonView = AudioManager.Instnace.gameObject.GetComponent<PhotonView>();
    }

    void Update()
    {
        if (photonView.IsMine)
        {
            if (Input.GetAxis("Horizontal") != 0 || Input.GetAxis("Vertical") != 0)
            {
                h = Input.GetAxis("Horizontal");
                isWalk = true;
            }
            else
            {
                v = Input.GetAxis("Vertical");
                isWalk = false;
            }

            if (isWalk)
            {
                if (Input.GetKey(KeyCode.RightShift))
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

            // 스페이스는 구르기
            if (Input.GetKeyDown(KeyCode.Space))
            {
                photonView.RPC("PlaySpecialAnimation", RpcTarget.All, "isRoll");
                _audioManagerPhotonView.RPC("PlayEffectPun", RpcTarget.All, SoundEffect_List.Rolling, 0.3f, 0f);
            }
            // 돌기, 바운스, 두려움, 앉기, 선택된
            if (Input.GetKeyDown(KeyCode.Alpha1))
            {
                photonView.RPC("PlaySpecialAnimation", RpcTarget.All, "isFear");
                _audioManagerPhotonView.RPC("PlayEffectPun", RpcTarget.All, SoundEffect_List.SmallDogBark, 0.3f, 0f);
            }
            if (Input.GetKeyDown(KeyCode.Alpha2))
            {
                photonView.RPC("PlaySpecialAnimation", RpcTarget.All, "isBounce");
                _audioManagerPhotonView.RPC("PlayEffectPun", RpcTarget.All, SoundEffect_List.Bounce, 0.3f, 0f);
            }
            if (Input.GetKeyDown(KeyCode.Alpha3))
            {
                photonView.RPC("PlaySpecialAnimation", RpcTarget.All, "isSpin");
                _audioManagerPhotonView.RPC("PlayEffectPun", RpcTarget.All, SoundEffect_List.Spin, 0.3f, 0f);
            }
            if (Input.GetKeyDown(KeyCode.Alpha4))
            {
                photonView.RPC("PlaySpecialAnimation", RpcTarget.All, "isSit");
                _audioManagerPhotonView.RPC("PlayEffectPun", RpcTarget.All, SoundEffect_List.Smell, 0.3f, 0f);
            }
            if (Input.GetKeyDown(KeyCode.Alpha5))
            {
                photonView.RPC("PlaySpecialAnimation", RpcTarget.All, "isClicked");
                _audioManagerPhotonView.RPC("PlayEffectPun", RpcTarget.All, SoundEffect_List.KkingKKing, 0.3f, 0f);

            }
        }
  
    }

    public void OnPhotonSerializeView(PhotonStream stream, PhotonMessageInfo info)
    {
        //내 Player 라면
        if (stream.IsWriting)
        {
            //나의 위치값을 보낸다.
            stream.SendNext(transform.position);
            //나의 회전값을 보낸다.
            stream.SendNext(transform.rotation);
            //h 값 보낸다.
            stream.SendNext(h);
            //v 값 보낸다.
            stream.SendNext(v);
        }
        //내 Player 아니라면
        else
        {
            //위치값을 받자.
            receivePos = (Vector3)stream.ReceiveNext();
            //회전값을 받자.
            receiveRot = (Quaternion)stream.ReceiveNext();
            //h 값 받자.
            h = (float)stream.ReceiveNext();
            //v 값 받자.
            v = (float)stream.ReceiveNext();
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
