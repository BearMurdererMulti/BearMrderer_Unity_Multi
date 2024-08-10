using System;
using System.Collections;
using System.Collections.Generic;
using UnityEngine;

public class LobbyScene : MonoBehaviour
{
    [SerializeField] GameObject[] characterPrefabs; // �������� ���� �ϰ���� character���� �������� ���� assign
    [SerializeField] Transform spawnTransofm; // �ش� ������Ʈ�� mesh�� off��ä�� ����

    // Start is called before the first frame update
    private void awake()
    {
        characterPrefabs = new GameObject[13];
    }

    private void Start()
    {
        SpawnRandomCharacterPrefab();
    }

    private void SpawnRandomCharacterPrefab()
    {
        int randomNumber = UnityEngine.Random.Range(0, characterPrefabs.Length);
        GameObject go = Instantiate(characterPrefabs[randomNumber], spawnTransofm.position, new Quaternion(0, 180f, 0, 0));
        Vector3 scale = new Vector3(2.5f, 2.5f, 2.5f);
        go.transform.localScale = scale;
    }
}
