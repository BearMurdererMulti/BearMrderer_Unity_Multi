using System;
using System.Collections;
using System.Collections.Generic;
using UnityEngine;

public class LobbyScene : MonoBehaviour
{
    [SerializeField] GameObject[] characterPrefabs; // 랜덤으로 스폰 하고싶은 character들의 프리팹을 직접 assign
    [SerializeField] Transform spawnTransofm; // 해당 오브젝트의 mesh는 off한채로 시작

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
