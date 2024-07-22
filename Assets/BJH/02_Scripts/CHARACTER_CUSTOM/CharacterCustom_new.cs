using System;
using System.Collections;
using System.Collections.Generic;
using UnityEngine;
using UnityEngine.Events;
using UnityEngine.Timeline;
using UnityEngine.UI;

public class CharacterCustom_new : MonoBehaviour
{
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

    // 커스텀 기능을 수행하는 버튼이 없으면
    // Instantiate해주는 버튼
    private void InstantiateCustomButton()
    {
        
    }

    internal void SetCustomDictionary(string keyName, int type)
    {
        // 딕셔너리 설정
        customDictionary[keyName] = type;

        // 캐릭터에 적용
        var material = bodyRenderer.material; // 임시
        var mesh = bodyRenderer.sharedMesh;
        switch(keyName)
        {
            case "body":
                material = bodyRenderer.material;
                material = bodyMaterials[type];
                bodyRenderer.material = material;

                material = earsRenderer.material;
                material = bodyMaterials[type];
                earsRenderer.material = material;

                material = tailRenderer.material;
                material = bodyMaterials[type];
                tailRenderer.material = material;
                break;
            case "eyes":
                material = eyesRenderer.material;
                material = eyesMaterials[type];
                eyesRenderer.material = material;
                break;
            case "ears":
                mesh = earsRenderer.sharedMesh;
                mesh = earsMesh[type];
                earsRenderer.sharedMesh = mesh;
                break;
            case "mouth":
                material = mouthRenderer.material;
                material = mouthMaterials[type];
                mouthRenderer.material = material;
                break;
            case "tail":
                mesh = tailRenderer.sharedMesh;
                mesh = tailMesh[type];
                tailRenderer.sharedMesh = mesh;
                break;
        }
    }



    [SerializeField] Button bodyButton;
    [SerializeField] Button eyesButton;
    [SerializeField] Button earsButton;
    [SerializeField] Button mouthButton;
    [SerializeField] Button tailButton;




}
