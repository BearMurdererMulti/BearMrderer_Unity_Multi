using Photon.Pun;
using System.Collections;
using System.Collections.Generic;
using UnityEngine;
using UnityEngine.UI;

public class KJY_KeyWord : MonoBehaviourPun
{
    public static KJY_KeyWord instance; 
    
    [SerializeField] List<Image> weaponSprite;
    [SerializeField] List<KeyWordData> keyWordData;
    [SerializeField] GameObject grid;
    private List<int> index = new List<int>();
    private int i = 0;

    private void Awake()
    {
        instance = this;
    }

    private void Start()
    {
        grid.SetActive(false);
    }

    [PunRPC]
    public void SetUI(string name)
    {
        keyWordData[i].SetKeyWordUI(name);
        i++;
    }
}
