using System;
using System.Collections.Generic;
using UnityEngine;

public class WeaponManager : MonoBehaviour
{
    [SerializeField] private List<GameObject> weaponList = new List<GameObject>();
    [SerializeField] private List<Transform> weaponSpawnPoints = new List<Transform>();
    [SerializeField] private List<int> generatedNumbers = new List<int>();


    private void Awake()
    {
        ReplaceWeapons(weaponList.Count);
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
}
