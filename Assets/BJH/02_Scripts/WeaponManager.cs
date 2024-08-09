using Photon.Pun;
using System;
using System.Collections.Generic;
using UnityEngine;

public class WeaponManager : MonoBehaviourPunCallbacks
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

    
    [SerializeField] public InventoryItems inventoryItems; // 캐릭터 canvas에서 코드로 assign

    [SerializeField] public List<Sprite> weaponSprites = new List<Sprite>(); // 무기 이미지들


    private void Awake()
    {
        InitInstance();

        ReplaceWeapons(7); // 임시 // 무기 개수가 7개씩 올거임
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

    [PunRPC]
    public void UpdateInventoryImage(string weaponName)
    {
        foreach (var sprite in weaponSprites)
        {
            if (weaponName == sprite.name)
            {
                Debug.Log("일치하는 이름의 sprite를 찾았습니다!");
                inventoryItems.UpdateInventoryItemImages(sprite);

                break;
            }
        }
    }
}
