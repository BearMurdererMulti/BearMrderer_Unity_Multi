using System.Collections;
using System.Collections.Generic;
using UnityEngine;
using UnityEngine.UI;

public class InventoryItems : MonoBehaviour
{
    public List<Image> inventoryItemImages = new List<Image> ();
    private int index = 0;

    public void UpdateInventoryItemImages(Sprite sprite)
    {
        Debug.Log("���޹��� sprite�� �����մϴ�.");
        inventoryItemImages[index].sprite = sprite;
        index++;
    }
}
