using System.Collections;
using UnityEngine;

public class CutsceneKuchisakeAkhir : MonoBehaviour
{
    [Header("Referensi Objek Hantu")]
    public GameObject kuchiJahat;
    public GameObject kuchiAsli;

    [Header("Pengaturan Player (Taku)")]
    public MonoBehaviour scriptGerakTaku;
    [Tooltip("Nama parameter float di Animator player saat berjalan")]
    public string namaParameterJalan = "Speed"; // Sesuaikan dengan parameter Animator Taku

    [Header("Pengaturan Jarak & Posisi")]
    public float jarakMasukKamera = 3.5f; 
    public Transform titikKaburBawah;     
    public Transform titikKembaliMap2;    

    [Header("Pengaturan Waktu Cutscene")]
    public float waktuKaget = 1.5f;
    public float speedKabur = 6f; 

    private bool cutsceneAktif = false;

    private void OnTriggerEnter2D(Collider2D collision)
    {
        if (collision.CompareTag("Player") && !cutsceneAktif)
        {
            // Lempar objek player (Taku) ke dalam coroutine agar komponennya bisa dibaca
            StartCoroutine(MainkanAdegan(collision.gameObject));
        }
    }

    IEnumerator MainkanAdegan(GameObject player)
    {
        cutsceneAktif = true;

        // --- 1. REM FISIKA & STOP ANIMASI JALAN (Gaya Cutscene Kucing) ---
        if (scriptGerakTaku != null) scriptGerakTaku.enabled = false;

        Rigidbody2D rb = player.GetComponent<Rigidbody2D>();
        if (rb != null)
        {
            rb.linearVelocity = Vector2.zero; // Rem total menggunakan fitur Unity 6
        }

        Animator animTaku = player.GetComponentInChildren<Animator>();
        if (animTaku != null)
        {
            animTaku.SetFloat(namaParameterJalan, 0f); // Paksa Taku posisi diam
        }

        // --- 2. TUNGGU HANTU MASUK KAMERA ---
        while (Vector2.Distance(kuchiJahat.transform.position, player.transform.position) > jarakMasukKamera)
        {
            yield return null; 
        }

        // --- 3. HANTU KAGET ---
        HantuPatroli aiJahat = kuchiJahat.GetComponent<HantuPatroli>();
        if (aiJahat != null) aiJahat.enabled = false;

        Animator animJahat = kuchiJahat.GetComponent<Animator>();
        SpriteRenderer srJahat = kuchiJahat.GetComponent<SpriteRenderer>();

        // (Pastikan nama ini sudah sama persis dengan yang ada di Animator)
        if (animJahat != null) animJahat.Play("HantuIdle_Animation"); 
        
        if (kuchiAsli.transform.position.x > kuchiJahat.transform.position.x)
            srJahat.flipX = false;
        else
            srJahat.flipX = true;

        yield return new WaitForSeconds(waktuKaget);

        // --- 4. HANTU LARI KE BAWAH ---
        if (animJahat != null) animJahat.Play("HantuJalanBawah_Animation"); 
        
        while (Vector2.Distance(kuchiJahat.transform.position, titikKaburBawah.position) > 0.1f)
        {
            kuchiJahat.transform.position = Vector2.MoveTowards(kuchiJahat.transform.position, titikKaburBawah.position, speedKabur * Time.deltaTime);
            yield return null; 
        }

        // --- 5. RESET HANTU JAHAT ---
        kuchiJahat.transform.position = titikKembaliMap2.position;
        if (aiJahat != null) 
        {
            aiJahat.enabled = true; 
            aiJahat.ResetHantu();   
        }
        kuchiJahat.SetActive(false); 

        // --- 6. KEMBALIKAN KONTROL TAKU ---
        if (scriptGerakTaku != null) scriptGerakTaku.enabled = true;
    }
    
    public void ResetCutscene()
    {
        cutsceneAktif = false;
    }
}