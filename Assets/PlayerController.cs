using UnityEngine;
using System.Collections;

public class PlayerController : MonoBehaviour
{
    float laju = 10.0f;
    float putar = 30.0f;

    // Start is called before the first frame update
    void Start()
    {

    }

    // Update is called once per frame
    void FixedUpdate()
    {
        //PUTAR: KIRI-KANAN
        float hor = Input.GetAxis("Horizontal");
        transform.Rotate(Vector3.up * hor * putar * Time.deltaTime); //PUTAR THD SB.Y

        //MOVE: MAJU-MUNDUR
        float ver = Input.GetAxis("Vertical");
        transform.position += transform.forward * ver * laju * Time.deltaTime; //MAJU PADA SB.Z
    }
}
