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
    [SerializeField] private GameObject pickedWeapon;

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
            Debug.Log("무기네요");

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
        pickedWeapon.gameObject.SetActive(false);
    }
}
