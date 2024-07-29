using System;
using System.Collections;
using System.Collections.Generic;
using UnityEngine;
using UnityEngine.Events;
using UnityEngine.Timeline;
using UnityEngine.UI;

public class CharacterCustom : MonoBehaviour
{
    public GameObject dollBody;
    public GameObject customButtonPre;

    //[SerializeField] private Dictionary<string, GameObject> meshDic = new Dictionary<string, GameObject>();
    [SerializeField] private GameObject[] tailList = new GameObject[4];
    [SerializeField] private GameObject player;
    string preTail = null;

    private void Start()
    {
        preTail = "Tail_01";
    }

    // 클릭했을 때 색 변경
    public void OnClickColor(GameObject go)
    {
        string name = go.name;
        
        // materials는 read 전용이기때문에 새로운 배열을 할당하는 방식으로 코드 작성
        Material newMaterial = Resources.Load<Material>($"Color/{name}");

        if(newMaterial != null)
        {
            Material[] materials = new Material[1];

            materials[0] = newMaterial;

            dollBody.GetComponent<SkinnedMeshRenderer>().materials = materials;
            //InfoManagerKJY.instance.body = newMaterial.name; // body랑 color 동일시
        }
    }

    // 입 버튼 생성


    // 클릭했을 때 꼬리 변경
    // 꼬리 탭 선택했을 때 캐릭터 회전하는 것도 넣어야 할듯?

    
    public void OnClickTail(int i)
    {

        // 이전에 선택된 꼬리와 현재 선택된 꼬리가 다르면 이전에 선택된 꼬리를 active false로 변경하고 이전에 선택된 꼬리를 현재 선택된 꼬리로 업데이트한다.
        if (preTail != tailList[i].name)
        {
            // 이전에 선택한 꼬리를 끄고
            for(int n = 0; n < tailList.Length; n++)
            {
                if (tailList[n].name == preTail)
                {
                    tailList[n].SetActive(false);
                }
            }

            tailList[i].SetActive(true);
            //InfoManagerKJY.instance.tail = tailList[i].name;

            preTail = tailList[i].name;
            
        }
        else
        {
            return;
        }
    }













































    //// 캐릭터
    //public Transform characterTr;
    //public Transform tailTr;
    //public Renderer characterMouthRen;
    //public Renderer renderer;
    //public List<Material> materialList;

    //// 프리팹
    //public GameObject characterCustomBtnPre;

    //// 입
    //public GameObject boxMouthGo;
    //public List<Image> imageList;

    //// 꼬리
    //public GameObject tailBtnPre;
    //public GameObject boxtailGo;
    //public List<Material> tailMaterialList;

    //void Start()
    //{
    //    // 꼬리
    //    // 버튼에 이미지 넣어서 생성
    //    // Tail에 있는 GameObject > 첫번째 자식 > Render > Material
    //    tailMaterialList = new List<Material>();
    //    GameObject[] goArr = Resources.LoadAll<GameObject>("Tail");
    //    foreach (GameObject go in goArr)
    //    {
    //        Debug.Log(go.transform.GetChild(0).GetComponent<SkinnedMeshRenderer>().material);
    //        tailMaterialList.Add(go.transform.GetChild(0).GetComponent<SkinnedMeshRenderer>().material);
    //    }

    //    int index = 0;
    //    // 꼬리 버튼 생성
    //    foreach(Material material in tailMaterialList)
    //    {
    //        GameObject go = Instantiate(tailBtnPre, boxtailGo.transform);
    //        go.name = material.name;

    //        // OnClick() 설정

    //        UnityAction<GameObject> callback = new UnityAction<GameObject>(OnClickTailBtn);
    //        go.GetComponent<Button>().onClick.AddListener(() => callback(goArr[index]));
    //        index++;
    //    }

    //    // 이미지 리스트 가져오기
    //    for (int i = 0; i < tailTr.childCount; i++)
    //    {
    //        Image image = tailTr.GetChild(i).gameObject.GetComponent<Image>();
    //        image.sprite = ChangeMaterialToSprite(tailMaterialList[i]);

    //    }
    //}

    //private void OnClickTailBtn(GameObject go)
    //{
    //    if(tailTr.childCount >= 1)
    //    {
    //        Destroy(tailTr.GetChild(0).gameObject);
    //    }

    //    Instantiate(go, tailTr);
    //}

    //void MouthCustom()
    //{
    //    // material을 recource에서 읽어오기 -> material 리스트에 담기
    //    Material[] materialArr = Resources.LoadAll<Material>("Mouth");
    //    materialList = new List<Material>();
    //    materialList.AddRange(materialArr);

    //    // Mouth btn 생성
    //    foreach (Material mat in materialList)
    //    {
    //        GameObject go = Instantiate(characterCustomBtnPre, boxMouthGo.transform);
    //        go.name = mat.name;

    //        // 버튼 생성이 완료되었다면? OnClick함수 추가
    //        UnityAction<Material> callback = new UnityAction<Material>(OnclickMouthBtn);
    //        Button btn = go.GetComponent<Button>();
    //        btn.onClick.AddListener(() => callback(mat));
    //    }

    //    // boxMouth 이미지 리스트 가져오기
    //    imageList = new List<Image>();
    //    for (int i = 0; i < boxMouthGo.transform.childCount; i++)
    //    {
    //        imageList.Add(boxMouthGo.transform.GetChild(i).gameObject.GetComponent<Image>());
    //        imageList[i].sprite = ChangeMaterialToSprite(materialList[i]);
    //    }
    //}

    //Sprite ChangeMaterialToSprite(Material material)
    //{
    //    Texture texture = material.mainTexture;
    //    Texture2D texture2D = texture as Texture2D;
    //    Sprite sprite = Sprite.Create(texture2D, new Rect(0, 0, texture2D.width, texture2D.height), new Vector2(0.5f, 0.5f));
    //    return sprite;
    //}

    //// 마우스 버튼을 누르면 해당하는 material이 캐릭터에 적용됨
    //public void OnclickMouthBtn(Material material)
    //{
    //    characterMouthRen.material = material;
    //    print(material.name);
    //}
}
