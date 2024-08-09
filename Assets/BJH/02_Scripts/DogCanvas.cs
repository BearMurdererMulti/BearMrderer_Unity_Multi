using System.Collections;
using System.Collections.Generic;
using UnityEngine;

public class DogCanvas : MonoBehaviour
{
    [SerializeField] private GameObject inventoryImage;

    private void Start()
    {
        inventoryImage.SetActive(false);
    }
}
