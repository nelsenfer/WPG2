using System.Collections;
using UnityEngine;

public class CutsceneKucing : MonoBehaviour
{
    [Header("Pengaturan Cutscene")]
    public int triggerDiMisiKe = 2;
    public GameObject kucing;
    public GameObject kunci;
    public Transform titikKameraMelihat;

    [Header("Matikan Script Ini Saat Cutscene")]
    public MonoBehaviour scriptMovementPlayer;
    public MonoBehaviour scriptKameraFollow;

    [Header("Pengaturan Animasi")]
    [Tooltip("Nama parameter float di Animator player saat berjalan")]
    public string namaParameterJalan = "Speed"; // Bisa diganti langsung dari Inspector Unity!

    private bool cutsceneMulai = false;

    void Start()
    {
        // Sembunyikan Kunci dan Yokai pas game baru mulai
        if (kunci != null) kunci.SetActive(false);
        if (kucing != null) kucing.SetActive(false);
    }

    private void OnTriggerEnter2D(Collider2D collision)
    {
        CekDanMulaiCutscene(collision);
    }

    private void OnTriggerStay2D(Collider2D collision)
    {
        CekDanMulaiCutscene(collision);
    }

    void CekDanMulaiCutscene(Collider2D collision)
    {
        if (collision.CompareTag("Player") && !cutsceneMulai)
        {
            if (ObjectiveManager.instance != null && ObjectiveManager.instance.indeksMisiSaatIni == triggerDiMisiKe)
            {
                StartCoroutine(MulaiCutscene(collision.gameObject));
            }
        }
    }

    IEnumerator MulaiCutscene(GameObject player)
    {
        cutsceneMulai = true;

        // --- 0. TELEPORT KUCING KE LOKASI KUNCI! ---
        if (kucing != null && kunci != null)
        {
            kucing.transform.position = kunci.transform.position;
            kucing.SetActive(true);
        }

        // --- 1. REM FISIKA & STOP ANIMASI JALAN (Anti-Moonwalk) ---
        Rigidbody2D rb = player.GetComponent<Rigidbody2D>();
        if (rb != null)
        {
            rb.linearVelocity = Vector2.zero; // Udah pakai linearVelocity khusus Unity 6
        }

        Animator anim = player.GetComponentInChildren<Animator>();
        if (anim != null)
        {
            anim.SetFloat(namaParameterJalan, 0f); // Paksa Taku posisi diam
        }

        // --- 2. BEKUKAN PLAYER & KAMERA BAWAAN ---
        if (scriptMovementPlayer != null) scriptMovementPlayer.enabled = false;
        if (scriptKameraFollow != null) scriptKameraFollow.enabled = false;

        SpriteRenderer srPlayer = player.GetComponentInChildren<SpriteRenderer>();
        if (srPlayer != null) srPlayer.flipX = false; // Ubah true/false biar Taku hadap ke Kucing

        // --- 3. GESER KAMERA KE KIRI ---
        Camera cam = Camera.main;
        Vector3 posisiAwalKamera = cam.transform.position;
        Vector3 posisiTujuanKamera = new Vector3(titikKameraMelihat.position.x, titikKameraMelihat.position.y, posisiAwalKamera.z);

        float waktu = 0;
        while (waktu < 1f)
        {
            cam.transform.position = Vector3.Lerp(posisiAwalKamera, posisiTujuanKamera, waktu);
            waktu += Time.deltaTime;
            yield return null;
        }
        cam.transform.position = posisiTujuanKamera;

        // --- 4. KUCING MENATAP TAKU ---
        yield return new WaitForSeconds(1.5f);

        // --- 5. KUCING LARI KE KIRI ---
        if (kucing != null)
        {
            Vector3 posisiKucingAwal = kucing.transform.position;
            Vector3 posisiKucingLari = posisiKucingAwal + new Vector3(-4f, 0, 0);

            waktu = 0;
            while (waktu < 0.5f)
            {
                kucing.transform.position = Vector3.Lerp(posisiKucingAwal, posisiKucingLari, waktu / 0.5f);
                waktu += Time.deltaTime;
                yield return null;
            }
            kucing.SetActive(false);
        }

        // --- 6. KUNCI MUNCUL ---
        if (kunci != null)
        {
            kunci.SetActive(true);
        }

        yield return new WaitForSeconds(1.5f);

        // --- 7. KAMERA KEMBALI KE TAKU ---
        waktu = 0;
        while (waktu < 1f)
        {
            cam.transform.position = Vector3.Lerp(posisiTujuanKamera, posisiAwalKamera, waktu);
            waktu += Time.deltaTime;
            yield return null;
        }

        // --- 8. KEMBALIKAN KONTROL KE PEMAIN ---
        if (scriptMovementPlayer != null) scriptMovementPlayer.enabled = true;
        if (scriptKameraFollow != null) scriptKameraFollow.enabled = true;

        Destroy(gameObject);
    }
}