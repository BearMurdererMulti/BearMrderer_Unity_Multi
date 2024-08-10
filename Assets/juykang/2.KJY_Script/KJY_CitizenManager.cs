using System.Collections;
using System.Collections.Generic;
using UnityEngine;
using DG.Tweening;
using UnityEngine.UI;
using static ConnectionKJY;
using Unity.VisualScripting;
using Photon.Pun;


public class KJY_CitizenManager : MonoBehaviourPunCallbacks
{
    public static KJY_CitizenManager Instance;

    public List<GameObject> npcList;
    public GameObject player;
    public GameObject dog;
    public GameObject npcSpots;
    [SerializeField] Transform[] npcSpotList;
    [SerializeField] Transform playerSpots;
    [SerializeField] Transform dogSpot;
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
        if (InfoManagerKJY.instance.role == "Detective")
        {
            player = GameObject.FindGameObjectWithTag("Detective");
        }
        else
        {
            dog = GameObject.FindGameObjectWithTag("User");
        }

        SetNpcList();

        for (int i = 0; i < npcList.Count; i++)
        {
            npcList[i].GetComponent<KJY_NPCHighlight>().TmpName();
            dummyNpcList[i].GetComponent<KJY_NPCHighlight>().TmpName();
        }

        GameManager_KJY.instance.npcList = npcList;
        GameManager_KJY.instance.dieNpcList = dieNpcList;

        call = false;
        FirstVictim();
        //UI.instance.SetNoteUI(); // note ui를 셋팅하는 메서드입니다. 이따가 삭제
        if (InfoManagerKJY.instance.role == "Detective")
        {
            buttonInterAction = FindFirstObjectByType<KJY_InterAction>();
        }
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

            if (PhotonNetwork.IsMasterClient)
            {
                SetNpcRandomization(); // Randomize NPCs only on the Master Client
            }

            //if (npcInfoList[j].npcMaterial == null)
            //{
            //    SetNpcRandomModel(data, j);
            //}
            //else
            //{
            //    data.npcMaterial = npcInfoList[j].npcMaterial;
            //    data.mouthMaterial = npcInfoList[j].npcMaterial;
            //    data.earCollider = npcInfoList[j].earCollider;
            //    data.tailCollider = npcInfoList[j].tailCollider;

            //    npcList[j].GetComponent<KJY_NPCHighlight>().GetMaterialInformation(data);
            //}
        }
    }

    [PunRPC]
    public void CallCitizen()
    {
        StartCoroutine(CitizenCall());
    }

    private IEnumerator CitizenCall()
    {
        // navi mesh 꺼주기
        //GameManagerBJH.instance.OnOffNaviAgent(false);
        for (int i = 0; i < dieNpcList.Count; i++)
        {
            dieNpcList[i].SetActive(false);
        }

        image.DOFade(1, 2);
        yield return new WaitForSeconds(1);
        if (InfoManagerKJY.instance.role == "Detective")
        {
            player.GetComponent<CharacterController>().enabled = false;
            player.transform.position = playerSpots.position;
            player.transform.GetChild(0).rotation = playerSpots.rotation;
        }
        else
        {
            dog.transform.position = dogSpot.position;
            dog.transform.root.rotation = dogSpot.rotation;
            //dog.GetComponent<CharacterController>().enabled = false;
        }

        SetnpcSpot(true);

        for (int i = 0; i < npcList.Count; i++)
        {
            npcList[i].GetComponent<KJY_NPCHighlight>().SetActiveMaterial(false);
            //npcList[i].gameObject.SetActive(false);
        }

        image.DOFade(0, 2);
        if (InfoManagerKJY.instance.role == "Detective")
        {
            player.GetComponent<CharacterController>().enabled = true;
            UI.instance.talkBtn.SetActive(false);
        }
        UI.instance.skipBtn.SetActive(true);
        call = true;

        if (InfoManagerKJY.instance.role == "Detective")
        {
            buttonInterAction.ButtonInterAction();
        }
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
        image.DOFade(1, 2);
    }

    void FadeIn()
    {
        image.DOFade(0, 2);
    }

    public void DieNpcSpotSet()
    {
        for (int i = 0; i < dieSpotList.Count; i++)
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

    private void SetNpcRandomization()
    {
        if (PhotonNetwork.IsMasterClient)
        {
            List<int> availableColors = new List<int>();

            // 0부터 bodyColor.Count-1까지의 인덱스를 리스트에 추가
            for (int i = 0; i < bodyColor.Count; i++)
            {
                availableColors.Add(i);
            }

            // 리스트를 무작위로 섞음
            for (int i = 0; i < availableColors.Count; i++)
            {
                int temp = availableColors[i];
                int randomIndex = Random.Range(i, availableColors.Count);
                availableColors[i] = availableColors[randomIndex];
                availableColors[randomIndex] = temp;
            }

            int[] customizationDataArray = new int[npcList.Count * 4];

            for (int i = 0; i < npcList.Count; i++)
            {
                int randomBody = availableColors[i];
                int randomMouth = Random.Range(0, mouth.Length);
                int randomEar = Random.Range(0, earCollider.Length);
                int randomTail = Random.Range(0, tailCollider.Length);

                // 데이터를 배열에 저장
                customizationDataArray[i * 4] = randomBody;
                customizationDataArray[i * 4 + 1] = randomMouth;
                customizationDataArray[i * 4 + 2] = randomEar;
                customizationDataArray[i * 4 + 3] = randomTail;
            }
            photonView.RPC("ApplyNpcCustomization", RpcTarget.All, customizationDataArray);
        }
    }

    [PunRPC]
    private void ApplyNpcCustomization(int[] customizationDataArray)
    {
        for (int i = 0; i < npcList.Count; i++)
        {
            int bodyIndex = customizationDataArray[i * 4];
            int mouthIndex = customizationDataArray[i * 4 + 1];
            int earIndex = customizationDataArray[i * 4 + 2];
            int tailIndex = customizationDataArray[i * 4 + 3];

            SetNpcCustomization(i, bodyIndex, mouthIndex, earIndex, tailIndex);
        }
    }

    private void SetNpcCustomization(int npcIndex, int bodyIndex, int mouthIndex, int earIndex, int tailIndex)
    {
        if (npcIndex < 0 || npcIndex >= npcList.Count) return;

        NpcData data = npcList[npcIndex].GetComponent<NpcData>();

        data.npcMaterial = bodyColor[bodyIndex];
        data.mouthMaterial = mouth[mouthIndex];
        data.earCollider = earCollider[earIndex];
        data.tailCollider = tailCollider[tailIndex];

        npcList[npcIndex].GetComponent<KJY_NPCHighlight>().GetMaterialInformation(data);
        dummyNpcList[npcIndex].GetComponent<KJY_NPCHighlight>().GetMaterialInformation(data);
    }
}
