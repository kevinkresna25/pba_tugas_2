using System.Collections;
using System.Collections.Generic;
using UnityEngine;

public class Tembak : MonoBehaviour
{
    float F = 300.0f;
    float vz = 15.0f;
    float vy = 1f;

    public GameObject proj;
    public Transform pucuk;

    // Start is called before the first frame update
    void Start()
    {

    }

    // Update is called once per frame
    void Update()
    {
        Vector3 pos = pucuk.position;
        if (Input.GetButtonDown("Fire1"))//MOUSE KIRI
        {
            var p = Instantiate(proj, pos, Quaternion.identity);
            p.GetComponent<Rigidbody>().AddForce(transform.TransformDirection(0, vy, vz) * F);
            Destroy(p, 2.0f);//3 detik hilang


        }
    }
}
