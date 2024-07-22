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
    [SerializeField] SkinnedMeshRenderer earsRenderer; // ����� Mesh, material ����

    [SerializeField] List<Material> mouthMaterials = new List<Material>();
    [SerializeField] SkinnedMeshRenderer mouthRenderer;

    [SerializeField] List<Mesh> tailMesh = new List<Mesh>();
    [SerializeField] SkinnedMeshRenderer tailRenderer; // ����� Mesh, material ����
    

    private Dictionary<string, int> customDictionary = new Dictionary<string, int>();

    // Ŀ���� ����� �����ϴ� ��ư�� ������
    // Instantiate���ִ� ��ư
    private void InstantiateCustomButton()
    {
        
    }

    internal void SetCustomDictionary(string keyName, int type)
    {
        // ��ųʸ� ����
        customDictionary[keyName] = type;

        // ĳ���Ϳ� ����
        var material = bodyRenderer.material; // �ӽ�
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
