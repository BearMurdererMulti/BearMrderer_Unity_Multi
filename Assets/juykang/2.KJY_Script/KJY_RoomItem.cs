using System;
using System.Collections;
using System.Collections.Generic;
using TMPro;
using UnityEngine;
using UnityEngine.UI;

public class KJY_RoomItem : MonoBehaviour
{
    public TextMeshProUGUI roomName;
    public TextMeshProUGUI roomMaster;
    public TextMeshProUGUI roomJob;
    public TextMeshProUGUI roomNumber;
    public int roomIndex;
    public string roomCode;
    public Image isPrivateImg;
    public bool isRoomPrivate;
    public Button myRoomBtn;
    public string participant;

    // 클릭 되었을 때 호출 해줄 함수를 담을 변수
    public Action<string, string, bool> onChangeRoomName;

    public void SetInfo(string roomNm, string roomMs, string roomMasterJob, int currPlayer, int maxPlayer, bool isPrivate, string roomCd, int roomIdx)
    {
        //나의 게임오브젝 이름을 방이름로 하자
        name = roomNm;

        //방 정보를 Text 에 설정 
        //방 이름 ( 5 / 10 )
        roomName.text = roomNm;
        roomMaster.text = roomMs;
        roomJob.text = roomMasterJob;
        roomNumber.text = currPlayer + "/" + maxPlayer;
        roomIndex = roomIdx;
        if (isPrivate)
        {
            isPrivateImg.sprite = Resources.Load<Sprite>("lockImage/lock");
            isRoomPrivate = true;
        }
        else
        {
            isPrivateImg.sprite = Resources.Load<Sprite>("lockImage/unlock");
            isRoomPrivate = false;
        }
        roomCode = roomCd;
    }

    public void SetCountInfo(int currPlayer, int maxPlayer)
    {
        roomNumber.text = currPlayer + "/" + maxPlayer;
    }

    public void OnClick()
    {
        //onChangeRoomName 가 null 이 아니라면
        if (onChangeRoomName != null)
        {
            print("dada");
            onChangeRoomName(roomName.text, roomCode, isRoomPrivate);
        }


        ////1. InputRoomName 게임오브젝트 찾자.
        //GameObject go = GameObject.Find("InputRoomName");
        ////2. 찾은 게임오브젝트에서 InputField 컴포넌 가져오자
        //InputField inputField = go.GetComponent<InputField>();
        ////3. 가져온 컴포넌트를 이용해서 Text 값 변경
        //inputField.text = name;
    }

}
