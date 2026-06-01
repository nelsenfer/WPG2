using UnityEngine;

public class TriggerMulaiKejar : MonoBehaviour
{
    [Header("Referensi Objek")]
    public GameObject kuchiJahat;
    public Transform titikSpawnBawah; // Buat titik kosong di bawah area duri

    private bool sudahTerpicu = false;

    private void OnTriggerEnter2D(Collider2D collision)
    {
        if (collision.CompareTag("Player") && !sudahTerpicu)
        {
            sudahTerpicu = true;

            // Tarik Kuchi Jahat dari Map 2 ke bawah layar Map 4
            if (titikSpawnBawah != null) kuchiJahat.transform.position = titikSpawnBawah.position;
            kuchiJahat.SetActive(true);

            // Paksa hantu mengejar
            HantuPatroli aiJahat = kuchiJahat.GetComponent<HantuPatroli>();
            if (aiJahat != null)
            {
                aiJahat.PaksaKejar(collision.transform);
            }
        }
    }

    // Panggil fungsi ini lewat Game Over Manager jika player mati agar trigger bisa diinjak lagi
    public void ResetTrigger()
    {
        sudahTerpicu = false;
    }
}