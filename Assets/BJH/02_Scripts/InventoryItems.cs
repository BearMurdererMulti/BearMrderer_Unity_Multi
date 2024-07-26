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
        Debug.Log("전달받은 sprite를 적용합니다.");
        inventoryItemImages[index].sprite = sprite;
        index++;
    }
}
