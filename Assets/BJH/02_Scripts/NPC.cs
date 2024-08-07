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
    public bool isTalking = false; // NPC가 대화 중인지 판별

    public bool cheating;

    NpcFaceMove npcface;

    public bool isUpdate = false;

    //KJY 추가
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
        // destination 자식들의 tranform을 destinations에 저장
        destinations = destination.transform.GetComponentsInChildren<Transform>();

        // kjy
        tmpDestinations = destination.transform.GetComponentsInChildren<Transform>();

        // destination target 설정
        SetTarget();

        // 현재 위치를 이전 위치에 저장 후
        prePosition = ts.position;

        // 걷기 상태 변경
        // 걷기 상태에 따라서 애니메이션 실행 여부 결정
        isWalking = true;
        animator.SetBool("Walk", true);
        
        if(GetComponent<NpcData>().status == "ALIVE")
        {
            if (PhotonNetwork.IsMasterClient)
            {
                // nac에 목적지를 지정
                nav.SetDestination(target.position);
            }
        }

        //KJY 추가
        if (!photonView.IsMine) // 동기화할 때 초기 위치 설정
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


    // destination target 추적
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


    // 목적이에 닿이면 10초 대기 후 다음 목적지 탐색
    private void OnTriggerEnter(UnityEngine.Collider other)
    {
        // destination에 닿이면
        if (other.gameObject.tag == "Destination" && other.gameObject.name == destinations[preTarget].gameObject.name)
        {
            isWalking = false;
            animator.SetBool("Walk", false);

            prePosition = transform.position;
            StartCoroutine(nameof(coWaitForASec)); // 7초 대기
        }

        // 플레이어와 닿으면 이동을 멈춤
        if (ChatManager.instance.talk == false)
        {
            if (other.gameObject.tag == "Player" && other.GetComponent<KJY_InterAction>().isOccupy == false)
            {
                other.GetComponent<KJY_InterAction>().isOccupy = true;
                NavIsStooped(true);
                isWalking = false;
                animator.SetBool("Walk", false);
            }
        }
    }

    [PunRPC]
    private void UpdateTouchDestination(UnityEngine.Collider other)
    {
        isWalking = false;
        animator.SetBool("Walk", false);

        prePosition = transform.position;
        StartCoroutine(nameof(coWaitForASec)); // 7초 대기
    }

    [PunRPC]
    private void UpdateTouchPlayer(UnityEngine.Collider other)
    {
        other.GetComponent<KJY_InterAction>().isOccupy = true;
        NavIsStooped(true);
        isWalking = false;
        animator.SetBool("Walk", false);
    }

    [PunRPC]
    private void UpdateGonePlayer(UnityEngine.Collider other)
    {
        other.GetComponent<KJY_InterAction>().isOccupy = false;
        isTalking = false;
        NavIsStooped(false);
        isWalking = true;
        animator.SetBool("Walk", true);
    }

    private void OnTriggerExit(UnityEngine.Collider other)
    {
        // 플레이어와 멀어지면
        // 다시 걷는다.
        if(other.gameObject.tag == "Player")
        {
            other.GetComponent<KJY_InterAction>().isOccupy = false;
            isTalking = false;
            NavIsStooped(false);
            isWalking = true;
            animator.SetBool("Walk", true);
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

        // 다음 목적지 설정
        SetTarget();

        // 현재 위치를 이전 위치에 저장 후
        prePosition = ts.position;

        // 걷기 상태 변경
        // 걷기 상태에 따라서 애니메이션 실행 여부 결정
        isWalking = true;
        animator.SetBool("Walk", true);

        // nac에 목적지를 지정
        nav.SetDestination(target.position);
    }

    IEnumerator WaitForNpcInterection()
    {
        yield return new WaitForSeconds(10f);
        isTalking = false;
        isWalking = true;
        animator.SetBool("Walk", true);
        NavIsStooped(false);
    }


    void StartCommunication()
    {
        Debug.Log("NPC가 통신을 시작");
    }

    public void GoAway()
    {

    }

    // NPC가 죽으면?
    // 죽음 애니메이션을 실행시키고 콜라이더와 컨트롤러, navi mesh를 종료한다.
    public void Die() // KJY 추가한 코드
    {
        animator.SetBool("Walk", false);
        isWalking = false;
        animator.SetBool("Dead", true);

        GetComponent<SphereCollider>().enabled = false;
        GetComponent<CharacterController>().enabled = false;
        nav.enabled = false;

        Vector3 tmp = transform.position;
        tmp.y -= 0.1f;
        transform.position = tmp;   
    }
}
