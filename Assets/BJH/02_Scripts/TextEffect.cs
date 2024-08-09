using System.Collections;
using System.Collections.Generic;
using TMPro;
using UnityEngine;

public class TextEffect : MonoBehaviour
{
    [SerializeField] private float _durationTime;
    [SerializeField] private TMP_Text _text;

    private void Start()
    {
        _text = GetComponent<TMP_Text>();
        StartCoroutine(Blink());
    }

    IEnumerator Blink()
    {
        while (true)
        {
            // ������ �������
            yield return StartCoroutine(Fade(1f, 0f, _durationTime));

            // ������ ��Ÿ����
            yield return StartCoroutine(Fade(0f, 1f, _durationTime));
        }
    }

    IEnumerator Fade(float startAlpha, float endAlpha, float duration)
    {
        float elapsedTime = 0f;

        while (elapsedTime < duration)
        {
            elapsedTime += Time.deltaTime;
            float alpha = Mathf.Lerp(startAlpha, endAlpha, elapsedTime / duration);

            _text.alpha = alpha;
            yield return null;
        }
        _text.alpha = endAlpha;
    }
}
