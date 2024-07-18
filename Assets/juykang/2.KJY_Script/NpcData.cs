using System.Collections;
using System.Collections.Generic;
using UnityEngine;

public class NpcData : MonoBehaviour
{
    //npcNumber
    public long id;
    //이름
    public string npcName;

    //npc직업
    public string npcJob;

    //엔피시 여부
    public bool isNpc;

    //마피아 여부 
    public bool isMafia;
    
    //성격
    public string personality;
    //특징
    public string feature;

    //죽었는지 살았는지
    public string status;
    
    //죽은 날짜
    public float npcDeathNightNumer;

    //죽은 장소
    public string deathLocation;

    //피부색 결정
    public Material npcMaterial;

    //입 결정
    public Material mouthMaterial;

    public Mesh earCollider;

    public Mesh tailCollider;
}
