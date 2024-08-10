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
        
    [SerializeField] public GameObject canvas; // ������ canvas�� �ڵ�� assign(dog canvas)
    [SerializeField] public InventoryItems inventoryItems; // ĳ���� canvas���� �ڵ�� assign
    [SerializeField] public Button putDownButton; // ĳ���Ͱ� ���� dog canvas ��ũ��Ʈ���� �ڵ�� assign
    [SerializeField] public WeaponSubmitter weaponSubmitter; // dog�� ���� ��ũ��Ʈ���� assign


    [SerializeField] public List<Sprite> weaponSprites = new List<Sprite>(); // ���� �̹�����


    private void Awake()
    {
        InitInstance();

        ReplaceWeapons(7); // �ӽ� // ���� ������ 7���� �ð���
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
                Debug.Log("��ġ�ϴ� �̸��� sprite�� ã�ҽ��ϴ�!");
                inventoryItems.UpdateInventoryItemImages(sprite);

                break;
            }
        }
    }

    // ���⸦ �������� ��ư ui�� ��/Ȱ��ȭ �ϴ� �޼���
    // ������ ���� ���� �� �ֿ��̰� ȣ��
    public void PutdownButtonActive()
    {
        putDownButton.gameObject.SetActive(!putDownButton.gameObject.activeSelf);
    }

    // �������� ���⸦ ����ȭ�ϴ� �޼���
    [PunRPC]
    public void PutdownSelectedWeapon(string weaponName)
    {
        Debug.Log($"���⸦ �������� ����ȭ �ϰڽ��ϴ�");
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
