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
            int index = i; // 로컬 변수에 인덱스를 저장하여 캡처
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
            { "레오", new List<string> {
                "안녕하세요, 탐정님! 노래로 힌트를 드릴까요?",
                "용기를 잃지 마세요. 진실은 노래처럼 아름답습니다.",
                "음악이 주는 영감으로 사건을 해결해보세요!"
            }},
            { "소피아", new List<string> {
                "조용히 관찰하다 보면 진실이 보일 거예요.",
                "때로는 침묵 속에서 가장 큰 비밀이 들려요.",
                "무엇을 찾고 계신가요? 도움이 필요하다면 말씀해주세요."
            }},
            { "마르코", new List<string> {
                "밤의 그림자 속에 숨겨진 비밀을 찾고 계신가요?",
                "달빛 아래서는 모든 것이 다르게 보이죠.",
                "어둠이 내리면 진실이 모습을 드러내기도 해요."
            }},
            { "이사벨", new List<string> {
                "안녕하냥, 탐정님! 오늘은 어떤 발견을 했냐냥?",
                "냥냥! 수상한 사람을 보면 꼭 알려드릴게냥!",
                "우리 마을의 평화를 지켜주셔서 감사하다냥~"
            }},
            { "알렉스", new List<string> {
                "새로운 모험은 언제나 흥미진진하죠. 무엇을 발견하셨나요?",
                "마을 구석구석을 살펴보셨나요? 놓친 곳이 있을지도 모르죠.",
                "여행은 나를 발견하는 과정이에요. 이 수사도 마찬가지죠."
            }},
            { "니콜라스", new List<string> {
                "그대의 지혜로 이 사건이 해결되기를 바라옵니다.",
                "옛 문서에서 힌트를 찾으셨는지요?",
                "시간이 모든 것을 밝혀줄 것이옵니다."
            }},
            { "올리버", new List<string> {
                "밤의 세계는 낮과는 다른 비밀을 품고 있죠.",
                "조심하세요, 탐정님. 속임수에 넘어가지 않도록.",
                "때로는 가장 명백해 보이는 것이 가장 큰 거짓일 수 있어요."
            }},
            { "비비안", new List<string> {
                "이 사건에 대해 들은 소문이 있는데 들어보실래요?",
                "모든 이야기 속에는 진실의 조각이 숨어 있어요.",
                "오늘은 어떤 흥미로운 이야기를 들려주실 건가요?"
            }},
            { "테오", new List<string> {
                "음악처럼 사건도 리듬이 있어요. 그 흐름을 따라가보세요.",
                "제 노래가 당신의 수사에 영감을 줄 수 있기를.",
                "때로는 불협화음 속에서 진실이 드러나기도 해요."
            }},
            { "자스민", new List<string> {
                "증거 없는 추측은 위험해요. 신중하게 접근하세요.",
                "모든 것을 의심해보는 것도 좋은 방법이죠.",
                "진실은 때로 우리가 믿고 싶은 것과 다를 수 있어요."
            }},
            { "마일로", new List<string> {
                "새로운 장소를 탐험하면서 뜻밖의 단서를 발견할 수 있어요!",
                "포기하지 마세요! 아직 찾지 못한 곳이 있을 거예요.",
                "모험은 언제나 예상치 못한 곳에서 시작되죠."
            }},
            { "레나", new List<string> {
                "자연은 많은 비밀을 간직하고 있어요. 주의 깊게 관찰해보세요.",
                "때로는 가장 작은 나뭇잎에도 큰 비밀이 숨어 있을 수 있어요.",
                "자연의 순리를 따라가다 보면 진실에 다가갈 수 있을 거예요."
            }},
            { "애쉬", new List<string> {
                "이 사건, 웃기지 않나요? 너무 심각해하지 마세요~",
                "범인을 잡으려다 웃음벨에 걸리지 않게 조심하세요~",
                "유머 감각을 잃지 마세요. 때로는 웃음 속에 진실이 있답니다!"
            }},
            { "김쿵야", new List<string> {
                "탐정님을 위한 특별한 마술, 준비되셨나요?",
                "모자 속에서 단서를 찾아볼까요? 짜잔~!",
                "마법처럼 사라진 진실, 제 마술로 되찾아드릴게요!"
            }},
            { "박동식", new List<string> {
                "저 좀 봐주세요, 탐정님! 뭔가 달라 보이지 않나요?",
                "탐정님, 제가 찾은 단서가 있어요. 관심 좀 가져주세요!",
                "오늘은 특별히 열심히 단서를 찾았어요. 칭찬해주실 거죠?"
            }},
            { "짠짠영", new List<string> {
                "춘식이 굿즈로 범인의 취향을 유추해볼 수 있지 않을까요?",
                "이 한정판 춘식이, 범인이 갖고 싶어 할지도 몰라요!",
                "춘식이처럼 귀여운 아이가 범인은 아니겠죠? 하하!"
            }},
            { "태근티비", new List<string> {
                "이 사건을 유튜브에 올리면 조회수 대박날 것 같아요!",
                "탐정님, 오늘의 수사 비하인드를 좀 찍어도 될까요?",
                "다음 영상 제목은 '천재 탐정의 하루'로 하면 어떨까요?"
            }},
            { "박윤주", new List<string> {
                "그림 속에 숨겨진 단서를 발견하셨나요?",
                "섬세한 붓터치 속에 진실이 숨어 있을지도 몰라요.",
                "오늘은 사건 현장의 분위기를 담아 그려봤어요. 도움이 될까요?"
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

        // 변지환 추가
        // 취조방 입장하는 메서드
        PhotonView pv = WeaponManager.Instance.gameObject.GetComponent<PhotonView>();
        pv.RPC("PutdownButtonActive", RpcTarget.Others, true);

        //PhotonConnection.Instance.UpdatePutdownButtonActive();
        //StartTalkinterrogation();//임시
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
            return ("잠시만 기달려주세요..");
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
            dialog.text = "탐정님, 전 범인이 아닙니다!";
            TextEffectInterr();
            if (InfoManagerKJY.instance.role == "Detective")
            {
                ConnectionKJY.instance.RequestInterrogationStart(npcdata.npcName, weapon);
            }
        }
    }

    // 여기서부터 TalkCode
    public void OnclickSend()
    {
        inputFieldObject.SetActive(false);
        // 임시
        string url = "http://ec2-43-201-108-241.ap-northeast-2.compute.amazonaws.com:8081/api/v1/chat/send";
        string chatContent = field.text;

        string receiver = npcdata.npcName;
        int chatDay = UI.instance.dayInt;

        // 구조체 사용
        StrTryChat str;
        str.url = url;
        str.gameSetNo = InfoManagerKJY.instance.gameSetNo; // 임시
        str.secertKey = "mafia"; // 임시
        str.chatContent = chatContent;
        str.receiver = receiver;
        str.chatDay = chatDay;
        str.sender = InfoManagerKJY.instance.playerName;

        //KJY - bool값 제어
        npctalk = true;

        // text 제어
        talkingName.text = InfoManagerKJY.instance.nickname;
        field.text = "";


        // 생성자 실행 후 바로 통신 시작
        ChatConnection chatConnection = new ChatConnection(str);
    }

    // NPC의 대화창에서 다음 버튼을 의미
    // 버튼을 누르면 플레이어 input filed가 활성화
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
                // 변지환 추가
                // 취조실 나가는 로직
                PhotonView pv = WeaponManager.Instance.gameObject.GetComponent<PhotonView>();
                pv.RPC("PutdownButtonActive", RpcTarget.Others, true);
                inputFieldObject.SetActive(false);
            }
            inputFieldObject.SetActive(true);
            dialog.text = string.Empty;
            inputButton.SetActive(true);
        }

        //KJY - bool값 제어
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
