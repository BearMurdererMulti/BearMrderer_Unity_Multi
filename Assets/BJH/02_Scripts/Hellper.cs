using System.Collections;
using System.Collections.Generic;
using System.Reflection;
using UnityEditor;
using UnityEngine;
using UnityEngine.Rendering.PostProcessing;

public class Hellper : MonoBehaviour
{
    [Header("�߷�")]
    private CharacterController cc;
    public float gravity = -9.8f;
    private Vector3 velocity;

    [Header("����")]
    Vector3 myPosition;
    Vector3 targetPosition;
    Vector3 myForward;
    float speed = 3.0f;
    public float distance = 1.0f;
    public int angle = 150;

    [Header("�ִϸ��̼�")]
    bool isAnimation;
    Animator animator;


    // Start is called before the first frame update
    void Start()
    {
        cc = GetComponent<CharacterController>();

        myPosition = transform.position;
        myForward = transform.forward;

        animator = GetComponent<Animator>();
    }

    // Update is called once per frame
    void Update()
    {
        if(cc.isGrounded && velocity.y < 0)
        {
            velocity.y = 0f;
        }

        velocity.y += gravity * Time.deltaTime;
        cc.Move(velocity * Time.deltaTime);

        
        if(isAnimation == true)
        {
            Debug.Log("player������ �̵��մϴ�.");
            PlayerAnimation();
            RunToTarget();
        }

        //if(HellperManager._instance.isArrive == true)
        //{
        //    isAnimation = false;
        //    StopAnimation();
        //    HellperManager._instance.isArrive = false;
        //}
    }

    // play to animation
    private void PlayerAnimation()
    {
        animator.SetBool("Run", true);
    }

    private void StopAnimation()
    {
        animator.SetBool("Run", false);
    }

    private void RunToTarget()
    {
        transform.position = Vector3.MoveTowards(transform.position, targetPosition, speed * Time.deltaTime);

        if(Vector3.Distance(transform.position, targetPosition) <= distance)
        {
            isAnimation = false;
            StopAnimation();
        }
    }

    private void OnTriggerEnter(UnityEngine.Collider other)
    {
        if(other.CompareTag("Player") && isAnimation == false)
        {
            targetPosition = other.gameObject.transform.position; // player ��ġ
            myPosition = transform.position; // hellper ��ġ
            Vector3 targetDirection = (targetPosition - myPosition).normalized; // target ����
            float dot = Vector3.Dot(myForward.normalized, targetDirection); // ����
            float theta = Mathf.Acos(dot) * Mathf.Rad2Deg; // ��
            isAnimation = theta <= angle / 2;
            targetPosition += new Vector3(0, 0, -2.0f);

            if(isAnimation)
            {
                Quaternion rotation = Quaternion.LookRotation(targetDirection);
                transform.rotation = rotation;
            }
        }
    }
}
