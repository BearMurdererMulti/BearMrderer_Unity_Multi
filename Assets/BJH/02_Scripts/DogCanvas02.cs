using System.Collections;
using System.Collections.Generic;
using UnityEngine;
using UnityEngine.UI;

public class DogCanvas02 : MonoBehaviour
{
    public Canvas canvas;
    public Image dogHeart;
    public Camera targetCamera;

    private void Start()
    {
        dogHeart.gameObject.SetActive(false);
        Transform camTr = Camera.main.transform;
    }

    private void LateUpdate()
    {
        canvas.transform.LookAt(canvas.transform.position + (targetCamera.transform.rotation * Vector3.forward));
    }

    public void SetActiveDogheard(bool state)
    {
        dogHeart.gameObject.SetActive(state);
    }
}
