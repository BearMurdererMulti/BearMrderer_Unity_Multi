using System.Collections;
using System.Collections.Generic;
using UnityEngine;
using UnityEngine.UI;

public class CharacterCustomManager : MonoBehaviour
{
   [SerializeField] private List<Button> bodyButtons = new List<Button>();

    [SerializeField] private CharacterCustom_new characterCustom;

    // �÷��̾� ȸ��
    [SerializeField] private Transform playerTransform; // ĳ���� Transform
    private float rotationDuration; // ȸ���ϴ� �ð�
    private Vector3 targetRotationEuler; // ��ǥ ȸ����

    public void OnClickCustomButton(string keyName)
    {
        int buttonNumber = int.Parse(UnityEngine.EventSystems.EventSystem.current.currentSelectedGameObject.name);
        characterCustom.SetCustomDictionary(keyName, buttonNumber);
    }

    public void OnClickCategory(int index)
    {
        // ���� �ε����� ĳ���� �ڷ� ������
        if(index == 4)
        {
            rotationDuration = 1f;
            targetRotationEuler = new Vector3(0, 350, 0);
            StartCoroutine(CoRotateCharater(targetRotationEuler, rotationDuration));
        }
        else
        {
            rotationDuration = 1f;
            targetRotationEuler = new Vector3(0, 150, 0);
            StartCoroutine(CoRotateCharater(targetRotationEuler, rotationDuration));
        }
        characterCustom.ActiveCategoryContent(index);
    }

    IEnumerator CoRotateCharater(Vector3 targetRotation, float duration)
    {
        Quaternion initialRotation = playerTransform.rotation; //�ʱ� ȸ����
        Quaternion finalRotation = Quaternion.Euler(targetRotation); // ��ǥ ȸ����

        float time = 0f;

        while(time < duration)
        {
            time += Time.deltaTime;

            playerTransform.rotation = Quaternion.Slerp(initialRotation, finalRotation, time/duration);

            // �� ������ ���
            yield return null;
        }

        playerTransform.rotation = finalRotation;
    }

}
