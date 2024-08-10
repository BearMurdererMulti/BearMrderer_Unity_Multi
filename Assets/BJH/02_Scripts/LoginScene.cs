using System.Collections;
using System.Collections.Generic;
using UnityEngine;

public class LoginScene : MonoBehaviour
{
    void Start()
    {
        AudioManager.Instnace.PlaySound(SoundList.LoginBG, 0.3f, 1f);
    }
}
