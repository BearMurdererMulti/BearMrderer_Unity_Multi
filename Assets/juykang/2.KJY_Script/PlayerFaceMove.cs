using System.Collections;
using System.Collections.Generic;
using UnityEngine;

public class PlayerFaceMove : MonoBehaviour
{
    [SerializeField] Transform target;
    public float speed = 1f;

    // Start is called before the first frame update
    void Start()
    {

    }

    // Update is called once per frame
    void Update()
    {
        if (ChatManager.instance.talk == true)
        {
            target = ChatManager.instance.nowNpc.transform;
            Vector3 l_vector = target.transform.position - transform.position;
            l_vector.y = 0;

            Quaternion rot = Quaternion.LookRotation(l_vector.normalized);

            transform.rotation = rot;
        }
    }
}
