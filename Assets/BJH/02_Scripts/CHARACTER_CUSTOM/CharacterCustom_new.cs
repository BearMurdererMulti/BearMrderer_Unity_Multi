using Photon.Pun;
using System;
using System.Collections;
using System.Collections.Generic;
using System.Security.Cryptography;
using UnityEditor;
using UnityEngine;
using UnityEngine.Events;
using UnityEngine.Timeline;
using UnityEngine.UI;

public class CharacterCustom_new : MonoBehaviourPunCallbacks
{
    [SerializeField] private GameObject doll;

    [SerializeField] List<Material> bodyMaterials = new List<Material>();
    [SerializeField] SkinnedMeshRenderer bodyRenderer;

    [SerializeField] List<Material>  eyesMaterials = new List<Material>();
    [SerializeField] SkinnedMeshRenderer eyesRenderer;


    [SerializeField] List<Mesh> earsMesh = new List<Mesh>();
    [SerializeField] SkinnedMeshRenderer earsRenderer; // ����� Mesh, material ����

    [SerializeField] List<Material> mouthMaterials = new List<Material>();
    [SerializeField] SkinnedMeshRenderer mouthRenderer;

    [SerializeField] List<Mesh> tailMesh = new List<Mesh>();
    [SerializeField] SkinnedMeshRenderer tailRenderer; // ����� Mesh, material ����
    

    private Dictionary<string, int> customDictionary = new Dictionary<string, int>();
    [SerializeField] List<GameObject> contents = new List<GameObject>();
    private int preContentIndex;

    private string[] categoryNames = { "body", "eyes", "ears", "mouth", "tail" };
    

    [SerializeField] private ScrollRect scrollRect;

    private void Start()
    {
        contents[0].SetActive(true);
        preContentIndex = 0;
        for (int i = 1; i < contents.Count; i++)
        {
            contents[i].SetActive(false);
        }

        foreach(string s in categoryNames)
        {
            customDictionary[s] = 0;
            InfoManagerKJY.instance.customDictionary[s] = 0;
        }
    }

    private void Update()
    {
        if (InfoManagerKJY.instance.customDictionary["eyes"] == 4)
        {
            Debug.Log("4�� �ٲ����");
        }

        Debug.Log(InfoManagerKJY.instance.customDictionary["eyes"]);
    }

    public void OnClickCheckButton()
    {
        // ���忡�� ���� �ȵǴ� �̽�
        //PrefabUtility.SaveAsPrefabAsset(doll, "Assets/BJH/Resources/CustomDoll.prefab");
        foreach(string s in InfoManagerKJY.instance.customDictionary.Keys)
        {
            Debug.Log("���� ���õ� dictionary"); // �� ���⼭ eyes�� 4�� �ɱ�.. �ؿ��� �� ���µ�
            Debug.Log(s + "    " + InfoManagerKJY.instance.customDictionary[s]);
        }
        CharacterCustomConnection_BJH connection = new CharacterCustomConnection_BJH(customDictionary);
    }

    [PunRPC]
    private void UpdateDollSkin(string keyName, int index)
    {
        InfoManagerKJY.instance.customDictionary[keyName] = index;
        Debug.Log(keyName + "    " + index);
        Debug.Log(InfoManagerKJY.instance.customDictionary[keyName]);
    }



    // Ŀ���� ����� �����ϴ� ��ư�� ������
    // Instantiate���ִ� ��ư
    public void SetCustomDictionary(string keyName, int index)
    {
        // ��ųʸ� ����
        customDictionary[keyName] = index;

        // doll ĳ���� ���� ������Ʈ ����ȭ
        photonView.RPC("UpdateDollSkin", RpcTarget.All, keyName, index);

        // ĳ���Ϳ� ����
        var material = bodyRenderer.material; // �ӽ�
        var mesh = bodyRenderer.sharedMesh;
        switch(keyName)
        {
            case "body":
                material = bodyRenderer.material;
                material = bodyMaterials[index];
                bodyRenderer.material = material;

                material = earsRenderer.material;
                material = bodyMaterials[index];
                earsRenderer.material = material;

                material = tailRenderer.material;
                material = bodyMaterials[index];
                tailRenderer.material = material;
                break;
            case "eyes":
                var materials = eyesRenderer.materials;
                materials[0] = eyesMaterials[index];
                materials[1] = eyesMaterials[index];
                eyesRenderer.materials = materials;
                break;
            case "ears":
                mesh = earsRenderer.sharedMesh;
                mesh = earsMesh[index];
                earsRenderer.sharedMesh = mesh;
                break;
            case "mouth":
                material = mouthRenderer.material;
                material = mouthMaterials[index];
                mouthRenderer.material = material;
                break;
            case "tail":
                mesh = tailRenderer.sharedMesh;
                mesh = tailMesh[index];
                tailRenderer.sharedMesh = mesh;
                break;
        }
    }

    internal void ActiveCategoryContent(int index)
    {
        if(preContentIndex == index)
        {
            return;
        }

        contents[preContentIndex].SetActive(false);
        preContentIndex = index;
        contents[preContentIndex].SetActive(true);
        scrollRect.content = contents[index].GetComponent<RectTransform>();
    }
}
