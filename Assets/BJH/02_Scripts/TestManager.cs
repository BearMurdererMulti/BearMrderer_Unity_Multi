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
            PhotonConnection.Instance.UpdateMinusLife();
        }

        if(UIManagerBJH.lifeCount < 0)
        {

        }

        // ¸ØÃß±â
        if (Input.GetKeyDown(KeyCode.O))
        {
            npc02.isWalking = false;
        }

        // °È±â
        if (Input.GetKeyDown(KeyCode.I))
        {
            npc02.isWalking = true;
        }

        // ¹ã
        if(Input.GetKey(KeyCode.U))
        {
            PhotonConnection.Instance.UpdateDayAndNight(false);
        }

        // ³·
        if (Input.GetKey(KeyCode.Y))
        {
            PhotonConnection.Instance.UpdateDayAndNight(true);
        }
    }
}
