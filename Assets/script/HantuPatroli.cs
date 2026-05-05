using System.Collections;
using UnityEngine;

public class HantuPatroli : MonoBehaviour
{
    [Header("Pengaturan Patroli")]
    public Transform[] titikPatroli;
    public float speedPatroli = 3f;

    [Header("Pengaturan Kejar")]
    public float speedKejar = 4.5f; // Hantu lari lebih cepat saat ngejar
    public float jarakMati = 0.8f;  // Seberapa dekat hantu harus menyentuh Taku buat Game Over

    [Header("Pengaturan Pengecekan")]
    public int peluangCek = 40;
    public float waktuNgecek = 2f;

    [Header("Komponen Fisik")]
    public Transform jarakPandangCollider;

    private bool sedangNgecek = false;
    private bool sedangNgejar = false;
    private Transform targetTaku;
    private Collider2D takuCollider;
    private SpriteRenderer sr;
    private int indeksTujuan = 0;

    void Start()
    {
        sr = GetComponent<SpriteRenderer>();
    }

    void Update()
    {
        if (sedangNgecek || titikPatroli.Length == 0) return;
        if (DialogManager.instance != null && DialogManager.instance.sedangDialog) return;

        if (sedangNgejar && targetTaku != null)
        {
            // --- LOGIKA NGEJAR TAKU ---

            // Kalau Taku sembunyi di lemari (Collider mati), hantu kehilangan jejak!
            if (!takuCollider.enabled)
            {
                sedangNgejar = false;
                return;
            }

            transform.position = Vector2.MoveTowards(transform.position, targetTaku.position, speedKejar * Time.deltaTime);

            // Balik badan hadap Taku
            if (targetTaku.position.x > transform.position.x)
            {
                sr.flipX = false;
                if (jarakPandangCollider != null) jarakPandangCollider.localPosition = new Vector2(Mathf.Abs(jarakPandangCollider.localPosition.x), jarakPandangCollider.localPosition.y);
            }
            else if (targetTaku.position.x < transform.position.x)
            {
                sr.flipX = true;
                if (jarakPandangCollider != null) jarakPandangCollider.localPosition = new Vector2(-Mathf.Abs(jarakPandangCollider.localPosition.x), jarakPandangCollider.localPosition.y);
            }

            // CEK JARAK: Kalau nempel banget, MATI!
            if (Vector2.Distance(transform.position, targetTaku.position) <= jarakMati)
            {
                if (GameOverManager.instance != null) GameOverManager.instance.MatiDarah();
            }
        }
        else
        {
            // --- LOGIKA JALAN PATROLI NORMAL ---
            Transform tujuan = titikPatroli[indeksTujuan];
            transform.position = Vector2.MoveTowards(transform.position, tujuan.position, speedPatroli * Time.deltaTime);

            if (tujuan.position.x > transform.position.x)
            {
                sr.flipX = false;
                if (jarakPandangCollider != null) jarakPandangCollider.localPosition = new Vector2(Mathf.Abs(jarakPandangCollider.localPosition.x), jarakPandangCollider.localPosition.y);
            }
            else if (tujuan.position.x < transform.position.x)
            {
                sr.flipX = true;
                if (jarakPandangCollider != null) jarakPandangCollider.localPosition = new Vector2(-Mathf.Abs(jarakPandangCollider.localPosition.x), jarakPandangCollider.localPosition.y);
            }

            if (Vector2.Distance(transform.position, tujuan.position) < 0.1f)
            {
                indeksTujuan++;
                if (indeksTujuan >= titikPatroli.Length) indeksTujuan = 0;
            }
        }
    }

    private void OnTriggerEnter2D(Collider2D collision)
    {
        // Kalau Taku masuk ke kotak pandang panjang, hantu mulai ngejar!
        if (collision.CompareTag("Player") && !sedangNgejar)
        {
            sedangNgejar = true;
            targetTaku = collision.transform;
            takuCollider = collision.GetComponent<Collider2D>();
            return;
        }

        LemariSembunyi tempatSembunyi = collision.GetComponent<LemariSembunyi>();

        // Hantu cuma gacha ngecek lemari kalau lagi jalan santai (nggak lagi ngejar)
        if (tempatSembunyi != null && !sedangNgecek && !sedangNgejar)
        {
            int gacha = Random.Range(1, 101);
            if (gacha <= peluangCek)
            {
                StartCoroutine(ProsesNgecek(tempatSembunyi));
            }
        }
    }

    IEnumerator ProsesNgecek(LemariSembunyi tempatSembunyi)
    {
        sedangNgecek = true;
        yield return new WaitForSeconds(waktuNgecek);

        if (tempatSembunyi.playerSedangSembunyi)
        {
            if (GameOverManager.instance != null) GameOverManager.instance.MatiDarah();
        }

        sedangNgecek = false;
    }

    public void ResetHantu()
    {
        sedangNgejar = false;
        sedangNgecek = false;
        targetTaku = null;

        // Kembalikan hantu ke titik patroli pertama
        if (titikPatroli.Length > 0)
        {
            indeksTujuan = 0;
            transform.position = titikPatroli[0].position;
        }
    }
}