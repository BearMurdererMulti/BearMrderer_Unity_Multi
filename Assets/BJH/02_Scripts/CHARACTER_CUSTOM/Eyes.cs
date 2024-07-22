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
        Debug.Log($"눈 타입을 {eyeType}으로 변경합니다.");
    }
}
