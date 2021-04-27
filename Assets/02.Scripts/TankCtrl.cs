using System.Collections;
using System.Collections.Generic;
using UnityEngine;

public class TankCtrl : MonoBehaviour
{
    private Transform tr;
    public float speed = 10.0f;

    // Start is called before the first frame update
    void Start()
    {
        tr = GetComponent<Transform>();
        GetComponent<Rigidbody>().centerOfMass = new Vector3(0, -5.0f, 0);
;
    }

    // Update is called once per frame
    void Update()
    {
        float v = Input.GetAxis("Vertical");
        float h = Input.GetAxis("Horizontal");

        tr.Translate(Vector3.forward * Time.deltaTime * v * speed);
        tr.Rotate(Vector3.up * Time.deltaTime * 100.0f * h);
    }
}
