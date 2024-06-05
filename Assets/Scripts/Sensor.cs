using System.Collections;
using System.Collections.Generic;
using UnityEngine;

public class Sensor : MonoBehaviour
{
    public int isSensing;
    public int PLCOutput;
    public int isChange;

    void Start()
    {
        isSensing = 0;
        PLCOutput = 0;
        isChange = 0;
        
    }

    void Update()
    {
        
    }

    private void OnTriggerEnter(Collider other)
    {
        print("good");
        if(other.gameObject.layer == LayerMask.NameToLayer("CylinderPoint"))
        {
            if (isSensing == 0)
            { 
                isChange = 1;
                PLCOutput = 1;
                print("센서 작동");
                isSensing = 1;
            }
        }
    }

    private void OnTriggerExit(Collider other)
    {
        if (other.gameObject.layer == LayerMask.NameToLayer("CylinderPoint"))
        {
            if (isSensing == 1)
            {
                isChange = 1;
                PLCOutput = 0;
                print("센서 미작동");
                isSensing = 0;
            }
        }
    }
}
