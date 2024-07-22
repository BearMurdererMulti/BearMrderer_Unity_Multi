using System.Collections;
using System.Collections.Generic;
using UnityEngine;

public class Mouse : ICharacterPart
{
    private int mouseType;
    public int MouseType
    {
        get
        {
            return mouseType;
        }
        private set
        {
            mouseType = value;
        }
    }

    public Mouse(int mouseType)
    {
        mouseType = mouseType;
    }

    public void Apply(GameObject character)
    {
        Debug.Log($"�� Ÿ���� {mouseType}���� �����մϴ�.");
    }
}
