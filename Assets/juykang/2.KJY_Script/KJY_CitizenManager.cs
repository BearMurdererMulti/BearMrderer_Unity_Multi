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
        player = GameObject.FindGameObjectWithTag("Detective");
        dog = GameObject.FindGameObjectWithTag("Assistant");

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
        //dog.transform.position = dogSpot.position;
        //dog.transform.root.rotation = dogSpot.rotation;

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
        for (int i = 0; i < npcList.Count; i++)
        {
            int randomBody = Random.Range(0, bodyColor.Count - 1);
            int randomMouth = Random.Range(0, mouth.Length - 1);
            int randomEar = Random.Range(0, earCollider.Length - 1);
            int randomTail = Random.Range(0, tailCollider.Length - 1);

            photonView.RPC("SetNpcCustomization", RpcTarget.AllBuffered, i, randomBody, randomMouth, randomEar, randomTail);
        }
    }

    [PunRPC]
    public void SetNpcCustomization(int npcIndex, int bodyIndex, int mouthIndex, int earIndex, int tailIndex)
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
