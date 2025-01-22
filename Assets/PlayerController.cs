using UnityEngine;

public class PlayerController : MonoBehaviour
{
    float laju = 10.0f; // Kecepatan maju/mundur
    float putar = 30.0f; // Kecepatan putar

    public GameObject movementEffectPrefab; // Prefab untuk efek gerakan
    private GameObject currentEffectInstance; // Instance efek aktif
    private float effectDestroyDelay = 10f; // Waktu delay sebelum menghancurkan efek

    public bool hasKey = false;

    // Pintu
    public float doorOpenSpeed = 2f; // Kecepatan membuka pintu
    private bool doorOpen = false; // Status apakah pintu sedang membuka
    private Transform door; // Referensi pintu
    private Vector3 targetDoorPosition; // Posisi target pintu

    private Rigidbody rb;

    // Start is called before the first frame update
    void Start()
    {
        rb = GetComponent<Rigidbody>();
        if (movementEffectPrefab == null)
        {
            Debug.LogWarning("Prefab efek partikel belum diatur di Inspector!");
        }
    }

    void FixedUpdate()
    {
        // PUTAR: KIRI-KANAN
        float hor = Input.GetAxis("Horizontal");
        transform.Rotate(Vector3.up * hor * putar * Time.deltaTime);

        // MOVE: MAJU-MUNDUR
        float ver = Input.GetAxis("Vertical");
        transform.position += transform.forward * ver * laju * Time.deltaTime;

        // Update efek partikel
        UpdateMovementEffect(hor, ver);
    }

    void Update()
    {
        if (doorOpen && door != null)
        {
            door.position = Vector3.MoveTowards(door.position, targetDoorPosition, Time.deltaTime * doorOpenSpeed);
        }
    }

    private void UpdateMovementEffect(float hor, float ver)
    {
        // Cek apakah player bergerak
        if (Mathf.Abs(ver) > 0.01f || Mathf.Abs(hor) > 0.01f)
        {
            if (currentEffectInstance == null && movementEffectPrefab != null)
            {
                // Instansiasi efek partikel di posisi player
                currentEffectInstance = Instantiate(movementEffectPrefab, transform.position, Quaternion.identity);
                currentEffectInstance.transform.SetParent(transform); // Jadikan child agar mengikuti player
            }
        }
        else
        {
            // Hapus efek partikel jika player berhenti
            if (currentEffectInstance != null)
            {
                Destroy(currentEffectInstance, effectDestroyDelay); // Tambahkan delay sebelum menghancurkan efek
                currentEffectInstance = null; // Hindari instansiasi ulang
            }
        }

        // Pastikan efek partikel mengikuti posisi player
        if (currentEffectInstance != null)
        {
            currentEffectInstance.transform.position = transform.position;
        }
    }

    void OnTriggerEnter(Collider other)
    {
        if (other.gameObject.tag == "Key")
        {
            Destroy(other.gameObject);
            hasKey = true;
        }

        if (other.gameObject.tag == "Win") // player menekan -> win
        {
            //Application.LoadLevel("win");
        }
    }

    void OnCollisionEnter(Collision other)
    {
        if (other.gameObject.CompareTag("LockedDoor") && hasKey && !doorOpen)
        {
            Debug.Log("Membuka pintu...");

            // Atur pintu untuk mulai membuka
            door = other.transform;
            targetDoorPosition = door.position + new Vector3(12f, 0, 0); // Geser 5 unit ke kanan
            doorOpen = true;
        }
    }
}
