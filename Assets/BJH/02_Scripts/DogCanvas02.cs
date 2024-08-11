using System.Collections;
using System.Collections.Generic;
using UnityEngine;
using UnityEngine.UI;

public class DogCanvas02 : MonoBehaviour
{
    public Image dogHeart;

    private void Start()
    {
        dogHeart.gameObject.SetActive(false);
    }

    public void SetActiveDogheard(bool state)
    {
        dogHeart.gameObject.SetActive(state);
    }
}
