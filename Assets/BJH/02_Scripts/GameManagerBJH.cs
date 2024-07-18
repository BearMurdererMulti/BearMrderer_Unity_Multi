using System.Collections;
using System.Collections.Generic;
using Unity.VisualScripting;
using UnityEngine;
using UnityEngine.AI;

public class GameManagerBJH : MonoBehaviour
{
    public static GameManagerBJH instance;

    // NPC 정보
    public List<string> npcNameList = new List<string>();



    



    private void Awake()
    {
        instance = this;
        //npcNameList.Add("김쿵야");
        //npcNameList.Add("박동식");
        //npcNameList.Add("마르코");
        //npcNameList.Add("태근티비");
        //npcNameList.Add("마일로");
        //npcNameList.Add("올리버");
        //npcNameList.Add("소피아");
        //npcNameList.Add("사우르스");

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
