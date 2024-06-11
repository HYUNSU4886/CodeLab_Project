using System.Collections;
using System.Collections.Generic;
using UnityEngine;

public class Conveyor : MonoBehaviour
{
    public Transform Box;
    public Vector3 direction;
    public float speed;
    public int isActive;
    public int _direction;
    public bool isBoxIn;
    int isConveyorMoving;
    int isConveyorReversing;

    public char PLCInput1;
    public char PLCInput2;
    public char PLCInput3;

    // Start is called before the first frame update
    void Start()
    {
        PLCInput1 = '0';
        PLCInput2 = '0';
        PLCInput3 = '0';
        isBoxIn = false;
        _direction = 1;
        isConveyorMoving = 0;
        isConveyorReversing = 0;
    }

    // Update is called once per frame
    void Update()
    {
        if (PLCInput1 == '1' && PLCInput2 == '0')
        {
            if (isConveyorMoving == 0)
            {
                isConveyorMoving = 1;
                StartCoroutine(PLCConveyorOn(direction, speed));
            }
        }
        if (PLCInput3 == '1')
            _direction = -1;
        else if(PLCInput3 == '0')
            _direction = 1;
    }


    public void OnConveyorBtnClkEvent()
    {
        isActive = 1;
        StartCoroutine(Moving(direction,speed));
    }
    public void OffConveyorBtnClkEvent()
    {
        isActive = 0;
    }

    IEnumerator PLCConveyorOn(Vector3 direction, float speed)
    {
        if(isBoxIn)
        {
            Box.GetComponent<Rigidbody>().velocity = direction * speed * _direction;
            yield return new WaitForSeconds(0.01f);
            isConveyorMoving = 0;
        }
    }
    public void ReverseConveyorDirectionBtnClkEvent()
    {
        _direction = _direction * -1;
    }


    IEnumerator Moving(Vector3 direction, float speed)
    {
        while (isBoxIn)
        {
            Box.GetComponent<Rigidbody>().velocity = direction * speed * _direction;
            if (isActive == 0)
                break;
            yield return new WaitForSeconds(0.01f);
        }
    }
    private void OnTriggerEnter(Collider other)
    {
        if (other.gameObject.layer == LayerMask.NameToLayer("Box"))
        {
            Box = other.transform;
            isBoxIn = true;
        }
    }

    private void OnTriggerExit(Collider other)
    {
        if(other.gameObject.layer == LayerMask.NameToLayer("Box"))
        {
            isBoxIn = false;
        }
    }
}
