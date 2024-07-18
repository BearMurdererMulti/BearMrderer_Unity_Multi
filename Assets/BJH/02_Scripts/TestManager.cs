using System.Collections;
using System.Collections.Generic;
using UnityEngine;

public class TestManager : MonoBehaviour
{
    public UI UIManagerBJH;
    public NPC npc02;
    public NPC npc03;
    public NPC npc04;
    public NPC npc05;
    public NPC npc06;


    void Update()
    {
        if (Input.GetKeyDown(KeyCode.P))
        {
            UIManagerBJH.MinusLife();
        }

        if(UIManagerBJH.lifeCount < 0)
        {

        }

        // ���߱�
        if (Input.GetKeyDown(KeyCode.O))
        {
            npc02.isWalking = false;
        }

        // �ȱ�
        if (Input.GetKeyDown(KeyCode.I))
        {
            npc02.isWalking = true;
        }

        // ��
        if(Input.GetKey(KeyCode.U))
        {
            UI.instance.DayAndNight(false);
        }

        // ��
        if (Input.GetKey(KeyCode.Y))
        {
            UI.instance.DayAndNight(true);
        }
    }
}
