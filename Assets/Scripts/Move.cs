using System.Collections;
using System.Collections.Generic;
using UnityEngine;

public class Move : MonoBehaviour
{
    public Transform plate;
    float time = 0;
    public float speed;
    public float distance;
    public Vector3 direction;
    Vector3 origin;

    void Start()
    {
        origin = plate.position;
        time = 0;
        StartCoroutine(Moving(direction,speed));
    }

    // Update is called once per frame
    void Update()
    {

    }
    IEnumerator Moving(Vector3 direction, float speed)
    {
        while (true)
        {
            this.GetComponent<Rigidbody>().velocity = direction * speed;
            yield return new WaitForSeconds(0.01f);
        }
    }
}
