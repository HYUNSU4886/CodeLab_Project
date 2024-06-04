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
    public float speed;
    public float distance;
    public int startIndex;
    public int endIndex;
    float time = 0;

    void Start()
    {
        startIndex = 0;
        endIndex = 0;
    }

    // Update is called once per frame
    void Update()
    {
        if (startIndex == 1)
        {
            startIndex = 0;
            OnActivePistonBtnClkEvent();
        }
        if (startIndex == 2)
        {
            startIndex = 0;
            OnReversePistonBtnClkEvent();
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
        startIndex = 3;
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
            if (startIndex == 3)
                break;
            piston.position = Vector3.Lerp(Origin, Origin + distance * direction, time * speed / distance);
            yield return new WaitForSeconds(0.01f);
        }
        if(startIndex == 3)
        {
            startIndex = 0;
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
