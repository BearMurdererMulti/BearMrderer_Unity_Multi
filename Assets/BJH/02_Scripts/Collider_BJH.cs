using System.Collections;
using System.Collections.Generic;
using Unity.VisualScripting;
using UnityEngine;

public class Collider_BJH : MonoBehaviour
{
    public bool isTalking = false;

    private void OnTriggerEnter(UnityEngine.Collider other)
    {
        //print("트리거에 닿임");
        //if(other.gameObject.GetComponent<Collider>().isTalking == true)
        //{
        //    return;
        //}
        //if (other.gameObject.CompareTag("Npc") && other.gameObject.name != gameObject.name && other.gameObject.GetComponent<Collider_BJH>().isTalking == false && isTalking == false)

        if (other.gameObject.CompareTag("Npc") && other.gameObject.name != gameObject.name && other.gameObject.GetComponent<Collider_BJH>().isTalking == false)
        {
            // 내가 상대와 닿이면?
            // 나를 대화중인 상태로 변경하고
            // 걷기를 멈추며, 서로를 바라본다.
            isTalking = true;
            other.gameObject.GetComponent<Collider_BJH>().isTalking = true;

            gameObject.GetComponent<NPC>().isWalking = false;
            other.gameObject.GetComponent<NPC>().isWalking = false;

            gameObject.transform.LookAt(other.transform.position);
            other.transform.LookAt(gameObject.transform.position);


            GameObject go = other.gameObject;

            // 그리고 10초 뒤 다시 움직인다.
            StartCoroutine("CoWaitASec", go);
        }


    }

    IEnumerator CoWaitASec(GameObject go)
    {
        yield return new WaitForSeconds(10f);

        gameObject.GetComponent<NPC>().isWalking = true;
        isTalking = false;

        yield return new WaitForSeconds(3f);

        go.GetComponent<Collider_BJH>().isTalking = false;
        go.GetComponent<NPC>().isWalking = true;
    }
}
