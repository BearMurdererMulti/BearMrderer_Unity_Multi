using Photon.Pun;
using System;
using System.Collections;
using System.Collections.Generic;
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

    private string[] categoryNames = { "body", "eyes", "years", "mouth", "tail" };
    

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
        }
    }

    public void OnClickCheckButton()
    {
        // doll ĳ���� ���� ������Ʈ ����ȭ �� ��� ����
        //photonView.RPC("UpdateDollSkin", RpcTarget.AllBuffered);

        PrefabUtility.SaveAsPrefabAsset(doll, "Assets/BJH/Resources/CustomDoll.prefab");

        CharacterCustomConnection_BJH connection = new CharacterCustomConnection_BJH(customDictionary);
    }

    [PunRPC]
    private void UpdateDollSkin()
    {
        foreach(string keyName in customDictionary.Keys)
        {
            switch(keyName)
            {
                case "body":
                    bodyRenderer.material = bodyMaterials[customDictionary[keyName]];
                    continue;
                case "ears":
                    earsRenderer.sharedMesh = earsMesh[customDictionary[keyName]];
                    continue;
                case "motuh":
                    mouthRenderer.material = mouthMaterials[customDictionary[keyName]];
                    continue;
                case "eyes":
                    eyesRenderer.material = eyesMaterials[customDictionary[keyName]];
                    continue;
                case "tail":
                    tailRenderer.sharedMesh = tailMesh[customDictionary[keyName]];
                    continue;
            }
        }
    }



    // Ŀ���� ����� �����ϴ� ��ư�� ������
    // Instantiate���ִ� ��ư
    internal void SetCustomDictionary(string keyName, int index)
    {
        // ��ųʸ� ����
        customDictionary[keyName] = index;

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
