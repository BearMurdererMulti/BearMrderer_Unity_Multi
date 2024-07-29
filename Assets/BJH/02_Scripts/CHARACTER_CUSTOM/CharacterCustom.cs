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

    // Ŭ������ �� �� ����
    public void OnClickColor(GameObject go)
    {
        string name = go.name;
        
        // materials�� read �����̱⶧���� ���ο� �迭�� �Ҵ��ϴ� ������� �ڵ� �ۼ�
        Material newMaterial = Resources.Load<Material>($"Color/{name}");

        if(newMaterial != null)
        {
            Material[] materials = new Material[1];

            materials[0] = newMaterial;

            dollBody.GetComponent<SkinnedMeshRenderer>().materials = materials;
            //InfoManagerKJY.instance.body = newMaterial.name; // body�� color ���Ͻ�
        }
    }

    // �� ��ư ����


    // Ŭ������ �� ���� ����
    // ���� �� �������� �� ĳ���� ȸ���ϴ� �͵� �־�� �ҵ�?

    
    public void OnClickTail(int i)
    {

        // ������ ���õ� ������ ���� ���õ� ������ �ٸ��� ������ ���õ� ������ active false�� �����ϰ� ������ ���õ� ������ ���� ���õ� ������ ������Ʈ�Ѵ�.
        if (preTail != tailList[i].name)
        {
            // ������ ������ ������ ����
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













































    //// ĳ����
    //public Transform characterTr;
    //public Transform tailTr;
    //public Renderer characterMouthRen;
    //public Renderer renderer;
    //public List<Material> materialList;

    //// ������
    //public GameObject characterCustomBtnPre;

    //// ��
    //public GameObject boxMouthGo;
    //public List<Image> imageList;

    //// ����
    //public GameObject tailBtnPre;
    //public GameObject boxtailGo;
    //public List<Material> tailMaterialList;

    //void Start()
    //{
    //    // ����
    //    // ��ư�� �̹��� �־ ����
    //    // Tail�� �ִ� GameObject > ù��° �ڽ� > Render > Material
    //    tailMaterialList = new List<Material>();
    //    GameObject[] goArr = Resources.LoadAll<GameObject>("Tail");
    //    foreach (GameObject go in goArr)
    //    {
    //        Debug.Log(go.transform.GetChild(0).GetComponent<SkinnedMeshRenderer>().material);
    //        tailMaterialList.Add(go.transform.GetChild(0).GetComponent<SkinnedMeshRenderer>().material);
    //    }

    //    int index = 0;
    //    // ���� ��ư ����
    //    foreach(Material material in tailMaterialList)
    //    {
    //        GameObject go = Instantiate(tailBtnPre, boxtailGo.transform);
    //        go.name = material.name;

    //        // OnClick() ����

    //        UnityAction<GameObject> callback = new UnityAction<GameObject>(OnClickTailBtn);
    //        go.GetComponent<Button>().onClick.AddListener(() => callback(goArr[index]));
    //        index++;
    //    }

    //    // �̹��� ����Ʈ ��������
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
    //    // material�� recource���� �о���� -> material ����Ʈ�� ���
    //    Material[] materialArr = Resources.LoadAll<Material>("Mouth");
    //    materialList = new List<Material>();
    //    materialList.AddRange(materialArr);

    //    // Mouth btn ����
    //    foreach (Material mat in materialList)
    //    {
    //        GameObject go = Instantiate(characterCustomBtnPre, boxMouthGo.transform);
    //        go.name = mat.name;

    //        // ��ư ������ �Ϸ�Ǿ��ٸ�? OnClick�Լ� �߰�
    //        UnityAction<Material> callback = new UnityAction<Material>(OnclickMouthBtn);
    //        Button btn = go.GetComponent<Button>();
    //        btn.onClick.AddListener(() => callback(mat));
    //    }

    //    // boxMouth �̹��� ����Ʈ ��������
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

    //// ���콺 ��ư�� ������ �ش��ϴ� material�� ĳ���Ϳ� �����
    //public void OnclickMouthBtn(Material material)
    //{
    //    characterMouthRen.material = material;
    //    print(material.name);
    //}
}
