using UnityEngine;
using UnityEngine.UI; // Wajib untuk akses UI Image

public class MenuFotoManager : MonoBehaviour
{
    [Header("Pengaturan Foto")]
    public Image fotoGaleri;       // Tarik objek UI Image Foto 1 kamu ke sini
    public Sprite gambarDefault;   // Gambar gembok / siluet (sebelum tamat)
    public Sprite gambarTamat;     // Gambar rahasia (setelah tamat)

    void Start()
    {
        // Ngecek buku catatan gaib, apakah "GameTamat" nilainya 1?
        if (PlayerPrefs.GetInt("GameTamat", 0) == 1)
        {
            // Kalau udah tamat, ganti fotonya!
            if (fotoGaleri != null) fotoGaleri.sprite = gambarTamat;
            Debug.Log("FOTO RAHASIA TERBUKA!");
        }
        else
        {
            // Kalau belum tamat, pakai gambar default
            if (fotoGaleri != null) fotoGaleri.sprite = gambarDefault;
        }
    }

    // Tombol sakti buat ngetes / ngereset memory pas kamu lagi bikin game
    [ContextMenu("Reset Save Data Tamat")]
    public void HapusDataTamat()
    {
        PlayerPrefs.DeleteKey("GameTamat");
        Debug.Log("Data Tamat Dihapus! Foto kembali terkunci.");
    }
}