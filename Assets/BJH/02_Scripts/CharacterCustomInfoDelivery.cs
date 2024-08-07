using System.Collections;
using System.Collections.Generic;
using System.Security.Cryptography;
using UnityEngine;

// 캐릭터 정보를 토대로 시네머신 캐릭터 스폰 및 인게임 캐릭터 스폰용으로 사용되는 클래스
// 게임이 종료되면 파기해도 됩니다.
public class CharacterCustomInfoDelivery : MonoBehaviour
{
    private static CharacterCustomInfoDelivery _instance;

    [SerializeField] private SkinnedMeshRenderer test;

    [SerializeField] List<Material> bodyMaterials = new List<Material>();
    [SerializeField] List<Material> eyesMaterials = new List<Material>();
    [SerializeField] List<Mesh> earsMesh = new List<Mesh>();
    [SerializeField] List<Material> mouthMaterials = new List<Material>();
    [SerializeField] List<Mesh> tailMesh = new List<Mesh>();

    public Dictionary<string, int> _customDictionary = new Dictionary<string, int>();
    public Material targetBody, targetEyes, targetMouth;
    public Mesh targetEars, targetTail;

    public static CharacterCustomInfoDelivery Instance
    {
        get { return _instance; }
    }

    private void Awake()
    {
        if(_instance == null)
        {
            _instance = this;
            DontDestroyOnLoad(gameObject);
        }
        else
        {
            Destroy(gameObject);
        }
    }

    private void Update()
    {
        Debug.Log(test.material.ToString());
    }

    public void OnConnectedToServer(Dictionary<string, int> dic)
    {
        _customDictionary = dic;

        targetBody = bodyMaterials[_customDictionary["body"]];
        targetEyes = eyesMaterials[_customDictionary["eyes"]];
        targetEars = earsMesh[_customDictionary["ears"]];
        targetMouth = mouthMaterials[_customDictionary["mouth"]];
        targetTail = tailMesh[_customDictionary["tail"]];

        // 임시
        foreach(var key in _customDictionary.Keys)
        {
            Debug.Log($"{key}의 value는 {_customDictionary[key]} 입니다.");
        }
    }
}
