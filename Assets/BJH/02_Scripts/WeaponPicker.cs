using System.Collections;
using System.Collections.Generic;
using Unity.VisualScripting;
using UnityEngine;
using UnityEngine.UI;

public class WeaponPicker : MonoBehaviour
{
    [SerializeField] private Button pickButton;
    [SerializeField] private float outlineWidth;
    [SerializeField] private Color outlineColor;
    private GameObject pickedWeapon;
    
    [SerializeField] public List<Sprite> weaponSprites = new List<Sprite>(); // 무기 이미지들
    [SerializeField] private InventoryItems inventoryItems; // 하이어라키에 있는 canvas > inventoryItems를 assign
    //[SerializeField] private Dictionary<string, Sprite> dicWeaponSprite = new Dictionary<string, Sprite>();


    private void Start()
    {
        pickButton.interactable = false;
    }

    private void Update()
    {
        if(Input.GetKeyDown(KeyCode.L))
        {
            if(pickButton.IsActive())
            {
                AddWeaponInInventory();
            }
        }
    }

    private void OnTriggerEnter(Collider other)
    {
        Debug.Log(other.gameObject.name);
        MeshRenderer renderer = other.GetComponent<MeshRenderer>();

        if (renderer.enabled && other.CompareTag("Weapon"))
        {
            Material[] materials = renderer.materials;

            foreach (var material in materials)
            {
                material.SetFloat("_Outline", outlineWidth);
                material.SetColor("_OutlineColor", outlineColor);
            }
            pickedWeapon = other.gameObject;
            pickButton.interactable = true;
        }
    }

    private void OnTriggerExit(Collider other)
    {
        Debug.Log(other.gameObject.name);
        MeshRenderer renderer = other.GetComponent<MeshRenderer>();

        if (renderer.enabled && other.CompareTag("Weapon"))
        {
            Material[] materials = renderer.materials;

            foreach (var material in materials)
            {
                material.SetFloat("_Outline", 0f);
                material.SetColor("_OutlineColor", outlineColor);
            }
            pickedWeapon = null;
            pickButton.interactable = false;
        }
    }

    public void AddWeaponInInventory()
    {
        pickedWeapon.gameObject.SetActive(false); // 찾은 오브젝트 비활성화

        foreach (var sprite in weaponSprites)
        {
            if(pickedWeapon.name == sprite.name)
            {
                Debug.Log("일치하는 이름의 sprite를 찾았습니다!");
                WeaponManager.Instance.UpdateInventoryImage(sprite);
                break;
            }
        }
    }
}
