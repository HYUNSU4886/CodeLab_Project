using System;
using System.Collections;
using System.Collections.Generic;
using TMPro;
using Unity.VisualScripting;
using UnityEngine;
using UnityEngine.UI;

public class Transfer : MonoBehaviour
{

    public Transform TransferComponent;
    Vector3 Origin;
    public Button active;
    public Button reverse;
    public Vector3 direction;
    public int _direction;
    public float speed;
    public float distance;
    public char PLCInput1;
    public char PLCInput2;
    public int isTransfering;
    public int endIndex;
    float time = 0;
    public int sensing;
    public float location;
    public int ledCheck1;
    public int ledCheck2;
    public float deltaTime;

    public GameObject LED1;
    public GameObject LED2;

    void Start()
    {
        PLCInput1 = '0';
        PLCInput2 = '0';
        sensing = 0;
        endIndex = 0;
        isTransfering = 0;
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
            if (isTransfering == 0)
            {
                isTransfering = 1;
                StartCoroutine(FrontPLCTransfer());
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
            if (isTransfering == 0)
            {
                isTransfering = 1;
                StartCoroutine(BackPLCTransfer());
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

    public void OnActiveTransferBtnClkEvent()
    {
        Origin = TransferComponent.position;
        time = 0;
        print("Activate Cylinder");
        StartCoroutine(_Transfer(direction, speed, distance));
    }
    public void OnReverseTransferBtnClkEvent()
    {
        Origin = TransferComponent.position;
        time = 0;
        print("Reverse Activate Cylinder");
        StartCoroutine(_Transfer(-direction, speed, distance));
    }

    public void Onsensor()
    {
        sensing = 1;
    }

    IEnumerator FrontPLCTransfer()
    {
        if (location < distance)
        {
            location = location + _direction * speed * Time.deltaTime;
            TransferComponent.position = TransferComponent.position + direction * speed * Time.deltaTime;
            yield return new WaitForSeconds(Time.deltaTime);
        }
        isTransfering = 0;
    }
    IEnumerator BackPLCTransfer()
    {
        if (location > 0)
        {
            location = location + _direction * speed * Time.deltaTime;
            TransferComponent.position = TransferComponent.position - direction * speed * Time.deltaTime;
            yield return new WaitForSeconds(Time.deltaTime);
        }
        isTransfering = 0;
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
            TransferComponent.position = Vector3.Lerp(Origin, Origin + distance * direction, time * speed / distance);
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
                TransferComponent.position = Vector3.Lerp(Origin, Origin + distance * direction, time * speed / distance);
                yield return new WaitForSeconds(0.01f);
            }
        }
        reverse.interactable = true;
        active.interactable = true;
        endIndex = 1;
    }

}
