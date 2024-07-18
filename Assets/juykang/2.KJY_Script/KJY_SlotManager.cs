using System.Collections;
using System.Collections.Generic;
using TMPro;
using UnityEngine;
using UnityEngine.UI;

public class KJY_SlotManager : MonoBehaviour
{
    public static KJY_SlotManager Instance;

    List<LoginGameSetDTO> dtoList = new List<LoginGameSetDTO>();

    [SerializeField] List<Button> slotButton;

    [SerializeField] List<TMP_Text> slotDayText;

    [SerializeField] List<TMP_Text> slotTimeText;

    [SerializeField] GameObject loadGameData;

    [SerializeField] List<GameObject> nodataImage;

    private int id;

    private void Awake()
    {
        Instance = this;
    }

    private void CheckSlot()
    {
        for (int i = 0; i < dtoList.Count - 1; i++)
        {
            if (i == 3)
            {
                break;
            }
            if (dtoList[i] != null)
            {
                SetSlotUI(i);
                nodataImage[i].SetActive(false);
            }
            else
            {
                nodataImage[i].SetActive(true);
            }
        }
    }

    public void SetDTOList()
    {
        dtoList = InfoManagerKJY.instance.loginGameSetDTO;
        if (dtoList.Count != 0)
        {
            CheckSlot();
        }
    }

    public void SetSlotUI(int i)
    {
        slotButton[i].interactable = true;
        slotDayText[i].text = dtoList[i].gameDay.ToString();
        slotTimeText[i].text = (dtoList[i].modifiedAt - dtoList[i].createdAt).ToString();
    }

    public void ClickLoadSlot1()
    {
        id = 0;
    }

    public void ClickLoadSlot2()
    {
        id = 1;
    }

    public void ClickLoadSlot3()
    {
        id = 2;
    }

    public void ClickLoadSlot()
    {
        InfoManagerKJY.instance.gameSetNo = dtoList[id].gameSetNo;
        ConnectionKJY.instance.RequestLoad();
    }
}
