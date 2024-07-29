using System.Collections;
using System.Collections.Generic;
using UnityEngine;
using UnityEngine.UI;

public class InventoryItems : MonoBehaviour
{
    public List<Image> inventoryItemSelectors = new List<Image>();
    public List<Image> inventoryItemImages = new List<Image> ();
    private int index = 0;

    private void Start()
    {
        foreach (var selector in inventoryItemSelectors)
        {
            selector.enabled = false;
        }
    }

    public void UpdateInventoryItemImages(Sprite sprite)
    {
        Debug.Log("���޹��� sprite�� �����մϴ�.");
        inventoryItemImages[index].sprite = sprite;
        index++;
    }
}
