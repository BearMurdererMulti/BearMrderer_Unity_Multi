using System;
using System.Collections;
using System.Collections.Generic;
using Unity.Burst.CompilerServices;
using Unity.VisualScripting;
using UnityEngine;
using UnityEngine.AI;



public class NPC : MonoBehaviour
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
            // nac에 목적지를 지정
            nav.SetDestination(target.position);
        }
    }

    // Update is called once per frame
    void Update()
    {
        //if (nav.isOnNavMesh == true && transform.GetComponent<NpcData>().status == "ALIVE")
        //{
        //    // 걸을 수 있으면
        //    if(isWalk == true)
        //    {
        //        // 이동
        //        nav.isStopped = false;

        //        if (isTarget == true && transform.GetComponent<NpcData>().status == "ALIVE") //KJY 코드 추가 
        //        {
        //            isTarget = false;
        //            SetTarget();
        //            nav.SetDestination(target.position);
        //            animator.SetBool("Walk", true);
        //        }

        //    }
        //    // 걷는 중이 아니라면?
        //    else
        //    {
        //        // 멈춤
        //        nav.isStopped = true;
        //        animator.SetBool("Walk", false);
        //    }
        //}
    }


    // destination target 추적
    void SetTarget()
    {
        while(true)
        {
            System.Random random = new System.Random();

            int num = random.Next(1, destinations.Length);

            if (preTarget != num)
            {
                target = destinations[num];
                preTarget = num;
                break;
            }
        }
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
        if(other.gameObject.tag == "Player" && other.GetComponent<KJY_InterAction>().isOccupy == false && isTalking == false)
        {
            other.GetComponent<KJY_InterAction>().isOccupy = true;
            isTalking = true;
            NavIsStooped(true);
            isWalking = false;
            animator.SetBool("Walk", false);
        }


        // 다른 NPC랑 부딪히면 서로 10초가량 마주보기
        // 만약 10초가 초과하면 갈 길 가고
        // 10초 이내로 사용자가 인터렉션하면
        // 통신하여 서로 대화한다.
        //if (other.gameObject.tag == "NPC" && npcface.talking == false)

        // NPC 대화
        // 추후 버전에서 진행 할 예정
        //if (other.gameObject.tag == "Npc")
        //{
        //    Debug.Log("NPC끼리 닿였습니다.");
        //    if (!isTalking)
        //    {
        //        Debug.Log($"{gameObject.name} 대화를 시작합니다.");

        //        isTalking = true;
        //        isWalking = false;
        //        animator.SetBool("Walk", false);
        //        NavIsStooped(true);
        //        transform.LookAt(other.gameObject.transform.position);
        //        StartCoroutine(nameof(WaitForNpcInterection));
        //    }
        //}




        //if (!isTalking)
        //{
        //    isTalking = true;
        //    animator.SetBool("Walk", false);

            //    if (other.gameObject.tag == "NPC" && npcface.talking == false)
            //    {


            //        StartCoroutine(nameof(WaitForPlayerInteraction));

            //        //if (cheating == true)
            //        //{
            //        //    StopCoroutine(nameof(WaitForPlayerInteraction));
            //        //    StartCommunication();
            //        //}
            //    }

            //}





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
