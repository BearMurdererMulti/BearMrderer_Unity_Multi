using System;
using System.Collections;
using System.Collections.Generic;
using UnityEngine;
using UnityEngine.UI;

public class KJY_RoomItem : MonoBehaviour
{
    public Text roomName;
    public Text roomMaster;
    public Text roomNumber;

    // 클릭 되었을 때 호출 해줄 함수를 담을 변수
    public Action<string> onChangeRoomName;

    public void SetInfo(string roomNm, string roomMs, int currPlayer, int maxPlayer)
    {
        //나의 게임오브젝 이름을 방이름로 하자
        name = roomNm;

        //방 정보를 Text 에 설정 
        //방 이름 ( 5 / 10 )
        roomName.text = roomNm;
        roomMaster.text = roomMs;
        roomNumber.text = currPlayer + "/" + maxPlayer;
    }

    public void OnClick()
    {
        //onChangeRoomName 가 null 이 아니라면
        if (onChangeRoomName != null)
        {
            onChangeRoomName(name);
        }


        ////1. InputRoomName 게임오브젝트 찾자.
        //GameObject go = GameObject.Find("InputRoomName");
        ////2. 찾은 게임오브젝트에서 InputField 컴포넌 가져오자
        //InputField inputField = go.GetComponent<InputField>();
        ////3. 가져온 컴포넌트를 이용해서 Text 값 변경
        //inputField.text = name;
    }
}
