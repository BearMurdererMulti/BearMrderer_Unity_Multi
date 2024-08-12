using DG.Tweening;
using Photon.Pun;
using System.Collections;
using System.Collections.Generic;
using TMPro;
using UnityEngine;
using UnityEngine.UI;

public class ChatManager : MonoBehaviourPunCallbacks
{
    public static ChatManager instance;

    [Header("talkBtn")]
    public GameObject interactiveBtn;

    [Header("talkTextList")]
    public Text talkingName;
    public TextMeshProUGUI dialog;
    [SerializeField] private GameObject xButton;
    [SerializeField] private GameObject nameObject;
    [SerializeField] private GameObject chat;
    [SerializeField] private GameObject talkPanel;


    [Header("talkTextList_notInterr")]
    public GameObject ButtonObject;
    public List<Button> buttons;
    public List<TextMeshProUGUI> buttonTexts;
    public GameObject talkButton;
    public GameObject KeyWord;
    
    [Header("cam")]
    [SerializeField] private Camera cam;
    [SerializeField] private CameraTopDown camTopDown;
    [SerializeField] private CameraBack camBack;
    [SerializeField] private GameObject building;
    
    [Header("value")]
    public bool talk;
    public bool npctalk;
    public bool interrogation = false;
    public string weapon = "BabyHammer";

    [Header("talkTextList_Interr")]
    [SerializeField] private TMP_InputField field;
    public GameObject inputFieldObject;
    public GameObject inputButton;
    public TMP_Text talkText;

    [Header("npc")]
    public GameObject nowNpc;
    public NpcData npcdata;
    public int action;
    public Dictionary<string, List<string>> npc_dialog;
    public Dictionary<int, string[]> talkData;

    int talkLengthTmp;
    public string sender;
    public string content;
    public bool isTimerover = false;

    private void Awake()
    {
        instance = this;
        talkData = new Dictionary<int, string[]>();
        chat.SetActive(false);
        ButtonObject.SetActive(false);
        talk = false;
        DOTween.Init();
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
        InitializeNPCLines();
        if (InfoManagerKJY.instance.role == "Detective")
        {
            cam = GameObject.Find("DollMainCamera").GetComponent<Camera>();
        }
        camTopDown = cam.GetComponent<CameraTopDown>();
        camBack = cam.GetComponent<CameraBack>();
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
    }

    private void InitializeNPCLines()
    {
        npc_dialog = new Dictionary<string, List<string>>
        {
            { "����", new List<string> {
                "�ȳ��ϼ���, Ž����! �뷡�� ��Ʈ�� �帱���?",
                "��⸦ ���� ������. ������ �뷡ó�� �Ƹ�����ϴ�.",
                "������ �ִ� �������� ����� �ذ��غ�����!"
            }},
            { "���Ǿ�", new List<string> {
                "������ �����ϴ� ���� ������ ���� �ſ���.",
                "���δ� ħ�� �ӿ��� ���� ū ����� �����.",
                "������ ã�� ��Ű���? ������ �ʿ��ϴٸ� �������ּ���."
            }},
            { "������", new List<string> {
                "���� �׸��� �ӿ� ������ ����� ã�� ��Ű���?",
                "�޺� �Ʒ����� ��� ���� �ٸ��� ������.",
                "����� ������ ������ ����� �巯���⵵ �ؿ�."
            }},
            { "�̻级", new List<string> {
                "�ȳ��ϳ�, Ž����! ������ � �߰��� �߳ĳ�?",
                "�ɳ�! ������ ����� ���� �� �˷��帱�Գ�!",
                "�츮 ������ ��ȭ�� �����ּż� �����ϴٳ�~"
            }},
            { "�˷���", new List<string> {
                "���ο� ������ ������ �����������. ������ �߰��ϼ̳���?",
                "���� ���������� ���캸�̳���? ��ģ ���� �������� ����.",
                "������ ���� �߰��ϴ� �����̿���. �� ���絵 ����������."
            }},
            { "���ݶ�", new List<string> {
                "�״��� ������ �� ����� �ذ�Ǳ⸦ �ٶ�ɴϴ�.",
                "�� �������� ��Ʈ�� ã���̴�����?",
                "�ð��� ��� ���� ������ ���̿ɴϴ�."
            }},
            { "�ø���", new List<string> {
                "���� ����� ������ �ٸ� ����� ǰ�� ����.",
                "�����ϼ���, Ž����. ���Ӽ��� �Ѿ�� �ʵ���.",
                "���δ� ���� ����� ���̴� ���� ���� ū ������ �� �־��."
            }},
            { "����", new List<string> {
                "�� ��ǿ� ���� ���� �ҹ��� �ִµ� ���Ƿ���?",
                "��� �̾߱� �ӿ��� ������ ������ ���� �־��.",
                "������ � ��̷ο� �̾߱⸦ ����ֽ� �ǰ���?"
            }},
            { "�׿�", new List<string> {
                "����ó�� ��ǵ� ������ �־��. �� �帧�� ���󰡺�����.",
                "�� �뷡�� ����� ���翡 ������ �� �� �ֱ⸦.",
                "���δ� ����ȭ�� �ӿ��� ������ �巯���⵵ �ؿ�."
            }},
            { "�ڽ���", new List<string> {
                "���� ���� ������ �����ؿ�. �����ϰ� �����ϼ���.",
                "��� ���� �ǽ��غ��� �͵� ���� �������.",
                "������ ���� �츮�� �ϰ� ���� �Ͱ� �ٸ� �� �־��."
            }},
            { "���Ϸ�", new List<string> {
                "���ο� ��Ҹ� Ž���ϸ鼭 ����� �ܼ��� �߰��� �� �־��!",
                "�������� ������! ���� ã�� ���� ���� ���� �ſ���.",
                "������ ������ ����ġ ���� ������ ���۵���."
            }},
            { "����", new List<string> {
                "�ڿ��� ���� ����� �����ϰ� �־��. ���� ��� �����غ�����.",
                "���δ� ���� ���� �����ٿ��� ū ����� ���� ���� �� �־��.",
                "�ڿ��� ������ ���󰡴� ���� ���ǿ� �ٰ��� �� ���� �ſ���."
            }},
            { "�ֽ�", new List<string> {
                "�� ���, ������ �ʳ���? �ʹ� �ɰ������� ������~",
                "������ �������� �������� �ɸ��� �ʰ� �����ϼ���~",
                "���� ������ ���� ������. ���δ� ���� �ӿ� ������ �ִ�ϴ�!"
            }},
            { "������", new List<string> {
                "Ž������ ���� Ư���� ����, �غ�Ǽ̳���?",
                "���� �ӿ��� �ܼ��� ã�ƺ����? ¥��~!",
                "����ó�� ����� ����, �� ������ ��ã�Ƶ帱�Կ�!"
            }},
            { "�ڵ���", new List<string> {
                "�� �� ���ּ���, Ž����! ���� �޶� ������ �ʳ���?",
                "Ž����, ���� ã�� �ܼ��� �־��. ���� �� �����ּ���!",
                "������ Ư���� ������ �ܼ��� ã�Ҿ��. Ī�����ֽ� ����?"
            }},
            { "§§��", new List<string> {
                "����� ����� ������ ������ �����غ� �� ���� �������?",
                "�� ������ �����, ������ ���� �;� ������ �����!",
                "�����ó�� �Ϳ��� ���̰� ������ �ƴϰ���? ����!"
            }},
            { "�±�Ƽ��", new List<string> {
                "�� ����� ��Ʃ�꿡 �ø��� ��ȸ�� ��ڳ� �� ���ƿ�!",
                "Ž����, ������ ���� �����ε带 �� �� �ɱ��?",
                "���� ���� ������ 'õ�� Ž���� �Ϸ�'�� �ϸ� ����?"
            }},
            { "������", new List<string> {
                "�׸� �ӿ� ������ �ܼ��� �߰��ϼ̳���?",
                "������ ����ġ �ӿ� ������ ���� �������� �����.",
                "������ ��� ������ �����⸦ ��� �׷��þ��. ������ �ɱ��?"
            }}
        };
    }

    public void StartKeyWordCanVas()
    {
        KeyWord.SetActive(true);
    }

    public void StartTalk()
    {
       interactiveBtn.SetActive(false);
       inputFieldObject.SetActive(false);
       chat.SetActive(true);
       xButton.SetActive(true);
       talk = true;
       camTopDown.enabled = false;
       camBack.enabled = true;
       nowNpc.GetComponent<NpcFaceMove>().talking = true;
       nowNpc.GetComponent<Collider_BJH>().isTalking = true;
       ManageField();
    }

    public void Startinterrogation()
    {
        talk = true;
        interrogation = true;
        camTopDown.enabled = false;
        camBack.enabled = false;
        xButton.SetActive(false);

        // ����ȯ �߰�
        // ������ �����ϴ� �޼���
        PhotonView pv = WeaponManager.Instance.gameObject.GetComponent<PhotonView>();
        pv.RPC("PutdownButtonActive", RpcTarget.Others, true);

        //PhotonConnection.Instance.UpdatePutdownButtonActive();
        //StartTalkinterrogation();//�ӽ�
    }

    public void StartTalkinterrogation()
    {
        chat.SetActive(true);
        talkPanel.SetActive(true);
        ManageField();
    }

    public string ShowDialogue(string npcName)
    {
        if (npc_dialog.TryGetValue(npcName, out List<string> lines))
        {
            int randomIndex = Random.Range(0, lines.Count);
            string selectedLine = lines[randomIndex];
            return selectedLine;
        }
        else
        {
            return ("��ø� ��޷��ּ���..");
        }
    }

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
                PhotonConnection.Instance.UpdateDayAndNight(false);
                PhotonConnection.Instance.UpdateCitizenCall();
            }
        }

        talk = false;
        npctalk = false;
        
        interactiveBtn.SetActive(true);
        chat.SetActive(false);
        ButtonObject.SetActive(false);
        
        camTopDown.enabled = true;
        camBack.enabled = false;
        interrogation = false;

        nowNpc.GetComponent<Collider_BJH>().isTalking = false;
        nowNpc.GetComponent<NpcFaceMove>().talking = false;

        for (int i = 0; i <buttons.Count; i++)
        {
            buttons[i].interactable = true;
        }

        if (building != null)
        {
            building.SetActive(true);
        }
    }

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
            dialog.text = data;
            talkLengthTmp++;
        }
    }

    public void ManageField()
    {
        //talkLengthTmp = 0;
        talkButton.SetActive(false);
        talkingName.text = npcdata.npcName;
        npctalk = false;
        if (interrogation == false)
        {
            dialog.text = ShowDialogue(npcdata.npcName);
            ConnectionKJY.instance.Request_Question(npcdata.npcName, weapon);
        }
        else
        {
            dialog.text = "Ž����, �� ������ �ƴմϴ�!";
            TextEffectInterr();
            if (InfoManagerKJY.instance.role == "Detective")
            {
                ConnectionKJY.instance.RequestInterrogationStart(npcdata.npcName, weapon);
            }
        }
    }

    // ���⼭���� TalkCode
    public void OnclickSend()
    {
        inputFieldObject.SetActive(false);
        // �ӽ�
        string url = "http://ec2-43-201-108-241.ap-northeast-2.compute.amazonaws.com:8081/api/v1/chat/send";
        string chatContent = field.text;

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
        field.text = "";


        // ������ ���� �� �ٷ� ��� ����
        ChatConnection chatConnection = new ChatConnection(str);
    }

    // NPC�� ��ȭâ���� ���� ��ư�� �ǹ�
    // ��ư�� ������ �÷��̾� input filed�� Ȱ��ȭ
    public void OnClickNext()
    {
        if (interrogation == false)
        {
            if (UI.instance.lifeCount < 0)
            {
                if (npctalk == true)
                {
                    FinishTalk();
                    PhotonConnection.Instance.UpdateDayAndNight(false);
                    PhotonConnection.Instance.UpdateCitizenCall();
                }
                return;
            }
            dialog.text = ShowDialogue(npcdata.npcName);
            ButtonObject.SetActive(true);
            talkButton.SetActive(false);
        }
        else
        {
            if (GameManager_KJY.instance.heartRate >= 120 || isTimerover == true)
            {
                
                photonView.RPC("ResetInterrogation", RpcTarget.All);
                // ����ȯ �߰�
                // ������ ������ ����
                PhotonView pv = WeaponManager.Instance.gameObject.GetComponent<PhotonView>();
                pv.RPC("PutdownButtonActive", RpcTarget.Others, true);
                inputFieldObject.SetActive(false);
            }
            inputFieldObject.SetActive(true);
            dialog.text = string.Empty;
            inputButton.SetActive(true);
        }

        //KJY - bool�� ����
        npctalk = false;
        //talkPanel.gameObject.SetActive(false);
        //talkingName.text = InfoManagerKJY.instance.nickname;
    }

    public void OnClickChat(int index)
    {
        ConnectionKJY.instance.RequestAnswer(index, npcdata.npcName, weapon);
        PhotonConnection.Instance.UpdateMinusLife();
        ButtonObject.SetActive(false);
        buttons[index].interactable = false;
    }

    public void ChatButtonList(List<Questions> questions)
    {
        for(int i = 0;i < questions.Count; i++)
        {
            buttonTexts[i].text = questions[i].question;
        }
        ButtonObject.SetActive(true);
    }

    public void OnClickInterrogation()
    {
        if (InfoManagerKJY.instance.role == "Detective")
        {
            if (InfoManagerKJY.instance.role == "Detective")
            {
                string name = npcdata.npcName;
                ConnectionKJY.instance.RequestInterrogationConversation(name, talkText.text);
                talkingName.text = name;
                inputFieldObject.SetActive(false);
                inputButton.SetActive(false);
                field.text = string.Empty;
            }
        }
    }

    [PunRPC]
    private void TextEffectInterr()
    {
        StartCoroutine(effect());
    }

    private IEnumerator effect()
    {
        UI.instance.textObject.SetActive(true);
        yield return new WaitForSeconds(0.5f);
        UI.instance.textList[0].transform.DOPunchPosition(Vector3.down, 5, 10, 1, false);
        yield return new WaitForSeconds(0.5f);
        UI.instance.textList[1].transform.DOPunchPosition(Vector3.down, 5, 10, 1, false);
        yield return new WaitForSeconds(0.5f);
        UI.instance.textList[2].transform.DOPunchPosition(Vector3.down, 5, 10, 1, false);
        yield return new WaitForSeconds(0.5f);
        UI.instance.textList[3].transform.DOPunchPosition(Vector3.down, 5, 10, 1, false);
        yield return new WaitForSeconds(1f);
        UI.instance.textList[0].transform.DOMoveX(-1100, 2f, false);
        UI.instance.textList[1].transform.DOMoveY(-1100, 2f, false);
        UI.instance.textList[2].transform.DOMoveY(2000, 2f, false);
        UI.instance.textList[3].transform.DOMoveX(2500, 2f, false);
        yield return new WaitForSeconds(2f);
        UI.instance.textObject.SetActive(false);
        UI.instance.ResetText();
        talkButton.SetActive(true);
        PhotonConnection.Instance.StartTimer();
    }

    [PunRPC]
    public void SetWeapon(string name)
    {
        this.weapon = name;
    }

    [PunRPC]
    public void ResetInterrogation()
    {
        GameManager_KJY.instance.interrogationBtn(true);
        chat.SetActive(false);
        dialog.text = string.Empty;
        isTimerover = false;
        interrogation = false;
        talk = false;
    }
}
