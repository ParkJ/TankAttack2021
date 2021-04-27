using System.Collections;
using System.Collections.Generic;
using UnityEngine;
using Photon.Pun;
using UnityStandardAssets.Utility;

public class TankCtrl : MonoBehaviour
{
    private Transform tr;
    public float speed = 10.0f;
    private PhotonView pv;

    // Start is called before the first frame update
    void Start()
    {
        tr = GetComponent<Transform>();
        pv = GetComponent<PhotonView>();

        if (pv.IsMine)
        {
            Camera.main.GetComponent<SmoothFollow>().target = tr.Find("CamPivot").transform;
            GetComponent<Rigidbody>().centerOfMass = new Vector3(0, -5.0f, 0);
        }
        else
        {
            GetComponent<Rigidbody>().isKinematic = false;
        }
    }

    // Update is called once per frame
    void Update()
    {
        if (pv.IsMine)
        {
            float v = Input.GetAxis("Vertical");
            float h = Input.GetAxis("Horizontal");

            tr.Translate(Vector3.forward * Time.deltaTime * v * speed);
            tr.Rotate(Vector3.up * Time.deltaTime * 100.0f * h);
        }
    }
}
