using System.Collections;
using System.Collections.Generic;
using UnityEngine;

public class HantuPatroli : MonoBehaviour
{
    [Header("Pengaturan Patroli")]
    public Transform[] titikPatroli;
    public float speedPatroli = 3f;

    [Header("Pengaturan Kejar")]
    public float speedKejar = 4.5f;
    public float jarakMati = 0.8f;

    [Header("Pengaturan Pengecekan")]
    public int peluangCek = 40;
    public float waktuNgecek = 2f;

    [Header("Pengaturan Curiga")]
    public float durasiCuriga = 8f; // Berapa lama lemari masuk blacklist

    [Header("Komponen Fisik")]
    public Transform jarakPandangCollider;

    private bool sedangNgecek = false;
    private bool sedangNgejar = false;
    private bool sedangInvestigasi = false; // Status baru: paksa cek lemari curiga
    private Transform targetTaku;
    private Collider2D takuCollider;
    private SpriteRenderer sr;
    private int indeksTujuan = 0;
    private Coroutine coroutineNgecek;

    // Blacklist: lemari yang dicurigai + waktu kadaluarsanya
    private Dictionary<LemariSembunyi, float> lemariCuriga = new Dictionary<LemariSembunyi, float>();
    private LemariSembunyi targetInvestigasi;

    void Start()
    {
        sr = GetComponent<SpriteRenderer>();
    }

    void Update()
    {
        if (DialogManager.instance != null && DialogManager.instance.sedangDialog) return;
        if (titikPatroli.Length == 0) return;

        // Bersihkan blacklist yang sudah kadaluarsa
        List<LemariSembunyi> expired = new List<LemariSembunyi>();
        foreach (var entry in lemariCuriga)
            if (Time.time >= entry.Value) expired.Add(entry.Key);
        foreach (var lemari in expired)
            lemariCuriga.Remove(lemari);

        if (sedangNgecek) return; // Freeze saat ngecek, tapi investigasi tetap jalan di coroutine

        if (sedangNgejar && targetTaku != null)
        {
            // Kalau Taku sembunyi (collider mati), masuk mode Waspada
            if (!takuCollider.enabled)
            {
                sedangNgejar = false;
                MasukModeWaspada();
                return;
            }

            GerakkanKe(targetTaku.position, speedKejar);

            if (Vector2.Distance(transform.position, targetTaku.position) <= jarakMati)
            {
                if (GameOverManager.instance != null) GameOverManager.instance.MatiDarah();
            }
        }
        else if (sedangInvestigasi && targetInvestigasi != null)
        {
            // Jalan ke lemari yang dicurigai
            GerakkanKe(targetInvestigasi.transform.position, speedPatroli);

            if (Vector2.Distance(transform.position, targetInvestigasi.transform.position) < 0.5f)
            {
                // Sudah sampai, paksa ngecek tanpa gacha
                sedangInvestigasi = false;
                coroutineNgecek = StartCoroutine(ProsesNgecek(targetInvestigasi, dipaksa: true));
            }
        }
        else
        {
            // Patroli normal
            Transform tujuan = titikPatroli[indeksTujuan];
            GerakkanKe(tujuan.position, speedPatroli);

            if (Vector2.Distance(transform.position, tujuan.position) < 0.1f)
            {
                indeksTujuan++;
                if (indeksTujuan >= titikPatroli.Length) indeksTujuan = 0;
            }
        }
    }

    void MasukModeWaspada()
    {
        // Cek apakah ada lemari curiga di sekitar posisi player terakhir
        if (targetTaku != null)
        {
            Collider2D[] nearby = Physics2D.OverlapCircleAll(targetTaku.position, 2f);
            foreach (var col in nearby)
            {
                LemariSembunyi lemari = col.GetComponent<LemariSembunyi>();
                if (lemari != null && lemariCuriga.ContainsKey(lemari))
                {
                    // Ada lemari curiga di dekat sini, langsung investigasi!
                    targetInvestigasi = lemari;
                    sedangInvestigasi = true;
                    return;
                }
            }
        }

        // Tidak ada lemari curiga → balik patroli biasa
        sedangNgejar = false;
    }

    void GerakkanKe(Vector3 target, float speed)
    {
        transform.position = Vector2.MoveTowards(transform.position, target, speed * Time.deltaTime);

        if (target.x > transform.position.x)
        {
            sr.flipX = false;
            if (jarakPandangCollider != null)
                jarakPandangCollider.localPosition = new Vector2(Mathf.Abs(jarakPandangCollider.localPosition.x), jarakPandangCollider.localPosition.y);
        }
        else if (target.x < transform.position.x)
        {
            sr.flipX = true;
            if (jarakPandangCollider != null)
                jarakPandangCollider.localPosition = new Vector2(-Mathf.Abs(jarakPandangCollider.localPosition.x), jarakPandangCollider.localPosition.y);
        }
    }

    private void OnTriggerEnter2D(Collider2D collision)
    {
        if (collision.CompareTag("Player") && !sedangNgejar && !sedangNgecek)
        {
            sedangNgejar = true;
            sedangInvestigasi = false;
            targetTaku = collision.transform;
            takuCollider = collision.GetComponent<Collider2D>();
            return;
        }

        LemariSembunyi tempatSembunyi = collision.GetComponent<LemariSembunyi>();

        if (tempatSembunyi != null && !sedangNgecek && !sedangNgejar && !sedangInvestigasi)
        {
            // Kalau lemari ini ada di blacklist → langsung cek tanpa gacha
            if (lemariCuriga.ContainsKey(tempatSembunyi))
            {
                coroutineNgecek = StartCoroutine(ProsesNgecek(tempatSembunyi, dipaksa: true));
                return;
            }

            int gacha = Random.Range(1, 101);
            if (gacha <= peluangCek)
            {
                coroutineNgecek = StartCoroutine(ProsesNgecek(tempatSembunyi, dipaksa: false));
            }
        }
    }

    IEnumerator ProsesNgecek(LemariSembunyi tempatSembunyi, bool dipaksa)
    {
        sedangNgecek = true;
        sedangInvestigasi = false;
        yield return new WaitForSeconds(waktuNgecek);

        // Cek apakah player keluar saat dicheck (collider aktif = player keluar panik)
        if (targetTaku != null && takuCollider != null && takuCollider.enabled)
        {
            // Player ketahuan keluar! Masukkan lemari ini ke blacklist
            lemariCuriga[tempatSembunyi] = Time.time + durasiCuriga;
            sedangNgecek = false;
            sedangNgejar = true; // Langsung kejar karena ketahuan

            // --- GANTI RETURN JADI INI ---
            yield break;
        }

        if (tempatSembunyi.playerSedangSembunyi)
        {
            if (GameOverManager.instance != null) GameOverManager.instance.MatiDarah();
        }
        else if (dipaksa)
        {
            // Sudah dicek paksa tapi kosong → hapus dari blacklist
            lemariCuriga.Remove(tempatSembunyi);
        }

        sedangNgecek = false;
    }

    public void ResetHantu()
    {
        sedangNgejar = false;
        sedangNgecek = false;
        sedangInvestigasi = false;
        targetTaku = null;
        targetInvestigasi = null;
        lemariCuriga.Clear();

        if (coroutineNgecek != null) StopCoroutine(coroutineNgecek);

        if (titikPatroli.Length > 0)
        {
            indeksTujuan = 0;
            transform.position = titikPatroli[0].position;
        }
    }
}