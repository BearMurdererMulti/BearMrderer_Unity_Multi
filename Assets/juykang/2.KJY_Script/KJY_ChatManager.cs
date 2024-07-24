using Photon.Pun;
using System.Collections;
using System.Collections.Generic;
using UnityEngine;
using UnityEngine.UI;

public class KJY_ChatManager : MonoBehaviourPun
{
    //InputField 
    public InputField chatInput;

    //ChatItem Prefab
    public GameObject chatItemFactory;
    //ScrollView 에 있는 Content 의 RectTrasform
    public RectTransform rtContent;
    //ScrollView 의 RectTransform
    public RectTransform rtScrollView;
    //채팅이 추가되기 전의 Content H 의 값을 가지고 있는 변수
    float prevContentH;

    //닉네임 색상
    public Color nickNameColor;

    void Start()
    {
        //닉네임 색상 랜덤하게 설정
        nickNameColor = new Color32(
            (byte)Random.Range(0, 256),
            (byte)Random.Range(0, 256),
            (byte)Random.Range(0, 256),
            255);

        //엔터키를 누르면 InputField 에 있는 텍스트 내용 알려주는 함수 등록
        chatInput.onSubmit.AddListener(OnSubmit);

        //InputField 의 내용이 변경 될 때마다 호출해주는 함수 등록
        chatInput.onValueChanged.AddListener(OnValueChanged);

        //InputField 의 Focusing 이 사라졌을 때 호출해주는 함수 등록
        chatInput.onEndEdit.AddListener(OnEndEdit);
    }

    void Update()
    {

    }

    void OnSubmit(string s)
    {
        //s 의 길이가 0 이라면 함수를 나가라
        if (s.Length == 0) return;

        //새로운 채팅이 추가되기 전의 content 의 H 값을 저장
        prevContentH = rtContent.sizeDelta.y;

        //닉네임을 붙여서 채팅내용을 만들자
        //"<color=#ffff00> 원하는 내용 </color>"
        string chat = "<color=#" + ColorUtility.ToHtmlStringRGB(nickNameColor) + ">" +
            PhotonNetwork.NickName + "</color>" + " : " + s;

        //Rpc 함수로 모든 사람한테 채팅 내용을 전달
        photonView.RPC(nameof(AddChatRpc), RpcTarget.All, chat);

        //chatInput 값을 초기화
        chatInput.text = "";

        //chatInput 을 활성화 하자
        chatInput.ActivateInputField();
    }

    [PunRPC]
    void AddChatRpc(string chat)
    {
        //print("OnSubmit : " + s);
        //Chatitem 을 만든다.
        GameObject ci = Instantiate(chatItemFactory);
        //만들어진 item 의 부모를 content 로 한다.
        ci.transform.SetParent(rtContent);

        //만들어진 item 에서 ChatItem 컴포넌트를 가져온다.
        KJY_ChatItem item = ci.GetComponent<KJY_ChatItem>();
        //가져온 컴포넌트에서 SetText 함수를 실행
        item.SetText(chat);

        //자동으로 content 를 맨 밑으로 내리는 기능
        StartCoroutine(AutoScrollBottom());
    }

    IEnumerator AutoScrollBottom()
    {
        yield return 0;

        //scrollView 의 H 보다 content 의 H 값이 크다면 (스크롤이 가능한 상태라면)
        if (rtContent.sizeDelta.y > rtScrollView.sizeDelta.y)
        {
            //이전에 바닥에 닿아있었다면
            if (prevContentH - rtScrollView.sizeDelta.y <= rtContent.anchoredPosition.y)
            {
                //content 의 y 값을 재설정한다.
                rtContent.anchoredPosition =
                    new Vector2(0, rtContent.sizeDelta.y - rtScrollView.sizeDelta.y);
            }
        }
    }

    void OnValueChanged(string s)
    {
        //print("OnValueChanged : " + s);
    }

    void OnEndEdit(string s)
    {
        //print("OnEndEdit : " + s);
    }
}
