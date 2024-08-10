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
                Debug.Log("탐정입니다.");
                WeaponManager.Instance.inventoryItems = dollInventoryItems;
                Debug.Log("탐정의 인벤토리 아이템즈를 넣습니다." + dollInventoryItems +   "     " + WeaponManager.Instance.inventoryItems);

                inventoryImage.SetActive(false);
            }
            else
            {
                Debug.Log("강아지 입니다.");
                WeaponManager.Instance.inventoryItems = dogInventoryItems;
                Debug.Log("강아지 인벤토리 아이템즈를 넣습니다." + dogInventoryItems + "     " + WeaponManager.Instance.inventoryItems);
                inventoryImage.SetActive(false);

                WeaponManager.Instance.putDownButton = putDownButton;
                putDownButton.gameObject.SetActive(false);
            }
        }
    }

    // 누르면 선택된 무기와 함께 내려놓음
    public void OnClick_PuntDownButton()
    {
        PhotonView pv = WeaponManager.Instance.GetComponent<PhotonView>();
        //pv.RPC("PutdownSelectedWeapon", RpcTarget.All, weaponName);
    }
}
