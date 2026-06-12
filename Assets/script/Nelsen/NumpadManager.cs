using UnityEngine;
using TMPro;

public class NumpadManager : MonoBehaviour
{
    public static NumpadManager instance;

    [Header("UI Numpad")]
    public GameObject panelNumpad;
    public TMP_Text teksLayar; // Layar buat nampilin angka yang diketik

    // Laporan ke Satpam PlayerMovement
    public bool sedangBukaGembok = false;

    private string kodeBenar = "";
    private string inputSekarang = "";

    // Simpan referensi pintu mana yang lagi diutak-atik
    private Pintu pintuYangDicek;

    private void Awake()
    {
        if (instance == null) instance = this;
        else Destroy(gameObject);
    }

    private void Start()
    {
        panelNumpad.SetActive(false);
    }

    private void Update()
    {
        // Tombol darurat buat batalin/tutup UI gembok
        if (sedangBukaGembok && (Input.GetKeyDown(KeyCode.Escape) || Input.GetKeyDown(KeyCode.Backspace)))
        {
            TutupNumpad();
        }
    }

    // Fungsi ini dipanggil sama Pintu
    public void BukaNumpad(string passwordBenar, Pintu pintuTarget)
    {
        kodeBenar = passwordBenar;
        pintuYangDicek = pintuTarget;
        inputSekarang = "";
        teksLayar.text = "----"; // Tampilan awal di layar gembok

        sedangBukaGembok = true;
        panelNumpad.SetActive(true);
    }

    public void TutupNumpad()
    {
        sedangBukaGembok = false;
        panelNumpad.SetActive(false);
    }

    // --- FUNGSI INI AKAN DIPANGGIL OLEH TOMBOL-TOMBOL DI UI ---
    public void TambahAngka(string angka)
    {
        // Cegah ngetik kalau layarnya lagi nampilin tulisan ERROR atau OPEN
        if (teksLayar.text == "ERROR" || teksLayar.text == "OPEN") return;

        if (inputSekarang.Length < kodeBenar.Length)
        {
            inputSekarang += angka;
            teksLayar.text = inputSekarang;
        }

        // Kalau jumlah angka udah sama panjangnya, otomatis cek password!
        if (inputSekarang.Length == kodeBenar.Length)
        {
            CekPassword();
        }
    }

    // Tombol Delete (Opsional kalau kamu mau bikin tombol hapus)
    public void HapusAngkaTerakhir()
    {
        if (inputSekarang.Length > 0)
        {
            inputSekarang = inputSekarang.Substring(0, inputSekarang.Length - 1);
            teksLayar.text = inputSekarang;
        }
    }

    private void CekPassword()
    {
        if (inputSekarang == kodeBenar)
        {
            teksLayar.text = "OPEN";

            // Lapor ke pintu kalau passwordnya benar!
            if (pintuYangDicek != null)
            {
                pintuYangDicek.BukaKunciSukses();
            }

            Invoke("TutupNumpad", 1f); // Tutup layar otomatis setelah 1 detik
        }
        else
        {
            teksLayar.text = "ERROR";
            inputSekarang = ""; // Reset inputan
            Invoke("ResetLayar", 1f); // Balik kosong setelah 1 detik
        }
    }

    private void ResetLayar()
    {
        teksLayar.text = "----";
    }
}