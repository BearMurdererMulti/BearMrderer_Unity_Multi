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

    //�������� �Ѿ���� ��ġ��
    Vector3 receivePos;
    //�������� �Ѿ���� ȸ����
    Quaternion receiveRot = Quaternion.identity;

    private PhotonView pv;

    private void Start()
    {
        rigid = this.GetComponent<Rigidbody>();
        animator = gameObject.GetComponent<Animator>();

        //_audioManagerPhotonView = AudioManager.Instnace.gameObject.GetComponent<PhotonView>();
        //pv = GameObject.Find("AudioManager").GetComponent<PhotonView>();
        //Debug.Log(pv.gameObject.name);
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

            // �����̽��� ������
            if (Input.GetKeyDown(KeyCode.Space))
            {
                photonView.RPC("PlaySpecialAnimation", RpcTarget.All, "isRoll");
                AudioManager.Instnace.PlayEffect(SoundEffect_List.Rolling, 0.3f, 0);
            }
            // ����, �ٿ, �η���, �ɱ�, ���õ�
            if (Input.GetKeyDown(KeyCode.Alpha1))
            {
                photonView.RPC("PlaySpecialAnimation", RpcTarget.All, "isFear");
                AudioManager.Instnace.PlayEffect(SoundEffect_List.SmallDogBark, 0.3f, 0);
            }
            if (Input.GetKeyDown(KeyCode.Alpha2))
            {
                photonView.RPC("PlaySpecialAnimation", RpcTarget.All, "isBounce");
                AudioManager.Instnace.PlayEffect(SoundEffect_List.Bounce, 0.3f, 0);
            }
            if (Input.GetKeyDown(KeyCode.Alpha3))
            {
                photonView.RPC("PlaySpecialAnimation", RpcTarget.All, "isSpin");
                AudioManager.Instnace.PlayEffect(SoundEffect_List.Spin, 0.3f, 0);
            }
            if (Input.GetKeyDown(KeyCode.Alpha4))
            {
                photonView.RPC("PlaySpecialAnimation", RpcTarget.All, "isSit");
                AudioManager.Instnace.PlayEffect(SoundEffect_List.KkingKKing, 0.3f, 0);
            }
            if (Input.GetKeyDown(KeyCode.Alpha5))
            {
                photonView.RPC("PlaySpecialAnimation", RpcTarget.All, "isClicked");
                AudioManager.Instnace.PlayEffect(SoundEffect_List.Smell, 0.3f, 0);

            }
        }
  
    }

    public void OnPhotonSerializeView(PhotonStream stream, PhotonMessageInfo info)
    {
        //�� Player ���
        if (stream.IsWriting)
        {
            //���� ��ġ���� ������.
            stream.SendNext(transform.position);
            //���� ȸ������ ������.
            stream.SendNext(transform.rotation);
            //h �� ������.
            stream.SendNext(h);
            //v �� ������.
            stream.SendNext(v);
        }
        //�� Player �ƴ϶��
        else
        {
            //��ġ���� ����.
            receivePos = (Vector3)stream.ReceiveNext();
            //ȸ������ ����.
            receiveRot = (Quaternion)stream.ReceiveNext();
            //h �� ����.
            h = (float)stream.ReceiveNext();
            //v �� ����.
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

        if(dir.magnitude >= 0.1f) // ������ ũ��
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
            Quaternion targetRotation = Quaternion.LookRotation(dir); // ��ǥ ȸ���� ���
            transform.rotation = Quaternion.Slerp(transform.rotation, targetRotation, Time.deltaTime * rotSpeed);
        }
    }

    // ������
    private void Jump()
    {
        isGround = Physics.CheckSphere(transform.position, 0.1f, groundLayer); // �ٴ� �Ǻ�

        if (isGround && Input.GetButtonDown("Jump"))
        {
            rigid.AddForce(Vector3.up * jumpForce, ForceMode.Impulse); // ��ü�� �������� ���� ����
        }
    }


}
