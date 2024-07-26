using System;
using System.Collections.Generic;
using UnityEngine;

public class WeaponManager : MonoBehaviour
{
    #region instance
    private static WeaponManager _instance;

    public static WeaponManager Instance
    {
        get { return _instance; }
    }
            
    #endregion

    [SerializeField] private List<GameObject> weaponList = new List<GameObject>();
    [SerializeField] private List<Transform> weaponSpawnPoints = new List<Transform>();
    [SerializeField] private List<int> generatedNumbers = new List<int>();

    
    [SerializeField] private InventoryItems inventoryItems;



    private void Awake()
    {
        InitInstance();

        ReplaceWeapons(weaponList.Count);
    }

    private void InitInstance()
    {
        if (_instance == null)
        {
            _instance = this;
        }
        else
        {
            Destroy(gameObject);
        }
    }

    private void ReplaceWeapons(int count)
    {
        generatedNumbers.Clear();
        int index = 0;
        while(generatedNumbers.Count < count)
        {
            int randomNumber = UnityEngine.Random.Range(0, weaponSpawnPoints.Count);

            if(!generatedNumbers.Contains(randomNumber))
            {
                generatedNumbers.Add(randomNumber);
                weaponList[index].transform.position = weaponSpawnPoints[randomNumber].position;
                weaponList[index].GetComponent<MeshRenderer>().enabled = false;
                index++;
            }
        }

    }

    public void UpdateInventoryImage(Sprite sprite)
    {
        Debug.Log("업데이트 요청을 game manager가 받았습니다.");
        inventoryItems.UpdateInventoryItemImages(sprite);
    }
}
