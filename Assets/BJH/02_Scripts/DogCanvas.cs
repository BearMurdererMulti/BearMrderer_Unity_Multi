using Photon.Pun;
using System.Collections;
using System.Collections.Generic;
using UnityEngine;
using UnityEngine.UI;

public class DogCanvas : MonoBehaviour
{
    [SerializeField] private GameObject inventoryImage;
    [SerializeField] private InventoryItems dollInventoryItems;
    [SerializeField] private InventoryItems dogInventoryItems;
    [SerializeField] public Button putDownButton; // ������ �� ���� on


    private void Start()
    {
        PhotonView pv = gameObject.GetComponent<PhotonView>();
        if(pv.IsMine)
        {
            // find ���� �ν��Ͻ��� �Ǵ��� Ȯ�� �ϱ� -����ȯ-
            // �׸��� �� ������ ���� ���� canvas�ڱ� �ڽ��� manager�� ������ ������ ��� ������ �������� ���� ������? -����ȯ- �����丵 ���
            if(PhotonNetwork.IsMasterClient)
            {
                Debug.Log("Ž���Դϴ�.");
                WeaponManager.Instance.inventoryItems = dollInventoryItems;
                Debug.Log("Ž���� �κ��丮 ������� �ֽ��ϴ�." + dollInventoryItems +   "     " + WeaponManager.Instance.inventoryItems);

                inventoryImage.SetActive(false);
            }
            else
            {
                Debug.Log("������ �Դϴ�.");
                WeaponManager.Instance.inventoryItems = dogInventoryItems;
                Debug.Log("������ �κ��丮 ������� �ֽ��ϴ�." + dogInventoryItems + "     " + WeaponManager.Instance.inventoryItems);
                inventoryImage.SetActive(false);

                WeaponManager.Instance.putDownButton = putDownButton;
                putDownButton.gameObject.SetActive(false);
            }
        }
    }

    // ������ ���õ� ����� �Բ� ��������
    public void OnClick_PuntDownButton()
    {
        PhotonView pv = WeaponManager.Instance.GetComponent<PhotonView>();
        //pv.RPC("PutdownSelectedWeapon", RpcTarget.All, weaponName);
    }
}
