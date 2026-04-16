using UnityEngine;
using UnityEngine.UI;
using TMPro;

[System.Serializable]
public struct BarisDialog
{
    public string namaKarakter;
    [TextArea] public string isiTeks;
    public Sprite potretKarakter;
    public bool potretDiKiri;
}

public class DialogManager : MonoBehaviour
{
    public static DialogManager instance;

    [Header("UI Dialog")]
    public GameObject panelDialog;
    public TMP_Text teksNama;
    public TMP_Text teksDialog;
    public Image potretKiri;
    public Image potretKanan;

    [Header("Data Cerita Awal Game")]
    public BarisDialog[] alurCerita;

    private int indexKalimat;

    // Penanda buat Satpam PlayerMovement
    public bool sedangDialog = false;

    // Penanda kalau ini cuma interaksi benda (1 baris aja)
    private bool modeInteraksiBenda = false;

    private void Awake()
    {
        if (instance == null) instance = this;
        else Destroy(gameObject);
    }

    private void Start()
    {
        panelDialog.SetActive(false);
        MulaiDialogAwal(); // <-- Sekarang dia bakal otomatis jalan pas game mulai!
    }

    private void Update()
    {
        // Pakai Klik Kiri, Spasi, atau Enter buat nutup dialog
        if (sedangDialog && (Input.GetKeyDown(KeyCode.Space) || Input.GetKeyDown(KeyCode.Return) || Input.GetMouseButtonDown(0)))
        {
            if (modeInteraksiBenda)
            {
                // Kalau cuma ngecek pintu, langsung tutup panelnya!
                SelesaiDialog();
            }
            else
            {
                // Kalau lagi cutscene panjang, lanjut laci berikutnya
                LanjutKeKalimatBerikutnya();
            }
        }
    }

    // Fungsi 1: Buat Cutscene Panjang Berantai (Array)
    public void MulaiDialogAwal()
    {
        sedangDialog = true;
        modeInteraksiBenda = false;
        panelDialog.SetActive(true);
        indexKalimat = 0;
        TampilkanKalimat(alurCerita[indexKalimat]);
    }

    public void LanjutKeKalimatBerikutnya()
    {
        indexKalimat++;
        if (indexKalimat < alurCerita.Length) TampilkanKalimat(alurCerita[indexKalimat]);
        else SelesaiDialog();
    }

    private void TampilkanKalimat(BarisDialog baris)
    {
        teksDialog.text = baris.isiTeks;
        if (teksNama != null) teksNama.text = baris.namaKarakter;

        if (baris.potretDiKiri)
        {
            potretKiri.gameObject.SetActive(true);
            potretKanan.gameObject.SetActive(false);
            if (baris.potretKarakter != null) potretKiri.sprite = baris.potretKarakter;
        }
        else
        {
            potretKanan.gameObject.SetActive(true);
            potretKiri.gameObject.SetActive(false);
            if (baris.potretKarakter != null) potretKanan.sprite = baris.potretKarakter;
        }

        if (baris.potretKarakter == null)
        {
            potretKiri.gameObject.SetActive(false);
            potretKanan.gameObject.SetActive(false);
        }
    }

    // --- INI FUNGSI BARU BUAT PINTU / BENDA ---
    // --- FUNGSI BARU BUAT PINTU / BENDA (YANG UDAH BISA NERIMA GAMBAR) ---
    public void TampilkanDialogBenda(string nama, string teks, Sprite gambarKarakter = null)
    {
        sedangDialog = true;
        modeInteraksiBenda = true; // Kasih tahu sistem kalau ini cuma 1 baris
        panelDialog.SetActive(true);

        // Langsung ganti teksnya
        if (teksNama != null) teksNama.text = nama;
        if (teksDialog != null) teksDialog.text = teks;

        // Logika memunculkan gambar
        if (gambarKarakter != null)
        {
            // Kalau Pintunya ngirim gambar, nyalakan potret kiri!
            if (potretKiri != null)
            {
                potretKiri.gameObject.SetActive(true);
                potretKiri.sprite = gambarKarakter;
            }
            if (potretKanan != null) potretKanan.gameObject.SetActive(false);
        }
        else
        {
            // Kalau Pintunya gak ngirim gambar, matikan potretnya
            if (potretKiri != null) potretKiri.gameObject.SetActive(false);
            if (potretKanan != null) potretKanan.gameObject.SetActive(false);
        }
    }
    // ------------------------------------------

    private void SelesaiDialog()
    {
        sedangDialog = false;
        modeInteraksiBenda = false;
        panelDialog.SetActive(false);
    }
}