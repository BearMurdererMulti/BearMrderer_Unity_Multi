using System.Collections;
using System.Collections.Generic;
using TMPro;
using UnityEngine;
using UnityEngine.UI;

public class KeyWordData : MonoBehaviour
{
    public string KeyWord = null;
    [SerializeField] private Image sprite;
    [SerializeField] private TextMeshProUGUI keyName;

    public void SetKeyWord()
    {
        ChatManager.instance.weapon = KeyWord;
    }

    public void SetKeyWordUI(string name)
    {
        sprite.sprite = Resources.Load<Sprite>("WeaponSprite/" + name);
        keyName.text = name;
        KeyWord = name;
    }
}
