using System.Collections;
using System.Collections.Generic;
using UnityEngine;

public class KJY_BuildingTest : MonoBehaviour
{
    [SerializeField] Camera cam;
    // Start is called before the first frame update
    void Start()
    {
        
    }

    // Update is called once per frame
    void Update()
    {
        Ray ray = cam.ViewportPointToRay(new Vector3(0.5f, 0.5f, 0f));
        RaycastHit hit;
        if (Physics.Raycast(ray, out hit, 10f))
        {
            if (hit.collider.CompareTag("Buildings"))
            {
                hit.collider.gameObject.SetActive(false);
            }
        }

    }
}
