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

    private Animator anim;

    void Start()
    {
        sr = GetComponent<SpriteRenderer>();
        anim = GetComponent<Animator>();
    }

    void Update()
    {
        // (Kode pengaman dan dialog biarkan seperti aslinya)

        if (sedangNgecek) return;

        if (sedangNgejar)
        {
            if (targetTaku != null)
            {
                // Hantu jalan mendekati Taku
                GerakkanKe(targetTaku.position, speedKejar);

                // --- TAMBAHAN BARU: CEK JARAK MATI ---
                // Jika jarak hantu dan Taku lebih kecil atau sama dengan jarakMati, eksekusi kekalahan
                if (Vector2.Distance(transform.position, targetTaku.position) <= jarakMati)
                {
                    if (GameOverManager.instance != null)
                    {
                        GameOverManager.instance.MatiDarah();
                    }
                }
            }
        }
        else
        {
            // (Logika patroli normal)
        }
    }

    void GerakkanKe(Vector3 target, float speed)
    {
        transform.position = Vector2.MoveTowards(transform.position, target, speed * Time.deltaTime);

        float bedaX = target.x - transform.position.x;
        float bedaY = target.y - transform.position.y;

        // --- PERBAIKAN 3: PENGAMAN ANIMATOR & SPRITERENDERER ---
        if (Mathf.Abs(bedaX) > Mathf.Abs(bedaY))
        {
            if (bedaX > 0)
            {
                if (anim != null) anim.Play("HantuJalanKanan_Animation");
                if (sr != null) sr.flipX = false;

                if (jarakPandangCollider != null) jarakPandangCollider.localPosition = new Vector2(Mathf.Abs(jarakPandangCollider.localPosition.x), jarakPandangCollider.localPosition.y);
            }
            else
            {
                if (anim != null) anim.Play("HantuJalanKiri_Animation");
                if (sr != null) sr.flipX = true;

                if (jarakPandangCollider != null) jarakPandangCollider.localPosition = new Vector2(-Mathf.Abs(jarakPandangCollider.localPosition.x), jarakPandangCollider.localPosition.y);
            }
        }
        else
        {
            if (bedaY > 0)
            {
                if (anim != null) anim.Play("HantuJalanAtas_Animation");
            }
            else
            {
                if (anim != null) anim.Play("HantuJalanBawah_Animation");
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

        anim.Play("HantuIdle_Animation");

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

    public void PaksaKejar(Transform target)
    {
        sedangNgejar = true;
        sedangNgecek = false;
        sedangInvestigasi = false;
        targetTaku = target;

        // Opsional: Langsung ubah animasi ke lari jika perlu
        Animator anim = GetComponent<Animator>();
        if (anim != null) anim.Play("HantuJalanKanan"); // Sesuaikan arah hadap awal
    }
}