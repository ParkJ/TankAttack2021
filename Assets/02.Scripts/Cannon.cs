using System.Collections;
using System.Collections.Generic;
using UnityEngine;

public class Cannon : MonoBehaviour
{
    // private Transform cannonTr;
    // private Transform cannonPos;

    public float speed = 2000.0f;

    // Start is called before the first frame update
    void Start()
    {
        // cannonTr = GetComponent<Transform>();
        // cannonPos = GetComponent<Transform>();

        GetComponent<Rigidbody>().AddRelativeForce(Vector3.forward * speed);
    }

}
