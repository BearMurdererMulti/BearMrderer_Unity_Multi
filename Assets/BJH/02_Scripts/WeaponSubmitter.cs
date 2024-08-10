using Photon.Pun;
using System.Collections;
using System.Collections.Generic;
using UnityEngine;

public class WeaponSubmitter : MonoBehaviourPunCallbacks
{
    [SerializeField] private Transform putdownTr; // ��� ���������� ��ġ ��Ƽ� �Ʒ� �ڵ�� assign
    [SerializeField] private List<GameObject> weaponPrefabs; // PhotonNetwork.Instantiate�� ���⸦ ã���� �����յ� ���� �̸� ������ �̸��� �����ϴ°� ��� ���� -����ȯ- �����丵 ���
    private void Start()
    {
        WeaponManager.Instance.weaponSubmitter = this; // �ڱ� �ڽ��� �ڵ�� assign

        weaponPrefabs = new List<GameObject>(); // �ʱ�ȭ

        putdownTr = transform.Find("SubmitWeaponPosition").transform; // ���� ���������� ���� ��ġ�� ��ü���� ã�� �ʰ� �ڽ� ���׿� ��Ƽ� �ڽ����� ã��
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
                PhotonNetwork.Instantiate(weaponName, putdownTr.position, Quaternion.identity);
            }
        }

        WeaponManager.Instance.ChangeActice_InventoryImage();

        PhotonView pv = ChatManager.instance.GetComponent<PhotonView>();
        pv.RPC("SetWeapon", RpcTarget.All, weaponName);
        //pv.RPC("StartTalkinterrogation", RpcTarget.MasterClient);
        ChatManager.instance.StartTalkinterrogation();
    }
}
