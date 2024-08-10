using Photon.Pun;
using System.Collections;
using System.Collections.Generic;
using UnityEngine;
using UnityEngine.UI;

public class DogCanvas : MonoBehaviour
{
    [SerializeField] public GameObject inventoryImage;
    [SerializeField] private InventoryItems dollInventoryItems;
    [SerializeField] private InventoryItems dogInventoryItems;

    [SerializeField] private List<Image> imtems; // canvs�� imtems�ȿ� item�� �ִ�.. �̰� ���� assign
    [SerializeField] private string _selectedWeaponImageName;

    public string SelectedWeaponImageName
    {
        get { return _selectedWeaponImageName; }
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

        WeaponManager.Instance.canvas = this.gameObject;
    }

    // ������ ���õ� ����� �Բ� ��������
    public void OnClick_PuntDownButton()
    {
        Debug.Log("���⸦ �������ڽ��ϴ� ����..");
        Debug.Log($"�������� ���� �̸��� {_selectedWeaponImageName} �Դϴ�.");


        PhotonView pv = WeaponManager.Instance.GetComponent<PhotonView>();

        pv.RPC("PutdownSelectedWeapon", RpcTarget.All, _selectedWeaponImageName);
        //pv.RPC("PutdownSelectedWeapon", RpcTarget.All, weaponName);
    }

    // item�� onclick�� �߰�
    // �׸��� �ڱ� �ڽ��� ����
    public void OnClick_SelectWeapon(Image itemSourceImage)
    {
        _selectedWeaponImageName = itemSourceImage.sprite.name;
    }
}
