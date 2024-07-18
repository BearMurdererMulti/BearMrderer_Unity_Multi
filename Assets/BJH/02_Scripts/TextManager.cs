using System.Collections;
using System.Collections.Generic;
using TMPro;
using Unity.VisualScripting;
using UnityEngine;
using UnityEngine.UI;

public class TextManager : MonoBehaviour
{
    public static TextManager Instance;


    public TMP_Text tmp01;
    public TMP_Text tmp02;
    public TMP_Text tmp03;

    public string greeting;
    public string content;
    public string closing;

    public Button nextBtn;
    public bool isSkip;

    private void Awake()
    {
        if(Instance == null)
        {
            Instance = this;
        }    
    }

    // Start is called before the first frame update
    void Start()
    {
        greeting = InfoManagerKJY.instance.greeting;
        content = InfoManagerKJY.instance.content;
        closing = InfoManagerKJY.instance.closing;

        tmp01.text = InfoManagerKJY.instance.greeting;
        StartCoroutine(nameof(CoTyping));
    }

    void Update()
    {
        if(Input.GetMouseButtonDown(0))
        {
            isSkip = true;
        }
    }

    IEnumerator CoTyping()
    {
        yield return null;
        
        for(int i =  0; i < content.Length; i++)
        {
            if(isSkip)
            {
                tmp02.text = content;
                isSkip = false;
                break;
            }
            tmp02.text = content.Substring(0, i+1);
            yield return new WaitForSeconds(0.06f);
        }

        for (int i = 0; i < closing.Length; i++)
        {
            if (isSkip)
            {
                tmp03.text = closing;
                isSkip = false;
                break;
            }
            tmp03.text = closing.Substring(0, i+1);
            yield return new WaitForSeconds(0.06f);
        }
        nextBtn.gameObject.SetActive(true);
    }
}
