using System.Collections;
using System.Collections.Generic;
using UnityEngine;
using UnityEngine.UI;

public class KJY_SelectNpc : MonoBehaviour
{
    Ray ray;
    RaycastHit hit;
    Camera cam;
    List<GameObject> list;

    Quaternion rotation;

    [SerializeField] Button_BJH btn;
    [SerializeField] Button_BJH btn2;

    [SerializeField] GameObject selectUI;

    [SerializeField] GameObject obj;

    // Start is called before the first frame update
    void Start()
    {
        cam = Camera.main;
        list = KJY_CitizenManager.Instance.npcList;
        rotation = Quaternion.identity;
    }

    // Update is called once per frame
    void Update()
    {
        if (Input.GetMouseButtonDown(0))
        {
            ray = cam.ScreenPointToRay(Input.mousePosition);
            if (Physics.Raycast(ray, out hit) )
            {
                if (hit.collider.CompareTag("Npc"))
                {
                    rotation = hit.collider.gameObject.transform.rotation;
                    hit.collider.gameObject.transform.LookAt(cam.transform.position);
                    selectUI.SetActive(true);
                    obj = hit.collider.gameObject;
                }
            }
        }
    }

    public void SelectNpc()
    {
        if (obj != null)
        {
            obj.SetActive(false);
        }
    }

    public void CancleSeletNpc()
    {

    }
}
