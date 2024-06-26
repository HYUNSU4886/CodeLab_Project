using System.Collections;
using System.Collections.Generic;
using UnityEngine;
using System;

public class BoxSupply : MonoBehaviour
{
    public Conveyor Conveyor;
    public Transform SupplyPoint;
    public GameObject Box1;
    public GameObject Box2;
    System.Random Random = new System.Random();

    void Start()
    {
        
    }

    // Update is called once per frame
    void Update()
    {
        if(Conveyor.isBoxIn == false && Conveyor.PLCInput1 == '0')
        {
            if (Random.Next(1, 100) < 51)
                Instantiate(Box1, SupplyPoint.position, Quaternion.Euler(0, Random.Next(0, 180), 0));
            else
                Instantiate(Box2, SupplyPoint.position, Quaternion.Euler(0, Random.Next(0, 180), 0));
            //Instantiate(Box1, SupplyPoint.position, Quaternion.Euler(0,90,0));
            Conveyor.isBoxIn = true;
        }
    }
}
