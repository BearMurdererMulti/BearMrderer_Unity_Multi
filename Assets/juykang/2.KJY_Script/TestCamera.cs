using System.Collections;
using System.Collections.Generic;
using UnityEngine;

public class TestCamera : MonoBehaviour
{


    // Update is called once per frame
    void Update()
    {
        if (Input.GetKeyDown(KeyCode.Tab))
        {
            this.GetComponent<Camera>().enabled = !this.GetComponent<Camera>().enabled;
        }
    }
}
