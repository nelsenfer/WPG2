using System.Collections;
using UnityEngine;

public class LantaiBerduri : MonoBehaviour
{
    [Header("Pengaturan Gambar (Sprite)")]
    public Sprite gambarAman;   // Tarik gambar lantai biasa (duri nutup)
    public Sprite gambarNetral; // Tarik gambar peringatan (duri setengah keluar)
    public Sprite gambarBahaya; // Tarik gambar duri tajam nyala penuh

    [Header("Pengaturan Waktu Pola")]
    public float waktuBahaya = 1.5f;
    public float waktuNetral = 1f;
    public float waktuAman = 1.5f;
    public float waktuTundaAwal = 0f;

    [Header("Sistem Toleransi")]
    public float durasiToleransi = 0.2f;

    [Header("Pengaturan Kondisi Awal")]
    public bool mulaiDariBahaya = false;

    private Collider2D duriCollider;
    private SpriteRenderer sr;

    void Start()
    {
        duriCollider = GetComponent<Collider2D>();
        sr = GetComponent<SpriteRenderer>();

        // Pastikan warna dasar kembali ke putih normal agar gambar aslinya tidak tercampur warna
        sr.color = Color.white;

        StartCoroutine(SiklusDuri());
    }

    IEnumerator SiklusDuri()
    {
        if (waktuTundaAwal > 0)
        {
            yield return new WaitForSeconds(waktuTundaAwal);
        }

        bool apakahBahaya = mulaiDariBahaya;

        while (true)
        {
            if (apakahBahaya)
            {
                SoundManager.Instance.PlaySound3D("SpikeTrap", transform.position);
                // --- 1. FASE BAHAYA DENGAN TOLERANSI ---
                sr.sprite = gambarBahaya; // Ganti gambar jadi duri tajam
                duriCollider.enabled = false;

                yield return new WaitForSeconds(durasiToleransi);

                duriCollider.enabled = true;
                yield return new WaitForSeconds(waktuBahaya - durasiToleransi);

                // --- 2. FASE NETRAL (PERSIAPAN TURUN) ---
                sr.sprite = gambarNetral; // Ganti gambar jadi duri setengah masuk
                duriCollider.enabled = false;
                yield return new WaitForSeconds(waktuNetral);
            }
            else
            {
                // --- 3. FASE AMAN (SEMBUNYI) ---
                sr.sprite = gambarAman; // Ganti gambar jadi lantai rata
                duriCollider.enabled = false;
                yield return new WaitForSeconds(waktuAman);

                // --- 4. FASE NETRAL (PERINGATAN MAU NAIK) ---
                sr.sprite = gambarNetral; // Ganti gambar jadi duri mau keluar
                duriCollider.enabled = false;
                yield return new WaitForSeconds(waktuNetral);
            }

            apakahBahaya = !apakahBahaya;
        }
    }

    private void OnTriggerEnter2D(Collider2D collision)
    {
        if (collision.CompareTag("Player") && duriCollider.enabled)
        {
            if (GameOverManager.instance != null)
            {
                GameOverManager.instance.MatiDarah();
            }
        }
    }
}