using System.Collections;
using System.Collections.Generic;
using UnityEngine;

public class NextScene_Test : MonoBehaviour
{
    private void Start()
    {
        StartCoroutine(CoWaitAndNextScene());
    }

    IEnumerator CoWaitAndNextScene()
    {
        yield return new WaitForSeconds(5f);

        KJY_SceneManager.instance.ChangeScene(SceneName.CharacterCustom_new);
    }
}
