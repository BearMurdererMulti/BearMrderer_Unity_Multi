using JetBrains.Annotations;
using System.Collections;
using System.Collections.Generic;
using TMPro;
using Unity.VisualScripting.FullSerializer;
using UnityEngine;
using UnityEngine.UI;
using static ConnectionKJY;
using static System.Net.WebRequestMethods;

public class UI : MonoBehaviour
{


    public Button_BJH startBtn;


    public static UI instance;

    [Tooltip("Day")]
    public Camera cam;

    [Tooltip("Day")] 
    public GameObject dayObject; //KJY 추가
    public Image dayImg;
    public TMP_Text tmp01, tmp02, tmp03; 
    public int dayInt = 1;
    //public Slider timeSlider;
    //public Image silderBG, sliderFill;
    //public float timerSpeed = 0.1f;

    [Tooltip("Light")]
    public Transform trLight;
    public float lerpSpeed = 0.1f; 
    private float lerpTime = 0; 
    float lerpX = 0;
    float x = 140;

    [Tooltip("Chat")]
    public GameObject chat;

    public GameObject lifeObject; //KJY 추가
    public Image[] lifes;
    public int lifeCount = 4;

    public Button_BJH noteBtn;
    public TMP_Text noteBtnTmp;
    public GameObject noteImg;
    public GameObject profile;
    public Transform parent;
    public Transform checkListParent;
    public GameObject noteObject; //KJY 추가
    
    public GameObject settingImg;

    [Tooltip("Death Info")]
    public GameObject deathInfoImg;
    public TMP_Text deathInfoText;

    [Tooltip("Button")]
    public GameObject talkBtn, skipBtn, selectBtn;
    public Button talkBt;


    [Tooltip("ChatHistory")]
    public Transform chatHistoryParent;
    public GameObject chatHistoryScrollView;
    public GameObject npcChatGroup;
    public GameObject playerChatGroup;
    public Dictionary<string, bool> dicChatHistoryState = new Dictionary<string, bool>(); // NCP1ChatHistoryImg : true



    public Transform ox;
    public GameObject oxBox;
    public GameObject checkListProfilePre;

    // chat list

    private void Awake()
    {
        instance = this;
    }

    private void Start()
    {
        //startBtn.gameObject.SetActive(false);

        noteImg.SetActive(false);

        //timeSlider.value = 0;
        //sliderFill.GetComponent<Image>().color = Color.green;
        //tmp01.text = dayInt.ToString(); (KJY 추가함)
        dayInt = InfoManagerKJY.instance.gameDay;
        tmp01.text = (InfoManagerKJY.instance.gameDay + 1).ToString();

        settingImg.SetActive(false);

        deathInfoImg.SetActive(false);

        skipBtn.SetActive(false);
        selectBtn.SetActive(false);

        chatHistoryScrollView.SetActive(false);
    }

    // 통신된 npc list를 가져와서 ui를 셋팅하는 함수
    public void SetNoteUI()
    {
        //List<TestGameNpcList> npcNameList = InfoManagerBJH.instance.gameNpcList;
        List<GameNpcSetting> npcNameList = InfoManagerKJY.instance.gameNpcList;

        int npcNum = 1;
        // 수첩 '채팅 리스트 이미지', '체크리스트 박스' 생성
        for (int i = 0; i < npcNameList.Count; i++)
        {
            // Instantiate
            Instantiate(profile, parent);
            Instantiate(checkListProfilePre, checkListParent);

            Instantiate(chatHistoryScrollView, chatHistoryParent);
            Instantiate(oxBox, ox);

            // rename
            Transform tr = parent.GetChild(i); // profile prefab
            Transform t2 = checkListParent.GetChild(i); // check list profile

            Transform t3 = chatHistoryParent.GetChild(i); // chat history img
            Transform trOxBox = ox.GetChild(i);

            tr.gameObject.name = npcNameList[i].npcName;
            t2.gameObject.name = npcNameList[i].npcName;
            t2.GetComponent<CheckListProfilePre>().nickName.text = npcNameList[i].npcName;
            t3.gameObject.name = npcNameList[i].npcName + "ChatHistoryImg";
            dicChatHistoryState[t3.gameObject.name] = false;
            trOxBox.gameObject.name = npcNameList[i].npcName;

            // set profile image
            Image img = tr.Find("Image").GetComponent<Image>();
            Image imgg = t2.Find("CheckListProfile").GetComponent<Image>();

            string npc = "npc0";
            
            Sprite pic = Resources.Load<Sprite>("NPC/" + npc + npcNum.ToString());
            Sprite picc = Resources.Load<Sprite>("NPC/" + npc + npcNum.ToString());
            Debug.Log("NPC/" + npc + npcNum.ToString());
            npcNum++;

            img.sprite = pic;
            imgg.sprite = picc;

            // set active
            t3.gameObject.SetActive(false);
        }
    }

    //private void Timer()
    //{
    //    timeSlider.value += Time.deltaTime * timerSpeed;

    //    if (timeSlider.value >= 1)
    //    {
    //        timeSlider.value = 0;
    //        sliderFill.GetComponent<Image>().color = Color.green;
    //        tmp03.text = "��";
    //        dayInt++;
    //        tmp01.text = dayInt.ToString();
    //        //GameManagerBJH.instance.Day();
    //    }
    //    else if (timeSlider.value >= 0.9f)
    //    {
    //        sliderFill.GetComponent<Image>().color = Color.red;
    //        //GameManagerBJH.instance.Night();

    //    }
    //    else if (timeSlider.value >= 0.6f)
    //    {
    //        sliderFill.GetComponent<Image>().color = Color.yellow;
    //        //GameManagerBJH.instance.AfterNoon();

    //        tmp03.text = "��";
    //    }
    //}

    // �ϼ��� �����ϴ� �Լ�
    public void UpdateDay()
    {
        dayInt++;
        tmp01.text = dayInt.ToString();


    }

    // ��, ���� �����ϴ� �Լ�
    public void DayAndNight(bool b)
    {
        // true == ��
        if(b)
        {
            tmp03.text = "일차";
            trLight.rotation = Quaternion.Euler(170, 240, 0);

            //lerpTime += Time.deltaTime * lerpSpeed;

            //lerpX = Mathf.Lerp(x, 140, lerpTime);

            //Vector3 vector3 = new Vector3(140, 240, 0);
            //trLight.rotation = Quaternion.Slerp(transform.rotation, Quaternion.Euler(vector3), lerpTime * Time.deltaTime);

            //x = 140;

            //if(lerpTime >= 1)
            //{
            //    lerpTime = 0;
            //}
        }

        // false == ��
        if (!b)
        {
            tmp03.text = "밤";
            trLight.rotation = Quaternion.Euler(190, 240, 0);

            //lerpTime += Time.deltaTime * lerpSpeed;

            //lerpX = Mathf.Lerp(x, 195, lerpTime);

            //trLight.rotation = Quaternion.Euler(lerpX, trLight.rotation.eulerAngles.y, trLight.rotation.eulerAngles.z);

            //x = 195;

            //if (lerpTime >= 1)
            //{
            //    lerpTime = 0;
            //}
        }

    }

    public void MinusLife()
    {
        lifes[lifeCount].gameObject.SetActive(false);
        lifeCount--;

        if(lifeCount < 0 && Input.GetKeyDown(KeyCode.P))
        {
            DayAndNight(false);
            StartCoroutine(KJY_CitizenManager.Instance.CitizenCall());
        }
    }

    public void InitLife()
    {
        foreach(Image img in lifes)
        {
            img.gameObject.SetActive(true);
        }
        lifeCount = 4;
    }

    public void OnclickOpen(GameObject go)
    {
        go.SetActive(true);
    }

    public void OnClickClose(GameObject go)
    {
        go.SetActive(false);
    }

    
    // �÷��̾ npc�� �������� ��
    public void DeathINfo(string deathName, string job)
    {
        string text = $"{deathName}�� {job}(��)�����ϴ�.";

        StartCoroutine(CoText(text));
    }

    // �÷��̾ npc�� �������� �ʾ��� ��
    public void DeathInfo()
    {
        string text = "아무일도 일어나지 않았습니다.";

        StartCoroutine(CoText(text));

    }

    // ���Ǿư� NPC�� ���̰ų� ������ �ʾ��� ��
    public void DeathINfo(bool state, string dethName)
    {
        if(state)
        {
            string text = $"{dethName}��(��) ���ش��߽��ϴ�.";
            StartCoroutine(CoText(text));

        }
        else if(!state)
        {
            string text = "�ƹ��� ���ش����� �ʾҽ��ϴ�.";
            StartCoroutine(CoText(text));

        }
    }

    // text ����ϴ� �ڷ�ƾ
    IEnumerator CoText(string text)
    {
        yield return new WaitForSeconds(1f);

        deathInfoImg.SetActive(true);

        for (int i = 0; i < text.Length; i++)
        {
            deathInfoText.text = text.Substring(0, i);
            yield return new WaitForSeconds(0.1f);
        }
    }




    public void AppearBtn()
    {
        startBtn.gameObject.SetActive(true);
    }






}

// observer pattern으로 구현
public class Subject : ISubject
{
    public List<IObserver> observers;

    public Subject()
    {
        observers = new List<IObserver>();
    }

    public void Add(IObserver observer)
    {
        observers.Add(observer);
    }


    public void Remove(IObserver observer)
    {
        if (observers.IndexOf(observer) >= 0)
        {
            observers.Remove(observer);
        }
        else
        {
            Debug.Log("삭제 할 옵저버가 없습니다.");
        }
    }
    public void Notify(ISubject subject)
    {
        observers.ForEach(ob => ob.Notify());
    }



}