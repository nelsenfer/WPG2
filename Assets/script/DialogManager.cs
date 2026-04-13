using UnityEngine;
using UnityEngine.UI; // Wajib untuk akses Image
using TMPro;

// Ini "Resep Laci" baru kita. [Serializable] bikin laci ini bisa nongol di Inspector Unity!
[System.Serializable]
public struct BarisDialog
{
    public string namaKarakter;
    [TextArea] public string isiTeks;
    public Sprite potretKarakter;
    public bool potretDiKiri; // Centang = Kiri (Player), Gak dicentang = Kanan (NPC)
}

public class DialogManager : MonoBehaviour
{
    public static DialogManager instance;

    [Header("UI Dialog")]
    public GameObject panelDialog;
    public TMP_Text teksNama;   // Taruh UI teks nama di sini (kalau ada)
    public TMP_Text teksDialog;
    public Image potretKiri;    // Taruh UI Image Kiri di sini
    public Image potretKanan;   // Taruh UI Image Kanan di sini

    [Header("Data Cerita Awal Game")]
    public BarisDialog[] alurCerita;

    private int indexKalimat;
    private bool sedangDialog = false;

    private void Awake()
    {
        if (instance == null) instance = this;
        else Destroy(gameObject);
    }

    private void Start()
    {
        panelDialog.SetActive(false);
        MulaiDialogAwal();
    }

    private void Update()
    {
        if (sedangDialog && (Input.GetKeyDown(KeyCode.Space) || Input.GetKeyDown(KeyCode.Return) || Input.GetMouseButtonDown(0)))
        {
            LanjutKeKalimatBerikutnya();
        }
    }

    public void MulaiDialogAwal()
    {
        sedangDialog = true;
        panelDialog.SetActive(true);
        indexKalimat = 0;
        Time.timeScale = 0f; // Pause waktu

        TampilkanKalimat(alurCerita[indexKalimat]);
    }

    public void LanjutKeKalimatBerikutnya()
    {
        indexKalimat++;

        if (indexKalimat < alurCerita.Length)
        {
            TampilkanKalimat(alurCerita[indexKalimat]);
        }
        else
        {
            SelesaiDialog();
        }
    }

    // Fungsi khusus untuk ngatur UI tiap kali teks ganti
    private void TampilkanKalimat(BarisDialog baris)
    {
        // 1. Tampilkan Teks & Nama
        teksDialog.text = baris.isiTeks;
        if (teksNama != null) teksNama.text = baris.namaKarakter;

        // 2. Logika Potret Kiri/Kanan
        if (baris.potretDiKiri)
        {
            // Nyalakan potret kiri, matikan kanan
            potretKiri.gameObject.SetActive(true);
            potretKanan.gameObject.SetActive(false);

            if (baris.potretKarakter != null) potretKiri.sprite = baris.potretKarakter;
        }
        else
        {
            // Nyalakan potret kanan, matikan kiri
            potretKanan.gameObject.SetActive(true);
            potretKiri.gameObject.SetActive(false);

            if (baris.potretKarakter != null) potretKanan.sprite = baris.potretKarakter;
        }

        // Opsional: Kalau lacinya nggak diisi gambar sama sekali, sembunyikan potretnya
        if (baris.potretKarakter == null)
        {
            potretKiri.gameObject.SetActive(false);
            potretKanan.gameObject.SetActive(false);
        }
    }

    private void SelesaiDialog()
    {
        sedangDialog = false;
        panelDialog.SetActive(false);
        Time.timeScale = 1f; // Waktu jalan lagi
    }
}