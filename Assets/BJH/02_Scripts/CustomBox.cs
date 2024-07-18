using System.Collections;
using System.Collections.Generic;
using UnityEngine;
using UnityEngine.UI;

public class CustomBox : MonoBehaviour
{
    [Header("버튼 프리팹")]
    [SerializeField] private GameObject customButtonPre;
    private int cnt = 12;

    [Header("입")]
    [SerializeField] private GameObject mouth;

    [Header("꼬리")]
    [SerializeField] private GameObject tail;
    [SerializeField] private GameObject[] tile = new GameObject[4];

    void Start()
    {
        if(gameObject.name == "MouthBox")
        {
            // gameobject가 활성화되면 입 버튼 생성
            Material[] materials = Resources.LoadAll<Material>("Mouth");

            foreach (Material mat in materials)
            {
                GameObject go = Instantiate(customButtonPre, gameObject.transform);
                go.name = mat.name;

                Sprite sprite = MaterialToSprite(mat);
                go.GetComponent<Image>().sprite = sprite;

                go.GetComponent<Button>().onClick.AddListener(() => OnClickMouth(go));
            }
        }

        if(gameObject.name == "TailBox")
        {
        }
    }

    private Sprite MaterialToSprite(Material material)
    {
        Texture texture = material.mainTexture;
        Texture2D texture2D = texture as Texture2D;
        Sprite sprite = Sprite.Create(texture2D, new Rect(0, 0, texture2D.width, texture2D.height), new Vector2(0.5f, 0.5f));
        return sprite;
    }

    // 입이 클릭되면 플레이어 입도 변경
    private void OnClickMouth(GameObject go)
    {
        Material material = Resources.Load<Material>($"Mouth/{go.name}");
        Material[] materials = new Material[1];
        materials[0] = material;

        mouth.GetComponent<SkinnedMeshRenderer>().materials = materials;
    }
}
