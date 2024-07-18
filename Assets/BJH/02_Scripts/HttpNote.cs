using System.Collections;
using System.Collections.Generic;
using UnityEngine;

public class HttpNote : MonoBehaviour
{
    // ��ø�� ������ ����� ���ִ� Ŭ�����Դϴ�.
    // ä�ø���Ʈ, AI���, üũ����Ʈ ����� �õ��մϴ�.


    public void StartCheckListConnection()
    {
        var npcNameList = GameManagerBJH.instance.npcNameList;
        var npcOxDic = InfoManagerKJY.instance.npcOxDic;

        List<CheckListInfo> checkListInfo = new List<CheckListInfo>();

        foreach(var npcName in npcNameList)
        {
            CheckListInfo info = new CheckListInfo();
            info.npcName = npcName;
            info.mark = npcOxDic[npcName];

            checkListInfo.Add(info);
        }
        
        string url = "https://8335-115-136-106-231.ngrok-free.app/api/v1/checklist";
        long no = 1L;

        CheckListConnection checkListConnection = new CheckListConnection(url, no, checkListInfo);


    }
}
