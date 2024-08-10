using System.Collections;
using System.Collections.Generic;
using UnityEngine;
using UnityEngine.UI;

public class KJY_KeyWord : MonoBehaviour
{
    [SerializeField] List<Image> weaponSprite;
    [SerializeField] List<KeyWordData> keyWordData;
    [SerializeField] Button btn;
    [SerializeField] GameObject grid;
    private List<int> index = new List<int>();
    private int i = 0;

    private void Start()
    {
        grid.SetActive(false);
    }

    public void SetUI(string name)
    {
        keyWordData[i].SetKeyWordUI(name);
        i++;
    }
}
