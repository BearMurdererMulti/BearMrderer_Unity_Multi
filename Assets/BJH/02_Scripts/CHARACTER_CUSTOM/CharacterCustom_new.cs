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
    [SerializeField] SkinnedMeshRenderer earsRenderer; // 변경시 Mesh, material 변경

    [SerializeField] List<Material> mouthMaterials = new List<Material>();
    [SerializeField] SkinnedMeshRenderer mouthRenderer;

    [SerializeField] List<Mesh> tailMesh = new List<Mesh>();
    [SerializeField] SkinnedMeshRenderer tailRenderer; // 변경시 Mesh, material 변경
    

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
        // doll 캐릭터 정보 업데이트 동기화 후 통신 진행
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



    // 커스텀 기능을 수행하는 버튼이 없으면
    // Instantiate해주는 버튼
    internal void SetCustomDictionary(string keyName, int index)
    {
        // 딕셔너리 설정
        customDictionary[keyName] = index;

        // 캐릭터에 적용
        var material = bodyRenderer.material; // 임시
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
