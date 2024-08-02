using System.Collections;
using System.Collections.Generic;
using TMPro;
using UnityEngine;
using UnityEngine.UI;

public class ChatManager : MonoBehaviour
{
    public static ChatManager instance;

    public GameObject interactiveBtn;
    [SerializeField] GameObject dialog;
    public TMP_Text talkText;
    public Text talkingName;
    [SerializeField] Camera cam;
    public bool talk;
    public bool npctalk;
    [SerializeField] CameraTopDown camTopDown;
    [SerializeField] CameraBack camBack;
    [SerializeField] GameObject building;
    [SerializeField] TMP_InputField field;
    

    public GameObject nowNpc;
    public NpcData npcdata;
    public int action;

    int talkLengthTmp;

    public Dictionary<int, string[]> talkData;

    public List<Button > buttons;

    public List<TextMeshProUGUI> buttonTexts;

    // Chat
    public TMP_InputField inputText;
    public TMP_Text npcText;

    bool isFirst = true;

    public GameObject ButtonObject;
    public string sender;
    public string content;
    public bool isConnection;

    string weapon = "Į";

    private void Awake()
    {
        instance = this;
        talkData = new Dictionary<int, string[]>();
        dialog.SetActive(false);
        field.gameObject.SetActive(false);
        ButtonObject.SetActive(false);
        talk = false;
    }

    private void Start()
    {
        npctalk = false;
        talkLengthTmp = 0;
        
        for (int i = 0; i < buttons.Count; i++)
        {
            int index = i; // ���� ������ �ε����� �����Ͽ� ĸó
            buttons[i].onClick.AddListener(() => OnClickChat(index));
        }
    }

    private void Update()
    {
        if (talk == true)
        {
            Ray ray = cam.ViewportPointToRay(new Vector3(0.5f, 0.5f, 0f));
            RaycastHit hit;
            if (Physics.Raycast(ray, out hit, 100f))
            {
                if (hit.collider.CompareTag("Buildings"))
                {
                    building = hit.collider.gameObject;
                    building.SetActive(false);
                }
            }
        }

        if (Input.GetKeyDown(KeyCode.Alpha1))
        {
            weapon = "Į";
        }
        if (Input.GetKeyDown(KeyCode.Alpha2))
        {
            weapon = "�վ";
        }
        if (Input.GetKeyDown(KeyCode.Alpha3))
        {
            weapon = "����";
        }

        if(isConnection)
        {
            npcText.text = content;
            //isConnection = false;
        }
        else 
        {
            npcText.text = "��ø� ��޷��ּ���...";
        }
    }

    public void StartTalk()
    {
        interactiveBtn.SetActive(false);
        dialog.SetActive(true);
        talk = true;
        ManageField();
        camTopDown.enabled = false;
        camBack.enabled = true;
        nowNpc.GetComponent<NpcFaceMove>().talking = true;
        isConnection = true;
    }

    //public string GetTalk(int id, int talkIndex)
    //{
    //    if (talkIndex == talkData[id].Length)
    //    {
    //        return null;
    //    }
    //    else
    //    {
    //        return talkData[id][talkIndex];
    //    }
    //}

    public string GetTalkTmp(string value)
    {
        if (talkLengthTmp != 0)
        {
            return null;
        }
        else
        {
            talkLengthTmp++;
            return value;
        }
    }

    public void FinishTalk()
    {
        if (UI.instance.lifeCount < 0)
        {
            if (npctalk == true)
            {
                UI.instance.DayAndNight(false);
                StartCoroutine(KJY_CitizenManager.Instance.CitizenCall());
            }
        }

        talk = false;
        npctalk = false;
        isConnection = false;
        
        interactiveBtn.SetActive(true);
        dialog.SetActive(false);
        
        camTopDown.enabled = true;
        camBack.enabled = false;

        nowNpc.GetComponent<NpcFaceMove>().talking = false;

        if (building != null)
        {
            building.SetActive(true);
        }
    }

    //void Talk(long id, bool isNpc)
    //{

    //    string data = GetTalk((int)id, talkLength);
    //    if (UI.instance.lifeCount < 0)
    //    {
    //        FinishTalk();
    //    }
    //    else if (data == null)
    //    {
    //        ManageField();
    //    }
    //    else
    //    {
    //        name.text = npcdata.npcName;
    //        talkText.text = data;
    //        talkLength++;
    //    }
    //}

    void TalkTmp(string value)
    {

        string data = value;
        if (UI.instance.lifeCount < 0)
        {
            FinishTalk();
        }
        else if (data == null)
        {
            ManageField();
        }
        else
        {
            talkingName.text = npcdata.npcName;
            npcText.text = data;
            talkLengthTmp++;
        }
    }

    public void ManageField()
    {
        //talkLengthTmp = 0;
        //field.gameObject.SetActive(true);
        talkingName.text = npcdata.npcName;
        isConnection = true;
        talkText.text = "������ �ݰ���! ������ ���� ����� �־�?";
        ConnectionKJY.instance.Request_Question(npcdata.npcName, weapon);
        npctalk = false;
    }

    public void EmitChat() 
    { 
        if (field.text.Length > 0) 
        {
            UI.instance.MinusLife();
            field.gameObject.SetActive(false);
            field.text = "";
            TalkTmp(npcText.text);
            npctalk = true;
        } 
    }

    // ���⼭���� TalkCode
    public void OnclickSend()
    {
        UI.instance.MinusLife();
        inputText.gameObject.SetActive(false);
        // �ӽ�
        string url = "http://ec2-43-201-108-241.ap-northeast-2.compute.amazonaws.com:8081/api/v1/chat/send";
        string chatContent = inputText.text;

        string receiver = npcdata.npcName;
        int chatDay = UI.instance.dayInt;

        // ����ü ���
        StrTryChat str;
        str.url = url;
        str.gameSetNo = InfoManagerKJY.instance.gameSetNo; // �ӽ�
        str.secertKey = "mafia"; // �ӽ�
        str.chatContent = chatContent;
        str.receiver = receiver;
        str.chatDay = chatDay;
        str.sender = InfoManagerKJY.instance.playerName;

        //KJY - bool�� ����
        npctalk = true;

        // text ����
        talkingName.text = InfoManagerKJY.instance.nickname;
        inputText.text = "";


        // ������ ���� �� �ٷ� ��� ����
        ChatConnection chatConnection = new ChatConnection(str);
    }

    // NPC�� ��ȭâ���� ���� ��ư�� �ǹ�
    // ��ư�� ������ �÷��̾� input filed�� Ȱ��ȭ
    public void OnClickNext()
    {
        if (UI.instance.lifeCount < 0)
        {
            if (npctalk == true)
            {
                FinishTalk();
                UI.instance.DayAndNight(false);
                StartCoroutine(KJY_CitizenManager.Instance.CitizenCall());
            }
            return;
        }

        //KJY - bool�� ����
        npctalk = false;

        isConnection = false;

        talkingName.text = InfoManagerKJY.instance.nickname;
        ButtonObject.SetActive(true);
    }

    public void OnClickChat(int index)
    {
        //ConnectionKJY.instance.re
        ConnectionKJY.instance.RequestAnswer(index, npcdata.npcName, weapon);
        UI.instance.MinusLife();
    }

    public void ChatButtonList(List<Questions> questions)
    {
        for(int i = 0;i < questions.Count; i++)
        {
            buttonTexts[i].text = questions[i].question;
        }
    }
}
