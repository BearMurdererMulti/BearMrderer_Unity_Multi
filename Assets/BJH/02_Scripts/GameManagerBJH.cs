using System.Collections;
using System.Collections.Generic;
using Unity.VisualScripting;
using UnityEngine;
using UnityEngine.AI;

public class GameManagerBJH : MonoBehaviour
{
    public static GameManagerBJH instance;

    // NPC ����
    public List<string> npcNameList = new List<string>();



    



    private void Awake()
    {
        instance = this;
        //npcNameList.Add("������");
        //npcNameList.Add("�ڵ���");
        //npcNameList.Add("������");
        //npcNameList.Add("�±�Ƽ��");
        //npcNameList.Add("���Ϸ�");
        //npcNameList.Add("�ø���");
        //npcNameList.Add("���Ǿ�");
        //npcNameList.Add("��츣��");

        //foreach(string name in npcNameList)
        //{
        //    InfoManagerBJH.instance.npcOxDic.Add(name, null);
        //}
    }

    private void Start()
    {
    }

    public void OnOffNaviAgent(bool b)
    {
        GameObject[] npcs = GameObject.FindGameObjectsWithTag("Npc");

        foreach (GameObject npc in npcs)
        {
            npc.GetComponent<NavMeshAgent>().enabled = b;
        }
    }

    
}
