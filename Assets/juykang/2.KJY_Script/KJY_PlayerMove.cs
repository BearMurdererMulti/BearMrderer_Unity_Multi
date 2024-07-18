using System.Collections;
using System.Collections.Generic;
using UnityEngine;

public class KJY_PlayerMove : MonoBehaviour
{
    //움직임 변수
    public float speed = 5;

    //점프 변수
    public int MaxJumpCount = 1;
    public float jumpPower = 5;
    public float gravity = -9.81f;
    float yVeclocity = 0;
    bool isJump = false;
    bool isRun = false;

    Transform body;
    //CC
    CharacterController cc;

    //애니메이션
    Animator animator;

    // Start is called before the first frame update
    void Start()
    {
        cc = GetComponent<CharacterController>();
        animator = transform.GetChild(0).GetComponent<Animator>();
        body = transform.GetChild(0);
    }

    // Update is called once per frame
    void Update()
    {
        if (ChatManager.instance.talk == true || KJY_CitizenManager.Instance.call == true)
        {
            animator.SetBool("Move", false);
            return;
        }
        //플레이어 움직임
        float h = Input.GetAxis("Horizontal");
        float v = Input.GetAxis("Vertical");

        Vector3 dirH = transform.right * h;
        Vector3 dirV = transform.forward * v;

        Vector3 dir = -dirH + -dirV;
        dir.Normalize();
        //body.forward = dir;
        if (dirH != Vector3.zero || dirV != Vector3.zero)
        {
            body.forward = dir;
            animator.SetBool("Move", true);
        }
        else
        {
            animator.SetBool("Move", false);
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
}
