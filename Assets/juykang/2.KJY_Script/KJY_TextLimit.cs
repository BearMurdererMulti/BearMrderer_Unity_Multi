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
        // ���� ���ڸ� ����� ���͸��մϴ�.
        string filteredWord = Regex.Replace(word, @"[^0-9a-zA-Z��-�R]", "");

        // ���͸��� ���ڿ��� ���� ������ �ʰ����� �ʵ��� �ڸ��ϴ�.
        if (filteredWord.Length > account.characterLimit)
        {
            filteredWord = filteredWord.Substring(0, account.characterLimit);
        }

        // ���͸��� ���ڿ��� InputField�� �ٽ� �Ҵ��մϴ�.
        accounCount.text = filteredWord;
    }

    private void TextLimitnickName()
    {
        nickname.characterLimit = 6;
        nickname.onValueChanged.AddListener(OnNickNameChanged);
    }

    private void OnNickNameChanged(string word)
    {
        // ���� ���ڸ� ����� ���͸��մϴ�.
        string filteredWord = Regex.Replace(word, @"[^0-9a-zA-Z��-�R]", "");

        // ���͸��� ���ڿ��� ���� ������ �ʰ����� �ʵ��� �ڸ��ϴ�.
        if (filteredWord.Length > nickname.characterLimit)
        {
            filteredWord = filteredWord.Substring(0, nickname.characterLimit);
        }

        // ���͸��� ���ڿ��� InputField�� �ٽ� �Ҵ��մϴ�.
        nickname.text = filteredWord;
    }

    private void TextLimitnickroomName()
    {
        roomName.characterLimit = 6; // ���� �� ������ �����մϴ�.
        roomName.onValueChanged.AddListener(OnRoomNameChanged);
    }

    private void OnRoomNameChanged(string word)
    {
        // ���� ���ڸ� ����� ���͸��մϴ�.
        string filteredWord = Regex.Replace(word, @"[^0-9a-zA-Z��-�R]", "");

        // ���͸��� ���ڿ��� ���� ������ �ʰ����� �ʵ��� �ڸ��ϴ�.
        if (filteredWord.Length > roomName.characterLimit)
        {
            filteredWord = filteredWord.Substring(0, roomName.characterLimit);
        }

        // ���͸��� ���ڿ��� InputField�� �ٽ� �Ҵ��մϴ�.
        roomName.text = filteredWord;
    }

    private void TextLimitChatText()
    {
        text.characterLimit = 50;
        text.onValueChanged.AddListener(OnTextChanged);
    }

    private void OnTextChanged(string word)
    {
        // ���� ���ڸ� ����� ���͸��մϴ�.
        string filteredWord = Regex.Replace(word, @"[^0-9a-zA-Z��-�R]", "");

        // ���͸��� ���ڿ��� ���� ������ �ʰ����� �ʵ��� �ڸ��ϴ�.
        if (filteredWord.Length > text.characterLimit)
        {
            filteredWord = filteredWord.Substring(0, text.characterLimit);
        }

        // ���͸��� ���ڿ��� InputField�� �ٽ� �Ҵ��մϴ�.
        text.text = filteredWord;
    }

    private void TextLimitPassword()
    {
        password.characterLimit = 15;
        password.onValueChanged.AddListener(OnPasswordChanged);
    }

    private void OnPasswordChanged(string word)
    {
        // ���� ���ڸ� ����� ���͸��մϴ�.
        string filteredWord = Regex.Replace(word, @"[^0-9a-zA-Z��-�R]", "");

        // ���͸��� ���ڿ��� ���� ������ �ʰ����� �ʵ��� �ڸ��ϴ�.
        if (filteredWord.Length > password.characterLimit)
        {
            filteredWord = filteredWord.Substring(0, password.characterLimit);
        }

        // ���͸��� ���ڿ��� InputField�� �ٽ� �Ҵ��մϴ�.
        password.text = filteredWord;
    }
}
