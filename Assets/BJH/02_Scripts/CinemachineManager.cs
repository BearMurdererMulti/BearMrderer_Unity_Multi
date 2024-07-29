using System.Collections;
using System.Collections.Generic;
using UnityEngine;

public class CinemachineManager : MonoBehaviour
{
    [SerializeField] GameObject doll, dog;
    [SerializeField] float setSpawnTime;

    [SerializeField] List<Animator> trafficLightAnimators;
    [SerializeField] float setTrafficLightTime;


    private void Start()
    {
        StartCoroutine(CoSpawnDollAndDog());
        StartCoroutine(CoTurnOffTrafficLight());
    }

    IEnumerator CoSpawnDollAndDog()
    {
        yield return new WaitForSeconds(setSpawnTime);
        doll.SetActive(true);
        dog.SetActive(true);
    }

    IEnumerator CoTurnOffTrafficLight()
    {
        yield return new WaitForSeconds(setTrafficLightTime);
        foreach(Animator animator in trafficLightAnimators)
        {
            animator.SetBool("isExit", true);
        }
    }


}
