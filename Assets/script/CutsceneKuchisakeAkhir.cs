using System.Collections;
using UnityEngine;

public class CutsceneKuchisakeAkhir : MonoBehaviour
{
    [Header("Referensi Objek")]
    public GameObject kuchiJahat;
    public GameObject kuchiAsli;
    public MonoBehaviour scriptGerakTaku;

    [Header("Pengaturan Jarak & Posisi")]
    public float jarakMasukKamera = 3.5f; // Jarak hantu ke Taku sebelum hantu dipaksa kaget
    public Transform titikKaburBawah;     // Titik di bawah layar buat hantu kabur
    public Transform titikKembaliMap2;    // Titik asal hantu di Map 2 (untuk reset)

    [Header("Pengaturan Waktu Cutscene")]
    public float waktuKaget = 1.5f;
    public float speedKabur = 6f;

    private bool cutsceneAktif = false;

    private void OnTriggerEnter2D(Collider2D collision)
    {
        if (collision.CompareTag("Player") && !cutsceneAktif)
        {
            StartCoroutine(MainkanAdegan(collision.transform));
        }
    }

    IEnumerator MainkanAdegan(Transform takuTransform)
    {
        cutsceneAktif = true;

        // 1. FREEZE TAKU, TAPI HANTU TETAP JALAN MENDEKAT
        if (scriptGerakTaku != null) scriptGerakTaku.enabled = false;

        // 2. TUNGGU SAMPAI HANTU MASUK LAYAR (JARAK DEKAT)
        while (Vector2.Distance(kuchiJahat.transform.position, takuTransform.position) > jarakMasukKamera)
        {
            yield return null; // Sistem akan menunda kode di bawahnya sampai hantu mendekat
        }

        // 3. HANTU SAMPAI, MATIKAN OTAKNYA
        HantuPatroli aiJahat = kuchiJahat.GetComponent<HantuPatroli>();
        if (aiJahat != null) aiJahat.enabled = false;

        Animator animJahat = kuchiJahat.GetComponent<Animator>();
        SpriteRenderer srJahat = kuchiJahat.GetComponent<SpriteRenderer>();

        // 4. KUCHI JAHAT KAGET
        if (animJahat != null) animJahat.Play("HantuIdle");

        if (kuchiAsli.transform.position.x > kuchiJahat.transform.position.x)
            srJahat.flipX = false;
        else
            srJahat.flipX = true;

        yield return new WaitForSeconds(waktuKaget);

        // 5. KUCHI JAHAT LARI KABUR KE BAWAH
        if (animJahat != null) animJahat.Play("HantuJalanBawah"); // Animasi lari ke bawah

        while (Vector2.Distance(kuchiJahat.transform.position, titikKaburBawah.position) > 0.1f)
        {
            kuchiJahat.transform.position = Vector2.MoveTowards(kuchiJahat.transform.position, titikKaburBawah.position, speedKabur * Time.deltaTime);
            yield return null;
        }

        // 6. TELEPORT KEMBALI KE MAP 2 DAN RESET
        kuchiJahat.transform.position = titikKembaliMap2.position;
        if (aiJahat != null)
        {
            aiJahat.enabled = true; // Nyalakan lagi otaknya
            aiJahat.ResetHantu();   // Kembalikan ke mode patroli normal di Map 2
        }
        kuchiJahat.SetActive(false); // Matikan sementara (Nanti dinyalakan lagi sama Trigger 1)

        // 7. KEMBALIKAN KONTROL TAKU
        if (scriptGerakTaku != null) scriptGerakTaku.enabled = true;
    }

    // Fungsi ini dipanggil jika Taku mati agar cutscene bisa diulang
    public void ResetCutscene()
    {
        cutsceneAktif = false;
    }
}