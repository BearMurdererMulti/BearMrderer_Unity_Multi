using System.Collections;
using System.Collections.Generic;
using UnityEngine;

public class NpcData : MonoBehaviour
{
    //npcNumber
    public long id;
    //�̸�
    public string npcName;

    //npc����
    public string npcJob;

    //���ǽ� ����
    public bool isNpc;

    //���Ǿ� ���� 
    public bool isMafia;
    
    //����
    public string personality;
    //Ư¡
    public string feature;

    //�׾����� ��Ҵ���
    public string status;
    
    //���� ��¥
    public float npcDeathNightNumer;

    //���� ���
    public string deathLocation;

    //�Ǻλ� ����
    public Material npcMaterial;

    //�� ����
    public Material mouthMaterial;

    public Mesh earCollider;

    public Mesh tailCollider;
}
