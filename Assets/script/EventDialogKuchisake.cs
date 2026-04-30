using System.Collections;
using UnityEngine;
using UnityEngine.UI;
using TMPro;
using UnityEngine.SceneManagement;
using UnityEngine.EventSystems; // WAJIB DITAMBAHIN BUAT FITUR KEYBOARD

public class EventDialogKuchisake : MonoBehaviour
{
    [Header("UI Pilihan Ganda")]
    public GameObject panelPilihan;

    // --- FITUR BARU: FOKUS KEYBOARD ---
    [Tooltip("Tarik tombol opsi pertama (atas) ke sini biar langsung kefokus")]
    public GameObject tombolPertama;

    public TMP_Text teksTombol1;
    public TMP_Text teksTombol2;

    [Header("Perubahan Wujud")]
    public SpriteRenderer spriteWorldKuchisake;
    public Sprite pixelSeram;
    public Sprite pixelCantik;
    public Sprite potretTaku;
    public Sprite potretSeram;
    public Sprite potretCantik;

    [Header("Naskah Sesi 1 - 4 (Isi pakai Auto Fill)")]
    public BarisDialog[] naskahIntro;
    public BarisDialog[] opsi1Lose; public BarisDialog[] opsi1Win; public BarisDialog[] reaksi1;
    public BarisDialog[] opsi2Win; public BarisDialog[] opsi2Lose; public BarisDialog[] reaksi2;
    public BarisDialog[] opsi3Win; public BarisDialog[] opsi3Lose; public BarisDialog[] reaksi3;
    public BarisDialog[] opsi4Win; public BarisDialog[] opsi4Lose;
    public BarisDialog[] naskahPenutup;

    [Header("Cutscene Ending (BARU)")]
    public Transform titikJalanKeluar;
    public Image layarPutihTransisi;
    public string namaSceneMainMenu = "Epilog"; // Pastikan namamu bener ya, misal Epilog atau MainMenu

    [HideInInspector] public GameObject promptUI;
    private int pilihanPemain = 0;
    private bool sudahMulai = false;
    private bool playerDiDekat = false;

    void Start()
    {
        if (panelPilihan != null) panelPilihan.SetActive(false);
        if (layarPutihTransisi != null) layarPutihTransisi.gameObject.SetActive(false);

        Canvas canvasChild = GetComponentInChildren<Canvas>(true);
        if (canvasChild != null)
        {
            promptUI = canvasChild.gameObject;
            promptUI.SetActive(false);
        }

        if (spriteWorldKuchisake != null && pixelSeram != null)
            spriteWorldKuchisake.sprite = pixelSeram;
    }

    void Update()
    {
        if (DialogManager.instance != null && DialogManager.instance.sedangDialog) return;
        if (playerDiDekat && Input.GetKeyDown(KeyCode.E) && !sudahMulai)
            StartCoroutine(MulaiDramaKuchisake());
    }

    private void OnTriggerEnter2D(Collider2D collision)
    {
        if (collision.CompareTag("Player"))
        {
            playerDiDekat = true;
            if (promptUI != null && !sudahMulai) promptUI.SetActive(true);
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

    public void KlikOpsi1() { pilihanPemain = 1; }
    public void KlikOpsi2() { pilihanPemain = 2; }

    // --- FUNGSI TAMPILKAN PILIHAN YANG UDAH DI-UPGRADE ---
    void TampilkanPilihan(string teks1, string teks2)
    {
        pilihanPemain = 0;
        if (teksTombol1 != null) teksTombol1.text = teks1;
        if (teksTombol2 != null) teksTombol2.text = teks2;

        if (panelPilihan != null)
        {
            panelPilihan.SetActive(true);

            // Paksa Unity menyorot tombol pertama pakai keyboard
            if (tombolPertama != null)
            {
                EventSystem.current.SetSelectedGameObject(null);
                EventSystem.current.SetSelectedGameObject(tombolPertama);
            }
        }

        if (DialogManager.instance != null) DialogManager.instance.sedangDialog = true;
    }

    public void ResetEvent()
    {
        StopAllCoroutines();
        sudahMulai = false;
        pilihanPemain = 0;

        if (panelPilihan != null) panelPilihan.SetActive(false);

        if (spriteWorldKuchisake != null && pixelSeram != null)
            spriteWorldKuchisake.sprite = pixelSeram;
    }

    IEnumerator PutarDialog(BarisDialog[] naskah)
    {
        DialogManager.instance.MulaiDialogNPC(naskah);
        yield return new WaitForSeconds(0.1f);
        yield return new WaitUntil(() => !DialogManager.instance.sedangDialog);
    }

    IEnumerator ProsesKalah(BarisDialog[] naskahSalah)
    {
        yield return PutarDialog(naskahSalah);
        GameOverManager.instance.MatiDarah();
        yield break;
    }

    IEnumerator MulaiDramaKuchisake()
    {
        sudahMulai = true;
        if (promptUI != null) promptUI.SetActive(false);

        // Pertanyaan 1
        yield return PutarDialog(naskahIntro);
        TampilkanPilihan("Ya mau gimana lagi...", "Bukan berarti tak berharga.");
        yield return new WaitUntil(() => pilihanPemain != 0);
        panelPilihan.SetActive(false);
        if (pilihanPemain == 1) { yield return ProsesKalah(opsi1Lose); yield break; }
        yield return PutarDialog(opsi1Win); yield return PutarDialog(reaksi1);

        // Pertanyaan 2
        TampilkanPilihan("Bukan salah mereka sepenuhnya.", "Kesalahan sendiri.");
        yield return new WaitUntil(() => pilihanPemain != 0);
        panelPilihan.SetActive(false);
        if (pilihanPemain == 2) { yield return ProsesKalah(opsi2Lose); yield break; }
        yield return PutarDialog(opsi2Win); yield return PutarDialog(reaksi2);

        // Pertanyaan 3
        TampilkanPilihan("Coba nerima pelan-pelan.", "Ya masalahmu.");
        yield return new WaitUntil(() => pilihanPemain != 0);
        panelPilihan.SetActive(false);
        if (pilihanPemain == 2) { yield return ProsesKalah(opsi3Lose); yield break; }
        yield return PutarDialog(opsi3Win); yield return PutarDialog(reaksi3);

        // Pertanyaan 4
        TampilkanPilihan("Seseorang yang terluka.", "Sesuatu yang dijauhi.");
        yield return new WaitUntil(() => pilihanPemain != 0);
        panelPilihan.SetActive(false);
        if (pilihanPemain == 2) { yield return ProsesKalah(opsi4Lose); yield break; }
        yield return PutarDialog(opsi4Win);

        // --- ENDING (TRANSFORMASI CANTIK) ---
        if (spriteWorldKuchisake != null && pixelCantik != null)
            spriteWorldKuchisake.sprite = pixelCantik;

        yield return PutarDialog(naskahPenutup);

        // Ibuknya menghilang
        gameObject.GetComponent<SpriteRenderer>().enabled = false;
        if (GetComponent<Collider2D>() != null) GetComponent<Collider2D>().enabled = false; // Biar Taku ga nabrak gaib

        // --- CUTSCENE DIMULAI ---
        GameObject player = GameObject.FindGameObjectWithTag("Player");

        if (player != null && titikJalanKeluar != null)
        {
            // 1. Matikan script PlayerMovement
            MonoBehaviour[] scripts = player.GetComponents<MonoBehaviour>();
            foreach (MonoBehaviour s in scripts) { if (s.GetType().Name == "PlayerMovement") s.enabled = false; }

            // 2. Nyalakan Animasi Jalan
            Animator animTaku = player.GetComponent<Animator>();
            if (animTaku != null)
            {
                animTaku.SetFloat("Horizontal", 0f);
                animTaku.SetFloat("Vertical", 1f); // Hadap atas
                animTaku.SetFloat("Speed", 1f);    // Jalan
            }

            // 3. Taku meluncur pelan-pelan ke pintu
            while (Vector2.Distance(player.transform.position, titikJalanKeluar.position) > 0.1f)
            {
                player.transform.position = Vector2.MoveTowards(player.transform.position, titikJalanKeluar.position, 2f * Time.deltaTime);
                yield return null;
            }

            // 4. Matikan Animasi setelah sampai
            if (animTaku != null)
            {
                animTaku.SetFloat("Speed", 0f);
            }
        }

        // --- LAYAR FADE IN PUTIH ---
        if (layarPutihTransisi != null)
        {
            layarPutihTransisi.gameObject.SetActive(true);
            Color warna = layarPutihTransisi.color;
            warna.a = 0;
            layarPutihTransisi.color = warna;

            float waktu = 0;
            while (waktu < 2f)
            {
                waktu += Time.deltaTime;
                warna.a = waktu / 2f;
                layarPutihTransisi.color = warna;
                yield return null;
            }
        }

        yield return new WaitForSeconds(1f);

        // SIMPAN DATA GAME TAMAT!
        PlayerPrefs.SetInt("GameTamat", 1);
        PlayerPrefs.Save();

        // Balik ke Main Menu / Epilog
        SceneManager.LoadScene(namaSceneMainMenu);
    }

    [ContextMenu("✨ ISI OTOMATIS NASKAH KUCHISAKE ✨")]
    private void AutoIsiNaskah()
    {
        naskahIntro = new BarisDialog[] {
            new BarisDialog { namaKarakter = "Kuchisake Onna", isiTeks = "Akhirnya aku bertemu denganmu, Tachibana Takumi.", potretKarakter = potretSeram },
            new BarisDialog { namaKarakter = "Taku", isiTeks = "Aku tidak berniat macam-macam padamu, aku hanya terjebak disini dan ingin keluar dari tempat ini.", potretKarakter = potretTaku },
            new BarisDialog { namaKarakter = "Kuchisake Onna", isiTeks = "Jika kau bisa menjawab tiga pertanyaan dariku, maka pergilah sesuai arahan yang kuberikan.", potretKarakter = potretSeram },
            new BarisDialog { namaKarakter = "Taku", isiTeks = "Baiklah....", potretKarakter = potretTaku },
            new BarisDialog { namaKarakter = "Kuchisake Onna", isiTeks = "Kalau semua orang hanya melihat penampilan… menurutmu… apakah aku masih punya arti?", potretKarakter = potretSeram }
        };

        opsi1Lose = new BarisDialog[] { new BarisDialog { namaKarakter = "Taku", isiTeks = "Ya orang lihatnya gitu, mau gimana lagi.", potretKarakter = potretTaku } };
        opsi1Win = new BarisDialog[] { new BarisDialog { namaKarakter = "Taku", isiTeks = "Mungkin mereka cuma lihat dari luar… tapi itu bukan berarti kamu jadi tidak berharga.", potretKarakter = potretTaku } };
        reaksi1 = new BarisDialog[] { new BarisDialog { namaKarakter = "Kuchisake Onna", isiTeks = "Jawaban yang cukup unik.... Lalu, kalau seseorang disakiti sampai berubah entah secara fisik atau mental… apa mereka juga harus disalahkan?", potretKarakter = potretSeram } };

        opsi2Win = new BarisDialog[] { new BarisDialog { namaKarakter = "Taku", isiTeks = "Orang nggak bakal berubah tanpa alasan… jadi bagiku, itu bukan sepenuhnya salah mereka.", potretKarakter = potretTaku } };
        opsi2Lose = new BarisDialog[] { new BarisDialog { namaKarakter = "Taku", isiTeks = "Kalau sampai berubah gitu, itu kesalahan sendiri.", potretKarakter = potretTaku } };
        reaksi2 = new BarisDialog[] { new BarisDialog { namaKarakter = "Kuchisake Onna", isiTeks = "Kalau kamu berada di posisiku… apa kamu masih bisa menerima dirimu sendiri setelah semua yang telah terjadi?", potretKarakter = potretSeram } };

        opsi3Win = new BarisDialog[] { new BarisDialog { namaKarakter = "Taku", isiTeks = "Kalau aku di posisimu… mungkin aku juga kesulitan… tapi aku tetap mau coba nerima, walau pelan-pelan.", potretKarakter = potretTaku } };
        opsi3Lose = new BarisDialog[] { new BarisDialog { namaKarakter = "Taku", isiTeks = "Kalau nggak bisa nerima, ya masalahmu.", potretKarakter = potretTaku } };
        reaksi3 = new BarisDialog[] { new BarisDialog { namaKarakter = "Kuchisake Onna", isiTeks = "Kali ini aku membenci jawabanmu, keyakinanmu runtuh di pertanyaan ketiga. Sekarang pertanyaan terakhirku. Kamu melihatku sebagai apa?", potretKarakter = potretSeram } };

        opsi4Win = new BarisDialog[] { new BarisDialog { namaKarakter = "Taku", isiTeks = "Aku nggak lihat kamu sebagai sesuatu yang menakutkan… aku lihat kamu sebagai seseorang yang terluka.", potretKarakter = potretTaku } };
        opsi4Lose = new BarisDialog[] { new BarisDialog { namaKarakter = "Taku", isiTeks = "Aku lihat kamu… cuma sebagai sesuatu yang harus dijauhi.", potretKarakter = potretTaku } };

        naskahPenutup = new BarisDialog[] {
            new BarisDialog { namaKarakter = "", isiTeks = "(Kuchisake Onna tanpa sadar mulai meneteskan air mata. Perlahan wajah menyeramkannya berubah menjadi sosok wanita cantik...)", potretKarakter = null },
            new BarisDialog { namaKarakter = "", isiTeks = "(Terdengar suara pintu berat di belakang terbuka dengan sendirinya...)", potretKarakter = null },
            new BarisDialog { namaKarakter = "Kuchisake Onna", isiTeks = "…terima kasih… karena sudah melihatku…sebagai manusia.", potretKarakter = potretCantik },
            new BarisDialog { namaKarakter = "", isiTeks = "(Ia tersenyum manis lalu menghilang ke udara. Di kejauhan, Yokai lain tampak mengawasimu dengan kehangatan.)", potretKarakter = null }
        };

        Debug.Log("SULAP AI BERHASIL! Naskah Kuchisake + Animasi Ganti Wajah sudah terisi!");
    }
}