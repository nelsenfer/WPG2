using System.Collections;
using UnityEngine;

public class EventDialogBakeneko : MonoBehaviour
{
    [Header("UI Pilihan Ganda")]
    public GameObject panelPilihan;

    // --- FITUR BARU: LACI GAMBAR ---
    [Header("Potret Karakter (Wajib Diisi)")]
    public Sprite potretTaku;
    public Sprite potretBakeneko;

    [Header("Potongan Naskah Dialog")]
    public BarisDialog[] naskahIntro;
    public BarisDialog[] naskahBeriRoti;
    public BarisDialog[] naskahDiamSaja;
    public BarisDialog[] naskahPenutup;

    [HideInInspector]
    public GameObject promptUI;

    [Header("Pengaturan Trigger")]
    public bool mulaiOtomatisSaatNginjek = false;

    private int pilihanPemain = 0;
    private bool sudahMulai = false;
    private bool playerDiDekat = false;

    void Start()
    {
        if (panelPilihan != null) panelPilihan.SetActive(false);
        Canvas canvasChild = GetComponentInChildren<Canvas>(true);
        if (canvasChild != null)
        {
            promptUI = canvasChild.gameObject;
            promptUI.SetActive(false);
        }
    }

    void Update()
    {
        if (DialogManager.instance != null && DialogManager.instance.sedangDialog) return;
        if (NumpadManager.instance != null && NumpadManager.instance.sedangBukaGembok) return;

        if (playerDiDekat && Input.GetKeyDown(KeyCode.E) && !sudahMulai)
        {
            StartCoroutine(MulaiDramaBakeneko());
        }
    }

    private void OnTriggerEnter2D(Collider2D collision)
    {
        if (collision.CompareTag("Player"))
        {
            playerDiDekat = true;

            // Kalau centang auto-mulai nyala, Bakeneko langsung ngajak ngobrol!
            if (mulaiOtomatisSaatNginjek && !sudahMulai)
            {
                StartCoroutine(MulaiDramaBakeneko());
            }
            else if (promptUI != null && !sudahMulai)
            {
                promptUI.SetActive(true);
            }
        }
    }

    private void OnTriggerExit2D(Collider2D collision)
    {
        if (collision.CompareTag("Player"))
        {
            playerDiDekat = false;
            if (promptUI != null) promptUI.SetActive(false);
        }
    }

    public void KlikBeriRoti() { pilihanPemain = 1; }
    public void KlikDiamSaja() { pilihanPemain = 2; }

    IEnumerator MulaiDramaBakeneko()
    {
        sudahMulai = true;
        if (promptUI != null) promptUI.SetActive(false);

        DialogManager.instance.MulaiDialogNPC(naskahIntro);
        yield return new WaitForSeconds(0.1f);
        yield return new WaitUntil(() => !DialogManager.instance.sedangDialog);

        if (panelPilihan != null) panelPilihan.SetActive(true);
        if (DialogManager.instance != null) DialogManager.instance.sedangDialog = true;

        yield return new WaitUntil(() => pilihanPemain != 0);
        if (panelPilihan != null) panelPilihan.SetActive(false);

        if (pilihanPemain == 1) DialogManager.instance.MulaiDialogNPC(naskahBeriRoti);
        else if (pilihanPemain == 2) DialogManager.instance.MulaiDialogNPC(naskahDiamSaja);

        yield return new WaitForSeconds(0.1f);
        yield return new WaitUntil(() => !DialogManager.instance.sedangDialog);

        DialogManager.instance.MulaiDialogNPC(naskahPenutup);
        yield return new WaitForSeconds(0.1f);
        yield return new WaitUntil(() => !DialogManager.instance.sedangDialog);

        if (ObjectiveManager.instance != null) ObjectiveManager.instance.LanjutMisi();
        gameObject.SetActive(false);
    }

    // =========================================================================
    // FITUR MAGIC AI: AUTO FILL NASKAH (SEKARANG PLUS GAMBAR!)
    // =========================================================================
    [ContextMenu("✨ ISI OTOMATIS NASKAH BAKENEKO ✨")]
    private void AutoIsiNaskah()
    {
        // INTRO
        naskahIntro = new BarisDialog[2];
        naskahIntro[0] = new BarisDialog { namaKarakter = "Taku", isiTeks = "Bukankah kamu.... Yokai... Bakeneko??", potretKarakter = potretTaku };
        naskahIntro[1] = new BarisDialog { namaKarakter = "Bakeneko", isiTeks = "meow...", potretKarakter = potretBakeneko };

        // PILIHAN A: BERI ROTI
        naskahBeriRoti = new BarisDialog[4];
        naskahBeriRoti[0] = new BarisDialog { namaKarakter = "Taku", isiTeks = "!! aku baru ingat ada sisa roti yang ku makan tadi siang, hmmm... kasih aja kali ya.", potretKarakter = potretTaku };
        naskahBeriRoti[1] = new BarisDialog { namaKarakter = "Bakeneko", isiTeks = "HEI!! Kamu kira aku kucing apaan dikasih roti bekas gigitan manusia!?", potretKarakter = potretBakeneko };
        naskahBeriRoti[2] = new BarisDialog { namaKarakter = "Taku", isiTeks = "HWAAA..!!\n(Taku terkejut sampai terjatuh karena Bakeneko itu bisa berbicara seperti manusia)", potretKarakter = potretTaku };
        naskahBeriRoti[3] = new BarisDialog { namaKarakter = "Bakeneko", isiTeks = "kenapa kok kaget banget? Kayak pertama kali aja lihat ginian.", potretKarakter = potretBakeneko };

        // PILIHAN B: DIAM SAJA
        naskahDiamSaja = new BarisDialog[7];
        naskahDiamSaja[0] = new BarisDialog { namaKarakter = "Taku", isiTeks = "(Terdiam sambil memperhatikan apa yang Bakeneko itu lakukan)", potretKarakter = potretTaku };
        naskahDiamSaja[1] = new BarisDialog { namaKarakter = "Bakeneko", isiTeks = "meow....", potretKarakter = potretBakeneko };
        naskahDiamSaja[2] = new BarisDialog { namaKarakter = "Taku", isiTeks = "(ini bukannya kucing yang aku lihat di ruangan sebelumnya ya?....)", potretKarakter = potretTaku };
        naskahDiamSaja[3] = new BarisDialog { namaKarakter = "Bakeneko", isiTeks = "meoww....", potretKarakter = potretBakeneko };
        naskahDiamSaja[4] = new BarisDialog { namaKarakter = "Taku", isiTeks = "(ehh? Kok keknya dia kayak respon omongan yang ada di pikiranku ini?)", potretKarakter = potretTaku };
        naskahDiamSaja[5] = new BarisDialog { namaKarakter = "Bakeneko", isiTeks = "WOY!! Dasar manusia, kalo mbatin dipikir mas.", potretKarakter = potretBakeneko };
        naskahDiamSaja[6] = new BarisDialog { namaKarakter = "Taku", isiTeks = "HWAAA..!!\n(Taku terkejut sampai terjatuh karena Bakeneko itu bisa berbicara)", potretKarakter = potretTaku };

        // PENUTUP
        naskahPenutup = new BarisDialog[13];
        naskahPenutup[0] = new BarisDialog { namaKarakter = "Taku", isiTeks = "maksud gerangan apa kesini?", potretKarakter = potretTaku };
        naskahPenutup[1] = new BarisDialog { namaKarakter = "Bakeneko", isiTeks = "ya gapapa dongg...?? emang salah ya? Gk mungkin dong aku muncul tanpa sebab. Dipikir secara logika aja.", potretKarakter = potretBakeneko };
        naskahPenutup[2] = new BarisDialog { namaKarakter = "Taku", isiTeks = "gaul banget kalo ngomong kayak umurnya seumuran Gen Z aja", potretKarakter = potretTaku };
        naskahPenutup[3] = new BarisDialog { namaKarakter = "Bakeneko", isiTeks = "BRISIK!", potretKarakter = potretBakeneko };
        naskahPenutup[4] = new BarisDialog { namaKarakter = "", isiTeks = "(Bakeneko mulai merajuk dan mulai akan pergi)", potretKarakter = null };
        naskahPenutup[5] = new BarisDialog { namaKarakter = "Taku", isiTeks = "EH EH!! Tunggu!", potretKarakter = potretTaku };
        naskahPenutup[6] = new BarisDialog { namaKarakter = "Bakeneko", isiTeks = "akhirnya... (sambil tersenyum tipis)", potretKarakter = potretBakeneko };
        naskahPenutup[7] = new BarisDialog { namaKarakter = "Taku", isiTeks = "Kamu tau gk teka teki yang ada di tempat ini, harusnya tau dongg....", potretKarakter = potretTaku };
        naskahPenutup[8] = new BarisDialog { namaKarakter = "Bakeneko", isiTeks = "moh", potretKarakter = potretBakeneko };
        naskahPenutup[9] = new BarisDialog { namaKarakter = "Taku", isiTeks = "Eh?? Eeee.... Baru juga nanya. M-maaf deh kalo aku salah, y-ya udah deh aku bakalan turutin apa yang kamu mau asalkan bisa kasih petunjuk...", potretKarakter = potretTaku };
        naskahPenutup[10] = new BarisDialog { namaKarakter = "Bakeneko", isiTeks = "carikan dulu aku minyak lampu sebanyak tiga botol baru aku akan memberikan barang itu.", potretKarakter = potretBakeneko };
        naskahPenutup[11] = new BarisDialog { namaKarakter = "Taku", isiTeks = "minyak lampu??", potretKarakter = potretTaku };
        naskahPenutup[12] = new BarisDialog { namaKarakter = "Bakeneko", isiTeks = "aduhhh dasar manusia ini ya, pokoknya cariin kalo mau barang terakhir.", potretKarakter = potretBakeneko };

        Debug.Log("SULAP AI BERHASIL! Naskah Bakeneko & GAMBAR sudah terisi otomatis!");
    }
}