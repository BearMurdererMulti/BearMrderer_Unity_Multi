using System.Collections;
using System.Collections.Generic;
using UnityEngine;

public class UserInfoManager_BJH : MonoBehaviour
{
    public static UserInfoManager_BJH instance;

    private void Awake()
    {
        if(instance == null)
        {
            instance = this;
        }
    }

    // ���� ������
    public string userName;
    public string token;
    
}
