using Photon.Pun;
using System.Collections;
using System.Collections.Generic;
using Unity.VisualScripting;
using UnityEngine;
using UnityEngine.UI;

public class WeaponPicker : MonoBehaviourPunCallbacks
{
    [SerializeField] private Button pickButton;
    [SerializeField] private float outlineWidth;
    [SerializeField] private Color outlineColor;
    private GameObject pickedWeapon;
    
    //[SerializeField] public List<Sprite> weaponSprites = new List<Sprite>(); // ���� �̹�����
    [SerializeField] private InventoryItems inventoryItems; // ���̾��Ű�� �ִ� canvas > inventoryItems�� assign
    //[SerializeField] private Dictionary<string, Sprite> dicWeaponSprite = new Dictionary<string, Sprite>();


    private void Start()
    {
        pickButton.interactable = false;
    }

    private void Update()
    {
        if(Input.GetKeyDown(KeyCode.L))
        {
            AddWeaponInInventory();
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
        if (pickButton.interactable)
        {
            pickedWeapon.gameObject.SetActive(false); // ã�� ������Ʈ ��Ȱ��ȭ

            PhotonView targetPhotonView = GameObject.Find("WeaponManager").GetComponent<PhotonView>();

            targetPhotonView.RPC("UpdateInventoryImage", RpcTarget.All, pickedWeapon.name);
        }

    }
}
