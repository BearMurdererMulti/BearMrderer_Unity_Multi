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
    public bool isTalking = false; // NPC�� ��ȭ ������ �Ǻ�

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
        animator.SetBool("Walk", true);
        
        if(GetComponent<NpcData>().status == "ALIVE")
        {
            // nac�� �������� ����
            nav.SetDestination(target.position);
        }
    }

    // Update is called once per frame
    void Update()
    {
        //if (nav.isOnNavMesh == true && transform.GetComponent<NpcData>().status == "ALIVE")
        //{
        //    // ���� �� ������
        //    if(isWalk == true)
        //    {
        //        // �̵�
        //        nav.isStopped = false;

        //        if (isTarget == true && transform.GetComponent<NpcData>().status == "ALIVE") //KJY �ڵ� �߰� 
        //        {
        //            isTarget = false;
        //            SetTarget();
        //            nav.SetDestination(target.position);
        //            animator.SetBool("Walk", true);
        //        }

        //    }
        //    // �ȴ� ���� �ƴ϶��?
        //    else
        //    {
        //        // ����
        //        nav.isStopped = true;
        //        animator.SetBool("Walk", false);
        //    }
        //}
    }


    // destination target ����
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



    // �����̿� ���̸� 10�� ��� �� ���� ������ Ž��
    private void OnTriggerEnter(UnityEngine.Collider other)
    {
        // destination�� ���̸�
        if (other.gameObject.tag == "Destination" && other.gameObject.name == destinations[preTarget].gameObject.name)
        {
            isWalking = false;
            animator.SetBool("Walk", false);

            prePosition = transform.position;
            StartCoroutine(nameof(coWaitForASec)); // 7�� ���
        }

        // �÷��̾�� ������ �̵��� ����
        if(other.gameObject.tag == "Player" && other.GetComponent<KJY_InterAction>().isOccupy == false && isTalking == false)
        {
            other.GetComponent<KJY_InterAction>().isOccupy = true;
            isTalking = true;
            NavIsStooped(true);
            isWalking = false;
            animator.SetBool("Walk", false);
        }


        // �ٸ� NPC�� �ε����� ���� 10�ʰ��� ���ֺ���
        // ���� 10�ʰ� �ʰ��ϸ� �� �� ����
        // 10�� �̳��� ����ڰ� ���ͷ����ϸ�
        // ����Ͽ� ���� ��ȭ�Ѵ�.
        //if (other.gameObject.tag == "NPC" && npcface.talking == false)

        // NPC ��ȭ
        // ���� �������� ���� �� ����
        //if (other.gameObject.tag == "Npc")
        //{
        //    Debug.Log("NPC���� �꿴���ϴ�.");
        //    if (!isTalking)
        //    {
        //        Debug.Log($"{gameObject.name} ��ȭ�� �����մϴ�.");

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
        // �÷��̾�� �־�����
        // �ٽ� �ȴ´�.
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

        // ���� ������ ����
        SetTarget();

        // ���� ��ġ�� ���� ��ġ�� ���� ��
        prePosition = ts.position;

        // �ȱ� ���� ����
        // �ȱ� ���¿� ���� �ִϸ��̼� ���� ���� ����
        isWalking = true;
        animator.SetBool("Walk", true);

        // nac�� �������� ����
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
        Debug.Log("NPC�� ����� ����");
    }

    public void GoAway()
    {

    }

    // NPC�� ������?
    // ���� �ִϸ��̼��� �����Ű�� �ݶ��̴��� ��Ʈ�ѷ�, navi mesh�� �����Ѵ�.
    public void Die() // KJY �߰��� �ڵ�
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
