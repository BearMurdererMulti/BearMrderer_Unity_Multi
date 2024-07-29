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
    public string roomCode;
    public Image isPrivateImg;
    private bool isRoomPrivate;

    // Ŭ�� �Ǿ��� �� ȣ�� ���� �Լ��� ���� ����
    public Action<string> onChangeRoomName;

    public void SetInfo(string roomNm, string roomMs, string roomMasterJob, int currPlayer, int maxPlayer, bool isPrivate, string roomCd)
    {
        //���� ���ӿ����� �̸��� ���̸��� ����
        name = roomNm;

        //�� ������ Text �� ���� 
        //�� �̸� ( 5 / 10 )
        roomName.text = roomNm;
        roomMaster.text = roomMs;
        roomJob.text = roomMasterJob;
        roomNumber.text = currPlayer + "/" + maxPlayer;
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

    public void OnClick()
    {
        //onChangeRoomName �� null �� �ƴ϶��
        if (onChangeRoomName != null)
        {
            onChangeRoomName(name);
        }


        ////1. InputRoomName ���ӿ�����Ʈ ã��.
        //GameObject go = GameObject.Find("InputRoomName");
        ////2. ã�� ���ӿ�����Ʈ���� InputField ������ ��������
        //InputField inputField = go.GetComponent<InputField>();
        ////3. ������ ������Ʈ�� �̿��ؼ� Text �� ����
        //inputField.text = name;
    }

    public void RoomJoinRoom()
    {
        if (isRoomPrivate == true)
        {
            KJY_PhotonRobby.Instance.JoinRoomPrivateSetting(roomCode);
        }
        else
        {
            KJY_PhotonRobby.Instance.JoinRoomNotPrivate();
        }
    }

}
