using System.Collections;
using System.Collections.Generic;
using UnityEngine;

public class FindObjects : MonoBehaviour
{
    [SerializeField] private float initRadius;
    [SerializeField] private float targetRadius;
    [SerializeField] private float grouthRate; // �ݶ��̴��� Ŀ���� �ӵ�
    private bool isCircleActive = false;

    private void Update()
    {
        if(Input.GetKeyDown(KeyCode.P))
        {
            NearWeaponFind();
        }
    }

    public void NearWeaponFind()
    {
        // �ֺ��� �ִ� ��� weapon�� �����ؼ� �迭�� ����
        Collider[] colls = Physics.OverlapSphere(transform.position, targetRadius, 1 << 8);

        foreach (Collider coll in colls)
        {
            coll.gameObject.GetComponent<MeshRenderer>().enabled = true;
        }
    }
}
