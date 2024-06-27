using System.Collections;
using System.Collections.Generic;
using TMPro;
using Unity.VisualScripting;
using UnityEngine;
using UnityEngine.UI;

public class Cylinder : MonoBehaviour
{

    public Transform piston;
    public Button active;
    public Button reverse;
    public Vector3 direction;
    public int _direction;
    public float speed;
    public float distance;
    public char PLCInput1;
    public char PLCInput2;
    public int PLCInput3;
    public int isPistonMoving;
    public float location;
    public int ledCheck1;
    public int ledCheck2;

    public GameObject LED1;
    public GameObject LED2;

    void Start()
    {
        PLCInput1 = '0';
        PLCInput2 = '0';
        PLCInput3 = 0;
        isPistonMoving = 0;
    }

    // Update is called once per frame
    void Update()
    {
        if (PLCInput1 == '1')
        {
            if(ledCheck1 == 0)
            {
                ledCheck1 = 1;
                LED1.GetComponent<Image>().color = Color.green;
            }
            if(isPistonMoving == 0)
            {
                isPistonMoving = 1;
                StartCoroutine(FrontPLCPistons());
            }
        }
        if(PLCInput1 == '0')
        {
            if(ledCheck1 == 1)
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
            if (isPistonMoving == 0)
            {
                isPistonMoving = 1;
                StartCoroutine(BackPLCPistons());
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

    IEnumerator FrontPLCPistons()
    {
        if(location < distance) 
        { 
            location = location + _direction * speed * Time.deltaTime;
            piston.localPosition = piston.localPosition + direction * speed * Time.deltaTime;
            yield return new WaitForSeconds(Time.deltaTime);
        }
        isPistonMoving = 0;
    }
    IEnumerator BackPLCPistons()
    {
        if (location > 0)
        {

            location = location - _direction * speed * Time.deltaTime;
            piston.localPosition = piston.localPosition - direction * speed * Time.deltaTime;
            yield return new WaitForSeconds(Time.deltaTime);
        }
        isPistonMoving = 0;
    }
}
