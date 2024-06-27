using System;
using System.Collections;
using System.Collections.Generic;
using System.Runtime.CompilerServices;
using TMPro;
using Unity.VisualScripting;
using UnityEngine;
using UnityEngine.UI;
using UnityEngine.UIElements;

public class Transfer : MonoBehaviour
{

    public Transform TransferComponent;
    public Vector3 direction;
    public int _direction;
    public float speed;
    public float distance;
    public char PLCInput1;
    public char PLCInput2;
    public char PLCInput5;
    public char PLCInput6;
    public int PLCInput3;
    public int PLCInput4;
    public int isTransfering;
    public float location;
    public float tempLocation;
    public int ledCheck1;
    public int ledCheck2;
    public float deltaTime;
    public float ratio = 10;

    public int isStartF;
    public int isStartB;
    public GameObject LED1;
    public GameObject LED2;
    public TMP_Text Text;

    void Start()
    {
        PLCInput1 = '0';
        PLCInput2 = '0';
        PLCInput5 = '0';
        PLCInput6 = '0';
        PLCInput3 = 0;
        PLCInput4 = 0;
        isTransfering = 0;
        ratio = 20;
    }

    void Update()
    {
        if (PLCInput1 == '1' && isStartF == 0)
        {
            isStartF = 1;
            tempLocation = location;
        }
        if(isStartF == 1)
        {
            if (ledCheck1 == 0)
            {
                ledCheck1 = 1;
                LED1.GetComponent<UnityEngine.UI.Image>().color = Color.green;
            }
            if (isTransfering == 0)
            {
                isTransfering = 1;
                if((tempLocation + PLCInput3 / ratio - location) >= 0)
                    Text.text = $"{Convert.ToInt32((tempLocation + PLCInput3 / ratio - location) * 20)}";
                FrontPLCTransfer();
            }
        }
        if (PLCInput1 == '0')
        {
            if (ledCheck1 == 1)
            {
                ledCheck1 = 0;
                LED1.GetComponent<UnityEngine.UI.Image>().color = Color.white;
            }
        }
        if (PLCInput2 == '1' && isStartB == 0)
        {
            isStartB = 1;
            tempLocation = location;
        }
        if (isStartB == 1)
        {
            if (ledCheck2 == 0)
            {
                ledCheck2 = 1;
                LED2.GetComponent<UnityEngine.UI.Image>().color = Color.green;
            }
            if (isTransfering == 0)
            {
                isTransfering = 1;
                if((location - tempLocation + PLCInput4 / ratio) >= 0)
                    Text.text = $"{Convert.ToInt32((location - tempLocation + PLCInput4 / ratio) * 20)}";
                BackPLCTransfer();
            }
        }
        if (PLCInput2 == '0')
        {
            if (ledCheck2 == 1)
            {
                ledCheck2 = 0;
                LED2.GetComponent<UnityEngine.UI.Image>().color = Color.white;
            }
        }
        if (PLCInput5 == '1')
        {
            if (ledCheck1 == 0)
            {
                ledCheck1 = 1;
                LED1.GetComponent<UnityEngine.UI.Image>().color = Color.green;
            }
            if (isTransfering == 0)
            {
                isTransfering = 1;
                PLCTransferOn(direction, speed);
            }
        }
        if (PLCInput5 == '0')
        {
            if (ledCheck1 == 1)
            {
                ledCheck1 = 0;
                LED1.GetComponent<UnityEngine.UI.Image>().color = Color.white;
            }
        }
        if (PLCInput6 == '1')
        {
            if (ledCheck2 == 0)
            {
                ledCheck2 = 1;
                LED2.GetComponent<UnityEngine.UI.Image>().color = Color.green;
            }
            if (isTransfering == 0)
            {
                isTransfering = 1;
                PLCTransferRevOn(direction, speed);
            }
        }
        if (PLCInput6 == '0')
        {
            if (ledCheck2 == 1)
            {
                ledCheck2 = 0;
                LED2.GetComponent<UnityEngine.UI.Image>().color = Color.white;
            }
        }
    }


    public void FrontPLCTransfer()
    {
        if (location < distance)
        {
            if(location < tempLocation + PLCInput3/ratio || PLCInput3 == 0)
            {
                location = location + _direction * speed * Time.deltaTime;
                TransferComponent.localPosition = TransferComponent.localPosition + direction * speed * Time.deltaTime;
            }
            else
            {
                if (isStartF == 1 && PLCInput1 == '0')
                {
                    TransferComponent.localPosition = TransferComponent.localPosition - direction * (location - tempLocation - PLCInput3/ratio);
                    location = tempLocation + PLCInput3 / ratio;
                    isStartF = 0;
                }
            }
        }
        else if (isStartF == 1 && PLCInput1 == '0')
        {
            TransferComponent.localPosition = TransferComponent.localPosition + direction * (location - distance);
            location = distance;
            isStartF = 0;
        }
        isTransfering = 0;
    }
    public void BackPLCTransfer()
    {
        if (location > 0)
        {
            if(location > tempLocation - PLCInput4 / ratio || PLCInput4 == 0)
            {
                location = location - _direction * speed * Time.deltaTime;
                TransferComponent.localPosition = TransferComponent.localPosition - direction * speed * Time.deltaTime;
            }
            else
            {
                if (isStartB == 1 && PLCInput2 == '0')
                {
                    TransferComponent.localPosition = TransferComponent.localPosition + direction * (tempLocation - PLCInput4 / ratio - location);
                    location = tempLocation - PLCInput4 / ratio;
                    isStartB = 0;
                }
            }
        }
        else if(isStartB == 1 && PLCInput2 == '0')
        {
            TransferComponent.localPosition = TransferComponent.localPosition + direction * (0 - location);
            location = 0;
            isStartB = 0;
        }
        isTransfering = 0;
    }


    public void PLCTransferOn(Vector3 direction, float speed)
    {
        if (location < distance)
        {
            location = location + _direction * speed * Time.deltaTime;
            TransferComponent.localPosition = TransferComponent.localPosition + direction * speed * Time.deltaTime;
        }
        isTransfering = 0;
    }
    public void PLCTransferRevOn(Vector3 direction, float speed)
    {
        if (0 < location)
        {
            location = location + _direction * speed * Time.deltaTime;
            TransferComponent.localPosition = TransferComponent.localPosition + direction * speed * Time.deltaTime;
        }
        isTransfering = 0;
    }

}
