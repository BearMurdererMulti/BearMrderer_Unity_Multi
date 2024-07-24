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

    // Ŭ�� �Ǿ��� �� ȣ�� ���� �Լ��� ���� ����
    public Action<string> onChangeRoomName;

    public void SetInfo(string roomNm, string roomMs, int currPlayer, int maxPlayer)
    {
        //���� ���ӿ����� �̸��� ���̸��� ����
        name = roomNm;

        //�� ������ Text �� ���� 
        //�� �̸� ( 5 / 10 )
        roomName.text = roomNm;
        roomMaster.text = roomMs;
        roomNumber.text = currPlayer + "/" + maxPlayer;
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
}
