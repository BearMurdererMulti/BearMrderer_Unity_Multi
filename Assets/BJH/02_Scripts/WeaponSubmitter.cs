using Photon.Pun;
using System.Collections;
using System.Collections.Generic;
using UnityEngine;

public class WeaponSubmitter : MonoBehaviourPunCallbacks
{
    [SerializeField] private Transform putdownTr; // ��� ���������� ��ġ ��Ƽ� �Ʒ� �ڵ�� assign
    [SerializeField] private List<GameObject> weaponPrefabs = new List<GameObject>(); // PhotonNetwork.Instantiate�� ���⸦ ã���� �����յ� ���� �̸� ������ �̸��� �����ϴ°� ��� ���� -����ȯ- �����丵 ���
    private void Start()
    {
        WeaponManager.Instance.weaponSubmitter = this; // �ڱ� �ڽ��� �ڵ�� assign

        putdownTr = WeaponManager.Instance.SubmitWeaponPosition;
    }

    // ���� ���� �޼���
    // ��ư�� ������ �Ŵ������� �ش� �޼��带 ȣ�� �� ��
    // ������ ��ġ�� ���õ� gameobject�� ��������
    // �̰� ����ȭ �Ǿ�� �ϴ°�.. yes.. �׷� �ܼ��� ���� �ؾߵ�
    // ���⼭ weapon prefab�� ��� ��� �ִٰ� string�̶� ��ġ�ϴ� ���⸦ �������� �ؾߵǰ�
    // rpc�� ȣ�� �ؾߵ�
    // ��ħ �÷��̾� ���� �޷������� �� photonview ����ϸ� �� ��
    // ���⼭ pun �ް� �ƴ϶� �긦 ȣ���ϴ� �ʱ� �޼��忡 pun�� �޾ƾ�
    
    public void PutDownWeaponOnTr(string weaponName)
    {
        Debug.Log("weaponsubmitter�Դϴ�. ���� �������ڻ�. ���� �����" + weaponName);
        foreach(GameObject weapon in weaponPrefabs)
        {
            if(weapon.name == weaponName)
            {
                GameObject go = Instantiate(weapon, putdownTr.position, Quaternion.identity);
                Vector3 targetScale = new Vector3(0.3f, 0.3f, 0.3f);
                go.transform.localScale = targetScale;
                StartCoroutine(CoDeleteWeapon(go));
            }
        }

        WeaponManager.Instance.ChangeActice_InventoryImage();

        PhotonView pv = ChatManager.instance.GetComponent<PhotonView>();
        pv.RPC("SetWeapon", RpcTarget.All, weaponName);
        //pv.RPC("StartTalkinterrogation", RpcTarget.MasterClient);
        ChatManager.instance.StartTalkinterrogation();
    }

    IEnumerator CoDeleteWeapon(GameObject go)
    {
        yield return new WaitForSeconds(118f);
        Destroy(go);
    }
}
