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
        if(other.gameObject.layer == LayerMask.NameToLayer("CylinderPoint"))
        {
            if (isSensing == 0)
            { 
                isChange = 1;
                PLCOutput = 1;
                isSensing = 1;
            }
        }
        if (other.gameObject.layer == LayerMask.NameToLayer("Box"))
        {
            if (isSensing == 0)
            {
                isChange = 1;
                PLCOutput = 1;
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
                isSensing = 0;
            }
        }
        if (other.gameObject.layer == LayerMask.NameToLayer("Box"))
        {
            if (isSensing == 1)
            {
                isChange = 1;
                PLCOutput = 0;
                isSensing = 0;
            }
        }
    }
}
