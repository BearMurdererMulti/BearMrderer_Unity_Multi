using System.Collections;
using System.Collections.Generic;
using UnityEngine;

public class Eyes : ICharacterPart
{
    private int eyeType;
    public int EyeType
    {
        get
        {
            return eyeType;
        }
        private set
        {
            eyeType = value;
        }
    }

    public Eyes(int eyeType)
    {
        eyeType = eyeType;
    }

    public void Apply(GameObject character)
    {
        Debug.Log($"�� Ÿ���� {eyeType}���� �����մϴ�.");
    }
}
