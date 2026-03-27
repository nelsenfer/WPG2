using UnityEngine;
using TMPro;

public class ObjectiveManager : MonoBehaviour
{
    // Trik "Singleton" agar script lain bisa memanggil script ini dengan mudah
    public static ObjectiveManager instance;

    [Header("Pengaturan UI")]
    public GameObject objectivePanel;
    public TMP_Text objectiveText;

    [Header("Daftar Misi")]
    // Array string ini akan muncul sebagai list di Inspector Unity
    public string[] daftarMisi;

    // Penunjuk misi nomor berapa yang sedang aktif (dimulai dari 0)
    private int misiSekarang = 0;

    void Awake()
    {
        // Mengaktifkan sistem Singleton saat game mulai
        instance = this;
    }

    void Start()
    {
        UpdateUIMisi();
    }

    void Update()
    {
        // Jika tombol Tab DITEKAN dan DITAHAN, panel muncul
        if (Input.GetKeyDown(KeyCode.Tab))
        {
            objectivePanel.SetActive(true);
        }
        // Jika tombol Tab DILEPAS, panel kembali hilang
        else if (Input.GetKeyUp(KeyCode.Tab))
        {
            objectivePanel.SetActive(false);
        }
    }

    // Fungsi ini bisa dipanggil dari script Item atau Pintu!
    public void LanjutMisi()
    {
        // Cek agar tidak error jika misi sudah habis
        if (misiSekarang < daftarMisi.Length - 1)
        {
            misiSekarang++; // Pindah ke misi selanjutnya
            UpdateUIMisi(); // Perbarui teks di layar
            Debug.Log("Misi Diperbarui!");
        }
        else
        {
            objectiveText.text = "MISI:\nSemua Misi Selesai!";
        }
    }

    void UpdateUIMisi()
    {
        if (daftarMisi.Length > 0)
        {
            objectiveText.text = "MISI SEKARANG:\n" + daftarMisi[misiSekarang];
        }
    }
}