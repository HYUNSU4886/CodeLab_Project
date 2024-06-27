using System.Collections;
using System.Collections.Generic;
using TMPro;
using Unity.VisualScripting;
using UnityEngine;
using UnityEngine.UI;

public class Rotate : MonoBehaviour
{

    public Transform RotateComponent;
    public Button active;
    public Button reverse;
    public Vector3 direction;
    public int _direction;
    public float speed;
    public float distance;
    public char PLCInput1;
    public char PLCInput2;
    public int isRotating;
    public float angle;
    public int ledCheck1;
    public int ledCheck2;

    public GameObject LED1;
    public GameObject LED2;

    void Start()
    {
        PLCInput1 = '0';
        PLCInput2 = '0';
        isRotating = 0;
    }

    void Update()
    {
        if (PLCInput1 == '1')
        {
            if (ledCheck1 == 0)
            {
                ledCheck1 = 1;
                LED1.GetComponent<Image>().color = Color.green;
            }
            if (isRotating == 0)
            {
                isRotating = 1;
                StartCoroutine(FrontPLCRotate());
            }
        }
        if (PLCInput1 == '0')
        {
            if (ledCheck1 == 1)
            {
                ledCheck1 = 0;
                LED1.GetComponent<Image>().color = Color.white;
            }
        }
        if (PLCInput2 == '1')
        {
            if (ledCheck2 == 0)
            {
                ledCheck2 = 1;
                LED2.GetComponent<Image>().color = Color.green;
            }
            if (isRotating == 0)
            {
                isRotating = 1;
                StartCoroutine(BackPLCRotate());
            }
        }
        if (PLCInput2 == '0')
        {
            if (ledCheck2 == 1)
            {
                ledCheck2 = 0;
                LED2.GetComponent<Image>().color = Color.white;
            }
        }
    }


    IEnumerator FrontPLCRotate()
    {
        if (angle < distance)
        {
            angle = angle + _direction * speed;
            RotateComponent.localRotation = RotateComponent.localRotation * Quaternion.Euler(direction * speed);
            yield return new WaitForSeconds(0.01f);
        }
        if (angle > distance)
        {
            RotateComponent.localRotation = RotateComponent.localRotation * Quaternion.Euler(direction * (distance - angle));
        }
        isRotating = 0;
    }
    IEnumerator BackPLCRotate()
    {
        if (angle > 0)
        {

            angle = angle - _direction * speed;
            RotateComponent.localRotation = RotateComponent.localRotation * Quaternion.Euler(direction * speed * -1);
            yield return new WaitForSeconds(0.01f);
        }
        if(angle < 0)
        {
            RotateComponent.localRotation = RotateComponent.localRotation * Quaternion.Euler(direction * (0 - angle));
        }
        isRotating = 0;
    }
}
