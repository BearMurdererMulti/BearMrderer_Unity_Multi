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

    [SerializeField] private List<Image> imtems; // canvs�� imtems�ȿ� item�� �ִ�.. �̰� ���� assign
    [SerializeField] private string _selctedWeaponImageName;

    public string SelectedWeaponImageName
    {
        get { return _selctedWeaponImageName; }
    }

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
                WeaponManager.Instance.inventoryItems = dollInventoryItems;
                inventoryImage.SetActive(false);
            }
            else
            {
                WeaponManager.Instance.inventoryItems = dogInventoryItems;
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
        pv.RPC("PutdownSelectedWeapon", RpcTarget.All, _selctedWeaponImageName);
        //pv.RPC("PutdownSelectedWeapon", RpcTarget.All, weaponName);
    }

    // item�� onclick�� �߰�
    // �׸��� �ڱ� �ڽ��� ����
    public void OnClick_SelectWeapon(Image itemSourceImage)
    {
        _selctedWeaponImageName = itemSourceImage.sprite.name;
    }
}
