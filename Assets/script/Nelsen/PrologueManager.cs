using UnityEngine;
using UnityEngine.UI;
using UnityEngine.SceneManagement;
using System.Collections;

public class PrologueManager : MonoBehaviour
{
    [Header("Referensi UI")]
    public Image targetLayar;

    [Header("Database Gambar")]
    public Sprite[] kumpulanGambar;

    [Header("Pengaturan Waktu")]
    public float waktuFade = 1f;         // Durasi transisi gelap ke terang
    public float waktuWajibTampil = 2f;  // Durasi wajib sebelum bisa diklik
    public string namaSceneGame;

    private int index = 0;

    void Start()
    {
        // Pastikan gambar transparan (Alpha 0) saat pertama kali mulai
        Color warnaAwal = targetLayar.color;
        warnaAwal.a = 0f;
        targetLayar.color = warnaAwal;

        StartCoroutine(SiklusProlog());
    }

    IEnumerator SiklusProlog()
    {
        while (index < kumpulanGambar.Length)
        {
            // 1. Siapkan gambar sesuai indeks saat ini
            targetLayar.sprite = kumpulanGambar[index];

            // 2. Mainkan efek FADE IN (Alpha 0 ke 1)
            yield return StartCoroutine(EfekFade(0f, 1f));

            // 3. Kunci layar selama durasi wajib tampil
            yield return new WaitForSeconds(waktuWajibTampil);

            // 4. Stay dan tunggu sampai pemain melakukan klik
            yield return new WaitUntil(() => Input.GetMouseButtonDown(0) || Input.GetKeyDown(KeyCode.Space));

            // Jeda 1 frame agar input klik tidak tembus atau tereksekusi ganda ke siklus berikutnya
            yield return null;

            // 5. Mainkan efek FADE OUT (Alpha 1 ke 0)
            yield return StartCoroutine(EfekFade(1f, 0f));

            // Lanjut ke gambar berikutnya
            index++;
        }

        // Jika semua gambar sudah ditampilkan dan pudar, masuk ke game utama
        SceneManager.LoadScene(namaSceneGame);
    }

    // Fungsi khusus untuk mengatur kalkulasi transisi warna Alpha
    IEnumerator EfekFade(float alphaAwal, float alphaTujuan)
    {
        float waktuBerjalan = 0f;
        Color warnaSekarang = targetLayar.color;

        while (waktuBerjalan < waktuFade)
        {
            waktuBerjalan += Time.deltaTime;
            // Mathf.Lerp menghitung nilai mulus di antara dua titik berdasarkan persentase waktu
            warnaSekarang.a = Mathf.Lerp(alphaAwal, alphaTujuan, waktuBerjalan / waktuFade);
            targetLayar.color = warnaSekarang;
            yield return null; // Tunggu frame berikutnya
        }

        // Pastikan nilai mentok di angka tujuan untuk mencegah nilai desimal yang nanggung
        warnaSekarang.a = alphaTujuan;
        targetLayar.color = warnaSekarang;
    }
}