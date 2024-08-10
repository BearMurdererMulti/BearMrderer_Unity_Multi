using System.Collections;
using System.Collections.Generic;
using UnityEngine;

public class SuccessScene : MonoBehaviour
{
    [SerializeField] List<GameObject> gameObjects;
    [SerializeField] List<NpcData> npcData = new List<NpcData>();
    [SerializeField] Transform spot;

    private void Start()
    {
        for (int i = 0; i < InfoManagerKJY.instance.npcListInfo.Count; i++)
        {
            gameObjects[i] = new GameObject();
            npcData.Add(InfoManagerKJY.instance.npcListInfo[i].GetComponent<NpcData>());
            gameObjects[i].GetComponent<KJY_NPCHighlight>().GetMaterialInformation(npcData[i]);
        }
        SetPositionBumin();
    }

    private void SetPositionBumin()
    {
        for (int i = 0; i < gameObjects.Count; i++)
        {
            if (gameObjects[i].GetComponent<NpcData>().npcName == InfoManagerKJY.instance.voteNpcName)
            {
                gameObjects[i].transform.position = spot.position;
                gameObjects[i].transform.rotation = spot.rotation;
            }
        }
    }
   
}
