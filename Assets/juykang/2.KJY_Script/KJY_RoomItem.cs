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

    // Ŭ�� �Ǿ��� �� ȣ�� ���� �Լ��� ���� ����
    public Action<string, string, bool> onChangeRoomName;

    public void SetInfo(string roomNm, string roomMs, string roomMasterJob, int currPlayer, int maxPlayer, bool isPrivate, string roomCd, int roomIdx)
    {
        //���� ���ӿ����� �̸��� ���̸��� ����
        name = roomNm;

        //�� ������ Text �� ���� 
        //�� �̸� ( 5 / 10 )
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
        //onChangeRoomName �� null �� �ƴ϶��
        if (onChangeRoomName != null)
        {
            print("dada");
            onChangeRoomName(roomName.text, roomCode, isRoomPrivate);
        }


        ////1. InputRoomName ���ӿ�����Ʈ ã��.
        //GameObject go = GameObject.Find("InputRoomName");
        ////2. ã�� ���ӿ�����Ʈ���� InputField ������ ��������
        //InputField inputField = go.GetComponent<InputField>();
        ////3. ������ ������Ʈ�� �̿��ؼ� Text �� ����
        //inputField.text = name;
    }

}
