using System.Collections;
using System.Collections.Generic;
using UnityEngine;
using UnityEngine.UI;

public class Sensor : MonoBehaviour
{
    public int isSensing;
    public int PLCOutput;
    public int isChange;
    public GameObject Button;

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
                Button.GetComponent<Image>().color = Color.green;
            }
        }
        if (other.gameObject.layer == LayerMask.NameToLayer("Box"))
        {
            if (isSensing == 0)
            {
                isChange = 1;
                PLCOutput = 1;
                isSensing = 1;
                Button.GetComponent<Image>().color = Color.green;
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
                Button.GetComponent<Image>().color = Color.white;
            }
        }
        if (other.gameObject.layer == LayerMask.NameToLayer("Box"))
        {
            if (isSensing == 1)
            {
                isChange = 1;
                PLCOutput = 0;
                isSensing = 0;
                Button.GetComponent<Image>().color = Color.white;
            }
        }
    }
}
