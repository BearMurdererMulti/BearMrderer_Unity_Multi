using System;
using System.Collections;
using System.Collections.Generic;
using TMPro;
using UnityEngine;
using UnityEngine.EventSystems;
using UnityEngine.UI;

public class CheckListInteraction : MonoBehaviour, IBeginDragHandler, IEndDragHandler, IDragHandler
{
    public static CheckListInteraction instance;
    public static Vector2 DefaultPos;

    // chat ui
    public ShowChatList chatList;
    public GameObject userChatUI;
    public GameObject npcChatUI;
    GameObject chatUIParent;
    public bool isInstantiate = false;

    public bool isUpdate;

    // chat list result
    public List<ChatListContent> chatListContent;

    private void Awake()
    {
        if(instance == null)
        {
            instance = this;

        }
    }

    private void Start()
    {
        // isInstantiate�� true�� �÷��̾� ê ����Ʈ�� ���
        if(isInstantiate)
        {
            InstantiateChatUI();
            isInstantiate = false;
        }
    }

    private void Update()
    {
        // chat list�� Ŭ������ ��
        // ����� �Ϸ�Ǿ��ٸ�? chat ������ ���
        if(isUpdate)
        {
            Debug.Log("chat list update�� �����մϴ�.");
            GameObject playerChat =  UI.instance.playerChatGroup;
            GameObject npcChat = UI.instance.npcChatGroup;
            foreach(ChatListContent content in chatListContent)
            {
                string sender = content.sender;
                int chatDay;
                string text;
                VerticalLayoutGroup v;
                
                // �÷��̾ ���� ä���̶��?
                if(sender == InfoManagerKJY.instance.playerName)
                {
                    text = content.chatContent;
                    chatDay = content.chatDay;

                    Transform parent = UI.instance.chatHistoryParent.Find(content.receiver + "ChatHistoryImg");
                    GameObject go = Instantiate(playerChat, parent.GetComponent<ChatListPrefabs>().chatHistoryMembercontent);
                    Debug.Log(parent);
                    Debug.Log(parent.GetComponent<ChatListPrefabs>());
                    Debug.Log(parent.GetComponent<ChatListPrefabs>().chatHistoryMembercontent);
                    Debug.Log(parent.GetComponent<ChatListPrefabs>().chatHistoryMembercontent.GetComponent<VerticalLayoutGroup>());


                    v = parent.GetComponent<ChatListPrefabs>().chatHistoryMembercontent.GetComponent<VerticalLayoutGroup>();
                    ChatGroup cg = go.GetComponent<ChatGroup>();
                    cg.name.text = sender;
                    cg.content.text = content.chatContent.ToString();
                    Fit(go.GetComponent<RectTransform>());
                }
                else
                {
                    Transform parent = UI.instance.chatHistoryParent.Find(sender + "ChatHistoryImg");
                    Debug.Log(parent.gameObject.name + "   " );
                    GameObject go = Instantiate(npcChat, parent.GetComponent<ChatListPrefabs>().chatHistoryMembercontent);
                    v = parent.GetComponent<ChatListPrefabs>().chatHistoryMembercontent.GetComponent<VerticalLayoutGroup>();


                    ChatGroup cg = go.GetComponent<ChatGroup>();
                    cg.name.text = sender;
                    cg.content.text = content.chatContent.ToString();
                    Fit(go.GetComponent<RectTransform>());

                }
                v.padding.top = 20;
            }
            
            isUpdate = false;
        }
    }

    private void Fit(RectTransform transform)
    {
        LayoutRebuilder.ForceRebuildLayoutImmediate(transform);
    }


    // chat list
    string pre = "";
    public void ClickNpcProfile()
    {
        //StartCoroutine(nameof(CoStartGetChatList));

        

        List<GameNpcSetting> npcNameList = InfoManagerKJY.instance.gameNpcList;
        for (int i = 0; i < npcNameList.Count; i++)
        {
            string key = $"{npcNameList[i].npcName}ChatHistoryImg";

            // �����ִ� �����϶� �������� �� �����ٸ�?
            if (UI.instance.dicChatHistoryState[key] == true)
            {
                // ������ ������ ê ���� ����� ����
                Transform go = UI.instance.chatHistoryParent.Find(key);
                Transform content = go.GetComponent<ChatListPrefabs>().chatHistoryMembercontent;
                int cnt = content.childCount;
                for(int j = 1; j < cnt; j++)
                {
                    content.GetComponent<ContentSizeFitter>().enabled = false;
                    content.GetComponent<VerticalLayoutGroup>().enabled = false;

                    Destroy(content.GetChild(j));
                }
                content.GetComponent<ContentSizeFitter>().enabled = true;
                content.GetComponent<VerticalLayoutGroup>().enabled = true;

                UI.instance.chatHistoryParent.Find(key).gameObject.SetActive(false);
                UI.instance.dicChatHistoryState[key] = false;
                pre = key;
            }


        }

        if (pre != gameObject.name + "ChatHistoryImg")
        {
            UI.instance.dicChatHistoryState[gameObject.name + "ChatHistoryImg"] = true;
            UI.instance.chatHistoryParent.Find(gameObject.name + "ChatHistoryImg").gameObject.SetActive(true);
        }


        // ä�� ���� ����Ʈ�� �������� ����� �����ؾߵ�
        string url = "http://ec2-43-201-108-241.ap-northeast-2.compute.amazonaws.com:8081/api/v1/chat/list";

        string userName = InfoManagerKJY.instance.playerName;
        string aiNpcName = gameObject.name.ToString();

        Debug.Log($"{InfoManagerKJY.instance.gameSetNo}�� ������ {aiNpcName}���� ��ȭ ����� ��û�߽��ϴ�.");

        long gameSetNo = InfoManagerKJY.instance.gameSetNo;

        chatList = new ShowChatList(url, userName, aiNpcName, gameSetNo);

        if (chatList.result != null)
        {
            isInstantiate = true;
            print("true�� ������");
        }
    }

    IEnumerator CoStartGetChatList()
    {
        List<GameNpcSetting> npcNameList = InfoManagerKJY.instance.gameNpcList;
        for (int i = 0; i < npcNameList.Count; i++)
        {

            string key = $"{npcNameList[i].npcName}ChatHistoryImg";
            if (UI.instance.dicChatHistoryState[key] == true)
            {
                // ���� ä�ø���Ʈ ����
                foreach(Transform child in  UI.instance.chatHistoryParent)
                {
                    if(child.gameObject.name == key)
                    {
                        int cnt = child.transform.childCount;
                        while(cnt > 1)
                        {
                            Destroy(child.transform.GetChild(1).gameObject);
                            cnt--;
                        }
                    }
                }

                UI.instance.chatHistoryParent.Find(key).gameObject.SetActive(false);
                UI.instance.dicChatHistoryState[key] = false;
                pre = key;

            }
        }
        if (pre != gameObject.name + "ChatHistoryImg")
        {
            UI.instance.dicChatHistoryState[gameObject.name + "ChatHistoryImg"] = true;
            UI.instance.chatHistoryParent.Find(gameObject.name + "ChatHistoryImg").gameObject.SetActive(true);
        }
        yield return new WaitForSeconds(1f);


        // ä�� ���� ����Ʈ�� �������� ����� �����ؾߵ�
        string url = "http://ec2-43-201-108-241.ap-northeast-2.compute.amazonaws.com:8081/api/v1/chat/list";

        string userName = InfoManagerKJY.instance.playerName;
        string aiNpcName = gameObject.name.ToString();

        Debug.Log($"{InfoManagerKJY.instance.gameSetNo}�� ������ {aiNpcName}���� ��ȭ ����� ��û�߽��ϴ�.");

        long gameSetNo = InfoManagerKJY.instance.gameSetNo;

        chatList = new ShowChatList(url, userName, aiNpcName, gameSetNo);

        if (chatList.result != null)
        {
            isInstantiate = true;
            print("true�� ������");
        }
    }

    public void InstantiateChatUI()
    {
        GameObject[] go = GameObject.FindGameObjectsWithTag("Assign");
        foreach (GameObject go2 in go)
        {
            if (go2.name == "������ChatHistoryImg")
            {
                chatUIParent = go2;
            }
        }
        foreach (ChatListContent content in chatList.response.message)
        {
            if (content.sender == "������")
            {
                Instantiate(userChatUI, chatUIParent.transform);
                print("�����1");
            }
            else
            {
                Instantiate(userChatUI, chatUIParent.transform);
                print("�����2");

            }
        }
    }

    // Note
    int oxCnt = 0;
    public void OnClickOXBtn()
    {
        string[] array = new string[3];

        array[0] = null;
        array[1] = "X";
        array[2] = "O";

        oxCnt++;

        if (oxCnt >= array.Length)
        {
            oxCnt = 0;
        }
        gameObject.transform.GetChild(0).GetComponent<TMP_Text>().text = array[oxCnt];
        InfoManagerKJY.instance.npcOxDic[gameObject.name] = array[oxCnt]; // npc name : "X"
    }

    Transform putPosition;
    public void OnBeginDrag(PointerEventData eventData)
    {
        DefaultPos = this.transform.position;
    }

    public void OnEndDrag(PointerEventData eventData)
    {
        Vector3 mousePosition = Input.mousePosition;
        mousePosition = Camera.main.ScreenToWorldPoint(mousePosition);
        RaycastHit2D hit = Physics2D.Raycast(mousePosition, transform.position);
        if(hit.collider.gameObject.CompareTag("Job"))
        {
            print(hit.collider.gameObject.name);
            putPosition = hit.collider.gameObject.transform;
        }

        Instantiate(gameObject, putPosition);
    }

    public void OnDrag(PointerEventData eventData)
    {
        Vector2 pos = eventData.position;
        this.transform.position = pos;
    }
}
