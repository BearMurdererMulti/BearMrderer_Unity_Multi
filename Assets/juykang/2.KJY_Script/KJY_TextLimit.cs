using System.Collections;
using System.Collections.Generic;
using System.Text.RegularExpressions;
using TMPro;
using Unity.VisualScripting;
using UnityEngine;
using UnityEngine.UI;

public class KJY_TextLimit : MonoBehaviour
{
    [SerializeField] private TMP_InputField account;
    [SerializeField] private TextMeshProUGUI accounCount;
    [SerializeField] private TMP_InputField nickname;
    [SerializeField] private TextMeshProUGUI nickNameCount;
    [SerializeField] private TMP_InputField password;
    [SerializeField] private TextMeshProUGUI passwordCount;
    [SerializeField] private TMP_InputField roomName;
    [SerializeField] private TextMeshProUGUI roomNameCount;
    [SerializeField] private TMP_InputField text;
    [SerializeField] private TextMeshProUGUI textCount;

    private void TextLimitaccount()
    {
        account.characterLimit = 15;
        account.onValueChanged.AddListener(OnAccountChanged);
    }

    private void OnAccountChanged(string word)
    {
        // 허용된 문자만 남기고 필터링합니다.
        string filteredWord = Regex.Replace(word, @"[^0-9a-zA-Z가-힣]", "");

        // 필터링된 문자열이 길이 제한을 초과하지 않도록 자릅니다.
        if (filteredWord.Length > account.characterLimit)
        {
            filteredWord = filteredWord.Substring(0, account.characterLimit);
        }

        // 필터링된 문자열을 InputField에 다시 할당합니다.
        accounCount.text = filteredWord;
    }

    private void TextLimitnickName()
    {
        nickname.characterLimit = 6;
        nickname.onValueChanged.AddListener(OnNickNameChanged);
    }

    private void OnNickNameChanged(string word)
    {
        // 허용된 문자만 남기고 필터링합니다.
        string filteredWord = Regex.Replace(word, @"[^0-9a-zA-Z가-힣]", "");

        // 필터링된 문자열이 길이 제한을 초과하지 않도록 자릅니다.
        if (filteredWord.Length > nickname.characterLimit)
        {
            filteredWord = filteredWord.Substring(0, nickname.characterLimit);
        }

        // 필터링된 문자열을 InputField에 다시 할당합니다.
        nickname.text = filteredWord;
    }

    private void TextLimitnickroomName()
    {
        roomName.characterLimit = 6; // 글자 수 제한을 설정합니다.
        roomName.onValueChanged.AddListener(OnRoomNameChanged);
    }

    private void OnRoomNameChanged(string word)
    {
        // 허용된 문자만 남기고 필터링합니다.
        string filteredWord = Regex.Replace(word, @"[^0-9a-zA-Z가-힣]", "");

        // 필터링된 문자열이 길이 제한을 초과하지 않도록 자릅니다.
        if (filteredWord.Length > roomName.characterLimit)
        {
            filteredWord = filteredWord.Substring(0, roomName.characterLimit);
        }

        // 필터링된 문자열을 InputField에 다시 할당합니다.
        roomName.text = filteredWord;
    }

    private void TextLimitChatText()
    {
        text.characterLimit = 50;
        text.onValueChanged.AddListener(OnTextChanged);
    }

    private void OnTextChanged(string word)
    {
        // 허용된 문자만 남기고 필터링합니다.
        string filteredWord = Regex.Replace(word, @"[^0-9a-zA-Z가-힣]", "");

        // 필터링된 문자열이 길이 제한을 초과하지 않도록 자릅니다.
        if (filteredWord.Length > text.characterLimit)
        {
            filteredWord = filteredWord.Substring(0, text.characterLimit);
        }

        // 필터링된 문자열을 InputField에 다시 할당합니다.
        text.text = filteredWord;
    }

    private void TextLimitPassword()
    {
        password.characterLimit = 15;
        password.onValueChanged.AddListener(OnPasswordChanged);
    }

    private void OnPasswordChanged(string word)
    {
        // 허용된 문자만 남기고 필터링합니다.
        string filteredWord = Regex.Replace(word, @"[^0-9a-zA-Z가-힣]", "");

        // 필터링된 문자열이 길이 제한을 초과하지 않도록 자릅니다.
        if (filteredWord.Length > password.characterLimit)
        {
            filteredWord = filteredWord.Substring(0, password.characterLimit);
        }

        // 필터링된 문자열을 InputField에 다시 할당합니다.
        password.text = filteredWord;
    }
}
