using System.Collections;
using System.Collections.Generic;
using UnityEngine;

public class HttpNote : MonoBehaviour
{
    // 수첩과 관련한 통신이 모여있는 클래스입니다.
    // 채팅리스트, AI요약, 체크리스트 통신을 시도합니다.


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
