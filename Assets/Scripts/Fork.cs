using System.Collections;
using System.Collections.Generic;
using TMPro;
using UnityEngine;

public class Fork : MonoBehaviour
{
    public Transform TransferComponent;
    public GameObject Box;

    public int Box1;
    public int Box2;

    public TMP_Text Box1EA;
    public TMP_Text Box2EA;
    // Start is called before the first frame update
    void Start()
    {
        Box1 = 0;
        Box2 = 0;
        Box1EA.text = $"  Box1 : {Box1} EA";
        Box2EA.text = $"  Box2 : {Box2} EA";
    }

    // Update is called once per frame
    void Update()
    {
        
    }

    private void OnTriggerEnter(Collider other)
    {
        if (other.gameObject.layer == LayerMask.NameToLayer("Box"))
        {
            Box = other.gameObject;
            Box.transform.SetParent(TransferComponent);
        }
    }
    private void OnTriggerExit(Collider other)
    {
        if (other.gameObject.layer == LayerMask.NameToLayer("Box"))
        {
            if (Box.CompareTag("Box1"))
            {
                Box1++;
                Box1EA.text = $"  Box1 : {Box1} EA";
            }
            if (Box.CompareTag("Box2"))
            {
                Box2++;
                Box2EA.text = $"  Box2 : {Box2} EA";
            }
            Box = null;
            other.transform.SetParent(null);
        }
    }
}
