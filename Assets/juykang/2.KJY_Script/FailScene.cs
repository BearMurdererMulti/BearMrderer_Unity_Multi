using System.Collections;
using System.Collections.Generic;
using UnityEngine;

public class FailScene : MonoBehaviour
{
    [SerializeField] private GameObject gameObjects;
    [SerializeField] private NpcData npc;
    // Start is called before the first frame update
    void Start()
    {
        long gameSetNo = InfoManagerKJY.instance.gameSetNo;
        EndingLetterConnection.Instance.LetterConnection(gameSetNo);

        for (int i = 0; i < InfoManagerKJY.instance.npcListInfo.Count; i++)
        {
            if (InfoManagerKJY.instance.npcListInfo[i].GetComponent<NpcData>().npcName ==InfoManagerKJY.instance.voteNpcName)
            {
                npc.GetComponent<KJY_NPCHighlight>().GetMaterialInformation(InfoManagerKJY.instance.npcListInfo[i].GetComponent<NpcData>());
                break;
            }
        }
    }

}
