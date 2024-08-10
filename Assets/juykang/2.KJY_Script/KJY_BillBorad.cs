using System.Collections;
using System.Collections.Generic;
using UnityEngine;

public class KJY_BillBorad : MonoBehaviour
{
    [SerializeField] Camera detectiveCamera;
    [SerializeField] Camera assistantCamera;

    private void Start()
    {
        if (InfoManagerKJY.instance.role == "Detective")
        {
           detectiveCamera = GameObject.Find("DollMainCamera").GetComponent<Camera>();
        }
        else
        {
            assistantCamera = GameObject.Find("DogMainCamera").GetComponent<Camera>();
        }
    }
    // Update is called once per frame
    void Update()
    {
        if (InfoManagerKJY.instance.role == "Detective")
        {
            if (detectiveCamera != null)
            {
                transform.forward = detectiveCamera.transform.forward;
            }
        }
        else
        {
            if (assistantCamera != null)
            {
                transform.forward = assistantCamera.transform.forward;
            }
        }
    }
}
