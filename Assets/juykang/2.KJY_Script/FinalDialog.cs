using System.Collections;
using System.Collections.Generic;
using TMPro;
using UnityEngine;

public class FinalDialog : MonoBehaviour
{
    [SerializeField] GameObject murderSpot;
    [SerializeField] TMP_Text finalWord;
    [SerializeField] string murdername;
    [SerializeField] GameObject murder;

    private void Start()
    {
        ConnectionKJY.instance.RequestFinal();
        //murdername = InfoManagerKJY.instance.voteNpcObjectName;
        //murder = GameObject.Find(murdername);
        //murder.transform.position = murderSpot.transform.position;
        //murder.transform.rotation = murderSpot.transform.rotation;
    }

    public void FinalTexts(string word)
    {
        finalWord.text = word;
    }


}
