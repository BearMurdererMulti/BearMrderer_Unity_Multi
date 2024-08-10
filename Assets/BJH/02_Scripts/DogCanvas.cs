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

    [SerializeField] private List<Image> imtems; // canvs에 imtems안에 item이 있다.. 이걸 직접 assign
    [SerializeField] private string _selectedWeaponImageName;

    public string SelectedWeaponImageName
    {
        get { return _selectedWeaponImageName; }
    }

    [SerializeField] public Button putDownButton; // 취조실 갈 때만 on


    private void Start()
    {
        PhotonView pv = gameObject.GetComponent<PhotonView>();
        if(pv.IsMine)
        {
            // find 말고 인스턴스로 되는지 확인 하기 -변지환-
            // 그리고 각 변수를 넣지 말고 canvas자기 자신을 manager에 변수로 넣으면 모든 변수에 접근하지 쉽지 않을까? -변지환- 리팩토링 요소
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

    // 누르면 선택된 무기와 함께 내려놓음
    public void OnClick_PuntDownButton()
    {
        Debug.Log("무기를 내려놓겠습니다 후후..");
        Debug.Log($"내려놓을 무기 이름은 {_selectedWeaponImageName} 입니다.");


        PhotonView pv = WeaponManager.Instance.GetComponent<PhotonView>();

        pv.RPC("PutdownSelectedWeapon", RpcTarget.All, _selectedWeaponImageName);
        //pv.RPC("PutdownSelectedWeapon", RpcTarget.All, weaponName);
    }

    // item에 onclick에 추가
    // 그리고 자기 자신을 삽입
    public void OnClick_SelectWeapon(Image itemSourceImage)
    {
        _selectedWeaponImageName = itemSourceImage.sprite.name;
    }
}
