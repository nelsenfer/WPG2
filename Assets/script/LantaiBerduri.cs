using System.Collections;
using UnityEngine;

public class LantaiBerduri : MonoBehaviour
{
    [Header("Pengaturan Waktu Pola")]
    public float waktuBahaya = 1.5f; // Waktu duri menusuk
    public float waktuNetral = 1f;   // Waktu jeda/peringatan
    public float waktuAman = 1.5f;   // Waktu duri sembunyi
    public float waktuTundaAwal = 0f;

    [Header("Pengaturan Kondisi Awal")]
    public bool mulaiDariBahaya = false;

    private Collider2D duriCollider;
    private SpriteRenderer sr;

    void Start()
    {
        duriCollider = GetComponent<Collider2D>();
        sr = GetComponent<SpriteRenderer>();

        StartCoroutine(SiklusDuri());
    }

    IEnumerator SiklusDuri()
    {
        // Jeda awal jika kamu ingin membuat pola ombak antar lantai
        if (waktuTundaAwal > 0)
        {
            yield return new WaitForSeconds(waktuTundaAwal);
        }

        bool apakahBahaya = mulaiDariBahaya;

        while (true)
        {
            if (apakahBahaya)
            {
                // --- 1. FASE BAHAYA (MENUSUK) ---
                sr.color = Color.red;
                duriCollider.enabled = true; // Bisa membunuh
                yield return new WaitForSeconds(waktuBahaya);

                // --- 2. FASE NETRAL (PERSIAPAN TURUN) ---
                sr.color = Color.yellow; // Warna peringatan
                duriCollider.enabled = false; // Aman diinjak
                yield return new WaitForSeconds(waktuNetral);
            }
            else
            {
                // --- 3. FASE AMAN (SEMBUNYI) ---
                sr.color = Color.white;
                duriCollider.enabled = false; // Aman diinjak
                yield return new WaitForSeconds(waktuAman);

                // --- 4. FASE NETRAL (PERINGATAN MAU NAIK) ---
                sr.color = Color.yellow; // Warna peringatan
                duriCollider.enabled = false; // Masih aman, tapi Taku harus segera lari!
                yield return new WaitForSeconds(waktuNetral);
            }

            // Balikkan status untuk putaran selanjutnya
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