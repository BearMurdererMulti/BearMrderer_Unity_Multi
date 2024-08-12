using System.Collections;
using System.Collections.Generic;
using UnityEngine;

public class PlayerCustom : MonoBehaviour
{
    private void Start()
    {
        var custom = transform.GetComponent<Doll>();
        var info = InfoManagerKJY.instance;

        // ¸ö »ö ¼³Á¤
        custom.bodyRenderer.material = info.CustomMaterialsAndMesh.bodyMaterials[info.customDictionary["body"]];
        custom.earsRenderer.material = info.CustomMaterialsAndMesh.bodyMaterials[info.customDictionary["body"]];
        custom.tailRenderer.material = info.CustomMaterialsAndMesh.bodyMaterials[info.customDictionary["body"]];

        Material[] eyesMaterialArray = custom.eyesRenderer.materials;
        eyesMaterialArray[0] = info.CustomMaterialsAndMesh.eyesMaterials[info.customDictionary["eyes"]];
        eyesMaterialArray[1] = info.CustomMaterialsAndMesh.eyesMaterials[info.customDictionary["eyes"]];
        custom.eyesRenderer.materials = eyesMaterialArray;

        custom.earsRenderer.sharedMesh = info.CustomMaterialsAndMesh.earsMesh[info.customDictionary["ears"]];
        custom.mouthRenderer.material = info.CustomMaterialsAndMesh.mouthMaterials[info.customDictionary["mouth"]];
        custom.tailRenderer.sharedMesh = info.CustomMaterialsAndMesh.tailMesh[info.customDictionary["tail"]];
    }
}
