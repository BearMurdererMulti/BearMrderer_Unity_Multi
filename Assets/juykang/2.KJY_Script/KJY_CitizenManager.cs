using System.Collections;
using System.Collections.Generic;
using UnityEngine;
using DG.Tweening;
using UnityEngine.UI;
using static ConnectionKJY;
using Unity.VisualScripting;


public class KJY_CitizenManager : MonoBehaviour
{
    public static KJY_CitizenManager Instance;

    public List<GameObject> npcList;
    public GameObject player;
    public GameObject npcSpots;
    [SerializeField] Transform[] npcSpotList;
    [SerializeField] Transform playerSpots;
    [SerializeField] Image image;
    [SerializeField] private List<Material> bodyColor;
    [SerializeField] private Material[] mouth;
    [SerializeField] private Mesh[] earCollider;
    [SerializeField] private Mesh[] tailCollider;
    public bool call;

    public List<Transform> dieSpotList;

    public List<GameNpcSetting> npcInfoList;
    //public List<TestGameNpcList> npcInfoList;

    public List<GameObject> dieNpcList;


    public List<GameObject> dummyNpcList;
    public List<NpcData> dummyNpc;

    public List<Canvas> dummyCanvas;

    KJY_InterAction buttonInterAction;

    private void Awake()
    {
        Instance = this;

        //GameObject[] npcs = GameObject.FindGameObjectsWithTag("Npc");

        //foreach (GameObject npc in npcs)
        //{
        //    npcList.Add(npc);
        //}


        npcInfoList = InfoManagerKJY.instance.gameNpcList;
    }

    void Start()
    {
        player = GameObject.FindGameObjectWithTag("Player");

        SetNpcList();

        for (int i = 0; i < npcList.Count; i++)
        {
            npcList[i].GetComponent<KJY_NPCHighlight>().TmpName();
            dummyNpcList[i].GetComponent<KJY_NPCHighlight>().TmpName();
        }

        DayAndNIghtManager.instance.npcList = npcList;
        DayAndNIghtManager.instance.dieNpcList = dieNpcList;

        UI.instance.SetNoteUI(); // note ui를 셋팅하는 메서드입니다. 이따가 삭제
        call = false;
        FirstVictim();
        buttonInterAction = FindFirstObjectByType<KJY_InterAction>();
    }

    public void SetNpcList()
    {
        for (int j = 0; j < npcList.Count; j++)
        {
            NpcData data = npcList[j].GetComponent<NpcData>();
            NpcData dummyData = dummyNpc[j].GetComponent<NpcData>();

            data.id = npcInfoList[j].gameNpcNo;
            dummyData.id = npcInfoList[j].gameNpcNo;

            data.npcName = npcInfoList[j].npcName;
            dummyData.npcName = npcInfoList[j].npcName;

            data.npcJob = npcInfoList[j].npcJob;
            dummyData.npcJob = npcInfoList[j].npcJob;

            data.personality = npcInfoList[j].npcPersonality;
            dummyData.personality = npcInfoList[j].npcPersonality;

            data.feature = npcInfoList[j].npcFeature;
            dummyData.feature = npcInfoList[j].npcFeature;

            data.status = npcInfoList[j].npcStatus;
            dummyData.status = npcInfoList[j].npcStatus;

            data.npcDeathNightNumer = npcInfoList[j].npcDeathNightNumber;
            dummyData.npcDeathNightNumer = npcInfoList[j].npcDeathNightNumber;

            data.deathLocation = npcInfoList[j].deathLocation;
            dummyData.deathLocation = npcInfoList[j].deathLocation;

            if (data.npcJob == "Murderer")
            {
                data.isMafia = true;
                dummyData.isMafia = true;
                InfoManagerKJY.instance.murderObjectNumber = j;
            }

            if (npcInfoList[j].npcMaterial == null)
            {
                SetNpcRandomModel(data, j);
            }
            else
            {
                data.npcMaterial = npcInfoList[j].npcMaterial;
                data.mouthMaterial = npcInfoList[j].npcMaterial;
                data.earCollider = npcInfoList[j].earCollider;
                data.tailCollider = npcInfoList[j].tailCollider;

                npcList[j].GetComponent<KJY_NPCHighlight>().GetMaterialInformation(data);
            }
        }
    }

    public IEnumerator CitizenCall()
    {
        // navi mesh 꺼주기
        //GameManagerBJH.instance.OnOffNaviAgent(false);
        for (int i = 0; i < dieNpcList.Count; i++)
        {
            dieNpcList[i].SetActive(false);
        }

        FadeOut();
        yield return new WaitForSeconds(1);
        player.GetComponent<CharacterController>().enabled = false;
        player.transform.position = playerSpots.position; 
        player.transform.GetChild(0).rotation = playerSpots.rotation;

        SetnpcSpot(true);

        for (int i = 0; i < npcList.Count; i++) 
        {
            npcList[i].GetComponent<KJY_NPCHighlight>().SetActiveMaterial(false);
            //npcList[i].gameObject.SetActive(false);
        }

        FadeIn();
        player.GetComponent<CharacterController>().enabled = true;
        UI.instance.talkBtn.SetActive(false);
        UI.instance.skipBtn.SetActive(true);
        call = true;

        buttonInterAction.ButtonInterAction();
    }

    public void SetnpcSpot(bool value)
    {
        for (int i = 0; i < dummyNpc.Count; i++)
        {
            if (dummyNpc[i].GetComponent<NpcData>().status == "DEAD")
            {
                dummyNpcList[i].SetActive(false);
            }
        }
        npcSpots.SetActive(value);
    }

    void FadeOut()
    {
        image.DOFade(1, 1);
    }

    void FadeIn()
    {
        image.DOFade(0, 1);
    }

    public void DieNpcSpotSet()
    {
        for(int i = 0;i < dieSpotList.Count;i++) 
        {
            if (InfoManagerKJY.instance.crimeScene == dieSpotList[i].name)
            {
                dieNpcList[dieNpcList.Count - 1].transform.position = dieSpotList[i].transform.position;
            }
        }
    }

    public void ActiveNpc()
    {
        for (int i = 0; i < npcList.Count; i++)
        {
            npcList[i].GetComponent<Animator>().SetBool("Walk", true);
        }
    }

    public void FirstVictim()
    {
        for (int i = 0; i < npcList.Count; i++)
        {
            if (InfoManagerKJY.instance.victim == npcList[i].GetComponent<NpcData>().npcName)
            {
                npcList[i].GetComponent<NpcData>().status = "Dead";
                npcList[i].GetComponent<NPC>().Die();
                dieNpcList.Add(npcList[i]);
                npcList.RemoveAt(i);
            }
        }
        DieNpcSpotSet();
    }

    public void OnOffCanvas(GameObject selectObj, bool value)
    {
        if (value == false)
        {
            for (int i = 0; i < dummyNpcList.Count; i++)
            {
                if (selectObj.GetComponent<NpcData>().npcName == dummyNpc[i].GetComponent<NpcData>().npcName)
                {
                    continue;
                }
                dummyCanvas[i].enabled = value;
            }
        }
        else
        {
            for (int i = 0; i < dummyNpcList.Count; i++)
            {
                dummyCanvas[i].enabled = value;
            }
        }
    }

    private void SetNpcRandomModel(NpcData data, int j)
    {
        NpcCustomInfos npcCustom = new NpcCustomInfos();
        npcCustom.npcName = data.npcName;

        int random = Random.Range(0, bodyColor.Count - 1);
        data.npcMaterial = bodyColor[random];
        
        npcCustom.body = random;
        bodyColor.RemoveAt(random);

        random = Random.Range(0, mouth.Length - 1);
        data.mouthMaterial = mouth[random];
        npcCustom.mouth = random;

        random = Random.Range(0, earCollider.Length - 1);
        data.earCollider = earCollider[random];
        npcCustom.ear = random;

        random = Random.Range(0, tailCollider.Length - 1);
        data.tailCollider = tailCollider[random];
        npcCustom.tail = random;

        InfoManagerKJY.instance.npcCustomLists.Add(npcCustom);
        print(InfoManagerKJY.instance.npcCustomLists[j].npcName);

        npcList[j].GetComponent<KJY_NPCHighlight>().GetMaterialInformation(data);
        dummyNpcList[j].GetComponent<KJY_NPCHighlight>().GetMaterialInformation(data);
    }
}
