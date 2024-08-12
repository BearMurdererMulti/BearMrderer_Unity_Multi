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
        // «„øÎµ» πÆ¿⁄∏∏ ≥≤±‚∞Ì « ≈Õ∏µ«’¥œ¥Ÿ.
        string filteredWord = Regex.Replace(word, @"[^0-9a-zA-Z∞°-∆R]", "");

        // « ≈Õ∏µµ» πÆ¿⁄ø≠¿Ã ±Ê¿Ã ¡¶«—¿ª √ ∞˙«œ¡ˆ æ µµ∑œ ¿⁄∏®¥œ¥Ÿ.
        if (filteredWord.Length > account.characterLimit)
        {
            filteredWord = filteredWord.Substring(0, account.characterLimit);
        }

        // « ≈Õ∏µµ» πÆ¿⁄ø≠¿ª InputFieldø° ¥ŸΩ√ «“¥Á«’¥œ¥Ÿ.
        accounCount.text = filteredWord;
    }

    private void TextLimitnickName()
    {
        nickname.characterLimit = 6;
        nickname.onValueChanged.AddListener(OnNickNameChanged);
    }

    private void OnNickNameChanged(string word)
    {
        // «„øÎµ» πÆ¿⁄∏∏ ≥≤±‚∞Ì « ≈Õ∏µ«’¥œ¥Ÿ.
        string filteredWord = Regex.Replace(word, @"[^0-9a-zA-Z∞°-∆R]", "");

        // « ≈Õ∏µµ» πÆ¿⁄ø≠¿Ã ±Ê¿Ã ¡¶«—¿ª √ ∞˙«œ¡ˆ æ µµ∑œ ¿⁄∏®¥œ¥Ÿ.
        if (filteredWord.Length > nickname.characterLimit)
        {
            filteredWord = filteredWord.Substring(0, nickname.characterLimit);
        }

        // « ≈Õ∏µµ» πÆ¿⁄ø≠¿ª InputFieldø° ¥ŸΩ√ «“¥Á«’¥œ¥Ÿ.
        nickname.text = filteredWord;
    }

    private void TextLimitnickroomName()
    {
        roomName.characterLimit = 6; // ±€¿⁄ ºˆ ¡¶«—¿ª º≥¡§«’¥œ¥Ÿ.
        roomName.onValueChanged.AddListener(OnRoomNameChanged);
    }

    private void OnRoomNameChanged(string word)
    {
        // «„øÎµ» πÆ¿⁄∏∏ ≥≤±‚∞Ì « ≈Õ∏µ«’¥œ¥Ÿ.
        string filteredWord = Regex.Replace(word, @"[^0-9a-zA-Z∞°-∆R]", "");

        // « ≈Õ∏µµ» πÆ¿⁄ø≠¿Ã ±Ê¿Ã ¡¶«—¿ª √ ∞˙«œ¡ˆ æ µµ∑œ ¿⁄∏®¥œ¥Ÿ.
        if (filteredWord.Length > roomName.characterLimit)
        {
            filteredWord = filteredWord.Substring(0, roomName.characterLimit);
        }

        // « ≈Õ∏µµ» πÆ¿⁄ø≠¿ª InputFieldø° ¥ŸΩ√ «“¥Á«’¥œ¥Ÿ.
        roomName.text = filteredWord;
    }

    private void TextLimitChatText()
    {
        text.characterLimit = 50;
        text.onValueChanged.AddListener(OnTextChanged);
    }

    private void OnTextChanged(string word)
    {
        // «„øÎµ» πÆ¿⁄∏∏ ≥≤±‚∞Ì « ≈Õ∏µ«’¥œ¥Ÿ.
        string filteredWord = Regex.Replace(word, @"[^0-9a-zA-Z∞°-∆R]", "");

        // « ≈Õ∏µµ» πÆ¿⁄ø≠¿Ã ±Ê¿Ã ¡¶«—¿ª √ ∞˙«œ¡ˆ æ µµ∑œ ¿⁄∏®¥œ¥Ÿ.
        if (filteredWord.Length > text.characterLimit)
        {
            filteredWord = filteredWord.Substring(0, text.characterLimit);
        }

        // « ≈Õ∏µµ» πÆ¿⁄ø≠¿ª InputFieldø° ¥ŸΩ√ «“¥Á«’¥œ¥Ÿ.
        text.text = filteredWord;
    }

    private void TextLimitPassword()
    {
        password.characterLimit = 15;
        password.onValueChanged.AddListener(OnPasswordChanged);
    }

    private void OnPasswordChanged(string word)
    {
        // «„øÎµ» πÆ¿⁄∏∏ ≥≤±‚∞Ì « ≈Õ∏µ«’¥œ¥Ÿ.
        string filteredWord = Regex.Replace(word, @"[^0-9a-zA-Z∞°-∆R]", "");

        // « ≈Õ∏µµ» πÆ¿⁄ø≠¿Ã ±Ê¿Ã ¡¶«—¿ª √ ∞˙«œ¡ˆ æ µµ∑œ ¿⁄∏®¥œ¥Ÿ.
        if (filteredWord.Length > password.characterLimit)
        {
            filteredWord = filteredWord.Substring(0, password.characterLimit);
        }

        // « ≈Õ∏µµ» πÆ¿⁄ø≠¿ª InputFieldø° ¥ŸΩ√ «“¥Á«’¥œ¥Ÿ.
        password.text = filteredWord;
    }
}
