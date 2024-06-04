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

    // Start is called before the first frame update
    void Start()
    {
        isBoxIn = false;
        _direction = 1;
    }

    // Update is called once per frame
    void Update()
    {
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
