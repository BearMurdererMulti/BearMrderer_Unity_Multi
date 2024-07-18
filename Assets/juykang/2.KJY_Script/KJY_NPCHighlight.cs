using System.Collections;
using System.Collections.Generic;
using TMPro;
using UnityEngine;
using UnityEngine.UI;

public class KJY_NPCHighlight : MonoBehaviour
{
    [SerializeField] Color baseC;
    [SerializeField] Material material;
    [SerializeField] Material material2;
    [SerializeField] Material material3;
    [SerializeField] SkinnedMeshRenderer body;
    [SerializeField] SkinnedMeshRenderer mouth;
    [SerializeField] GameObject ear;
    [SerializeField] GameObject tail;
    [SerializeField] SkinnedMeshRenderer re;
    [SerializeField] SkinnedMeshRenderer re2;
    [SerializeField] SkinnedMeshRenderer re3;
    [SerializeField] MeshRenderer re4;
    float resetValue;
    public float changevalue;
    [SerializeField] GameObject npcName;
    [SerializeField] TMP_Text npcNameText;
    
    private void Start()
    {
    }

    public void GetMaterialInformation(NpcData data)
    {
        //re = transform.GetChild(1).GetComponent<SkinnedMeshRenderer>();
        //material = re.sharedMaterial;
        //if (re2 != null)
        //{
        //    material2 = re2.material;
        //}
        //else
        //{
        //    material2 = re4.sharedMaterial;
        //}
        //material3 = re3.sharedMaterial;
        //baseC = material.GetColor("_OutlineColor");
        //resetValue = material.GetFloat("_Outline");

        material = data.npcMaterial;
        material = re.material;

        if (body != null) 
        { 
            body.sharedMaterial = data.npcMaterial;
        }
        if (mouth != null)
        {
            mouth.sharedMaterial = data.mouthMaterial;
        }

        tail.GetComponent<SkinnedMeshRenderer>().sharedMesh = data.tailCollider;
        tail.GetComponent<SkinnedMeshRenderer>().material = data.npcMaterial;
        re3 = tail.GetComponent<SkinnedMeshRenderer>();
        material3 = re3.sharedMaterial;

        if (ear.GetComponent<SkinnedMeshRenderer>() != null)
        {
            ear.GetComponent<SkinnedMeshRenderer>().sharedMesh = data.earCollider;
            ear.GetComponent<SkinnedMeshRenderer>().material = data.npcMaterial;
            re2 = ear.GetComponent<SkinnedMeshRenderer>();
            material2 = re2.material;
        }
        else
        {
            ear.GetComponent<MeshRenderer>().sharedMaterial = data.npcMaterial;
            ear.GetComponent<MeshFilter>().mesh = data.earCollider;
            re4 = ear.GetComponent<MeshRenderer>();
            material2 = re4.sharedMaterial;
        }

        material.SetFloat("_Outline", 0f);
        material.SetColor("_OutlineColor", Color.black);
        material2.SetFloat("_Outline", 0f);
        material2.SetColor("_OutlineColor", Color.black);
        material3.SetFloat("_Outline", 0f);
        material3.SetColor("_OutlineColor", Color.black);
    }

    private void OnMouseEnter()
    {
        if (KJY_CitizenManager.Instance.call == true && DayAndNIghtManager.instance.click == false)
        {
            material.SetFloat("_Outline", changevalue);
            material.SetColor("_OutlineColor", Color.yellow);
            material2.SetFloat("_Outline", changevalue);
            material2.SetColor("_OutlineColor", Color.yellow);
            material3.SetFloat("_Outline", changevalue);
            material3.SetColor("_OutlineColor", Color.yellow);
        }
    }

    private void OnMouseExit()
    {
        if (KJY_CitizenManager.Instance.call == true)
        {
            material.SetFloat("_Outline", resetValue);
            material.SetColor("_OutlineColor", baseC);
            material2.SetFloat("_Outline", resetValue);
            material2.SetColor("_OutlineColor", baseC);
            material3.SetFloat("_Outline", resetValue);
            material3.SetColor("_OutlineColor", baseC);
        }
    }

    public void TmpName()
    {
        npcNameText.text = GetComponent<NpcData>().npcName;
    }

    public void SetActiveMaterial(bool value)
    {
        re.enabled = value;
        if (re2 != null)
        {
            re2.enabled = value;
        }
        else
        {
            re4.enabled = value;
        }
        re3.enabled = value;
        npcName.SetActive(value);
    }
}
