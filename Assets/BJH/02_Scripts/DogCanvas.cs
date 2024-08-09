using Photon.Pun;
using System.Collections;
using System.Collections.Generic;
using UnityEngine;

public class DogCanvas : MonoBehaviour
{
    [SerializeField] private GameObject inventoryImage;
    [SerializeField] private InventoryItems dollInventoryItems;
    [SerializeField] private InventoryItems dogInventoryItems;


    private void Start()
    {
        GameObject weaponManagerGameobject = GameObject.Find("WeaponManager");

        if(PhotonNetwork.IsMasterClient)
        {
            weaponManagerGameobject.GetComponent<WeaponManager>().inventoryItems = dollInventoryItems;
            inventoryImage.SetActive(false);
        }
        else
        {
            weaponManagerGameobject.GetComponent<WeaponManager>().inventoryItems = dogInventoryItems; 
            inventoryImage.SetActive(false);

        }
    }
}
