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

    [Header("UI Notifikasi (HUD Permanen)")]
    public GameObject panelNotifUI; // Panel ini sekarang bakal nyala terus
    public TMP_Text teksNotifUI;    // Teks misi yang selalu mejeng di layar

    private void Awake()
    {
        if (instance == null) instance = this;
        else Destroy(gameObject);
    }

    private void Start()
    {
        // Pastikan UI Notif (HUD) langsung NYALA sejak game dimulai
        if (panelNotifUI != null) panelNotifUI.SetActive(true);

        // Sembunyikan UI Misi Detail (yang pakai Tab) saat game baru dimulai
        if (panelMisiUI != null) { isMisiOpen = false; panelMisiUI.SetActive(false); }

        UpdateUIMisi();
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

        // Kita HAPUS pemanggilan timer/coroutine di sini,
        // karena UpdateUIMisi() di atas udah otomatis mengganti teksnya.
    }

    public void UpdateUIMisi()
    {
        // Tentukan kata-katanya
        string kalimatMisi = (indeksMisiSaatIni < daftarMisi.Length) ? daftarMisi[indeksMisiSaatIni] : "Semua misi selesai!";

        // Update teks di Panel Tab
        if (teksMisiUI != null) teksMisiUI.text = "Objective:\n- " + kalimatMisi;

        // Update teks di Notif Pojok (HUD)
        if (teksNotifUI != null) teksNotifUI.text = "Misi Saat Ini:\n" + kalimatMisi;
    }
}