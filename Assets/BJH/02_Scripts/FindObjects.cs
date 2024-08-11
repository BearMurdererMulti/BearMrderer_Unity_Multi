using Photon.Pun;
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
        if(Input.GetKeyDown(KeyCode.F))
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
            AudioManager.Instnace.PlayEffect(SoundEffect_List.Pop_SoundEffect, 0.3f, 0f);

            StartCoroutine(CoOffDogHearts());
            gameObject.GetComponent<DogCanvas02>().SetActiveDogheard(true);
        }
    }

    IEnumerator CoOffDogHearts()
    {
        yield return new WaitForSeconds(2f);
        gameObject.GetComponent<DogCanvas02>().SetActiveDogheard(false);
    }
}
