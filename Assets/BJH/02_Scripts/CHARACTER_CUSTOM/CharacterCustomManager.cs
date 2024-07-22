using System.Collections;
using System.Collections.Generic;
using UnityEngine;
using UnityEngine.UI;

public class CharacterCustomManager : MonoBehaviour
{
   [SerializeField] private List<Button> bodyButtons = new List<Button>();

    [SerializeField] private CharacterCustom_new characterCustom;
    
    public void OnClickCustomButton(string keyName)
    {
        int buttonNumber = int.Parse(UnityEngine.EventSystems.EventSystem.current.currentSelectedGameObject.name);
        characterCustom.SetCustomDictionary(keyName, buttonNumber);
    }

    public void OnClickCategory(int index)
    {
        characterCustom.ActiveCategoryContent(index);
    }
}
