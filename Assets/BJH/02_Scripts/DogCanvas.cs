using System.Collections;
using System.Collections.Generic;
using UnityEngine;

public class DogCanvas : MonoBehaviour
{
    [SerializeField] private GameObject inventoryImage;
    [SerializeField] private InventoryItems inventoryItems;

    private void Start()
    {
        GameObject weaponManagerGameobject = GameObject.Find("WeaponManager");
        weaponManagerGameobject.GetComponent<WeaponManager>().inventoryItems = inventoryItems; 
        Debug.Log(weaponManagerGameobject.GetComponent<WeaponManager>().inventoryItems.ToString() + "¾î½ÎÀÎ Àß µÆ³Ä?");
        inventoryImage.SetActive(false);
    }
}
