using UnityEngine;
using System.Collections;
public class Boss : MonoBehaviour
{
    GameObject player; //player
    public GameObject key; // muncul key saat boss mati
    public GameObject particleEffectPrefab;
    public AudioClip hitSound;
    public AudioClip deathSound;
    public ParticleSystem effectPrefab; // Mengambil partikel sistem
    private Rigidbody rb;
    private AudioSource audio;
    private int health = 5; // nyawa boss
    private float speed = 5.0f; // kecepatan 
    private bool onePlay = false;
    private bool oneHit = false;
    private bool bossMoving = true;
    private int particleCount = 0; // Jumlah partikel yang telah keluar
    private int maxParticleCount = 50; // Batas maksimum partikel

    // Use this for initialization
    void Start()
    {
        rb = GetComponent<Rigidbody>();
        audio = GetComponent<AudioSource>();
        hitSound = Resources.Load<AudioClip>("villager");
        deathSound = Resources.Load<AudioClip>("aku-berlutut");
        player = GameObject.FindGameObjectWithTag("Player"); // merujuk ke GameObject = player
    }

    // Update is called once per frame
    void Update()
    {
        if (bossMoving)
        {
            if (Vector3.Distance(gameObject.transform.position, player.transform.position) <= 1000.0f)
            {
                gameObject.transform.LookAt(player.transform);
                rb.transform.Translate(Vector3.forward * Time.deltaTime * speed);

                // Panggil efek partikel saat bos bergerak
                TampilkanEffect();

                // Perbarui posisi efek partikel mengikuti posisi bos
                effectPrefab.transform.position = transform.position;
            }
        }
        else
        {
            // Hentikan efek jika bos tidak bergerak
            if (effectPrefab.isPlaying)
            {
                effectPrefab.Stop();
            }
        }

        if (health <= 0)
        {
            if (!onePlay)
            {
                audio.PlayOneShot(deathSound);
                onePlay = true;
            }
            BossDeath();
        }
    }

    private void TampilkanEffect()
    {
        if (!effectPrefab.isPlaying) // Cek jika efek belum berjalan
        {
            effectPrefab.Play(); // Mainkan efek
        }
    }

    void BossDeath()
    {
        if (particleEffectPrefab != null && particleCount < maxParticleCount)
        {
            // Instansiasi efek partikel di lokasi bos
            Instantiate(particleEffectPrefab, transform.position, Quaternion.identity);
            particleCount++;
        }
        Destroy(gameObject, 8f); // Hancurkan bos
    }

    void OnCollisionEnter(Collision other) // other = bukan bos, tapi player
    {
        if (other.gameObject.tag == "Player") // jika player dan bos tabrakan
        {
            Debug.Log("tabrak player");
            Application.LoadLevel(Application.loadedLevel); // kembali ke level awal
        }

        if (other.gameObject.tag == "Proj") // peluru dan bos tabrakan
        {
            health--; // nyawa boss berkurang 1
            Destroy(other.gameObject); // peluru langsung hilang
            //GetComponent<AudioSource>().PlayOneShot(hitSound); // suara bos terkena peluru
            audio.PlayOneShot(hitSound);
            Debug.Log("Bos " + health);

            if (health <= 0)
            {
                if (!oneHit)
                {
                    bossMoving = false; // Bos berhenti bergerak
                    rb.velocity = Vector3.zero; // Hentikan semua kecepatan Rigidbody
                    rb.angularVelocity = Vector3.zero; // Hentikan rotasi Rigidbody
                    rb.isKinematic = true; // Nonaktifkan pengaruh fisika agar tidak bergerak
                    Instantiate(key, transform.position, Quaternion.identity); // Munculkan kunci
                    oneHit = true;
                }
                BossDeath();
            }
        }
    }
}