using System.Collections;
using UnityEngine;

public class LantaiBerduri : MonoBehaviour
{
    [Header("Pengaturan Waktu Pola")]
    public float waktuMati = 1.5f;
    public float waktuNyala = 1.5f;
    public float waktuTundaAwal = 0f;

    [Header("Pengaturan Kondisi Awal")]
    public bool mulaiMenyala = false;

    [Header("Sistem Toleransi Transisi")]
    public float durasiToleransi = 0.2f; // Waktu aman bagi player saat melangkah (dalam detik)

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
        bool faseSaatIniNyala = mulaiMenyala;

        if (waktuTundaAwal > 0)
        {
            duriCollider.enabled = faseSaatIniNyala;
            sr.color = faseSaatIniNyala ? Color.red : Color.white;
            yield return new WaitForSeconds(waktuTundaAwal);
        }

        while (true)
        {
            if (faseSaatIniNyala)
            {
                // --- FASE MENUSUK (DENGAN TOLERANSI) ---
                sr.color = Color.red; // Warna berubah merah instan sebagai peringatan visual
                duriCollider.enabled = false; // Collider dimatikan dulu selama transisi

                yield return new WaitForSeconds(durasiToleransi); // Memberikan waktu player kabur

                duriCollider.enabled = true; // Duri baru benar-benar aktif membunuh
                yield return new WaitForSeconds(waktuNyala - durasiToleransi);
            }
            else
            {
                // --- FASE AMAN ---
                duriCollider.enabled = false;
                sr.color = Color.white;
                yield return new WaitForSeconds(waktuMati);
            }

            faseSaatIniNyala = !faseSaatIniNyala;
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