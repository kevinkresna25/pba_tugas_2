using UnityEngine;
using System.Collections;

public class Button : MonoBehaviour
{
    public Transform door;
    public float doorOpenSpeed = 2f; // Kecepatan membuka pintu
    private Vector3 openOffset = new Vector3(0, 0, -45); // Pergeseran posisi pintu
    private Vector3 targetDoorPosition;
    bool doorOpen = false;

    // Use this for initialization
    void Start()
    {
        // Hitung posisi target pintu dari awal
        targetDoorPosition = door.position + openOffset;
    }
    // Update is called once per frame
    void Update()
    {
        if (doorOpen)
        {
            // Gerakkan pintu secara bertahap menuju posisi target
            door.position = Vector3.Lerp(door.position, targetDoorPosition, Time.deltaTime * doorOpenSpeed);
        }
    }
    void OnTriggerEnter(Collider other)
    {
        if (other.gameObject.tag == "Player" && !doorOpen)
        {
            Debug.Log("The button has been pressed!");
            doorOpen = true; // Tandai pintu telah mulai membuka
        }
    }
}
