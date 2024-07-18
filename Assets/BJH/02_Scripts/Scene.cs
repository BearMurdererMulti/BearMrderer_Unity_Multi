using System.Collections;
using System.Collections.Generic;
using UnityEngine;

public class Scene : MonoBehaviour
{
    public void OnClickNextScene()
    {
        UnityEngine.SceneManagement.SceneManager.LoadScene("CharacterCustom");
    }
}
