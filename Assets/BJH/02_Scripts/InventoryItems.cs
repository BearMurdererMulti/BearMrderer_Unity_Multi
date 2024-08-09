using System.Collections;
using System.Collections.Generic;
using TMPro;
using UnityEngine;
using UnityEngine.UI;

public class InventoryItems : MonoBehaviour
{
    public List<Image> inventoryItemSelectors = new List<Image>();
    public List<Image> inventoryItemImages = new List<Image> ();
    private List<TextMeshProUGUI> weaponText = new List<TextMeshProUGUI> ();
    private int index = 0;

    private void Start()
    {
        foreach (var selector in inventoryItemSelectors)
        {
            selector.color = Color.white;
        }

        for (int i = 0; i < inventoryItemSelectors.Count; i++)
        {
            weaponText.Add(inventoryItemSelectors[i].transform.GetChild(1).GetComponent<TextMeshProUGUI>());
        }
    }

    public void UpdateInventoryItemImages(Sprite sprite)
    {
        Debug.Log("전달받은 sprite를 적용합니다.");
        inventoryItemImages[index].sprite = sprite;
        //weaponText[index].text = sprite.name; //KJY 추가
        index++;
    }
}
