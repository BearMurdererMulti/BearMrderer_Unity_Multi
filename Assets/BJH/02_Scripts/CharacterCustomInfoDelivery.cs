using System.Collections;
using System.Collections.Generic;
using System.Security.Cryptography;
using UnityEngine;

// ĳ���� ������ ���� �ó׸ӽ� ĳ���� ���� �� �ΰ��� ĳ���� ���������� ���Ǵ� Ŭ����
// ������ ����Ǹ� �ı��ص� �˴ϴ�.
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

        // �ӽ�
        foreach(var key in _customDictionary.Keys)
        {
            Debug.Log($"{key}�� value�� {_customDictionary[key]} �Դϴ�.");
        }
    }
}
