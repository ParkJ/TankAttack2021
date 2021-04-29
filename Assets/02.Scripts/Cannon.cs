using System.Collections;
using System.Collections.Generic;
using UnityEngine;

public class Cannon : MonoBehaviour
{
    // private Transform cannonTr;
    // private Transform cannonPos;
    public GameObject expEffect;
    public float speed = 2000.0f;

    public string shooter; //발사한 사람

    // Start is called before the first frame update
    void Start()
    {
        // cannonTr = GetComponent<Transform>();
        // cannonPos = GetComponent<Transform>();

        GetComponent<Rigidbody>().AddRelativeForce(Vector3.forward * speed);
    }

    void OnCollisionEnter(Collision coll)
    {
        GameObject obj = Instantiate(expEffect, transform.position, Quaternion.identity);

        Destroy(obj, 3.0f);
        Destroy(this.gameObject);
    }

}
