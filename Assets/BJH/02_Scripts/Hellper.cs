using System.Collections;
using System.Collections.Generic;
using System.Reflection;
using UnityEditor;
using UnityEngine;
using UnityEngine.Rendering.PostProcessing;

public class Hellper : MonoBehaviour
{
    [Header("중력")]
    private CharacterController cc;
    public float gravity = -9.8f;
    private Vector3 velocity;

    [Header("내적")]
    Vector3 myPosition;
    Vector3 targetPosition;
    Vector3 myForward;
    float speed = 3.0f;
    public float distance = 1.0f;
    public int angle = 150;

    [Header("애니메이션")]
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
            Debug.Log("player쪽으로 이동합니다.");
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
            targetPosition = other.gameObject.transform.position; // player 위치
            myPosition = transform.position; // hellper 위치
            Vector3 targetDirection = (targetPosition - myPosition).normalized; // target 방향
            float dot = Vector3.Dot(myForward.normalized, targetDirection); // 내적
            float theta = Mathf.Acos(dot) * Mathf.Rad2Deg; // 각
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
