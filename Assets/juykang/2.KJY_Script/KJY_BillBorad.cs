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
           detectiveCamera = GameObject.FindWithTag("DetectiveCamera").GetComponent<Camera>();
        }
        else
        {
            assistantCamera = Camera.main;
        }
    }
    // Update is called once per frame
    void Update()
    {
        if (InfoManagerKJY.instance.role == "Detective")
        {
            transform.forward = detectiveCamera.transform.forward;
        }
        else
        {
            transform.forward = assistantCamera.transform.forward;
        }
    }
}
