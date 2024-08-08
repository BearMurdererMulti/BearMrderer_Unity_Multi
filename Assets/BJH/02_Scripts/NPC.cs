using Photon.Pun;
using System;
using System.Collections;
using System.Collections.Generic;
using Unity.Burst.CompilerServices;
using Unity.VisualScripting;
using UnityEngine;
using UnityEngine.AI;



public class NPC : MonoBehaviourPunCallbacks, IPunObservable
{
    public GameObject destination;
    [SerializeField] private Transform[] destinations;
    [SerializeField] private Transform target;
    private int preTarget = 0;
    public NavMeshAgent nav;
    private bool isTarget = false;

    [SerializeField] private Animator animator;
    private Transform ts;
    [SerializeField] private Vector3 prePosition;

    public Transform[] tmpDestinations; //KJY

    private CharacterController cc;

    public bool isWalking;
    public bool isTalking = false; // NPC�� ��ȭ ������ �Ǻ�

    public bool cheating;

    NpcFaceMove npcface;

    public bool isUpdate = false;

    //KJY �߰�
    private Vector3 networkPosition;
    private Quaternion networkRotation;
    private float networkMovementSpeed;

    private void Awake()
    {
        animator = transform.GetComponent<Animator>();
        ts = GetComponent<Transform>();
        nav = GetComponent<NavMeshAgent>();
        cc = GetComponent<CharacterController>();
    }


    void Start()
    {
        // destination �ڽĵ��� tranform�� destinations�� ����
        destinations = destination.transform.GetComponentsInChildren<Transform>();

        // kjy
        tmpDestinations = destination.transform.GetComponentsInChildren<Transform>();

        // destination target ����
        SetTarget();

        // ���� ��ġ�� ���� ��ġ�� ���� ��
        prePosition = ts.position;

        // �ȱ� ���� ����
        // �ȱ� ���¿� ���� �ִϸ��̼� ���� ���� ����
        isWalking = true;
        photonView.RPC("SetBoolRpc_NPC", RpcTarget.All, "Walk", true);
        
        if(GetComponent<NpcData>().status == "ALIVE")
        {
            if (PhotonNetwork.IsMasterClient)
            {
                // nac�� �������� ����
                nav.SetDestination(target.position);
            }
        }

        //KJY �߰�
        if (!photonView.IsMine) // ����ȭ�� �� �ʱ� ��ġ ����
        {
            networkPosition = transform.position;
            networkRotation = transform.rotation;
            networkMovementSpeed = nav.speed;
        }
    }

    void Update()
    {
        if (photonView.IsMine)
        {
            UpdateMovement();
        }
        else
        {
            SmoothlySyncTransform();
        }
    }

    private void SmoothlySyncTransform()
    {
        float lerpRate = 5f; // Adjust for smoother or faster interpolation
        ts.position = Vector3.Lerp(ts.position, networkPosition, Time.deltaTime * lerpRate);
        ts.rotation = Quaternion.Lerp(ts.rotation, networkRotation, Time.deltaTime * lerpRate);
    }

    public void OnPhotonSerializeView(PhotonStream stream, PhotonMessageInfo info)
    {
        if (stream.IsWriting)
        {
            stream.SendNext(ts.position);
            stream.SendNext(ts.rotation);
            stream.SendNext(nav.velocity.magnitude); // Send movement speed for animation sync
        }
        else
        {
            networkPosition = (Vector3)stream.ReceiveNext();
            networkRotation = (Quaternion)stream.ReceiveNext();
            networkMovementSpeed = (float)stream.ReceiveNext();
        }
    }

    private void UpdateMovement()
    {
        if (isWalking)
        {
            if (Vector3.Distance(ts.position, target.position) < 0.1f)
            {
                StartCoroutine(coWaitForASec());
            }
        }
    }


    // destination target ����
    void SetTarget()
    {
        while(true)
        {
            if (PhotonNetwork.IsMasterClient)
            {
                System.Random random = new System.Random();

                int num = random.Next(1, destinations.Length);

                if (preTarget != num)
                {
                   photonView.RPC("SetNumber", RpcTarget.All, num);
                   break;
                }
            }
            else
            {
                return;
            }
        }
    }

    [PunRPC]
    private void SetNumber(int num)
    {
        target = destinations[num];
        preTarget = num;
    }

    [PunRPC]
    void SetTriggerRpc(string parameter, bool value)
    {
        animator.SetBool(parameter, value);
    }

    // �����̿� ���̸� 10�� ��� �� ���� ������ Ž��
    private void OnTriggerEnter(UnityEngine.Collider other)
    {
        // destination�� ���̸�
        if (other.gameObject.tag == "Destination" && other.gameObject.name == destinations[preTarget].gameObject.name)
        {
            isWalking = false;
            photonView.RPC("SetBoolRpc_NPC", RpcTarget.All, "Walk", false);

            prePosition = transform.position;
            StartCoroutine(nameof(coWaitForASec)); // 7�� ���
        }

        // �÷��̾�� ������ �̵��� ����
        if (ChatManager.instance.talk == false)
        {
            if (other.gameObject.tag == "Player" && other.GetComponent<KJY_InterAction>().isOccupy == false)
            {
                other.GetComponent<KJY_InterAction>().isOccupy = true;
                NavIsStooped(true);
                isWalking = false;
                photonView.RPC("SetBoolRpc_NPC", RpcTarget.All, "Walk", false);
            }
        }
    }

    private void OnTriggerExit(UnityEngine.Collider other)
    {
        // �÷��̾�� �־�����
        // �ٽ� �ȴ´�.
        if(other.gameObject.tag == "Player")
        {
            other.GetComponent<KJY_InterAction>().isOccupy = false;
            isTalking = false;
            NavIsStooped(false);
            isWalking = true;
            photonView.RPC("SetBoolRpc_NPC", RpcTarget.All, "Walk", true);
        }
    }

    // controll navi agent
    public void NavIsStooped(bool b)
    {
        nav.isStopped = b;
    }

    IEnumerator coWaitForASec()
    {
        yield return new WaitForSeconds(7f);

        // ���� ������ ����
        SetTarget();

        // ���� ��ġ�� ���� ��ġ�� ���� ��
        prePosition = ts.position;

        // �ȱ� ���� ����
        // �ȱ� ���¿� ���� �ִϸ��̼� ���� ���� ����
        isWalking = true;
        photonView.RPC("SetBoolRpc_NPC", RpcTarget.All, "Walk", true);

        // nac�� �������� ����
        nav.SetDestination(target.position);
    }

    IEnumerator WaitForNpcInterection()
    {
        yield return new WaitForSeconds(10f);
        isTalking = false;
        isWalking = true;
        photonView.RPC("SetBoolRpc_NPC", RpcTarget.All, "Walk", true);
        NavIsStooped(false);
    }


    void StartCommunication()
    {
        Debug.Log("NPC�� ����� ����");
    }

    [PunRPC]
    private void SetBoolRpc_NPC(string parameter, bool value)
    {
        animator.SetBool(parameter, value);
    }

    // NPC�� ������?
    // ���� �ִϸ��̼��� �����Ű�� �ݶ��̴��� ��Ʈ�ѷ�, navi mesh�� �����Ѵ�.
    public void Die() // KJY �߰��� �ڵ�
    {
        photonView.RPC("SetBoolRpc_NPC", RpcTarget.All, "Walk", false);
        isWalking = false;
        photonView.RPC("SetBoolRpc_NPC", RpcTarget.All, "Dead", true);

        GetComponent<SphereCollider>().enabled = false;
        GetComponent<CharacterController>().enabled = false;
        nav.enabled = false;

        Vector3 tmp = transform.position;
        tmp.y -= 0.1f;
        transform.position = tmp;   
    }
}
