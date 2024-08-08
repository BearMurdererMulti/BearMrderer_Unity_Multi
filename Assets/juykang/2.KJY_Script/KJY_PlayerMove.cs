using Photon.Pun;
using System.Collections;
using System.Collections.Generic;
using UnityEngine;

public class KJY_PlayerMove : MonoBehaviourPun, IPunObservable
{
    //������ ����
    public float speed = 5;

    //���� ����
    public int MaxJumpCount = 1;
    public float jumpPower = 5;
    public float gravity = -9.81f;
    private float yVeclocity = 0;
    private bool isJump = false;
    private bool isRun = false;

    private Transform body;
    //CC
    private CharacterController cc;

    //�ִϸ��̼�
    private Animator animator;

    //������
    private float h;
    private float v;

    //�������� �Ѿ���� ��ġ��
    Vector3 receivePos;
    //�������� �Ѿ���� ȸ����
    Quaternion receiveRot = Quaternion.identity;
    float lerpSpeed = 50;

    // Start is called before the first frame update
    void Start()
    {
        cc = GetComponent<CharacterController>();
        if (InfoManagerKJY.instance.role == "Detective")
        {
            animator = transform.GetChild(0).GetComponent<Animator>();
        }
        else if (gameObject.name == "B")
        {
            animator = transform.GetComponent<Animator>();
        }
        body = transform.GetChild(0);
    }

    // Update is called once per frame
    void Update()
    {
        if (photonView.IsMine)
        {
            if (ChatManager.instance.talk == true || KJY_CitizenManager.Instance.call == true)
            {
                photonView.RPC(nameof(SetBoolRpc), RpcTarget.All, "Move", false);
                return;
            }
            //�÷��̾� ������
            h = Input.GetAxis("Horizontal");
            float v = Input.GetAxis("Vertical");

            Vector3 dirH = transform.right * h;
            Vector3 dirV = transform.forward * v;

            Vector3 dir = -dirH + -dirV;
            dir.Normalize();
            if (dirH != Vector3.zero || dirV != Vector3.zero)
            {
                body.forward = dir;
                photonView.RPC(nameof(SetBoolRpc), RpcTarget.All, "Move", true);
            }
            else
            {
                photonView.RPC(nameof(SetBoolRpc), RpcTarget.All, "Move", false);
            }

            if (cc.isGrounded == true)
            {
                yVeclocity = 0;
                isJump = false;
            }

            if (isJump == false)
            {
                Vector3 tmp = transform.position;
                tmp.y = 0;
                transform.position = tmp;
            }

            if (Input.GetKeyDown(KeyCode.Space) && isJump == false)
            {
                yVeclocity = jumpPower;
                isJump = true;
            }

            yVeclocity += gravity * Time.deltaTime;

            dir.y = yVeclocity;
            cc.Move(dir * speed * Time.deltaTime);

            if (Input.GetKeyDown(KeyCode.LeftShift))
            {
                if (isRun == true)
                {
                    isRun = false;
                    speed = 5;
                }
                else
                {
                    isRun = true;
                    speed = 8;
                }
            }
        }
        else
        {
            //��ġ ����
            transform.position = Vector3.Lerp(transform.position, receivePos, lerpSpeed * Time.deltaTime);
            //ȸ�� ����
            transform.rotation = Quaternion.Lerp(transform.rotation, receiveRot, lerpSpeed * Time.deltaTime);
        }
    }

    [PunRPC]
    private void SetBoolRpc(string parameter, bool value)
    {
        if (animator != null)
        {
            animator.SetBool(parameter, value);
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

}
