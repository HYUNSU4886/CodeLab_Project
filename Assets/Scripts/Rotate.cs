using System.Collections;
using System.Collections.Generic;
using TMPro;
using Unity.VisualScripting;
using UnityEngine;
using UnityEngine.UI;

public class Rotate : MonoBehaviour
{

    public Transform RotateComponent;
    Vector3 Origin;
    public Button active;
    public Button reverse;
    public Vector3 direction;
    public int _direction;
    public float speed;
    public float distance;
    public char PLCInput1;
    public char PLCInput2;
    public int isRotating;
    public int endIndex;
    float time = 0;
    public int sensing;
    public float angle;
    public int ledCheck1;
    public int ledCheck2;

    public GameObject LED1;
    public GameObject LED2;

    void Start()
    {
        PLCInput1 = '0';
        PLCInput2 = '0';
        sensing = 0;
        endIndex = 0;
        isRotating = 0;
    }

    // Update is called once per frame
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

    public void OnActiveRotateBtnClkEvent()
    {
        Origin = RotateComponent.position;
        time = 0;
        print("Activate Cylinder");
        StartCoroutine(_Transfer(direction, speed, distance));
    }
    public void OnReverseRotateBtnClkEvent()
    {
        Origin = RotateComponent.position;
        time = 0;
        print("Reverse Activate Cylinder");
        StartCoroutine(_Transfer(-direction, speed, distance));
    }

    public void Onsensor()
    {
        sensing = 1;
    }

    IEnumerator FrontPLCRotate()
    {
        if (angle <= distance)
        {
            angle = angle + _direction * speed;
            RotateComponent.rotation = RotateComponent.rotation * Quaternion.Euler(direction * speed);
            yield return new WaitForSeconds(0.01f);
        }
        isRotating = 0;
    }
    IEnumerator BackPLCRotate()
    {
        if (angle >= 0)
        {

            angle = angle - _direction * speed;
            RotateComponent.rotation = RotateComponent.rotation * Quaternion.Euler(direction * speed * -1);
            yield return new WaitForSeconds(0.01f);
        }
        isRotating = 0;
    }
    IEnumerator _Transfer(Vector3 direction, float speed, float distance)
    {
        active.interactable = false;
        reverse.interactable = false;
        while (true)
        {
            time += 0.01f;
            if (time > distance / speed)
                break;
            if (sensing == 1)
                break;
            RotateComponent.position = Vector3.Lerp(Origin, Origin + distance * direction, time * speed / distance);
            yield return new WaitForSeconds(0.01f);
        }
        if (sensing == 1)
        {
            sensing = 0;
            while (true)
            {
                time -= 0.01f;
                if (time <= 0)
                    break;
                RotateComponent.position = Vector3.Lerp(Origin, Origin + distance * direction, time * speed / distance);
                yield return new WaitForSeconds(0.01f);
            }
        }
        reverse.interactable = true;
        active.interactable = true;
        endIndex = 1;
    }
}
