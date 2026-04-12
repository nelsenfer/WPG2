using System.Collections; // Wajib ditambah untuk fitur Timer (Coroutine)
using UnityEngine;
using TMPro;

public class ObjectiveManager : MonoBehaviour
{
    public static ObjectiveManager instance;

    [Header("Daftar Misi (Tulis di Inspector)")]
    public string[] daftarMisi;
    public int indeksMisiSaatIni = 0;

    [Header("UI Misi Detail (Buka pakai Tab)")]
    public GameObject panelMisiUI; // ObjectivePanel lama milikmu
    public TMP_Text teksMisiUI;    // ObjectiveText lama milikmu
    private bool isMisiOpen = false;

    [Header("UI Notifikasi (Pojok Kiri Atas)")]
    public GameObject panelNotifUI; // Panel/Teks baru untuk pop-up sekilas
    public TMP_Text teksNotifUI;    // Teks di dalam pop-up tersebut
    public float lamaNotifMuncul = 4f; // Berapa detik notifnya mejeng di layar

    private void Awake()
    {
        if (instance == null) instance = this;
        else Destroy(gameObject);
    }

    private void Start()
    {
        UpdateUIMisi();

        // Sembunyikan semua UI Misi saat game baru dimulai
        if (panelMisiUI != null) { isMisiOpen = false; panelMisiUI.SetActive(false); }
        if (panelNotifUI != null) panelNotifUI.SetActive(false);
    }

    private void Update()
    {
        // Fitur Buka/Tutup Detail Misi pakai Tab
        if (Input.GetKeyDown(KeyCode.Tab))
        {
            if (panelMisiUI != null)
            {
                isMisiOpen = !isMisiOpen;
                panelMisiUI.SetActive(isMisiOpen);
            }
        }
    }

    public void LanjutMisi()
    {
        indeksMisiSaatIni++;
        UpdateUIMisi();

        // Fitur Tambahan: Munculkan Notifikasi Pop-up Pojok Kiri
        if (panelNotifUI != null)
        {
            StopAllCoroutines(); // Reset timer jaga-jaga kalau misi beruntun cepat
            StartCoroutine(MunculkanNotifSekilas());
        }
    }

    public void UpdateUIMisi()
    {
        // Tentukan kata-katanya
        string kalimatMisi = (indeksMisiSaatIni < daftarMisi.Length) ? daftarMisi[indeksMisiSaatIni] : "Semua misi selesai!";

        // Update teks di Panel Tab
        if (teksMisiUI != null) teksMisiUI.text = "Objective:\n- " + kalimatMisi;

        // Update teks di Pop-up Pojok Kiri
        if (teksNotifUI != null) teksNotifUI.text = "Update Misi:\n" + kalimatMisi;
    }

    // Ini adalah fungsi Timer pengatur waktu pop-up
    private IEnumerator MunculkanNotifSekilas()
    {
        panelNotifUI.SetActive(true); // Nyalakan UI Pojok Kiri
        yield return new WaitForSeconds(lamaNotifMuncul); // Tunggu selama 4 detik
        panelNotifUI.SetActive(false); // Matikan lagi UI Pojok Kiri
    }
}