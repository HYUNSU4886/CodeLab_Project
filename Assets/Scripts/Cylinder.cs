using System.Collections;
using System.Collections.Generic;
using TMPro;
using Unity.VisualScripting;
using UnityEngine;
using UnityEngine.UI;

public class Cylinder : MonoBehaviour
{

    public Transform piston;
    Vector3 Origin;
    public Button active;
    public Button reverse;
    public Vector3 direction;
    public int _direction;
    public float speed;
    public float distance;
    public char PLCInput1;
    public char PLCInput2;
    public int isPistonMoving;
    public int endIndex;
    float time = 0;
    public int sensing;
    public float location = 0;

    void Start()
    {
        PLCInput1 = '0';
        PLCInput2 = '0';
        sensing = 0;
        endIndex = 0;
        isPistonMoving = 0;
    }

    // Update is called once per frame
    void Update()
    {
        if (PLCInput1 == '1')
        {
            if(isPistonMoving == 0)
            {
                isPistonMoving = 1;
                StartCoroutine(FrontPLCPistons());
            }
        }
        if (PLCInput2 == '1')
        {
            if (isPistonMoving == 0)
            {
                isPistonMoving = 1;
                StartCoroutine(BackPLCPistons());
            }
        }
    }

    public void OnActivePistonBtnClkEvent()
    {
        Origin = piston.position;
        time = 0;
        print("Activate Cylinder");
        StartCoroutine(Pistons(direction, speed, distance));
    }
    public void OnReversePistonBtnClkEvent()
    {
        Origin = piston.position;
        time = 0;
        print("Reverse Activate Cylinder");
        StartCoroutine(Pistons(-direction, speed, distance));
    }

    public void Onsensor()
    {
        sensing = 1;
    }

    IEnumerator FrontPLCPistons()
    {
        if(location <= distance) 
        { 
            location = location + _direction * speed;
            piston.position = piston.position + direction * speed;
            yield return new WaitForSeconds(0.01f);
        }
        isPistonMoving = 0;
    }
    IEnumerator BackPLCPistons()
    {
        if (location >= 0)
        {

            location = location - _direction * speed;
            piston.position = piston.position - direction * speed;
            yield return new WaitForSeconds(0.01f);
        }
        isPistonMoving = 0;
    }
    IEnumerator Pistons(Vector3 direction,float speed,float distance)
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
            piston.position = Vector3.Lerp(Origin, Origin + distance * direction, time * speed / distance);
            yield return new WaitForSeconds(0.01f);
        }
        if(sensing == 1)
        {
            sensing = 0;
            while(true)
            {
                time -= 0.01f;
                if (time <= 0)
                    break;
                piston.position = Vector3.Lerp(Origin, Origin + distance * direction, time * speed / distance);
                yield return new WaitForSeconds(0.01f);
            }
        }
        reverse.interactable = true;
        active.interactable = true;
        endIndex = 1;
    }
}
