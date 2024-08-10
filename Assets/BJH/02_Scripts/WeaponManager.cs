using Photon.Pun;
using System;
using System.Collections.Generic;
using UnityEngine;
using UnityEngine.UI;

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
        
    [SerializeField] public GameObject canvas; // 강아지 canvas를 코드로 assign(dog canvas)
    [SerializeField] public InventoryItems inventoryItems; // 캐릭터 canvas에서 코드로 assign
    [SerializeField] public Button putDownButton; // 캐릭터가 가진 dog canvas 스크립트에서 코드로 assign
    [SerializeField] public WeaponSubmitter weaponSubmitter; // dog가 가진 스크립트에서 assign


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

    // 무기를 내려놓는 버튼 ui를 비/활성화 하는 메서드
    // 취조실 들어가고 나갈 때 주연이가 호출
    public void PutdownButtonActive()
    {
        putDownButton.gameObject.SetActive(!putDownButton.gameObject.activeSelf);
    }

    // 내려놓은 무기를 동기화하는 메서드
    [PunRPC]
    public void PutdownSelectedWeapon(string weaponName)
    {
        Debug.Log($"무기를 내려놓기 동기화 하겠습니다");
        weaponSubmitter.PutDownWeaponOnTr(weaponName);
    }

    public void ChangeActice_InventoryImage()
    {
        if(canvas != null)
        {
            DogCanvas dogCanvas = canvas.GetComponent<DogCanvas>();
            dogCanvas.inventoryImage.SetActive(!dogCanvas.inventoryImage.activeSelf);
        }
    }
}
